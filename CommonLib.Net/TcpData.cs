using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Net
{
    public class TcpData
    {
        #region 字段
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
        /// <summary>
        /// 消息标识
        /// </summary>
        byte _cmdId;
        /// <summary>
        /// 报文长度
        /// </summary>
        int _length;
        /// <summary>
        /// 校验码
        /// </summary>
        short _crc;

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
        #endregion

        #region 构造函数
        public TcpData(Encoding encoding)
        {
            _defaultEncoding = encoding;
        }
        public TcpData() : this(Encoding.UTF8)
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 自定义编码
        /// </summary>
        /// <param name="message"></param>
        public byte[] Encode(byte id, string message)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
                {
                    //写入头
                    bw.Write(HEADER1);
                    bw.Write(HEADER2);
                    //写入id
                    bw.Write(id);
                    //写入
                    byte[] data = _defaultEncoding.GetBytes(message);
                    //写入内容长度
                    byte[] lengthBytes = BitConverter.GetBytes(data.Length);
                    //写入报文的时候高位在前，低位在后
                    Array.Reverse(lengthBytes);
                    bw.Write(lengthBytes);
                    //写入内容
                    bw.Write(data);
                    //写入校验码
                    byte[] bytes = ms.ToArray();
                    ushort crc = CalculationCrc(bytes, bytes.Count());

                    byte[] crcBytes = BitConverter.GetBytes(crc);
                    Array.Reverse(crcBytes);
                    bw.Write(crcBytes);

                    _logger.Info(string.Format("数据发送  id：{0},内容：{1}", id, message));
                    //_logger.Info(string.Format("数据发送  id：{0},内容：{1}，编码成字节数据：{2}", id, message, BitConverter.ToString(ms.ToArray())));
                    return ms.ToArray();
                }
            }
        }
        /// <summary>
        /// 自定义解码
        /// </summary>
        /// <param name="data"></param>
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
                        //_logger.Info("CRC is invalid." + localCRC + "Remote CRC:" + crc);
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
        #endregion
    }
}
