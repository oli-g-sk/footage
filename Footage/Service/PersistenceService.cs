namespace Footage.Service
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Settings;
    using Newtonsoft.Json;
    using NLog;

    public class PersistenceService : IPersistenceService
    {
        private static ILogger Log => LogManager.GetCurrentClassLogger();
        
        private const string ApplicationDataFilename = "ApplicationData.json";

        private const string UserPreferencesFilename = "UserPreferences.json";
        
        public string SettingsFolderPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Footage");
        
        public ApplicationData ApplicationData { get; private set; }
        
        public UserPreferences UserPreferences { get; private set; }

        public void Initialize()
        {
            if (!Directory.Exists(SettingsFolderPath))
            {
                Log.Info($"Default application folder doesn't exist and will be created: {SettingsFolderPath}");
                Directory.CreateDirectory(SettingsFolderPath);
            }
            
            ApplicationData = Deserialize<ApplicationData>(ApplicationDataFilename);
            UserPreferences = Deserialize<UserPreferences>(UserPreferencesFilename);
        }
        
        public Task UpdateApplicationData()
        {
            Serialize(ApplicationData, ApplicationDataFilename);
            return Task.CompletedTask;
        }

        public Task UpdateUserPreferences()
        {
            Serialize(UserPreferences, UserPreferencesFilename);
            return Task.CompletedTask;
        }
        
        private void Serialize(object instance, string filename)
        {
            string? filePath = Path.Combine(SettingsFolderPath, filename);
            var serializer = JsonSerializer.CreateDefault();
            using var streamWriter = new StreamWriter(filePath);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            serializer.Serialize(jsonWriter, instance);
        }

        private T? Deserialize<T>(string filename)
        {
            string filePath = Path.Combine(SettingsFolderPath, filename);
            var serializer = JsonSerializer.CreateDefault();
            using var streamReader = new StreamReader(filePath);
            using var jsonReader = new JsonTextReader(streamReader);
            return serializer.Deserialize<T>(jsonReader);
        }
    }
}