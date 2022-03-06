using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace Footage.UI.Dialogs
{
    using Avalonia.Interactivity;

    public class SimpleDialog : Window
    {
        public SimpleDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BtnPositive_OnClick(object? sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void BtnNegative_OnClick(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }
        
        public static async Task<bool> ShowYesNo(Window owner, string title, string message)
        {
            return await Show(owner, title, message, "Yes", "No");
        }

        private static async Task<bool> Show(Window owner, string title, string message, string positive, string negative)
        {
            var dialog = new SimpleDialog();
            dialog.Title = title;
            
            var txtMessage = dialog.Find<TextBlock>("TxtMessage");
            var btnPositive = dialog.Find<Button>("BtnPositive");
            var btnNegative = dialog.Find<Button>("BtnNegative");
            
            txtMessage.Text = message;
            btnPositive.Content = positive;
            btnNegative.Content = negative;
            
            return await dialog.ShowDialog<bool>(owner);
        }
    }
}