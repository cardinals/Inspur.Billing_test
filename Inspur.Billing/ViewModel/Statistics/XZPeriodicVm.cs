using CommonLib.Net;
using ControlLib.Controls.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Inspur.Billing.Commom;
using Inspur.Billing.Model;
using Inspur.Billing.Model.Service.Statistics;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Inspur.Billing.ViewModel.Statistics
{
    public class XZPeriodicVm : ViewModelBase
    {
        #region 字段
        /// <summary>
        /// 请求报表的指令代码
        /// </summary>
        const byte Cmd = 0x04;
        /// <summary>
        /// 日志对象
        /// </summary>
        Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置
        /// </summary>
        private DateTime? _beginTime;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public DateTime? BeginTime
        {
            get { return _beginTime; }
            set
            {
                if (value != _beginTime)
                {
                    _beginTime = value;
                    if (ReportType == "1")
                    {
                        EndTime = BeginTime;
                    }
                    RaisePropertyChanged(() => this.BeginTime);
                }
            }
        }

        /// <summary>
        /// 获取或设置
        /// </summary>
        private DateTime? _endTime;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { Set<DateTime?>(ref _endTime, value, "EndTime"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _reportType;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string ReportType
        {
            get { return _reportType; }
            set
            {
                if (value != _reportType)
                {
                    _reportType = value;
                    switch (_reportType)
                    {
                        case "0":
                            Title = "X REPORT";
                            NumberVisibility = Visibility.Collapsed;
                            PeriodicVisibility = Visibility.Collapsed;
                            BeginTime = null;
                            EndTime = null;
                            IsBeginEnable = false;
                            IsEndEnable = false;
                            break;
                        case "1":
                            Title = "Z REPORT";
                            NumberVisibility = Visibility.Visible;
                            PeriodicVisibility = Visibility.Collapsed;
                            IsBeginEnable = true;
                            IsEndEnable = false;
                            break;
                        case "2":
                            Title = "PERIODIC REPORT";
                            NumberVisibility = Visibility.Collapsed;
                            PeriodicVisibility = Visibility.Visible;
                            IsBeginEnable = true;
                            IsEndEnable = true;
                            break;
                        default:
                            break;
                    }
                    RaisePropertyChanged(() => this.ReportType);
                }
            }
        }


        /// <summary>
        /// 获取或设置
        /// </summary>
        private List<CodeTable> _reportTypes = new List<CodeTable>
        {
            new CodeTable { Code="0",Name="X-report"},
            new CodeTable { Code="1",Name="Z-report"},
            new CodeTable { Code="2",Name="Periodic report"}
        };
        /// <summary>
        /// 获取或设置
        /// </summary>
        public List<CodeTable> ReportTypes
        {
            get { return _reportTypes; }
            set { Set<List<CodeTable>>(ref _reportTypes, value, "ReportTypes"); }
        }

        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _currentTime;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string CurrentTime
        {
            get { return _currentTime; }
            set { Set<string>(ref _currentTime, value, "CurrentTime"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private double _totalSlaes;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public double TotalSlaes
        {
            get { return _totalSlaes; }
            set { Set<double>(ref _totalSlaes, value, "TotalSlaes"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private double _totalTax;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public double TotalTax
        {
            get { return _totalTax; }
            set { Set<double>(ref _totalTax, value, "TotalTax"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private double _invoiceQuantity;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public double InvoiceQuantity
        {
            get { return _invoiceQuantity; }
            set { Set<double>(ref _invoiceQuantity, value, "InvoiceQuantity"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private List<ReportTaxItems> _taxItems;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public List<ReportTaxItems> TaxItems
        {
            get { return _taxItems; }
            set { Set<List<ReportTaxItems>>(ref _taxItems, value, "TaxItems"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _reportNumber;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string ReportNumber
        {
            get { return _reportNumber; }
            set { Set<string>(ref _reportNumber, value, "ReportNumber"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _beginDate;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string BeginDate
        {
            get { return _beginDate; }
            set { Set<string>(ref _beginDate, value, "BeginDate"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _endDate;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string EndDate
        {
            get { return _endDate; }
            set { Set<string>(ref _endDate, value, "EndDate"); }
        }



        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _title;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { Set<string>(ref _title, value, "Title"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private Visibility _numberVisibility;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public Visibility NumberVisibility
        {
            get { return _numberVisibility; }
            set { Set<Visibility>(ref _numberVisibility, value, "NumberVisibility"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private Visibility _periodicVisibility;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public Visibility PeriodicVisibility
        {
            get { return _periodicVisibility; }
            set { Set<Visibility>(ref _periodicVisibility, value, "PeriodicVisibility"); }
        }

        /// <summary>
        /// 获取或设置开始时间是否可以编辑
        /// </summary>
        private bool _isBeginEnable;
        /// <summary>
        /// 获取或设置开始时间是否可以编辑
        /// </summary>
        public bool IsBeginEnable
        {
            get { return _isBeginEnable; }
            set { Set<bool>(ref _isBeginEnable, value, "IsBeginEnable"); }
        }
        /// <summary>
        /// 获取或设置结束时间是否可以编辑
        /// </summary>
        private bool _isEndEnable;
        /// <summary>
        /// 获取或设置结束时间是否可以编辑
        /// </summary>
        public bool IsEndEnable
        {
            get { return _isEndEnable; }
            set { Set<bool>(ref _isEndEnable, value, "IsEndEnable"); }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 获取或设置
        /// </summary>
        private ICommand _queryCommand;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public ICommand QueryCommand
        {
            get
            {
                return _queryCommand ?? (_queryCommand = new RelayCommand(() =>
                {
                    ReportRequest request = new ReportRequest();
                    if (ReportType == "1" || ReportType == "2")
                    {
                        if (BeginTime == null)
                        {
                            MessageBoxEx.Show("Please select the BeginDate.");
                            return;
                        }
                        if (EndTime == null)
                        {
                            MessageBoxEx.Show("Please select the EndDate.");
                            return;
                        }
                        request.BeginDate = BeginTime.Value.ToString("yyyyMMdd");
                        request.EndDate = EndTime.Value.ToString("yyyyMMdd");
                    }
                    request.CurrentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    request.ReportType = int.Parse(ReportType);

                    string requestString = JsonConvert.SerializeObject(request);

                    switch (Const.Locator.ParameterSetting.CommModel)
                    {
                        case CommModel.NetPort:
                            NetRequest(requestString);
                            break;
                        case CommModel.SerialPort:
                            SerialRequest(requestString);
                            break;
                        default:
                            break;
                    }
                }, () =>
                {
                    return true;
                }));
            }
        }


        /// <summary>
        /// 获取或设置
        /// </summary>
        private ICommand _printCommand;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public ICommand PrintCommand
        {
            get
            {
                return _printCommand ?? (_printCommand = new RelayCommand(() =>
                {
                    Printer.Instance.Print(() =>
                    {
                        //打印自定义的表样
                        Printer.Instance.SetAlign(0);
                        switch (ReportType)
                        {
                            case "0":

                                break;
                            case "1":
                                Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("REPORT NO: {0}\r\n", ReportNumber));
                                break;
                            case "2":

                                break;
                            default:
                                break;
                        }
                        Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("DATE TIME:{0}\r\n", CurrentTime));
                        Printer.Instance.SetAlign(1);
                        Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("{0}\r\n", Title));
                        switch (ReportType)
                        {
                            case "0":
                                break;
                            case "1":
                                break;
                            case "2":
                                Printer.Instance.SetAlign(0);
                                Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("BETWEEN {0} AND {1}\r\n", BeginDate, EndDate));
                                break;
                            default:
                                break;
                        }
                        Printer.Instance.SetAlign(0);
                        Printer.Instance.PrintLine();

                        Printer.Instance.SetTwoColumnPrint("FISCAL RECEIPT QTY", "", "", InvoiceQuantity.ToString());
                        Printer.Instance.SetTwoColumnPrint("TOTAL SALES", "", "", TotalSlaes.ToString("0.00"));
                        Printer.Instance.SetTwoColumnPrint("TOTAL TAX", "", "", TotalTax.ToString("0.00"));
                        Printer.Instance.PrintString(0, 0, 0, 0, 0, "——————————TAX TOTALS—————————\r\n");
                        if (TaxItems != null && TaxItems.Count > 0)
                        {
                            foreach (var item in TaxItems)
                            {
                                Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("{0}{1}{2}\r\n",
                                                                                Printer.Instance.SetLeftPrint(6, item.TaxLabel),
                                                                                Printer.Instance.SetCenterPrint(29, item.TaxName),
                                                                                Printer.Instance.SetRightPrint(12, (item.TaxRate).ToString())));

                                Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("{0}{1}\r\n",
                                                                                Printer.Instance.SetLeftPrint(10, "TAX AMOUNT"),
                                                                                Printer.Instance.SetRightPrint(37, item.TaxAmount.ToString("0.00"))));
                            }
                        }

                        Printer.Instance.CutPaper(1, 5);
                        //}
                    });
                }, () =>
                {
                    return true;
                }));
            }
        }

        #endregion

        #region 方法
        public void NetRequest(string requestString)
        {
            if (requestString == null)
            {
                throw new ArgumentNullException("SignRequest data can not be null.");
            }
            TcpHelper _signTcpClient = new TcpHelper();
            if (string.IsNullOrWhiteSpace(Const.Locator.ParameterSetting.SdcUrl))
            {
                MessageBoxEx.Show("EFD URL can not be null.", MessageBoxButton.OK);
                return;
            }
            string[] sdc = Const.Locator.ParameterSetting.SdcUrl.Split(':');
            if (sdc != null && sdc.Count() != 2)
            {
                MessageBoxEx.Show("EFD URL is not in the right format.", MessageBoxButton.OK);
                return;
            }
            bool isConn = _signTcpClient.Connect(IPAddress.Parse(sdc[0]), int.Parse(sdc[1]));
            if (!isConn)
            {
                MessageBoxEx.Show("Failed to connect to EFD.", MessageBoxButton.OK);
                return;
            }

            _signTcpClient.Complated -= _signTcpClient_Complated;
            _signTcpClient.Complated += _signTcpClient_Complated;
            _signTcpClient.Send(Cmd, requestString);
            _signTcpClient.ReciveAsync();
        }

        public void SerialRequest(string requestString)
        {
            if (string.IsNullOrWhiteSpace(Const.Locator.ParameterSetting.SelectedPort))
            {
                throw new Exception("Port can not be null.");
            }
            if (string.IsNullOrWhiteSpace(Const.Locator.ParameterSetting.SelectedDataBits))
            {
                throw new Exception("DataBits can not be null.");
            }
            if (string.IsNullOrWhiteSpace(Const.Locator.ParameterSetting.SelectedBaudRate))
            {
                throw new Exception("BaudRate can not be null.");
            }
            if (string.IsNullOrWhiteSpace(Const.Locator.ParameterSetting.SelectedParity))
            {
                throw new Exception("Parity can not be null.");
            }
            if (string.IsNullOrWhiteSpace(Const.Locator.ParameterSetting.SelectedStopBits))
            {
                throw new Exception("StopBits can not be null.");
            }

            SerialClient _client = new SerialClient(Const.Locator.ParameterSetting.SelectedPort,
               int.Parse(Const.Locator.ParameterSetting.SelectedBaudRate),
               (Parity)Enum.Parse(typeof(Parity), Const.Locator.ParameterSetting.SelectedParity),
               int.Parse(Const.Locator.ParameterSetting.SelectedDataBits),
               (StopBits)Enum.Parse(typeof(StopBits), Const.Locator.ParameterSetting.SelectedStopBits));
            _client.Open();
            _client.Complated -= _signTcpClient_Complated;
            _client.Complated += _signTcpClient_Complated;
            _client.Send(Cmd, requestString);
        }

        private void _signTcpClient_Complated(object sender, MessageModel e)
        {
            if (e.MessageId == 0x03)
            {
                //返回错误
                ErrorInfo erroInfo = JsonConvert.DeserializeObject<ErrorInfo>(e.Message);
                if (erroInfo != null)
                {
                    MessageBoxEx.Show(erroInfo.Description, MessageBoxButton.OK);
                    return;
                }
            }
            if (e.MessageId != Cmd)
            {
                return;
            }
            try
            {
                ReportResponse response = JsonConvert.DeserializeObject<ReportResponse>(e.Message);
                switch (response.ReportType)
                {
                    case 0:
                        CurrentTime = response.X.CurrentTime;
                        TotalSlaes = response.X.TotalSales;
                        TotalTax = response.X.TotalTax;
                        TaxItems = response.X.TaxItems;
                        InvoiceQuantity = response.X.InvoiceQuantity;
                        break;
                    case 1:
                        CurrentTime = response.Z.CurrentTime;
                        TotalSlaes = response.Z.TotalSales;
                        TotalTax = response.Z.TotalTax;
                        TaxItems = response.Z.TaxItems;
                        InvoiceQuantity = response.Z.InvoiceQuantity;

                        ReportNumber = response.Z.ReportNumber;
                        BeginDate = response.Z.BeginDate;
                        EndDate = response.Z.EndDate;
                        break;
                    case 2:
                        CurrentTime = response.Periodic.CurrentTime;
                        TotalSlaes = response.Periodic.TotalSales;
                        TotalTax = response.Periodic.TotalTax;
                        TaxItems = response.Periodic.TaxItems;
                        InvoiceQuantity = response.Periodic.InvoiceQuantity;

                        BeginDate = response.Periodic.BeginDate;
                        EndDate = response.Periodic.EndDate;
                        break;
                    default:
                        break;
                }



                //CurrentTime = "20180720213030";
                //TotalSlaes = 300;
                //TotalTax = 200;
                //InvoiceQuantity = 2;
                //TaxItems = new List<Model.Service.Statistics.ReportTaxItems>
                //{
                //    new Model.Service.Statistics.ReportTaxItems
                //    {
                //        TaxLable="A",
                //        TaxName="aaaaaaaaa",
                //        TaxRate=0.12,
                //        TaxAmount=1000
                //    },
                //    new Model.Service.Statistics.ReportTaxItems
                //    {
                //        TaxLable="A",
                //        TaxName="aaaaaaaaa",
                //        TaxRate=0.12,
                //        TaxAmount=1000
                //    },
                //    new Model.Service.Statistics.ReportTaxItems
                //    {
                //        TaxLable="A",
                //        TaxName="aaaaaaaaa",
                //        TaxRate=0.12,
                //        TaxAmount=1000
                //    },
                //    new Model.Service.Statistics.ReportTaxItems
                //    {
                //        TaxLable="A",
                //        TaxName="aaaaaaaaa",
                //        TaxRate=0.12,
                //        TaxAmount=1000
                //    },
                //    new Model.Service.Statistics.ReportTaxItems
                //    {
                //        TaxLable="A",
                //        TaxName="aaaaaaaaa",
                //        TaxRate=0.12,
                //        TaxAmount=1000
                //    },
                //    new Model.Service.Statistics.ReportTaxItems
                //    {
                //        TaxLable="B",
                //        TaxName="bbbbbbbb",
                //        TaxRate=0.12,
                //        TaxAmount=500
                //    }
                //};

                //ReportNumber = "111111";
                //BeginDate = "20180715";
                //EndDate = "20180720";
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion
    }
}
