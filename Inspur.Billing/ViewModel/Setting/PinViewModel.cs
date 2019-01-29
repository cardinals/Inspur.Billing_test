using ControlLib.Controls.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Inspur.Billing.Commom;
using Inspur.Billing.Model.Service.Pin;
using JumpKick.HttpLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Inspur.Billing.ViewModel.Setting
{
    public class PinViewModel : ViewModelBase
    {
        #region 属性
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _pin = "6666";
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Pin
        {
            get { return _pin; }
            set
            {
                if (value != _pin)
                {
                    _pin = value;
                    RaisePropertyChanged(() => this.Pin);
                }
            }
        }
        #endregion


        #region 命令
        /// <summary>
        /// 获取或设置
        /// </summary>
        private ICommand _confirmCommand;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public ICommand ConfirmCommand
        {
            get
            {
                return _confirmCommand ?? (_confirmCommand = new RelayCommand(() =>
                {
                    //if (string.IsNullOrEmpty(Pin))
                    //{
                    //    MessageBoxEx.Show("PIN can not be null.", MessageBoxButton.OK);
                    //    return;
                    //}

                    //PinResponse pinResponse = ServiceHelper.VertifyPin(Pin);
                    //if (pinResponse.VPIN_GSC == "0100")
                    //{
                    //    //关闭校验窗口
                    //    Messenger.Default.Send<string>(null, "ClosePinView");
                    //}
                    //else
                    //{
                    //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //    {
                    //        ServiceHelper.ShowMessage(new string[1] { pinResponse.VPIN_GSC });
                    //    }));
                    //}
                }, () =>
                {
                    return true;
                }));
            }
        }

        #endregion
    }
}
