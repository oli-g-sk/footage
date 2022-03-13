namespace Footage.Service
{
    using System.Threading.Tasks;
    using Footage.Settings;

    public interface IPersistenceService
    {
        string SettingsFolderPath { get; }

        void Initialize();
        
        ApplicationData ApplicationData { get; }
        
        UserPreferences UserPreferences { get; }

        Task UpdateApplicationData();

        Task UpdateUserPreferences();
    }
}