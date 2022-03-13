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
        
        private const string ApplicationDataFilename = "ApplicationData";

        private const string UserPreferencesFilename = "UserPreferences";
        
        public string SettingsFolderPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Footage");
        
        public ApplicationData ApplicationData { get; private set; }
        
        public UserPreferences UserPreferences { get; private set; }

        public PersistenceService()
        {
            ApplicationData? loadedApplicationData;
            UserPreferences? loadedUserPreferences;
            
            if (!Directory.Exists(SettingsFolderPath))
            {
                Log.Info($"Default application persistence folder doesn't exist and will be created: {SettingsFolderPath}");
                Directory.CreateDirectory(SettingsFolderPath);
            }
            else
            {
                Log.Info("Loading application settings.");
                
                ApplicationData = Deserialize<ApplicationData>(ApplicationDataFilename)!;
                UserPreferences = Deserialize<UserPreferences>(UserPreferencesFilename)!;
            }

            if (ApplicationData == null)
            {
                ApplicationData = new ApplicationData();
                Log.Info("Default application data created.");
                UpdateApplicationData();
            }

            if (UserPreferences == null)
            {
                UserPreferences = new UserPreferences();
                Log.Info("Default user preferences created.");
                UpdateUserPreferences();
            }
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
            string? filePath = Path.Combine(SettingsFolderPath, $"{filename}.json");
            var serializer = JsonSerializer.CreateDefault();
            using var streamWriter = new StreamWriter(filePath);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            
            try
            {
                serializer.Serialize(jsonWriter, instance);
                Log.Debug($"Settings saved to disk in '{filename}'.");
            }
            catch (Exception ex)
            {
                Log.Warn($"Failed to save settings in '{filename}'. Reason: {ex.Message}");
            }
        }

        private T? Deserialize<T>(string filename)
        {
            string filePath = Path.Combine(SettingsFolderPath, $"{filename}.json");

            if (!File.Exists(filePath))
            {
                Log.Warn($"Settings file '{filename}' is missing; will revert to default.");
                return default;
            }
            else
            {
                var serializer = JsonSerializer.CreateDefault();
                using var streamReader = new StreamReader(filePath);
                using var jsonReader = new JsonTextReader(streamReader);

                try
                {
                    return serializer.Deserialize<T>(jsonReader);
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to load settings file {filename}. Reason: {ex.Message}.");
                }
            }

            return default;
        }
    }
}