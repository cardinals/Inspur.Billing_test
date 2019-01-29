using DataModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Inspur.Billing.Commom;
using Inspur.TaxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LinqToDB;
using ControlLib.Controls.Dialogs;
using Inspur.Billing.Model.Service.Sign;
using Inspur.Billing.Model.Service.Attention;
using Inspur.Billing.View.Setting;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using CommonLib.Net;
using System.Net;
using System.Windows;
using Inspur.Billing.Model;
using System.IO.Ports;

namespace Inspur.Billing.ViewModel.Issue
{
    public class PrintViewModel : ViewModelBase
    {
        #region 字段
        SignRequest signRequest;
        SignResponse signResponse;
        /// <summary>
        /// 签章请求
        /// </summary>
        public TcpHelper _signTcpClient = new TcpHelper();
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置
        /// </summary>
        private CreditViewModel _credit;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public CreditViewModel Credit
        {
            get { return _credit; }
            set { Set<CreditViewModel>(ref _credit, value, "Credit"); }
        }
        /// <summary>
        /// 获取或设置当前时间
        /// </summary>
        private string _currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        /// <summary>
        /// 获取或设置当前时间
        /// </summary>
        public string CurrentTime
        {
            get { return _currentTime; }
            set { Set<string>(ref _currentTime, value, "CurrentTime"); }
        }
        /// <summary>
        /// 获取或设置实际支付金额
        /// </summary>
        private double _actualPay;
        /// <summary>
        /// 获取或设置实际支付金额
        /// </summary>
        public double ActualPay
        {
            get { return _actualPay; }
            set
            {
                if (value != _actualPay)
                {
                    _actualPay = value;
                    if (Credit != null)
                    {
                        double chang = value - Credit.GrandTotal;
                        if (chang >= 0)
                        {
                            Change = chang;
                        }
                        else
                        {
                            Change = 0;
                        }
                    }
                    RaisePropertyChanged(() => this.ActualPay);
                }
            }
        }

        /// <summary>
        /// 获取或设置找零
        /// </summary>
        private double _change;
        /// <summary>
        /// 获取或设置找零
        /// </summary>
        public double Change
        {
            get { return _change; }
            set { Set<double>(ref _change, value, "Change"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private TaxPayer _taxPayerInfo = new TaxPayer();
        /// <summary>
        /// 获取或设置
        /// </summary>
        public TaxPayer TaxPayerInfo
        {
            get { return _taxPayerInfo; }
            set { Set<TaxPayer>(ref _taxPayerInfo, value, "TaxPayerInfo"); }
        }


        /// <summary>
        /// 获取或设置税款详细列表
        /// </summary>
        private List<InvoiceTax> _taxList;
        /// <summary>
        /// 获取或设置税款详细列表
        /// </summary>
        public List<InvoiceTax> TaxList
        {
            get { return _taxList; }
            set
            {
                if (value != _taxList)
                {
                    _taxList = value;
                    RaisePropertyChanged(() => this.TaxList);
                }
            }
        }
        /// <summary>
        /// 获取或设置税款合计
        /// </summary>
        private double _totalTaxAmount;
        /// <summary>
        /// 获取或设置税款合计
        /// </summary>
        public double TotalTaxAmount
        {
            get { return _totalTaxAmount; }
            set
            {
                if (value != _totalTaxAmount)
                {
                    _totalTaxAmount = value;
                    RaisePropertyChanged(() => this.TotalTaxAmount);
                }
            }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private ImageSource _qrPath;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public ImageSource QrPath
        {
            get { return _qrPath; }
            set { Set<ImageSource>(ref _qrPath, value, "QrPath"); }
        }



        /// <summary>
        /// 获取或设置是否可以签名，true签名打印sdc格式，false打印自定义
        /// </summary>
        private bool _isCanSign;
        /// <summary>
        /// 获取或设置是否可以签名，true签名打印sdc格式，false打印自定义
        /// </summary>
        public bool IsCanSign
        {
            get { return _isCanSign; }
            set { Set<bool>(ref _isCanSign, value, "IsCanSign"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _terminalID;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string TerminalID
        {
            get { return _terminalID; }
            set { Set<string>(ref _terminalID, value, "TerminalID"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _invoiceCode;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string InvoiceCode
        {
            get { return _invoiceCode; }
            set { Set<string>(ref _invoiceCode, value, "InvoiceCode"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _invoiceNumber;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set { Set<string>(ref _invoiceNumber, value, "InvoiceNumber"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _fiscalCode;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string FiscalCode
        {
            get { return _fiscalCode; }
            set { Set<string>(ref _fiscalCode, value, "FiscalCode"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private Visibility _operationModeVis;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public Visibility OperationModeVis
        {
            get { return _operationModeVis; }
            set { Set<Visibility>(ref _operationModeVis, value, "OperationModeVis"); }
        }
        #endregion

        #region 命令

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
                    try
                    {
                        switch (p)
                        {
                            case "Loaded":

                                try
                                {
                                    Sign();
                                }
                                catch
                                {
                                    MessageBoxEx.Show("Can not connect EFD,We will custom print.");
                                    signResponse = null;
                                    GetTaxPayerInfo();
                                }

                                break;
                            case "Unloaded":
                                QrPath = null;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.Show(ex.Message);
                    }
                }, a =>
                {
                    return true;
                }));
            }
        }

        private void SignSuccessData()
        {
            //时间赋值
            CurrentTime = DateTimeHelper.Converter(signResponse.ESDTime, "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss");

            SignSuccessDataInvoice();
            SignSuccessDataTaxItems();
            SignSuccessDataTaxPayerInfo();
            //处理二维码
            SignSuccessDataQr();
        }

        private void SignSuccessDataQr()
        {
            if (!Credit.IsMitQr && !string.IsNullOrWhiteSpace(signResponse.VerificationQRCode))
            {
                byte[] qrArr = Convert.FromBase64String(signResponse.VerificationQRCode);
                using (MemoryStream ms = new MemoryStream(qrArr))
                {
                    Bitmap bitmap = new Bitmap(ms);
                    bitmap.Save(Const.QrPath, System.Drawing.Imaging.ImageFormat.Bmp);

                    //防止图片占用
                    QrPath = InitImage(Const.QrPath);
                }
            }
        }

        private void SignSuccessDataTaxPayerInfo()
        {
            TaxPayerInfo.Tin = signResponse.TPIN;
            TaxPayerInfo.Name = signResponse.TaxpayerName;
            TaxPayerInfo.Address = signResponse.Addreess;
        }

        private void SignSuccessDataTaxItems()
        {
            if (signResponse.TaxItems != null)
            {
                List<InvoiceTax> items = new List<InvoiceTax>();
                InvoiceTax invoiceTax = null;
                foreach (var taxItem in signResponse.TaxItems)
                {
                    invoiceTax = new InvoiceTax
                    {
                        TaxItemCode = taxItem.TaxLabel,
                        TaxItemDesc = taxItem.CategoryName,
                        TaxRate = taxItem.Rate,
                        TaxAmount = taxItem.TaxAmount,
                    };
                    items.Add(invoiceTax);
                }
                TaxList = items;
                if (TaxList != null && TaxList.Count > 0)
                {
                    TotalTaxAmount = Math.Round(TaxList.Sum(a => a.TaxAmount), 2);
                };
            }
        }
        private void SignSuccessDataInvoice()
        {
            TerminalID = signResponse.TerminalID;
            FiscalCode = signResponse.FiscalCode;

            InvoiceCode = signResponse.InvoiceCode;
            InvoiceNumber = signResponse.InvoiceNumber;
        }

        private void GetTaxPayerInfo()
        {
            var taxPayer = (from a in Const.dB.TaxpayerJnfo
                            select a).FirstOrDefault();
            if (taxPayer != null)
            {
                TaxPayerInfo = EntityAdapter.TaxpayerJnfo2TaxPayer(taxPayer);
            }
        }

        /// <summary>
        /// 签名失败处理数据
        /// </summary>
        private void SignFilureData()
        {
            //签名失败，客户端处理税款明细
            if (Credit != null && Credit.Productes != null)
            {
                TaxList = Credit.Productes.GroupBy(a => a.TaxType.Id).Select(g => new InvoiceTax
                {
                    TaxItemCode = g.First().TaxType.Label,
                    TaxItemDesc = g.First().TaxType.Name,
                    TaxRate = g.First().TaxType.Rate,

                    TaxAmount = g.Sum(b =>
                    {
                        return TaxCalculation.Calculation(b.TaxInclusive, b.TaxType.CalculationMode, b.Price, b.Count, b.TaxType.Rate, b.TaxType.FixTaxAmount);
                    })
                }).ToList();
                if (TaxList != null && TaxList.Count > 0)
                {
                    TotalTaxAmount = Math.Round(TaxList.Sum(a => a.TaxAmount), 2);
                }
            }
        }

        /// <summary>
        /// 解决不同进程读取同一张图片的问题
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private BitmapImage InitImage(string filePath)
        {
            BitmapImage bitmapImage;
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                FileInfo fi = new FileInfo(filePath);
                byte[] bytes = reader.ReadBytes((int)fi.Length);
                reader.Close();

                //image = new Image();
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(bytes);
                bitmapImage.EndInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                //image.Source = bitmapImage;
                reader.Dispose();
            }
            return bitmapImage;
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
                    if (Credit == null)
                    {
                        return;
                    }
                    try
                    {
                        //打印
                        Print();
                        Const.Locator.Main.Message = "Print Success";
                        //保存
                        Save(signRequest, signResponse);
                        //关闭校验窗口
                        Messenger.Default.Send<string>(null, "ClosePrintView");
                        //清空数据
                        ActualPay = 0;
                        InvoiceCode = "";
                        InvoiceNumber = "";
                        FiscalCode = "";
                        TerminalID = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.Show(ex.Message);
                    }
                }, () =>
               {
                   return true;
               }));
            }
        }
        private void Sign()
        {
            signRequest = new SignRequest
            {
                POSVendor = "Inspur",
                PosSerialNumber = Config.PosSerialNumber,
                IssueTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                SaleType = 0,
                LocalPurchaseOrder = "1000582782",
                Cashier = Credit.Cashier,
                BuyerTPIN = string.IsNullOrWhiteSpace(Credit.Buyer.Tin) ? "" : Credit.Buyer.Tin,
                BuyerName = string.IsNullOrWhiteSpace(Credit.Buyer.Name) ? "" : Credit.Buyer.Name,
                BuyerTaxAccountName = "",
                BuyerAddress = string.IsNullOrWhiteSpace(Credit.Buyer.Address) ? "" : Credit.Buyer.Address,
                BuyerTel = string.IsNullOrWhiteSpace(Credit.Buyer.TelPhone) ? "" : Credit.Buyer.TelPhone,
                OriginalInvoiceCode = "",
                OriginalInvoiceNumber = ""
            };

            int transactionCode, paymentCode;
            if (int.TryParse(Credit.SelectedTransactionType.Code, out transactionCode))
            {
                signRequest.TransactionType = transactionCode;
            }
            else
            {
                MessageBoxEx.Show("TransactionType is not number.");
                return;
            }
            if (int.TryParse(Credit.SelectedPaymentType.Code, out paymentCode))
            {
                signRequest.PaymentMode = paymentCode;
            }
            else
            {
                MessageBoxEx.Show("PaymentMode is not number.");
                return;
            }



            if (!Const.Locator.OperationModeVm.IsNormal)
            {
                OperationModeVis = Visibility.Visible;
                if (Const.Locator.OperationModeVm.IsTest)
                {
                    signRequest.OperationMode = 1;
                }
                if (Const.Locator.OperationModeVm.IsSeperate)
                {
                    signRequest.OperationMode = 2;
                }
            }
            else
            {
                OperationModeVis = Visibility.Collapsed;
                signRequest.OperationMode = 0;
            }


            signRequest.Items = new List<SignGoodItem>();
            SignGoodItem signGoodItem;
            int id = 1;
            foreach (var item in Credit.Productes)
            {
                signGoodItem = new SignGoodItem();
                signGoodItem.GTIN = id;
                signGoodItem.BarCode = item.BarCode;
                id++;
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    MessageBoxEx.Show("Description is required.");
                    return;
                }
                signGoodItem.Name = item.Name;
                signGoodItem.Quantity = item.Count;
                signGoodItem.UnitPrice = item.Price;
                signGoodItem.TotalAmount = item.Amount;
                signGoodItem.IsTaxInclusive = true;
                signGoodItem.Labels = new string[1] { item.TaxType.Label };
                signRequest.Items.Add(signGoodItem);
            }
            switch (Const.Locator.ParameterSetting.CommModel)
            {
                case CommModel.NetPort:
                    SignRequest(signRequest);
                    break;
                case CommModel.SerialPort:
                    SignRequestSerial(signRequest);
                    break;
                default:
                    break;
            }

        }

        private void _signTcpClient_Complated(object sender, MessageModel e)
        {
            try
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
                if (e.MessageId != 0x02)
                {
                    return;
                }
                signResponse = JsonConvert.DeserializeObject<SignResponse>(e.Message);
                if (signResponse != null)
                {
                    //处理税款数据
                    SignSuccessData();
                }
                else
                {
                    SignFilureData();
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        public void SignRequest(SignRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("SignRequest data can not be null.");
            }
            _signTcpClient = new TcpHelper();
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
            string requestString = JsonConvert.SerializeObject(request);
            _signTcpClient.Complated -= _signTcpClient_Complated;
            _signTcpClient.Complated += _signTcpClient_Complated;
            _signTcpClient.Send(0x02, requestString);
            _signTcpClient.ReciveAsync();
        }

        public void SignRequestSerial(SignRequest request)
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
            string requestString = JsonConvert.SerializeObject(request);

            SerialClient _client = new SerialClient(Const.Locator.ParameterSetting.SelectedPort,
               int.Parse(Const.Locator.ParameterSetting.SelectedBaudRate),
               (Parity)Enum.Parse(typeof(Parity), Const.Locator.ParameterSetting.SelectedParity),
               int.Parse(Const.Locator.ParameterSetting.SelectedDataBits),
               (StopBits)Enum.Parse(typeof(StopBits), Const.Locator.ParameterSetting.SelectedStopBits));
            _client.Open();
            _client.Complated -= _signTcpClient_Complated;
            _client.Complated += _signTcpClient_Complated;
            _client.Send(0x02, requestString);
        }

        private void Save(SignRequest request, SignResponse signResponse)
        {
            //保存销售订单主表
            InvoiceAbbreviation invoiceAbbreviation = new InvoiceAbbreviation
            {
                CashierId = Const.CashierId,
                TaxpayerTin = TaxPayerInfo.Tin,
                TotalTaxAmount = TotalTaxAmount,
                TenderAmount = ActualPay,
                Change = Change,
                QrcodePath = "",
            };
            if (Credit != null)
            {
                invoiceAbbreviation.SalesorderNum = Credit.OrderNumber;
                if (Credit.SelectedPaymentType != null)
                {
                    invoiceAbbreviation.PaymentType = Credit.SelectedPaymentType.Code;
                }
                invoiceAbbreviation.BuyerTin = Credit.Buyer.Tin;
                invoiceAbbreviation.BuyerName = Credit.Buyer.Name;
                invoiceAbbreviation.TotalAmount = Credit.GrandTotal;
            }
            //if (request != null)
            //{
            //    invoiceAbbreviation.HashCode = request.Hash;
            //}
            ///sdc取值
            if (signResponse != null)
            {
                invoiceAbbreviation.TaxpayerName = signResponse.TaxpayerName;//sdc取值
                //invoiceAbbreviation.TaxpayerLocation = signResponse.LocationName;//sdc取值
                invoiceAbbreviation.TaxpayerAddress = signResponse.Addreess;//sdc取值
                //invoiceAbbreviation.TaxpayerDistrit = signResponse.District;//sdc取值
                invoiceAbbreviation.InvoiceNumber = signResponse.InvoiceNumber;//sdc取值
                invoiceAbbreviation.IssueDate = request.IssueTime;//sdc取值
                invoiceAbbreviation.VerificationUrl = signResponse.VerificationUrl;
            }
            Const.dB.Insert<InvoiceAbbreviation>(invoiceAbbreviation);
            //保存订单销售子表
            InvoiceItems invoiceItem = null;
            if (Credit != null && Credit.Productes != null)
            {
                foreach (var item in Credit.Productes)
                {
                    invoiceItem = new InvoiceItems();
                    invoiceItem.Sn = null;

                    invoiceItem.TaxtypeId = item.TaxType.Id;
                    invoiceItem.SalesorderNum = Credit.OrderNumber;
                    invoiceItem.GoodsId = item.No;
                    invoiceItem.GoodsGin = item.BarCode;
                    invoiceItem.GoodsDesc = item.Name;
                    invoiceItem.GoodsPrice = item.Price;
                    invoiceItem.GoodsQty = item.Count;
                    invoiceItem.TotalAmount = item.Price * item.Count;

                    invoiceItem.TaxtypeId = item.TaxType.Id;
                    invoiceItem.TaxItem = item.TaxType.Name;
                    invoiceItem.TaxRate = item.TaxType.Rate;
                    invoiceItem.TaxAmount = TaxCalculation.Calculation(item.TaxInclusive, item.TaxType.CalculationMode, item.Price, item.Count, item.TaxType.Rate, item.TaxType.FixTaxAmount);

                    Const.dB.Insert<InvoiceItems>(invoiceItem);
                }
            }
        }

        private void Print()
        {
            //_isHasPrint = true;
            Printer.Instance.Print(() =>
            {
                //if (signResponse != null && string.IsNullOrWhiteSpace(signResponse.Message) && !string.IsNullOrWhiteSpace(signResponse.Journal))
                //{
                //    //打印返回的表
                //    Printer.Instance.SetAlign(1);
                //    Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("{0}\r\n", signResponse.Journal));

                //    Printer.Instance.Reset();
                //    Printer.Instance.SetAlign(1);
                //    if (!Credit.IsMitQr && !string.IsNullOrWhiteSpace(signResponse.VerificationUrl))
                //    {
                //        Printer.Instance.PrintTwoDimensionalBarcodeA(signResponse.VerificationUrl);
                //    }
                //    Printer.Instance.CutPaper(1, 5);
                //}
                //else
                //{
                //打印自定义的表样
                Printer.Instance.SetAlign(1);
                Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("Order Number:{0}\r\n{1}\r\n", Credit.OrderNumber, CurrentTime));

                Printer.Instance.SetAlign(0);
                Printer.Instance.SetTwoColumnPrint("POSID", Credit.PosNumber, "Cashier:", Credit.Cashier);
                Printer.Instance.SetTwoColumnPrint("Buyer TIN", "", "", Credit.Buyer.Tin);
                Printer.Instance.SetTwoColumnPrint("Buyer Name", "", "", Credit.Buyer.Name);
                Printer.Instance.SetTwoColumnPrint("Buyer Address", "", "", Credit.Buyer.Address);
                Printer.Instance.SetTwoColumnPrint("Buyer Tel", "", "", Credit.Buyer.TelPhone);
                Printer.Instance.SetTwoColumnPrint("Invoice Code", "", "", InvoiceCode);
                Printer.Instance.SetTwoColumnPrint("Invoice Number", "", "", InvoiceNumber);
                Printer.Instance.PrintLine();

                Printer.Instance.SetAlign(1);
                Printer.Instance.PrintString(0, 1, 0, 0, 0, "Particular Of Items\r\n");
                //表格字符占用按照7 8 5 3 9来打印
                //表格字符占用按照11 11 5 3 9来打印
                Printer.Instance.SetAlign(0);
                Printer.Instance.PrintString(0, 1, 0, 0, 0, "Name           Price          Qty.       Amount\r\n");
                if (Credit != null && Credit.Productes != null)
                {
                    foreach (var item in Credit.Productes)
                    {
                        Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("{0}{1}{2}{3}\r\n",
                            Printer.Instance.SetLeftPrint(15, string.Format("{0} ({1})", item.Name, item.TaxType.Label.ToString())),
                            Printer.Instance.SetLeftPrint(12, item.Price.ToString("0.00")),
                            Printer.Instance.SetCenterPrint(8, item.Count.ToString()),
                            Printer.Instance.SetRightPrint(12, item.Amount.ToString("0.00"))));
                    }
                }
                //SetTwoColumnPrint("Total Value", "", "", Credit.GrandTotal.ToString("0.00"));
                Printer.Instance.PrintLine();

                Printer.Instance.SetAlign(1);
                Printer.Instance.PrintString(0, 1, 0, 0, 0, "Tax Amount\r\n");
                //表格字符占用按照8 8 8 8来打印
                Printer.Instance.SetAlign(0);
                Printer.Instance.PrintString(0, 1, 0, 0, 0, "Label       Name        Rate(%)      Tax Amount\r\n");
                if (TaxList != null)
                {
                    foreach (var item in TaxList)
                    {
                        Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("{0}{1}{2}{3}\r\n",
                            Printer.Instance.SetLeftPrint(6, item.TaxItemCode),
                            Printer.Instance.SetCenterPrint(18, item.TaxItemDesc),
                            Printer.Instance.SetCenterPrint(11, (item.TaxRate).ToString()),
                            Printer.Instance.SetRightPrint(12, item.TaxAmount.ToString("0.00"))));
                    }
                }
                Printer.Instance.SetTwoColumnPrint("Total Tax", "", "", TotalTaxAmount.ToString("0.00"));
                Printer.Instance.PrintLine();

                Printer.Instance.SetTwoColumnPrint("Total Amount", "", "", Credit.GrandTotal.ToString("0.00"));
                Printer.Instance.SetTwoColumnPrint("Payment Mode", "", "", Credit == null ? "" : (Credit.SelectedPaymentType == null ? "" : Credit.SelectedPaymentType.Name));
                Printer.Instance.SetTwoColumnPrint("Actual Payment", "", "", ActualPay.ToString("0.00"));
                Printer.Instance.SetTwoColumnPrint("Change", "", "", Change.ToString("0.00"));
                Printer.Instance.PrintLine();

                Printer.Instance.SetTwoColumnPrint("TPIN", "", "", TaxPayerInfo.Tin);
                Printer.Instance.SetTwoColumnPrint("Name", "", "", TaxPayerInfo.Name);
                Printer.Instance.SetTwoColumnPrint("Address", "", "", TaxPayerInfo.Address);
                Printer.Instance.SetTwoColumnPrint("Tel", "", "", TaxPayerInfo.Telphone);
                Printer.Instance.SetTwoColumnPrint("Terminal ID", "", "", TerminalID);
                Printer.Instance.SetTwoColumnPrint("Fiscal Code", "", "", FiscalCode);
                Printer.Instance.PrintLine();

                if (!Credit.IsMitQr && signResponse != null && !string.IsNullOrWhiteSpace(signResponse.VerificationUrl))
                {
                    Printer.Instance.SetAlign(1);
                    Printer.Instance.PrintTwoDimensionalBarcodeA(signResponse.VerificationUrl);
                }
                Printer.Instance.SetAlign(0);
                Printer.Instance.PrintString(0, 0, 0, 0, 0, "Dear sir madam,please keep the invoice properly so as to refunds & replaces \r\n\r\n");
                Printer.Instance.SetAlign(1);
                Printer.Instance.PrintString(0, 0, 0, 0, 0, "Thank You & Please Come Again \r\n");


                if (!Const.Locator.OperationModeVm.IsNormal)
                {
                    Printer.Instance.PrintString(0, 0, 0, 0, 0, "\r\n");
                    Printer.Instance.PrintString(0, 0, 0, 0, 0, "This is a testing invoice \r\n");
                }

                Printer.Instance.CutPaper(1, 5);
                //}
            });
        }
        #endregion
    }
}
