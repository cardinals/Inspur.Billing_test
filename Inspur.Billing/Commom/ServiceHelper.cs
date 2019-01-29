using CommonLib.Net;
using ControlLib.Controls.Dialogs;
using DataModels;
using Inspur.Billing.Model.Service.Attention;
using Inspur.Billing.Model.Service.Pin;
using Inspur.Billing.Model.Service.Status;
using Inspur.Billing.View.Setting;
using JumpKick.HttpLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LinqToDB;
using Inspur.Billing.Model.Service.Sign;
using System.Security.Cryptography;
using Inspur.Billing.Model.Service.LastSign;
using System.Net.Sockets;
using Inspur.Billing.Model;
using System.IO.Ports;

namespace Inspur.Billing.Commom
{
    class ServiceHelper
    {
        public static string CurrentTime = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        /// <summary>
        /// socket连接对象
        /// </summary>
        public static TcpHelper TcpClient = new TcpHelper();

        public static TcpHelper _statusTcpClient = new TcpHelper();

        public static void StatueRequest(string requestString)
        {
            _statusTcpClient = new TcpHelper();
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
            bool isConn = _statusTcpClient.Connect(IPAddress.Parse(sdc[0]), int.Parse(sdc[1]));
            if (!isConn)
            {
                MessageBoxEx.Show("Failed to connect to EFD.", MessageBoxButton.OK);
                return;
            }
            _statusTcpClient.Complated -= _statusTcpClient_Complated;
            _statusTcpClient.Complated += _statusTcpClient_Complated;
            _statusTcpClient.Send(0x01, requestString);
            _statusTcpClient.ReciveAsync();
        }

        private static void _statusTcpClient_Complated(object sender, MessageModel e)
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
                if (e.MessageId != 0x01)
                {
                    return;
                }
                if (!e.IsSuccess)
                {
                    ShowMessageBegin(e.ErrorMessage);
                    return;
                }
                //_statusTcpClient.Complated -= _statusTcpClient_Complated;
                StatusResponse statusResponse = JsonConvert.DeserializeObject<StatusResponse>(e.Message);
                if (statusResponse != null)
                {
                    //
                    Const.IsHasGetStatus = true;
                    //保存软件信息--此处处理未分开（每次都保存），正式使用的时候请
                    var info = (from a in Const.dB.PosInfo
                                select a).FirstOrDefault();
                    if (info != null)
                    {
                        Const.dB.Update<PosInfo>(new PosInfo { Id = info.Id, CompanyName = statusResponse.Manufacture, Desc = statusResponse.Model, Version = statusResponse.SoftwareVersion, IssueDate = info.IssueDate });
                    }
                    //记录税种信息
                    if (statusResponse.TaxInfo != null && statusResponse.TaxInfo.Count > 0)
                    {
                        Const.dB.CodeTaxtype.Delete();
                        long id = 1;
                        foreach (var item in statusResponse.TaxInfo)
                        {
                            if (item.Category != null && item.Category.Count > 0)
                            {
                                foreach (var itm in item.Category)
                                {
                                    Const.dB.Insert<CodeTaxtype>(new CodeTaxtype
                                    {
                                        TaxtypeId = id,
                                        TaxTypeName = item.TaxTpye,
                                        TaxTypeCode = item.TaxTpye,

                                        TaxItemLable = itm.TaxLabel,
                                        TaxItemName = itm.TaxName,
                                        TaxItemCode = itm.CategoryId.ToString(),
                                        TaxRate = itm.TaxRate,
                                        EffectDate = itm.EffectiveDate,
                                        ExpireDate = itm.ExpiredDate
                                    });
                                    id++;
                                }
                            }
                        }
                    }
                    //记录monitor信息


                    if (!statusResponse.isInitialized)
                    {
                        ShowMessageBegin("EFD is not initialized.");
                    }
                    else
                    {
                        if (statusResponse.isLocked)
                        {
                            ShowMessageBegin("EFD is locked.");
                        }
                        else
                        {
                            if (Const.IsNeedMessage)
                            {
                                ShowMessageBegin("EFD is available");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessageBegin(ex.Message);
            }
        }

        public static void StatueRequestSerial(string request)
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
            if (string.IsNullOrWhiteSpace(request))
            {
                throw new Exception("Request can not be null.");
            }

            StopBits ss = (StopBits)Enum.Parse(typeof(StopBits), Const.Locator.ParameterSetting.SelectedStopBits);


            SerialClient _client = new SerialClient(Const.Locator.ParameterSetting.SelectedPort,
               int.Parse(Const.Locator.ParameterSetting.SelectedBaudRate),
               (Parity)Enum.Parse(typeof(Parity), Const.Locator.ParameterSetting.SelectedParity),
               int.Parse(Const.Locator.ParameterSetting.SelectedDataBits),
               (StopBits)Enum.Parse(typeof(StopBits), Const.Locator.ParameterSetting.SelectedStopBits));
            _client.Open();
            _client.Complated += _statusTcpClient_Complated;
            _client.Send(0x01, request);
        }

        public static void SignRequest(SignRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("SignRequest data can not be null.");
            }
            string requestString = JsonConvert.SerializeObject(request);


            if (!TcpClient.IsConnected)
            {
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
                TcpClient.Connect(IPAddress.Parse(sdc[0]), int.Parse(sdc[1]));
            }
            TcpClient.Send(0x02, requestString);

            TcpClient.ReciveAsync();
        }

        public static SignResponse LastSignRequest(LastSignRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("SignRequest data can not be null.");
            }
            string requestString = JsonConvert.SerializeObject(request);

            HttpHelper httpHelper = new HttpHelper();
            HttpItem httpItem = new HttpItem();
            httpItem.Method = "POST";
            httpItem.URL = Const.SignUri;
            httpItem.Postdata = Convert.ToString(requestString);


            httpItem.ResultType = ResultType.String;
            HttpResult html = httpHelper.GetHtml(httpItem);
            if (html.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(string.IsNullOrEmpty(html.Html) ? (string.IsNullOrEmpty(html.StatusDescription) ? "Post Data Error!" : html.StatusDescription) : html.Html);
            }
            return JsonConvert.DeserializeObject<SignResponse>(html.Html);
        }

        public static bool CheckStatue()
        {
            bool result = false;
            try
            {
                StatusRequest statusRequest = new StatusRequest() { PosSerialNumber = Config.PosSerialNumber, PosVendor = Config.PosVendor };
                string requestString = JsonConvert.SerializeObject(statusRequest);
                switch (Const.Locator.ParameterSetting.CommModel)
                {
                    case CommModel.NetPort:
                        StatueRequest(requestString);
                        break;
                    case CommModel.SerialPort:
                        StatueRequestSerial(requestString);
                        break;
                    default:
                        break;
                }

            }
            catch (SocketException e)
            {
                ShowMessageBegin("Pos can not connect with ESDC.");
            }
            catch (Exception ex)
            {
                ShowMessageBegin(ex.Message);
                result = false;
            }
            return result;
        }

        public static void ShowMessage(string[] codes)
        {
            List<string> list = new List<string>();
            foreach (var item in codes)
            {
                if (Const.Statues != null)
                {
                    SystemStatu statu = Const.Statues.FirstOrDefault(a => a.Code == item);
                    if (statu != null)
                    {
                        list.Add(statu.Name);
                    }
                }
            }
            if (list.Count > 0)
            {
                MessageBoxEx.Show(string.Join(",", list.ToArray()));
            }
        }

        public static void ShowMessageBegin(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBoxEx.Show(message);
            }));
        }

        public static string CaclBase64Md5Hash(string data)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(data)));
        }
    }
}
