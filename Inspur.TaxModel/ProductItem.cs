using GalaSoft.MvvmLight;
using Inspur.TaxModel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.TaxModel
{
    /// <summary>
    /// 模块编号：实体类
    /// 作用：商品条目实体
    /// 作者：丁纪名
    /// 编写日期：2018-01-22
    /// </summary>
    public class ProductItem : ViewModelBase
    {
        /// <summary>
        /// 条目序列号
        /// </summary>
        public long No { get; set; }
        /// <summary>
        /// 获取或设置税种税目代码
        /// </summary>
        private string _categoryCode;
        /// <summary>
        /// 获取或设置税种税目代码
        /// </summary>
        public string CategroyCode
        {
            get { return _categoryCode; }
            set { Set<string>(ref _categoryCode, value, "CategroyCode"); }
        }
        /// <summary>
        /// 税种税目名称
        /// </summary>
        public string CategroyName { get; set; }
        /// <summary>
        /// 获取或设置商品名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 获取或设置商品名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set<string>(ref _name, value, "Name"); }
        }

        /// <summary>
        /// 获取或设置条形码
        /// </summary>
        private string _barCode;
        /// <summary>
        /// 获取或设置条形码
        /// </summary>
        public string BarCode
        {
            get { return _barCode; }
            set { Set<string>(ref _barCode, value, "BarCode"); }
        }
        /// <summary>
        /// 获取或设置商品数量
        /// </summary>
        private double _count = 1;
        /// <summary>
        /// 获取或设置商品数量
        /// </summary>
        public double Count
        {
            get { return _count; }
            set
            {
                if (value != _count)
                {
                    _count = value;
                    RaisePropertyChanged(() => this.Count);
                    RaisePropertyChanged(() => this.Amount);
                }
            }
        }

        /// <summary>
        /// 获取或设置支付金额
        /// </summary>
        private double _amount;
        /// <summary>
        /// 获取或设置支付金额
        /// </summary>
        public double Amount
        {
            get
            {
                ////含税 
                //if (TaxInclusive == ((int)ProductTaxInclusive.Yes).ToString())
                //{
                //    _amount = Math.Round(Price * Count, 2);
                //}
                //else
                //{
                //    //不含税 价格+税款
                //    if (TaxType.CalculationMode == ((int)CalculationMode.Fixed).ToString())
                //    {
                //        //固定税额
                //        _amount = Math.Round(Price * Count + Count * TaxType.FixTaxAmount, 2);
                //    }
                //    else
                //    {
                //        //税率
                //        _amount = Math.Round(Price * Count * (1 + TaxType.Rate / 100), 2);
                //    }
                //}
                return Math.Round(Price * Count, 2);
            }
            set { Set<double>(ref _amount, value, "Amount"); }
        }
        /// <summary>
        /// 获取或设置商品折扣金额
        /// </summary>
        private string _discount;
        /// <summary>
        /// 获取或设置商品折扣金额
        /// </summary>
        public string Discount
        {
            get { return _discount; }
            set { Set<string>(ref _discount, value, "Discount"); }
        }
        /// <summary>
        /// 获取或设置商品单价
        /// </summary>
        private double _price;
        /// <summary>
        /// 获取或设置商品单价
        /// </summary>
        public double Price
        {
            get { return _price; }
            set
            {
                if (value != _price)
                {
                    _price = value;
                    RaisePropertyChanged(() => this.Price);
                    RaisePropertyChanged(() => this.Amount);
                }
            }
        }
        /// <summary>
        /// 获取或设置商品是否含税
        /// </summary>
        public string TaxInclusive { get; set; }
        /// <summary>
        /// 获取或设置商品的税信息
        /// </summary>
        private TaxType taxType = new TaxType();
        /// <summary>
        /// 获取或设置商品的税信息
        /// </summary>
        public TaxType TaxType
        {
            get { return taxType; }
            set { Set<TaxType>(ref taxType, value, "TaxType"); }
        }

        public void CalculateTax()
        {
            this.RaisePropertyChanged(() => this.Amount);
        }
    }
}
