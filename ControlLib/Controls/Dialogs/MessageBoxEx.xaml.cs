using System.Windows;
using System.Windows.Input;

namespace ControlLib.Controls.Dialogs
{
    /// <summary>
    /// MessageBoxEx.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxEx : Window
    {
        public MessageBoxEx()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void ShowButton(MessageBoxButton messageBoxButton)
        {
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    btnOk.Visibility = Visibility.Visible;
                    gdOkCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.OKCancel:
                    btnOk.Visibility = Visibility.Collapsed;
                    gdOkCancel.Visibility = Visibility.Visible;
                    break;
                case MessageBoxButton.YesNoCancel:
                    break;
                case MessageBoxButton.YesNo:
                    break;
                default:
                    btnOk.Visibility = Visibility.Visible;
                    gdOkCancel.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void ShowText(string content)
        {
            tbkContent.Text = content;
        }
        public static bool? Show(string caption, string content, MessageBoxButton messageBoxButton)
        {
            
            MessageBoxEx messageDialog = new MessageBoxEx();
            messageDialog.Title = caption;
            messageDialog.ShowButton(messageBoxButton);
            messageDialog.ShowText(content);
            return messageDialog.ShowDialog();
        }
        public static bool? Show(string content, MessageBoxButton messageBoxButton)
        {
            return Show("System Prompt", content, messageBoxButton);
        }
        public static bool? Show(string content)
        {
            return Show("System Prompt", content, MessageBoxButton.OK);
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
