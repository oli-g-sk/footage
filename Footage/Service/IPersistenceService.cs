namespace Footage.Service
{
    using System.Threading.Tasks;
    using Footage.Settings;

    public interface IPersistenceService
    {
        string SettingsFolderPath { get; }
        
        ApplicationData ApplicationData { get; }
        
        UserPreferences UserPreferences { get; }

        Task UpdateApplicationData();

        Task UpdateUserPreferences();
    }
}