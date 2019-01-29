using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlLib.Controls
{
    public class FilterComboBox : ComboBox
    {
        static FilterComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterComboBox), new FrameworkPropertyMetadata(typeof(FilterComboBox)));
        }

        TextBox _editTextBox = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


            _editTextBox = (TextBox)GetTemplateChild("PART_EditableTextBox");
            if (_editTextBox != null)
            {
                _editTextBox.TextChanged += _editTextBox_TextChanged;
                _editTextBox.TextInput += _editTextBox_TextInput;
            }
        }

        private void _editTextBox_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            _editTextBox.Select(Text.Length, 0);
            //_editTextBox.SelectionStart = Text.Length;
        }

        private void _editTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //_editTextBox.Select(Text.Length, 0);
            //try
            //{
                _editTextBox.SelectionStart = Text.Length;
                if (string.IsNullOrWhiteSpace(Text))
                {
                    ItemsSource = null;
                }
                else
                {
                    if (MyItemsSource != null)
                    {

                        List<object> list = new List<object>();
                        foreach (var item in MyItemsSource)
                        {
                            string s = item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();
                            if (s.Contains(Text))
                            {
                                list.Add(item);
                            }
                        }
                        ItemsSource = list;
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
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        public IEnumerable MyItemsSource
        {
            get { return (IEnumerable)GetValue(MyItemsSourceProperty); }
            set { SetValue(MyItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyItemsSourceProperty =
            DependencyProperty.Register("MyItemsSource", typeof(IEnumerable), typeof(FilterComboBox), new PropertyMetadata(null, new PropertyChangedCallback(OnMyItemsSourceChanged)));

        private static void OnMyItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
