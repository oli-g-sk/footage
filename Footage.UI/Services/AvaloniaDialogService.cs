namespace Footage.UI.Services
{
    using System;
    using System.Threading.Tasks;
    using Avalonia.Controls;
    using Footage.Service;
    using Footage.UI.Dialogs;
    using Footage.UI.Views;

    public class AvaloniaDialogService : IDialogService
    {
        public async Task<string?> SelectFolder(string? startingPath = null, string? title = null)
        {
            var dialog = new OpenFolderDialog();

            if (startingPath != null)
            {
                dialog.Directory = startingPath;
            }

            if (title != null)
            {
                dialog.Title = title;
            }

            return await dialog.ShowAsync(MainWindow.Instance);
        }

        public async Task<bool> ShowYesNo(string title, string message)
        {
            return await SimpleDialog.ShowYesNo(MainWindow.Instance, title, message);
        }

        public async Task<(bool Confirmed, string InputValue)> ShowInput(string title, string message, string? inputText = null)
        {
            return await SimpleDialog.ShowInput(MainWindow.Instance, title, message, inputText);
        }
    }
}