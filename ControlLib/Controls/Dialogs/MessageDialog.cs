using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlLib.Controls.Dialogs
{
    public class MessageDialog : Window
    {
        static MessageDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageDialog), new FrameworkPropertyMetadata(typeof(MessageDialog)));
            var onExcuteCommand = new ExecutedRoutedEventHandler(OnExcuteCommand);
            var onCanExcuteCommand = new CanExecuteRoutedEventHandler(OnCanExcuteCommand);
            CommandManager.RegisterClassCommandBinding(typeof(MessageDialog), new CommandBinding(MessageDialog.YesCommand, onExcuteCommand, onCanExcuteCommand));
            CommandManager.RegisterClassCommandBinding(typeof(MessageDialog), new CommandBinding(MessageDialog.NoCommand, onExcuteCommand, onCanExcuteCommand));
            CommandManager.RegisterClassCommandBinding(typeof(MessageDialog), new CommandBinding(MessageDialog.ClosedCommand, onExcuteCommand, onCanExcuteCommand));
        }
        public MessageDialog()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private static void OnCanExcuteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void OnExcuteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            MessageDialog messageDialog = sender as MessageDialog;
            if (messageDialog == null)
            {
                return;
            }
            if (e.Command == MessageDialog.YesCommand)
            {
                messageDialog.DialogResult = true;
            }
            else if (e.Command == MessageDialog.NoCommand)
            {
                messageDialog.DialogResult = false;
            }
            else if (e.Command == MessageDialog.ClosedCommand)
            {
                messageDialog.DialogResult = false;
            }
        }

        /// <summary>
        /// 确定命令
        /// </summary>
        public static readonly RoutedCommand YesCommand = new RoutedCommand("Yes", typeof(MessageDialog));
        /// <summary>
        /// 否命令
        /// </summary>
        public static readonly RoutedCommand NoCommand = new RoutedCommand("No", typeof(MessageDialog));
        /// <summary>
        /// 关闭命令
        /// </summary>
        public static readonly RoutedCommand ClosedCommand = new RoutedCommand("Closed", typeof(MessageDialog));
        public MessageBoxButton MessageBoxButton
        {
            get { return (MessageBoxButton)GetValue(MessageBoxButtonProperty); }
            set { SetValue(MessageBoxButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageBoxButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageBoxButtonProperty =
            DependencyProperty.Register("MessageBoxButton", typeof(MessageBoxButton), typeof(MessageDialog), new PropertyMetadata(MessageBoxButton.OK));


        public static bool? Show(string caption, string content, MessageBoxButton messageBoxButton)
        {
            MessageDialog messageDialog = new MessageDialog();
            messageDialog.Content = content;
            messageDialog.Title = caption;
            messageDialog.MessageBoxButton = messageBoxButton;
            return messageDialog.ShowDialog();
        }
        public static bool? Show(string content, MessageBoxButton messageBoxButton)
        {
            return Show("System Prompt", content, messageBoxButton);
        }
    }
}
