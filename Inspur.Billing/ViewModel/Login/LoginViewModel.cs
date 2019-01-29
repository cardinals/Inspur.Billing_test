using CommonLib.Crypt;
using CommonLib.Helper;
using ControlLib.Controls.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Inspur.Billing.Commom;
using Inspur.Billing.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Inspur.Billing.ViewModel.Login
{
    public class LoginViewModel : ViewModelBase
    {
        #region 字段
        Window _loginView;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _userName = "admin";
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { Set<string>(ref _userName, value, "UserName"); }
        }
        /// <summary>
        /// 获取或设置
        /// </summary>
        private string _password = "111111";
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { Set<string>(ref _password, value, "Password"); }
        }

        #endregion

        #region Command
        /// <summary>
        /// 获取或设置
        /// </summary>
        private ICommand _loadedCommand;
        /// <summary>
        /// 获取或设置
        /// </summary>
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand<Window>(p =>
                {
                    _loginView = p;
                }, a =>
                {
                    return true;
                }));
            }
        }

        /// <summary>
        /// 获取或设置登录命令
        /// </summary>
        private ICommand _loginCommand;
        /// <summary>
        /// 获取或设置登录命令
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new RelayCommand(() =>
                {
                    try
                    {
                        if (string.IsNullOrEmpty(_userName))
                        {
                            MessageBoxEx.Show("UserName can not be null.", MessageBoxButton.OK);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(_password))
                        {
                            MessageBoxEx.Show("Password can not be null.", MessageBoxButton.OK);
                            return;
                        }
                        var cashierInfo = (from a in Const.dB.Cashiers
                                           where a.Name == _userName
                                           select a).ToList();
                        if (cashierInfo != null && cashierInfo.Count > 0)
                        {
                            if (!Md5Crypt.MD5Encrypt32(_password).ToLower().Equals(cashierInfo[0].Password))
                            {
                                MessageBoxEx.Show("Incorrect password.", MessageBoxButton.OK);
                                return;
                            }
                        }
                        else
                        {
                            MessageBoxEx.Show("User does not exist.", MessageBoxButton.OK);
                            return;
                        }
                        Const.CashierId = cashierInfo[0].CashierId;
                        Const.Statues = (from a in Const.dB.SystemStatu
                                         select a).ToList();
                        MainWindow mainWindow = new MainWindow();
                        Application.Current.MainWindow = mainWindow;
                        if (_loginView != null)
                        {
                            _loginView.Close();
                        }
                        mainWindow.ShowDialog();
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

        #endregion
    }
}
