using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CommonLib.Net
{
    public class SerialClient
    {
        #region 字段
        /// <summary>
        /// 串行端口通讯对象
        /// </summary>
        SerialPort _serialPort = new SerialPort();

        /// <summary>
        /// 日志对象
        /// </summary>
        Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 默认编码方式
        /// </summary>
        Encoding _defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// 包头第一个字节
        /// </summary>
        const byte HEADER1 = 0x1A;
        /// <summary>
        /// 包头第二个字节
        /// </summary>
        const byte HEADER2 = 0x5D;

        //一次未读取完剩余的字节
        List<byte> _unreadBuffer = new List<byte>();
        /// <summary>
        /// 正文之前的头长度
        /// </summary>
        const int HEADLENGTH = 7;
        /// <summary>
        /// RCR长度
        /// </summary>
        const int RCRLENGTH = 2;
        /// <summary>
        /// 超时控制
        /// </summary>
        private ManualResetEvent _timeOutObject;
        /// <summary>
        /// 超时时间
        /// </summary>
        int _timeOut = 10000;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portName">串口名</param>
        /// <param name="baudRates">波特率</param>
        /// <param name="parity">奇偶校验位</param>
        /// <param name="dataBits">数据长度</param>
        /// <param name="stopBits">停止位</param>
        public SerialClient(string portName, int baudRates, Parity parity, int dataBits, StopBits stopBits)
        {
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRates;
            _serialPort.Parity = parity;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = stopBits;

            _serialPort.ReadTimeout = 10000;
            _serialPort.WriteTimeout = 10000;
            _serialPort.ReadBufferSize = 1024 * 1024;
            _serialPort.DataReceived += _serialPort_DataReceived;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            _serialPort.Open();
            _logger.Info("Open the serial port successfully.");
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _logger.Info("Close the serial port successfully.");
            }
        }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Send(byte id, string message)
        {
            byte[] sendBytes = Encode(id, message);
            _serialPort.Write(sendBytes, 0, sendBytes.Count());
            _timeOutObject = new ManualResetEvent(false);
            if (!_timeOutObject.WaitOne(_timeOut, false))
            {
                Close();
                throw new Exception("Connection timed out.");
            }
        }
        private byte[] Encode(byte id, string message)
        {
            List<byte> sendBytes = new List<byte>();
            sendBytes.Add(0x1A);
            sendBytes.Add(0x5D);
            sendBytes.Add(id);
            byte[] data = Encoding.UTF8.GetBytes(message);
            //写入内容长度
            byte[] lengthBytes = BitConverter.GetBytes(data.Length);
            //写入报文的时候高位在前，低位在后
            Array.Reverse(lengthBytes);
            sendBytes.AddRange(lengthBytes);
            sendBytes.AddRange(data);

            byte[] bytes = sendBytes.ToArray();
            ushort crc = CalculationCrc(bytes, bytes.Count());

            byte[] crcBytes = BitConverter.GetBytes(crc);
            Array.Reverse(crcBytes);
            sendBytes.AddRange(crcBytes);
            _logger.Info(string.Format("串口数据发送 ,内容：{0}，编码成字节数据：{1}", message, BitConverter.ToString(sendBytes.ToArray())));
            return sendBytes.ToArray();
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
        public MessageModel Decode(byte[] data)
        {
            ////拷贝本次的有效字节  
            if (this._unreadBuffer.Count > 0)
            {
                //拷贝之前遗留的字节  
                this._unreadBuffer.AddRange(data);
                data = this._unreadBuffer.ToArray();
                this._unreadBuffer.Clear();
                this._unreadBuffer = new List<byte>();
            }

            MessageModel messageModel = new MessageModel();
            messageModel.IsSuccess = true;
            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms, _defaultEncoding);
            try
            {
                byte[] buff;

                if (!LoopReadHeader(br))
                {
                    return messageModel;
                }

                #region 包协议  
                //读取消息id
                byte messageId = br.ReadByte();
                messageModel.MessageId = messageId;
                //读取报文长度
                byte[] lengthBytes = br.ReadBytes(4);
                Array.Reverse(lengthBytes);
                int dataLength = BitConverter.ToInt32(lengthBytes, 0);
                #endregion

                #region 包解析  
                //剩余字节数大于本次需要读取的字节数  
                if (dataLength + 2 <= (br.BaseStream.Length - br.BaseStream.Position))
                {
                    //读取内容
                    byte[] contentBytes = br.ReadBytes(dataLength);
                    messageModel.Data = contentBytes;
                    messageModel.Message = _defaultEncoding.GetString(contentBytes);
                    Console.WriteLine(messageModel.Message);
                    //读取校验码
                    byte[] crcBytes = br.ReadBytes(2);
                    Array.Reverse(crcBytes);
                    //高位在前，低位在后
                    ushort crc = BitConverter.ToUInt16(crcBytes, 0);

                    byte[] messageBytes = new byte[HEADLENGTH + dataLength];
                    Array.Copy(data, br.BaseStream.Position - HEADLENGTH - RCRLENGTH - dataLength, messageBytes, 0, HEADLENGTH + dataLength);

                    //将未读数据添加到未读字节列表
                    while (br.BaseStream.Position < br.BaseStream.Length - 1)
                    {
                        _unreadBuffer.Add(br.ReadByte());
                    }
                    ushort localCRC = CalculationCrc(messageBytes, messageBytes.Count());
                    if (crc != localCRC)
                    {
                        messageModel.IsSuccess = false;
                        messageModel.ErrorMessage = "CRC is invalid.";
                        _logger.Info("CRC is invalid." + localCRC + "Remote CRC:" + crc);
                    }
                }
                else
                {
                    messageModel = new MessageModel();
                    _unreadBuffer.Add(HEADER1);
                    _unreadBuffer.Add(HEADER2);
                    _unreadBuffer.Add(messageId);
                    Array.Reverse(lengthBytes);
                    _unreadBuffer.AddRange(lengthBytes);
                    //剩余字节数刚好小于本次读取的字节数 存起来，等待接受剩余字节数一起解析  
                    buff = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position + 7));
                    _unreadBuffer.AddRange(buff);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (br != null)
                {
                    br.Dispose();
                }
                br.Close();
                ms.Close();
                if (ms != null)
                {
                    ms.Dispose();
                }
            }
            return messageModel;
        }
        private bool LoopReadHeader(BinaryReader br)
        {
            //循环读取包头             
            //判断本次解析的字节是否满足常量字节数   
            if ((br.BaseStream.Length - br.BaseStream.Position) < (HEADLENGTH + RCRLENGTH))
            {
                byte[] _buff = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
                this._unreadBuffer.AddRange(_buff);
                return false;
            }
            byte header1 = br.ReadByte();
            byte header2 = br.ReadByte();
            if (!(HEADER1 == header1 && HEADER2 == header2))
            {
                br.BaseStream.Seek(-1, SeekOrigin.Current);
                return LoopReadHeader(br);
            }
            return true;
        }


        private void Receive()
        {
            byte[] bytes = new byte[_serialPort.ReadBufferSize];
            int length = _serialPort.Read(bytes, 0, bytes.Length);
            if (length > 0)
            {
                byte[] data = new byte[length];
                Array.Copy(bytes, 0, data, 0, length);
                _logger.Info(string.Format("接收消息字节 {0}", BitConverter.ToString(data.ToArray())));
                MessageModel messageModel = Decode(data);
                if (messageModel.MessageId == 0)
                {
                    Receive();
                }
                else
                {
                    //接收数据成功之后，解除程序的阻塞
                    _timeOutObject.Set();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Complated(this, messageModel);
                    }));
                    _logger.Info(string.Format("接收消息 {0}", messageModel.Message));
                    Close();
                }
            }
        }
        #endregion

        #region 事件
        public event EventHandler<MessageModel> Complated;

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Receive();
            }
            catch (Exception ex)
            {
                _logger.Info(string.Format("_serialPort_DataReceived ,内容：{0}，位置：{1}", ex.Message, ex.StackTrace));
            }
        }
        #endregion
    }
}
