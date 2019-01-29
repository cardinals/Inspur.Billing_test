using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CommonLib.Net
{
    public class TcpHelper
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 通讯的socket对象
        /// </summary>
        Socket _socket;
        /// <summary>
        /// 之定义的Tcp包数据
        /// </summary>
        TcpData _tcpData;
        /// <summary>
        /// 是否连接到远程服务器
        /// </summary>
        bool _isConn;
        /// <summary>
        /// 超时控制
        /// </summary>
        private ManualResetEvent _timeOutObject;
        /// <summary>
        /// 超时时间
        /// </summary>
        int _timeOut = 10000;


        public TcpHelper()
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _tcpData = new TcpData(Encoding.ASCII);
            _timeOutObject = new ManualResetEvent(false);
        }
        public bool Connect(IPAddress iPAddress, int port)
        {
            if (!_socket.Connected)
            {
                IPEndPoint point = new IPEndPoint(iPAddress, port);
                _socket.BeginConnect(point, new AsyncCallback(ConnectComplate), _socket);
                if (!_timeOutObject.WaitOne(_timeOut, false))
                {
                    CloseSocket();
                    throw new Exception("Connection timed out.");
                }
            }
            return _isConn;
        }

        private void ConnectComplate(IAsyncResult ar)
        {
            try
            {
                Socket socket = ar.AsyncState as Socket;
                if (socket != null)
                {
                    socket.EndConnect(ar);
                    _isConn = true;
                    _logger.Info("Socket连接成功");
                }
            }
            catch (Exception ex)
            {
                _isConn = false;
                _logger.Info(ex.Message);
            }
            finally
            {
                _timeOutObject.Set();
            }
        }

        byte[] buffer;
        /// <summary>
        /// 异步接受收数据
        /// </summary>
        public void ReciveAsync()
        {
            buffer = new byte[1024 * 1024];
            _socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReciveCallback), _socket);
            _timeOutObject = new ManualResetEvent(false);
            if (!_timeOutObject.WaitOne(_timeOut, false))
            {
                CloseSocket();
                throw new Exception("Connection timed out.");
            }
        }
        /// <summary>
        /// 同步接收数据
        /// </summary>
        /// <returns></returns>
        public MessageModel Recive()
        {
            buffer = new byte[1024 * 1024];
            int count = _socket.Receive(buffer, 0, buffer.Length, 0);
            if (count > 0)
            {
                byte[] results = new byte[count];
                Array.Copy(buffer, 0, results, 0, count);
                MessageModel messageModel = _tcpData.Decode(results);
                if (messageModel.MessageId == 0)
                {
                    return Recive();
                }
                else
                {
                    return messageModel;
                }
            }
            return new MessageModel();
        }
        private void ReciveCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = ar.AsyncState as Socket;
                if (socket != null)
                {
                    int end = socket.EndReceive(ar);
                    if (end > 0)
                    {
                        byte[] data = new byte[end];
                        Array.Copy(buffer, 0, data, 0, end);
                        //_logger.Info(string.Format("接收消息字节 {0}", BitConverter.ToString(data.ToArray())));
                        MessageModel messageModel = _tcpData.Decode(data);
                        if (messageModel.MessageId == 0)
                        {
                            socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReciveCallback), socket);
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
                            CloseSocket();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + ex.StackTrace);
            }
        }

        public void Send(byte id, string data)
        {
            _socket.Send(_tcpData.Encode(id, data));
        }
        public void CloseSocket()
        {
            _socket.Close();
            _logger.Info("关闭Socket");
            _socket = null;
        }

        public event EventHandler<MessageModel> Complated;

        public bool IsConnected
        {
            get
            {
                return _socket != null ? _socket.Connected : false;
            }
        }
    }
}
