using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inspur.Billing.View.Issue
{
    /// <summary>
    /// PrintView.xaml 的交互逻辑
    /// </summary>
    public partial class PrintView : Window
    {
        public PrintView()
        {
            InitializeComponent();
            //注册MVVMLight消息
            Messenger.Default.Register<string>(this, "ClosePrintView", a => 
            {
                this.DialogResult = true;
            });

            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }
    }
}
