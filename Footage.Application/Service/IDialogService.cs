namespace Footage.Application.Service
{
    using System.Threading.Tasks;

    public interface IDialogService
    {
        Task<string?> SelectFolder(string? startingPath = null, string? title = null);

        Task<bool> ShowYesNo(string title, string message);

        Task<(bool Confirmed, string InputValue)> ShowInput(string title, string message, string? inputText = null);
    }
}