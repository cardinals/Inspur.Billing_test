using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.TaxModel
{
    /// <summary>
    /// 模块编号：实体类    
    /// 作用：买方信息
    /// 作者：丁纪名
    /// 编写日期：2018-2-10
    /// </summary>
    public class Buyer:ViewModelBase
    {
        public string Id { get; set; }
        /// <summary>
        /// 获取或设置纳税人识别号
        /// </summary>
        private string _tin;
        /// <summary>
        /// 获取或设置纳税人识别号
        /// </summary>
        public string Tin
        {
            get { return _tin; }
            set { Set<string>(ref _tin, value, "Tin"); }
        }
        /// <summary>
        /// 获取或设置姓名
        /// </summary>
        private string _name;
        /// <summary>
        /// 获取或设置姓名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set<string>(ref _name, value, "Name"); }
        }
        /// <summary>
        /// 获取或设置地址
        /// </summary>
        private string _address;
        /// <summary>
        /// 获取或设置地址
        /// </summary>
        public string Address
        {
            get { return _address; }
            set { Set<string>(ref _address, value, "Address"); }
        }
        /// <summary>
        /// 获取或设置电话号码
        /// </summary>
        private string _telphone;
        /// <summary>
        /// 获取或设置电话号码
        /// </summary>
        public string TelPhone
        {
            get { return _telphone; }
            set { Set<string>(ref _telphone, value, "TelPhone"); }
        }
    }
}
