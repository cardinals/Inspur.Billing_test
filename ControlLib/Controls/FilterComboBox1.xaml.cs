using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlLib.Controls
{
    /// <summary>
    /// FilterComboBox1.xaml 的交互逻辑
    /// </summary>
    public partial class FilterComboBox1 : UserControl
    {
        public FilterComboBox1()
        {
            InitializeComponent();
            this.DataContext = this;
            tb.TextChanged += Tb_TextChanged;
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                IsDropDownOpen = false;
            }
            else
            {
                if (MyItemsSource != null)
                {

                    List<object> list = new List<object>();
                    foreach (var item in MyItemsSource)
                    {
                        string s = item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();
                        if (s.Contains(tb.Text))
                        {
                            list.Add(item);
                        }
                    }
                    listbox.ItemsSource = list;
                    if (list.Count > 0)
                    {
                        IsDropDownOpen = true;
                    }
                    else
                    {
                        IsDropDownOpen = false;
                    }
                }
            }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDropDownOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(FilterComboBox1), new PropertyMetadata(false));


        public IEnumerable MyItemsSource
        {
            get { return (IEnumerable)GetValue(MyItemsSourceProperty); }
            set { SetValue(MyItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyItemsSourceProperty =
            DependencyProperty.Register("MyItemsSource", typeof(IEnumerable), typeof(FilterComboBox1), new PropertyMetadata(null));


        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(FilterComboBox1), new PropertyMetadata(null));


    }
}
