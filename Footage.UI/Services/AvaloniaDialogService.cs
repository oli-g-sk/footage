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

            // TODO await
            var task = dialog.ShowAsync(MainWindow.Instance);
            task.Wait();
            await Task.CompletedTask;
            
            string? path = task.Result;
            return path;
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