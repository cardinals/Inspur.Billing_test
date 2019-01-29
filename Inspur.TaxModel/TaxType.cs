using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.TaxModel
{
    public class TaxType : ViewModelBase
    {
        /// <summary>
        /// 获取或设置
        /// </summary>
        private long _id;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { Set<long>(ref _id, value, "Id"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private double _rate;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public double Rate
        {
            get { return _rate; }
            set { Set<double>(ref _rate, value, "Rate"); }
        }
        /// <summary>
        /// 获取或设置是否含税
        /// </summary>
        public string TaxInclusive { get; set; }
        /// <summary>
        /// 固定税额
        /// </summary>
        public double FixTaxAmount { get; set; }
        /// <summary>
        /// 计税方式 0-税率，1-固定税额
        /// </summary>
        public string CalculationMode { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 税种名称
        /// </summary>
        public string Name { get; set; }
    }
}
