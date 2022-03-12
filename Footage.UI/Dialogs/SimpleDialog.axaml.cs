using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace Footage.UI.Dialogs
{
    using Avalonia.Input;
    using Avalonia.Interactivity;

    public class SimpleDialog : Window
    {
        private bool inTextInputMode;
        
        private TextBlock txtMessage;
        private Button btnPositive;
        private Button btnNegative;
        private TextBox txtInput;
        
        public SimpleDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            txtMessage = this.Find<TextBlock>("TxtMessage");
            btnPositive = this.Find<Button>("BtnPositive");
            btnNegative = this.Find<Button>("BtnNegative");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BtnPositive_OnClick(object? sender, RoutedEventArgs e)
        {
            Close(GetResultObject(true));
        }

        private void BtnNegative_OnClick(object? sender, RoutedEventArgs e)
        {
            Close(GetResultObject(false));
        }

        private object GetResultObject(bool isConfirmed)
        {
            if (inTextInputMode)
            {
                return (isConfirmed, txtInput.Text);
            }

            return isConfirmed;
        }
        
        public static async Task<bool> ShowYesNo(Window owner, string title, string message)
        {
            var dialog = CreateDialog(title, message, "Yes", "No");
            return await dialog.ShowDialog<bool>(owner);
        }

        public static async Task<(bool Confirmed, string InputValue)> ShowInput(Window owner, string title, string message,
            string? inputText = null)
        {
            var dialog = CreateDialog(title, message, "Ok", "Cancel");

            dialog.inTextInputMode = true;
            dialog.txtInput.IsVisible = true;
            dialog.txtInput.Text = inputText;
            dialog.txtInput.SelectAll();
            
            dialog.GotFocus += Dialog_GotFocus;
            
            return await dialog.ShowDialog<(bool Confirmed, string InputValue)>(owner);
            
            void Dialog_GotFocus(object? sender, GotFocusEventArgs e)
            {
                dialog.GotFocus -= Dialog_GotFocus;
                dialog.txtInput.Focus();
            }
        }

        private static SimpleDialog CreateDialog(string title, string message, string positive, string negative)
        {
            var dialog = new SimpleDialog();
            dialog.Title = title;

            dialog.txtMessage.Text = message;
            dialog.btnPositive.Content = positive;
            dialog.btnNegative.Content = negative;

            return dialog;
        }
    }
}