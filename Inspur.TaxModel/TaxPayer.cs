using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.TaxModel
{
    /// <summary>
    /// 模块编号：实体类
    /// 作用：纳税人实体
    /// 作者：丁纪名
    /// 编写日期：2018-01-22
    /// </summary>
    [Table(Name = "taxpayer_jnfo")]
    public class TaxPayer : ViewModelBase
    {
        /// <summary>
        /// 获取或设置商户id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 获取或设置商户纳税人识别号
        /// </summary>
        private string _tin;
        /// <summary>
        /// 获取或设置商户纳税人识别号
        /// </summary>
        [Column(Name = "taxpayer_tin")]
        public string Tin
        {
            get { return _tin; }
            set { Set<string>(ref _tin, value, "Tin"); }
        }

        /// <summary>
        /// 获取或设置纳税人名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 获取或设置纳税人名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set<string>(ref _name, value, "Name"); }
        }
        /// <summary>
        /// 获取或设置纳税人bpn
        /// </summary>
        private string _bpn;
        /// <summary>
        /// 获取或设置纳税人bpn
        /// </summary>
        public string Bpn
        {
            get { return _bpn; }
            set { Set<string>(ref _bpn, value, "Bpn"); }
        }
        /// <summary>
        /// 获取或设置纳税人vat
        /// </summary>
        private string _vat;
        /// <summary>
        /// 获取或设置纳税人vat
        /// </summary>
        public string Vat
        {
            get { return _vat; }
            set { Set<string>(ref _vat, value, "Vat"); }
        }
        /// <summary>
        /// 获取或设置纳税人地址
        /// </summary>
        private string _address;
        /// <summary>
        /// 获取或设置纳税人地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { Set<string>(ref _address, value, "Address"); }
        }
        /// <summary>
        /// 获取或设置纳税人电话
        /// </summary>
        private string _telphone;
        /// <summary>
        /// 获取或设置纳税人电话
        /// </summary>
        public string Telphone
        {
            get { return _telphone; }
            set { Set<string>(ref _telphone, value, "Telphone"); }
        }
        /// <summary>
        /// 获取或设置纳税人银行账号
        /// </summary>
        private string _bankAccount;
        /// <summary>
        /// 获取或设置纳税人银行账号
        /// </summary>
        public string BankAccount
        {
            get { return _bankAccount; }
            set { Set<string>(ref _bankAccount, value, "BankAccount"); }
        }

    }
}
