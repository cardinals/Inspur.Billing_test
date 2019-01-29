using CommonLib.Net;
using ControlLib.Controls.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Inspur.Billing.Commom;
using Inspur.Billing.Model;
using Inspur.Billing.Model.Service.Attention;
using LinqToDB;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Inspur.Billing.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Timer timer = new Timer(3000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //CheckIsOnline();
        }

        private void CheckIsOnline()
        {
            try
            {
                switch (Const.Locator.ParameterSetting.CommModel)
                {
                    case CommModel.NetPort:
                        TcpHelper helper = new TcpHelper();
                        string[] sdc = Const.Locator.ParameterSetting.SdcUrl.Split(':');
                        if (sdc != null && sdc.Count() != 2)
                        {
                            _logger.Info("EFD URL is not in the right format.");
                            SetIsOnline(false);
                            return;
                        }
                        helper.Connect(IPAddress.Parse(sdc[0]), int.Parse(sdc[1]));
                        SetIsOnline(helper.IsConnected);
                        break;
                    case CommModel.SerialPort:
                        string[] ports = SerialPort.GetPortNames();
                        if (ports != null && ports.Count() > 0)
                        {
                            if (ports.Contains(Const.Locator.ParameterSetting.SelectedPort))
                            {
                                SetIsOnline(true);
                            }
                            else
                            {
                                SetIsOnline(false);
                            }
                        }
                        else
                        {
                            SetIsOnline(false);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }


        private void SetIsOnline(bool isOnline)
        {
            _isOnline = isOnline;
            if (isOnline)
            {
                OnLineVisibility = Visibility.Visible;
                OffLineVisibility = Visibility.Collapsed;
            }
            else
            {
                OnLineVisibility = Visibility.Collapsed;
                OffLineVisibility = Visibility.Visible;
            }
        }
        #region 属性
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _uri = "Issue/CreditView.xaml";
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Uri
        {
            get { return _uri; }
            set { Set<string>(ref _uri, value, "Uri"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _message;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { Set<string>(ref _message, value, "Message"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private Visibility _onLineVisibility = Visibility.Visible;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public Visibility OnLineVisibility
        {
            get { return _onLineVisibility; }
            set { Set<Visibility>(ref _onLineVisibility, value, "OnLineVisibility"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private Visibility _offLineVisibility = Visibility.Collapsed;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public Visibility OffLineVisibility
        {
            get { return _offLineVisibility; }
            set { Set<Visibility>(ref _offLineVisibility, value, "OffLineVisibility"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private bool _isBusy;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set<bool>(ref _isBusy, value, "IsBusy"); }
        }

        private bool _isOnline = true;
        /// <summary>
        /// sdc是否连接
        /// </summary>
        public bool IsOnline
        {
            get { return _isOnline; }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 获取或设置
        /// </summary>
        private ICommand _navigationCommand;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public ICommand NavigationCommand
        {
            get
            {
                return _navigationCommand ?? (_navigationCommand = new RelayCommand<string>(p =>
                {
                    if (p == null || string.IsNullOrEmpty(p))
                    {
                        return;
                    }
                    Uri = p;
                }, a =>
                {
                    return true;
                }));
            }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private ICommand _command;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public ICommand Command
        {
            get
            {
                return _command ?? (_command = new RelayCommand<string>(p =>
                           {
                               switch (p)
                               {
                                   case "Loaded":
                                       LoadSDCInfo();
                                       break;
                                   default:
                                       break;
                               }
                           }, a =>
                           {
                               return true;
                           }));
            }
        }

        #endregion

        #region 方法
        private void LoadSDCInfo()
        {
            var sdcInfoes = (from a in Const.dB.SdcInfo
                             select a).ToList();
            if (sdcInfoes != null && sdcInfoes.Count() > 0)
            {
                Const.Locator.ParameterSetting.SdcUrl = string.Format("{0}:{1}", sdcInfoes[0].SdcIp, sdcInfoes[0].SdcPort);
            }
        }
        #endregion
    }
}