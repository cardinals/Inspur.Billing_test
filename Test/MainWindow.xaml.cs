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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 计算校验码
        /// </summary>
        private ushort CalculationCrc(byte[] data, int length)
        {
            ushort i;
            uint crc = 0;

            foreach (var item in data)
            {
                for (i = 0x80; i != 0; i /= 2)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc *= 2;
                        crc ^= 0x18005;
                    }
                    else
                    {
                        crc *= 2;
                    }
                    if ((item & i) != 0)
                        crc ^= 0x18005;
                }
            }
            Console.WriteLine((ushort)crc);
            return (ushort)crc;
        }



        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            List<byte> dataBytes = new List<byte>();

            StringBuilder builder = new StringBuilder();
            builder.Append("1A ");
            builder.Append("5D ");
            byte cmd = byte.Parse(tbCmd.Text);

            builder.Append(cmd.ToString("x2") + " ");
            dataBytes.Add(0x1A);
            dataBytes.Add(0x5D);
            dataBytes.Add(cmd);

            byte[] data = Encoding.UTF8.GetBytes(tbContent.Text);

            //写入内容长度
            byte[] lengthBytes = BitConverter.GetBytes(data.Length);
            //写入报文的时候高位在前，低位在后
            Array.Reverse(lengthBytes);
            foreach (var item in lengthBytes)
            {
                builder.Append(item.ToString("x2") + " ");
                dataBytes.Add(item);
            }
            foreach (var item in data)
            {
                builder.Append(item.ToString("x2") + " ");
                dataBytes.Add(item);
            }

            ushort localCRC = CalculationCrc(dataBytes.ToArray(), dataBytes.Count);
            byte[] crc = BitConverter.GetBytes(localCRC);
            //写入报文的时候高位在前，低位在后
            Array.Reverse(crc);

            foreach (var item in crc)
            {
                builder.Append(item.ToString("x2") + " ");
            }


            Console.WriteLine(builder.ToString());
        }
    }
}
