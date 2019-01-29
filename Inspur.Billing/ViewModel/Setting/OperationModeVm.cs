using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Billing.ViewModel.Setting
{
    public class OperationModeVm : ViewModelBase
    {
        #region 属性
        /// <summary>
        /// 获取或设置
        /// </summary>
        private bool _isNormal = true;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool IsNormal
        {
            get { return _isNormal; }
            set { Set<bool>(ref _isNormal, value, "IsNormal"); }
        }

        /// <summary>
        /// 获取或设置
        /// </summary>
        private bool _isTest;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool IsTest
        {
            get { return _isTest; }
            set { Set<bool>(ref _isTest, value, "IsTest"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private bool _isSeperate;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool IsSeperate
        {
            get { return _isSeperate; }
            set { Set<bool>(ref _isSeperate, value, "IsSeperate"); }
        }

        #endregion
    }
}
