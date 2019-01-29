using ControlLib.Controls.Dialogs;
using DataModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Inspur.Billing.Commom;
using Inspur.Billing.Model;
using Inspur.Billing.Model.Service.Attention;
using Inspur.Billing.Model.Service.Sign;
using Inspur.Billing.View.Issue;
using Inspur.Billing.View.Setting;
using Inspur.TaxModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Inspur.Billing.ViewModel.Issue
{
    public class CreditViewModel : ViewModelBase
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        Logger _logger = LogManager.GetCurrentClassLogger();
        public CreditViewModel()
        {
            if (Const.Locator != null)
            {
                Cashier = Const.Locator.Login.UserName;
            }
            if (string.IsNullOrWhiteSpace(Printer.Instance.PrintPort))
            {
                Printer.Instance.PrintPort = Const.Locator.PrintSettingVm.PrintPort;
            }
        }

        #region 属性
        /// <summary>
        /// 获取或设置开票编号
        /// </summary>
        private string _orderNumber;
        /// <summary>
        /// 获取或设置开票编号
        /// </summary>
        public string OrderNumber
        {
            get { return _orderNumber; }
            set { Set<string>(ref _orderNumber, value, "OrderNumber"); }
        }
        /// <summary>
        /// 获取或设置买方信息
        /// </summary>
        private Buyer _buyer = new Buyer();
        /// <summary>
        /// 获取或设置买方信息
        /// </summary>
        public Buyer Buyer
        {
            get { return _buyer; }
            set { Set<Buyer>(ref _buyer, value, "Buyer"); }
        }

        /// <summary>
        /// 获取或设置交易类型
        /// </summary>
        private List<CodeTable> _transactionType = new List<CodeTable>
        {
            new CodeTable{ Name="Normal",Code="0"},
            new CodeTable{ Name="Credit Note",Code="1"},
            new CodeTable{ Name="Debit Note",Code="2"}
        };
        /// <summary>
        /// 获取或设置交易类型
        /// </summary>
        public List<CodeTable> TransactionType
        {
            get { return _transactionType; }
            set { Set<List<CodeTable>>(ref _transactionType, value, "TransactionType"); }
        }
        /// <summary>
        /// 获取或设置支付类型
        /// </summary>
        private List<CodeTable> _paymentType = new List<CodeTable>
        {
            new CodeTable{ Name="Cash",Code="0"},
            new CodeTable{ Name="Card",Code="1"},
            new CodeTable{ Name="Cheque",Code="2"},
            new CodeTable{ Name="Electronic Transfer",Code="3"},
            //new CodeTable{ Name="Wiretransfer",Code="4"},
            //new CodeTable{ Name="Voucher",Code="5"},
            //new CodeTable{ Name="MobileMoney",Code="6"}
        };
        /// <summary>
        /// 获取或设置支付类型
        /// </summary>
        public List<CodeTable> PaymentType
        {
            get { return _paymentType; }
            set { Set<List<CodeTable>>(ref _paymentType, value, "PaymentType"); }
        }

        /// <summary>
        /// 获取或设置选中的支付方式
        /// </summary>
        private CodeTable _selectedPaymentType;
        /// <summary>
        /// 获取或设置选中的支付方式
        /// </summary>
        public CodeTable SelectedPaymentType
        {
            get { return _selectedPaymentType; }
            set { Set<CodeTable>(ref _selectedPaymentType, value, "SelectedPaymentType"); }
        }

        /// <summary>
        /// 获取或设置选中的支付方式
        /// </summary>
        private CodeTable _selectedTransactionType;
        /// <summary>
        /// 获取或设置选中的支付方式
        /// </summary>
        public CodeTable SelectedTransactionType
        {
            get { return _selectedPaymentType; }
            set { Set<CodeTable>(ref _selectedPaymentType, value, "SelectedTransactionType"); }
        }
        /// <summary>
        /// 获取或设置商品集合
        /// </summary>
        private ObservableCollection<ProductItem> _productes = new ObservableCollection<ProductItem>();
        /// <summary>
        /// 获取或设置商品集合
        /// </summary>
        public ObservableCollection<ProductItem> Productes
        {
            get { return _productes; }
            set { Set<ObservableCollection<ProductItem>>(ref _productes, value, "Productes"); }
        }
        /// <summary>
        /// 获取或设置选中的商品
        /// </summary>
        private ProductItem _selectedItem;
        /// <summary>
        /// 获取或设置选中的商品
        /// </summary>
        public ProductItem SelectedItem
        {
            get { return _selectedItem; }
            set { Set<ProductItem>(ref _selectedItem, value, "SelectedItem"); }
        }

        /// <summary>
        /// 获取或设置
        /// </summary>
        private double _grandTotal;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public double GrandTotal
        {
            get { return _grandTotal; }
            set { Set<double>(ref _grandTotal, value, "GrandTotal"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _posNumber = "868491150136";
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string PosNumber
        {
            get { return _posNumber; }
            set { Set<string>(ref _posNumber, value, "PosNumber"); }
        }
        /// <summary>
        /// 获取或设置收银员姓名
        /// </summary>
        private string _cashier;
        /// <summary>
        /// 获取或设置收银员姓名
        /// </summary>
        public string Cashier
        {
            get { return _cashier; }
            set { Set<string>(ref _cashier, value, "Casher"); }
        }
        /// <summary>
        /// 获取或设置数据库中的商品
        /// </summary>
        private List<GoodsInfo> _goods;
        /// <summary>
        /// 获取或设置数据库中的商品
        /// </summary>
        public List<GoodsInfo> Goods
        {
            get { return _goods; }
            set { Set<List<GoodsInfo>>(ref _goods, value, "Goods"); }
        }
        /// <summary>
        /// 获取或设置税率集合
        /// </summary>
        private List<CodeTaxtype> _taxRates;
        /// <summary>
        /// 获取或设置税率集合
        /// </summary>
        public List<CodeTaxtype> TaxRates
        {
            get { return _taxRates; }
            set { Set<List<CodeTaxtype>>(ref _taxRates, value, "TaxRates"); }
        }
        /// <summary>
        /// 商品和税种表对应关系
        /// </summary>
        public List<GoodsTaxtype> GoodTaxType { get; set; }


        /// <summary>
        /// 获取或设置
        /// </summary>
        private bool _isMitQr;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool IsMitQr
        {
            get { return _isMitQr; }
            set { Set<bool>(ref _isMitQr, value, "IsMitQr"); }
        }

        /// <summary>
        /// 获取或设置
        /// </summary>
        private bool _isMitTexTual;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool IsMitTexTual
        {
            get { return _isMitTexTual; }
            set { Set<bool>(ref _isMitTexTual, value, "IsMitTexTual"); }
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
        /// <summary>
        /// 获取或设置
        /// </summary>
        private Visibility _maskVisibility = Visibility.Collapsed;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public Visibility MaskVisibility
        {
            get { return _maskVisibility; }
            set { Set<Visibility>(ref _maskVisibility, value, "MaskVisibility"); }
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
                                //Const.Locator.Main.IsBusy = true;

                                if (!Const.Locator.Main.IsOnline && Const.Locator.OperationModeVm.IsTest)
                                {
                                    MaskVisibility = Visibility.Visible;
                                    MessageBoxEx.Show("Has been disconnected from SDC, POS can no longer work in test mode, please adjust to normal mode and try again.");
                                    return;
                                }
                                MaskVisibility = Visibility.Collapsed;

                                if (!Const.Locator.OperationModeVm.IsNormal)
                                {
                                    OperationModeVis = Visibility.Visible;
                                }
                                else
                                {
                                    OperationModeVis = Visibility.Collapsed;
                                }
                                if (!Const.IsHasGetStatus)
                                {
                                    Const.IsNeedMessage = false;
                                    ServiceHelper.CheckStatue();
                                }
                                //Const.Locator.Main.IsBusy = false;
                                LoadGoods();
                                LoadTaxRate();
                                LoadGoodTaxType();
                                GetOrderNumber();
                                break;
                            case "OrderNumberCopy":
                                Clipboard.SetText(OrderNumber.ToString());
                                break;
                            case "Print":
                                if (Productes == null || Productes.Count == 0)
                                {
                                    throw new Exception("Please add the goods sold.");
                                }
                                foreach (var productItem in Productes)
                                {
                                    if (string.IsNullOrWhiteSpace(productItem.BarCode) || productItem.BarCode.Length < 8 || productItem.BarCode.Length > 14)
                                    {
                                        throw new Exception("GTIN can not be null,and the length must between 8 and 14.");
                                    }
                                    if (string.IsNullOrWhiteSpace(productItem.Name))
                                    {
                                        throw new Exception("Good name can not be null.");
                                    }
                                    if (productItem.TaxType == null || string.IsNullOrWhiteSpace(productItem.TaxType.Label))
                                    {
                                        throw new Exception("Please select the good rate.");
                                    }
                                    if (productItem.Price <= 0)
                                    {
                                        throw new Exception("Price must be more than 0.");
                                    }
                                    if (productItem.Count <= 0)
                                    {
                                        throw new Exception("Count must be more than 0.");
                                    }
                                }
                                PrintView printView = new PrintView();
                                Const.Locator.Print.Credit = this;
                                if (printView.ShowDialog() == true)
                                {
                                    //刷新orderNum,
                                    GetOrderNumber();
                                    if (Productes == null)
                                    {
                                        Productes = new ObservableCollection<ProductItem>();
                                    }
                                    else
                                    {
                                        Productes.Clear();
                                    }
                                    GrandTotal = 0;
                                    Buyer = new Buyer();
                                    IsMitQr = false;
                                    IsMitTexTual = false;
                                    //SelectedPaymentType = PaymentType.FirstOrDefault(a => a.Code == "1");
                                }
                                break;
                            case "BuyerTinLostFocus":
                                LoadBuyerInfo();
                                break;
                            case "ProductAdd":
                                ProductItem item = new ProductItem();
                                item.PropertyChanged += Item_PropertyChanged;
                                Productes.Add(item);
                                break;
                            case "ProductDelete":
                                if (_selectedItem == null)
                                {
                                    MessageBoxEx.Show("Please choose to delete entries.");
                                }
                                else
                                {
                                    Productes.Remove(SelectedItem);
                                }
                                break;
                            case "ProductCopy":
                                if (SelectedItem != null)
                                {
                                    Clipboard.SetText(string.Format("{0} {1} {2} {3} {4} {5}",
                                        SelectedItem.BarCode,
                                        SelectedItem.Name,
                                        SelectedItem.Price,
                                        SelectedItem.Count,
                                        SelectedItem.TaxType == null ? "" : SelectedItem.TaxType.Rate.ToString(),
                                        SelectedItem.Amount));
                                }
                                break;
                            case "ProductSelectionChanged":
                                if (SelectedItem != null)
                                {
                                    if (Goods == null || SelectedItem == null)
                                    {
                                        return;
                                    }
                                    GoodsInfo goodsInfo = Goods.FirstOrDefault(a => a.Barcode == SelectedItem.BarCode);
                                    if (goodsInfo != null)
                                    {
                                        EntityAdapter.GoodsInfo2ProductItem(goodsInfo, SelectedItem);
                                    }
                                    if (GoodTaxType != null)
                                    {
                                        long taxId = (from a in GoodTaxType
                                                      where a.GoodsId == SelectedItem.No
                                                      select a.TaxtypeId).FirstOrDefault();
                                        if (TaxRates == null)
                                        {
                                            return;
                                        }
                                        CodeTaxtype codeTaxtype = TaxRates.FirstOrDefault(a => (a.TaxtypeId != null && a.TaxtypeId.Value == taxId));
                                        if (codeTaxtype != null)
                                        {
                                            EntityAdapter.CodeTaxtype2TaxType(codeTaxtype, SelectedItem.TaxType);
                                        }
                                    }
                                }
                                break;
                            case "TaxRateSelectionChanged":
                                if (SelectedItem == null || SelectedItem.TaxType == null)
                                {
                                    return;
                                }
                                CodeTaxtype codeTax = TaxRates.FirstOrDefault(a => (a.TaxtypeId != null && a.TaxtypeId.Value == SelectedItem.TaxType.Id));
                                if (codeTax != null)
                                {
                                    EntityAdapter.CodeTaxtype2TaxType(codeTax, SelectedItem.TaxType);
                                    SelectedItem.CalculateTax();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message + ex.StackTrace);
                        MessageBoxEx.Show(ex.Message);
                    }
                }, a =>
                {
                    return true;
                }));
            }
        }
        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount")
            {
                if (Goods != null)
                {
                    GrandTotal = (from a in Productes
                                  select a.Amount).Sum();
                }
            }
        }

        #endregion

        #region 方法

        private void LoadBuyerInfo()
        {
            if (!string.IsNullOrWhiteSpace(_buyer.Tin))
            {
                var buyers = (from a in Const.dB.BuyerInfo
                              where a.BuyerTin == _buyer.Tin
                              select a).ToList();
                if (buyers != null && buyers.Count > 0)
                {
                    EntityAdapter.BuyerInfo2Buyer(buyers[0], Buyer);
                }
            }
        }
        /// <summary>
        /// 加载商品
        /// </summary>
        private void LoadGoods()
        {
            Goods = (from a in Const.dB.GoodsInfo
                     select a).ToList();

        }
        /// <summary>
        /// 加载税率
        /// </summary>
        private void LoadTaxRate()
        {
            TaxRates = (from a in Const.dB.CodeTaxtype
                        select a).ToList();
        }
        private void LoadGoodTaxType()
        {
            GoodTaxType = (from a in Const.dB.GoodsTaxtype
                           select a).ToList();
        }
        /// <summary>
        /// 获取订单编号
        /// </summary>
        private void GetOrderNumber()
        {
            var orders = from a in Const.dB.InvoiceAbbreviation
                         select a.SalesorderNum;
            if (orders != null && orders.Count() > 0)
            {
                double currentOrder = Convert.ToDouble(orders.Max()) + 1;
                OrderNumber = string.Format("{0:0000000000}", currentOrder);
            }
            else
            {
                OrderNumber = string.Format("{0:0000000000}", 1);
            }
        }


        #endregion
    }
}
