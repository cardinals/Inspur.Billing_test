using ControlLib.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inspur.Billing.Commom
{
    public class Printer
    {
        #region 构造函数
        private Printer()
        {
            Application.Current.Exit += Current_Exit;
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                POS_Port_Close(m_hPrinter);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private static readonly object obj = new object();
        private static Printer _instance;

        public static Printer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Printer();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 常量
        private const Int32 POS_PT_COM = 1000;
        private const Int32 POS_PT_LPT = 1001;
        private const Int32 POS_PT_USB = 1002;
        private const Int32 POS_PT_NET = 1003;

        // printer state
        private const Int32 POS_PS_NORMAL = 3001;
        private const Int32 POS_PS_PAPEROUT = 3002;
        private const Int32 POS_PS_HEAT = 3003;
        private const Int32 POS_PS_DOOROPEN = 3004;
        private const Int32 POS_PS_BUFFEROUT = 3005;
        private const Int32 POS_PS_CUT = 3006;
        private const Int32 POS_PS_DRAWERHIGH = 3007;

        private const Int32 POS_ES_PAPERENDING = 6; //纸将尽
        private const Int32 POS_ES_DRAWERHIGH = 5; //钱箱高电平
        private const Int32 POS_ES_CUT = 4; //切刀未复位
        private const Int32 POS_ES_DOOROPEN = 3; //纸仓门开
        private const Int32 POS_ES_HEAT = 2; //机头过热
        private const Int32 POS_ES_PAPEROUT = 1; //打印机缺纸
        private const Int32 POS_ES_SUCCESS = 0; //成功/发送成功/状态正常/打印完成
        private const Int32 POS_ES_INVALIDPARA = -1; //参数错误
        private const Int32 POS_ES_WRITEFAIL = -2; //写失败
        private const Int32 POS_ES_READFAIL = -3; //读失败
        private const Int32 POS_ES_NONMONOCHROMEBITMAP = -4; //非单色位图
        private const Int32 POS_ES_OVERTIME = -5; //超时/写超时/读超时/打印未完成
        private const Int32 POS_ES_FILEOPENERROR = -6; //文件/图片打开失败
        private const Int32 POS_ES_OTHERERRORS = -100; //其他原因导致的错误

        // barcode type
        private const Int32 POS_BT_UPCA = 4001;
        private const Int32 POS_BT_UPCE = 4002;
        private const Int32 POS_BT_JAN13 = 4003;
        private const Int32 POS_BT_JAN8 = 4004;
        private const Int32 POS_BT_CODE39 = 4005;
        private const Int32 POS_BT_ITF = 4006;
        private const Int32 POS_BT_CODABAR = 4007;
        private const Int32 POS_BT_CODE93 = 4073;
        private const Int32 POS_BT_CODE128 = 4074;


        // 2D barcode type
        private const Int32 POS_BT_PDF417 = 4100;
        private const Int32 POS_BT_DATAMATRIX = 4101;
        private const Int32 POS_BT_QRCODE = 4102;

        // HRI type
        private const Int32 POS_HT_NONE = 4011;
        private const Int32 POS_HT_UP = 4012;
        private const Int32 POS_HT_DOWN = 4013;
        private const Int32 POS_HT_BOTH = 4014;

        //TSPL
        private const Int32 TSPL_PRINTER_STATUS_OUTPAPER = 1;//打印机缺纸
        private const Int32 TSPL_PRINTER_STATUS_WORK = 2;	//打印中
        private const Int32 TSPL_PRINTER_STATUS_ENCLOSURENOCLOSE = 3;	//机壳未关
        private const Int32 TSPL_PRINTER_STATUS_ERROR = 4;	//打印机内部错误

        private const Int32 TSPL_PARAM_LESS_EQUAL_ZERO = -2;		//参数小于等于0
        private const Int32 TSPL_PARAM_GREAT_RANGE = -3;		//参数大于指定范围
        private const Int32 TSPL_SUCCESS = 0;
        private const Int32 TSPL_IDERROR = -1;

        /// <summary>
        /// 58系列打印机一行的大致字符数
        /// </summary>
        public const int Length58 = 32;
        #endregion

        #region 字段
        /// <summary>
        /// 打印机句柄
        /// </summary>
        Int32 m_hPrinter = -1;
        #endregion

        #region 属性
        /// <summary>
        /// 打印端口
        /// </summary>
        public string PrintPort { get; set; }
        #endregion

        #region API
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="lpName"></param>
        /// <param name="iPort"></param>
        /// <param name="bFile"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Port_OpenA")]
        static extern Int32 POS_Port_OpenA(String lpName, Int32 iPort, bool bFile, String path);
        /// <summary>
        /// 关闭端口
        /// </summary>
        /// <param name="printID"></param>
        /// <returns></returns>

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Port_Close")]
        static extern Int32 POS_Port_Close(Int32 printID);
        /// <summary>
        /// 打印测试页
        /// </summary>
        /// <param name="printID"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_PrintTestpage")]
        static extern Int32 POS_Control_PrintTestpage(Int32 printID);
        /// <summary>
        /// 缺纸查询
        /// </summary>
        /// <param name="printID"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Status_RTQueryStatus")]
        static extern Int32 POS_Status_RTQueryStatus(Int32 printID);
        /// <summary>
        /// 查询状态
        /// </summary>
        /// <param name="printID"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Status_RTQueryTypeStatus")]
        static extern Int32 POS_Status_RTQueryTypeStatus(Int32 printID, Int32 n);
        /// <summary>
        /// 初始化打印机
        /// </summary>
        /// <param name="printID"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_ReSet")]
        static extern Int32 POS_Control_ReSet(Int32 printID);
        /// <summary>
        /// 发送十六进制数据到打印机；
        /// </summary>
        /// <param name="printID">打印机句柄， 由打开端口的返回值确定</param>
        /// <param name="strBuff">将要发送的字符缓冲区数据</param>
        /// <param name="ilen">缓冲区大小</param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintData")]
        static extern Int32 POS_Output_PrintData(Int32 printID, byte[] strBuff, Int32 ilen);
        /// <summary>
        /// 选择字符对齐（居左/居中/居右） 方式；
        /// </summary>
        /// <param name="printID">打印机句柄， 由打开端口的返回值确定；</param>
        /// <param name="iAlignType">为 0 时左对齐， 为 1 时居中， 为 2 时右对齐；</param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_AlignType")]
        static extern Int32 POS_Control_AlignType(Int32 printID, Int32 iAlignType);
        /// <summary>
        /// 打印格式化后的字符串；
        /// </summary>
        /// <param name="printID">打印机句柄， 由打开端口的返回值确定；</param>
        /// <param name="iFont">为 0 时选择标准 ASCII 字体 A (12 × 24)， 为 1 时选择压缩 ASCII 字体 B (9 × 17)；</param>
        /// <param name="iThick">为 0 时取消加粗模式， 为 1 时选择加粗模式；</param>
        /// <param name="iWidth">为 0 时取消倍宽模式， 为 1 时选择倍宽模式；</param>
        /// <param name="iHeight">为 0 时取消倍高模式， 为 1 时选择倍高模式；</param>
        /// <param name="iUnderLine">为 0 时取消下划线模式， 为 1 时选择下划线模式；</param>
        /// <param name="lpString">以空字符结尾的字符串；</param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintFontStringA")]
        static extern Int32 POS_Output_PrintFontStringA(Int32 printID, Int32 iFont, Int32 iThick, Int32 iWidth, Int32 iHeight, Int32 iUnderLine, String lpString);
        /// <summary>
        /// 打印缓冲区内容， 进纸由参数 iLines 设置的行数并切纸；
        /// </summary>
        /// <param name="printID">打印机句柄， 由打开端口的返回值确定；</param>
        /// <param name="type">切纸类型 0 全切， 1 半切；</param>
        /// <param name="len">进纸行数；</param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_CutPaper")]
        static extern Int32 POS_Control_CutPaper(Int32 printID, Int32 type, Int32 len);
        /// <summary>
        /// 二维码打印
        /// </summary>
        /// <param name="printID"></param>
        /// <param name="iType"></param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <param name="lpString"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintTwoDimensionalBarcodeA")]
        static extern Int32 POS_Output_PrintTwoDimensionalBarcodeA(Int32 printID, Int32 iType, Int32 parameter1, Int32 parameter2, Int32 parameter3, String lpString);
        /// <summary>
        /// 图片打印
        /// </summary>
        /// <param name="printID"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintBmpDirectA")]
        static extern Int32 POS_Output_PrintBmpDirectA(Int32 printID, String strPath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printID"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintRamBmp")]
        static extern Int32 POS_Output_PrintRamBmp(Int32 printID, Int32 n);
        /// <summary>
        /// 走纸
        /// </summary>
        /// <param name="printID"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_FeedLines")]
        static extern Int32 POS_Control_FeedLines(Int32 printID, Int32 lines);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPrinterID"></param>
        /// <param name="iLeftMargin"></param>
        /// <param name="iWidth"></param>
        /// <returns></returns>
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_SetPrintPosition")]
        static extern Int32 POS_Control_SetPrintPosition(Int32 iPrinterID, Int32 iLeftMargin, Int32 iWidth);
        #endregion

        #region 方法
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="lpName">端口名称 此处使用默认使用usb类型的串口</param>
        public int OpenPort()
        {
            if (string.IsNullOrWhiteSpace(PrintPort))
            {
                throw new ArgumentNullException("Print serial port Can not be null.");
            }
            m_hPrinter = POS_Port_OpenA(PrintPort, POS_PT_USB, false, "");
            if (m_hPrinter < 0)
            {
                POS_Port_Close(m_hPrinter);
            }
            return m_hPrinter;
        }
        public void PrintTestPaper()
        {
            if (!CheckPrint())
            {
                return;
            }
            else
            {
                MessageBoxEx.Show("The printer works well.");
            }
            //Int32 ret;
            //ret = POS_Control_PrintTestpage(m_hPrinter);
            //switch (ret)
            //{
            //    case POS_ES_SUCCESS:
            //        MessageBoxEx.Show("Send success.");
            //        break;
            //    case POS_ES_INVALIDPARA:
            //        MessageBoxEx.Show("Parameter error.");
            //        break;
            //    case POS_ES_WRITEFAIL:
            //        MessageBoxEx.Show("Write failure.");
            //        break;
            //    case POS_ES_OVERTIME:
            //        MessageBoxEx.Show("Send Timeout.");
            //        break;
            //    case POS_ES_OTHERERRORS:
            //        MessageBoxEx.Show("Other mistakes.");
            //        break;
            //}
        }
        /// <summary>
        /// 查询状态返回处理
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        private bool StatusError(int errorCode)
        {
            switch (errorCode)
            {
                case POS_ES_INVALIDPARA:
                    MessageBoxEx.Show("Parameter error.");
                    return false;
                case POS_ES_WRITEFAIL:
                    MessageBoxEx.Show("Write failure.");
                    return false;
                case POS_ES_READFAIL:
                    MessageBoxEx.Show("Reading failure.");
                    return false;
                case POS_ES_OVERTIME:
                    MessageBoxEx.Show("Timeout.");
                    return false;
                case POS_ES_OTHERERRORS:
                    MessageBoxEx.Show("Other mistakes.");
                    return false;
            }
            return true;
        }

        public bool QueryStatus()
        {
            int nDrawerHigh = 0; //钱箱高电平
            int nDoorOpen = 0;   //纸仓开
            int nCut = 0;        //切刀错误
            int nPaperOut = 0;   //缺纸
            int nPaperEnding = 0; //纸将尽

            Int32 ret = POS_Status_RTQueryTypeStatus(m_hPrinter, 1);
            if (!StatusError(ret))
            {
                return false;
            }
            if (ret == POS_ES_DRAWERHIGH)
            {
                nDrawerHigh = 1;
            }
            ret = POS_Status_RTQueryTypeStatus(m_hPrinter, 2);
            if (!StatusError(ret))
            {
                return false;
            }
            if (ret == POS_ES_DOOROPEN)
            {
                nDoorOpen = 1;
            }
            ret = POS_Status_RTQueryTypeStatus(m_hPrinter, 3);
            if (!StatusError(ret))
            {
                return false;
            }
            if (ret == POS_ES_CUT)
            {
                nCut = 1;
            }
            ret = POS_Status_RTQueryTypeStatus(m_hPrinter, 4);
            if (!StatusError(ret))
            {
                return false;
            }
            if (ret == POS_ES_PAPEROUT)
            {
                nPaperOut = 1;
            }
            else if (ret == POS_ES_PAPERENDING)
            {
                nPaperEnding = 1;
            }

            string strMessage;
            strMessage = "#";
            if (nDrawerHigh == 1)
            {
                strMessage = strMessage + "钱箱高电平" + "#";
            }
            else
            {
                strMessage = strMessage + "钱箱低电平电平" + "#";
            }
            if (nDoorOpen == 1)
            {
                strMessage = strMessage + "纸仓开" + "#";
            }
            else
            {
                strMessage = strMessage + "纸仓未开" + "#";
            }
            if (nCut == 1)
            {
                strMessage = strMessage + "切刀错误" + "#";
            }
            else
            {
                strMessage = strMessage + "切刀正常" + "#";
            }
            if (nPaperOut == 1)
            {
                strMessage = strMessage + "缺纸" + "#";
            }
            else
            {
                strMessage = strMessage + "有纸" + "#";
            }
            if (nPaperEnding == 1)
            {
                strMessage = strMessage + "纸将尽" + "#";
            }
            else
            {
                strMessage = strMessage + "纸充足" + "#";
            }
            int result = (nDoorOpen | nCut | nPaperOut | nPaperEnding);
            if (result > 0)
            {
                MessageBoxEx.Show(strMessage);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 查询是否缺纸
        /// </summary>
        public bool QueryPaperStatus()
        {
            Int32 ret = POS_Status_RTQueryStatus(m_hPrinter);
            bool result = true;
            switch (ret)
            {
                case POS_ES_SUCCESS:
                    //MessageBoxEx.Show("有纸");
                    break;
                case POS_ES_PAPEROUT:
                    result = false;
                    MessageBoxEx.Show("缺纸");
                    break;
                case POS_ES_INVALIDPARA:
                    result = false;
                    MessageBoxEx.Show("参数错误");
                    break;
                case POS_ES_WRITEFAIL:
                    result = false;
                    MessageBoxEx.Show("写失败");
                    break;
                case POS_ES_READFAIL:
                    result = false;
                    MessageBoxEx.Show("读失败");
                    break;
                case POS_ES_OVERTIME:
                    result = false;
                    MessageBoxEx.Show("超时");
                    break;
                case POS_ES_OTHERERRORS:
                    result = false;
                    MessageBoxEx.Show("其他错误");
                    break;
            }
            return result;
        }
        /// <summary>
        /// 检查打印机
        /// </summary>
        /// <returns></returns>
        private bool CheckPrint()
        {
            if (m_hPrinter < 0)
            {
                OpenPort();
                if (OpenPort() < 0)
                {
                    MessageBoxEx.Show("Opening the print port failed.");
                    return false;
                }
            }
            if (!QueryStatus() || !QueryPaperStatus())
            {
                return false;
            }
            Reset();
            return true;
        }
        /// <summary>
        /// 设置对齐方式 为 0 时左对齐， 为 1 时居中， 为 2 时右对齐
        /// </summary>
        /// <param name="align"></param>
        /// <returns></returns>
        public int SetAlign(int align)
        {
            return POS_Control_AlignType(m_hPrinter, align);
        }
        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="font"></param>
        /// <param name="thick"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="underLine"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int PrintString(int font, int thick, int width, int height, int underLine, string content)
        {
            return POS_Output_PrintFontStringA(m_hPrinter, font, thick, width, height, underLine, content);
        }
        public int CutPaper(int type, int len)
        {
            return POS_Control_CutPaper(m_hPrinter, type, len);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpString"></param>
        /// <returns></returns>
        public int PrintTwoDimensionalBarcodeA(string lpString)
        {
            int result = POS_Output_PrintTwoDimensionalBarcodeA(m_hPrinter, POS_BT_QRCODE, 2, 77, (int)Config.QrMagnification, lpString);

            switch (result)
            {
                case POS_ES_INVALIDPARA:
                    MessageBox.Show("Parameter error.");
                    break;
                case POS_ES_WRITEFAIL:
                    MessageBox.Show("Fail in send。");
                    break;
                case POS_ES_OVERTIME:
                    MessageBox.Show("Timeout.");
                    break;
                case POS_ES_OTHERERRORS:
                    MessageBox.Show("Other mistakes.");
                    break;
            }

            POS_Control_ReSet(m_hPrinter);
            return result;
        }
        /// <summary>
        /// 打印本地位图
        /// </summary>
        /// <param name="path">打印图片的本地路径</param>
        /// <returns></returns>
        public int PrintBmpDirect(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("Path can not be null.");
            }
            if (!File.Exists(Const.QrPath))
            {
                throw new ArgumentNullException("File does not exist.");
            }
            int status = POS_Output_PrintBmpDirectA(m_hPrinter, path);
            switch (status)
            {
                case POS_ES_SUCCESS:
                    //MessageBox.Show("打印成功");
                    break;
                case POS_ES_INVALIDPARA:
                    MessageBoxEx.Show("Parameter error.");
                    break;
                case POS_ES_WRITEFAIL:
                    MessageBoxEx.Show("Write failure.");
                    break;
                case POS_ES_NONMONOCHROMEBITMAP:
                    MessageBoxEx.Show("Non monochromatic bitmap.");
                    break;
                case POS_ES_OVERTIME:
                    MessageBoxEx.Show("Download timeout.");
                    break;
                case POS_ES_FILEOPENERROR:
                    MessageBoxEx.Show("Picture open failure.");
                    break;
                default:
                    MessageBoxEx.Show("Other mistakes.");
                    break;
            }
            return status;
        }
        /// <summary>
        /// 打印
        /// </summary>
        public void Print(Action action)
        {
            if (!CheckPrint())
            {
                return;
            }
            Int32 ret = POS_Control_ReSet(m_hPrinter);
            if (!StatusError(ret))
            {
                return;
            }
            //char cmd[] = {0x1c, 0x26};
            byte[] cmd = new byte[] { 0x1c, 0x26 };
            POS_Output_PrintData(m_hPrinter, cmd, 2);
            if (action != null)
            {
                action();
            }
            POS_Control_ReSet(m_hPrinter);
        }
        public int Reset()
        {
            return POS_Control_ReSet(m_hPrinter);
        }
        /// <summary>
        /// 进纸
        /// </summary>
        /// <param name="lines">行数</param>
        /// <returns></returns>
        public int FeedLines(int lines)
        {
            return POS_Control_FeedLines(m_hPrinter, 2);
        }
        public int SetPosition(int lefMargin, int width)
        {
            return POS_Control_SetPrintPosition(m_hPrinter, lefMargin, width);
        }






        public void PrintLine()
        {
            PrintString(0, 0, 0, 0, 0, "————————————————————————\r\n");
        }

        /// <summary>
        /// 58pos -  32字符，80-
        /// </summary>
        const int PrintCharCount = 47;

        /// <summary>
        /// 目前在api中没有找到同一行两列的打印方式，先使用此方法
        /// </summary>
        /// <param name="leftName"></param>
        /// <param name="leftValue"></param>
        /// <param name="rightName"></param>
        /// <param name="rightValue"></param>
        public void SetTwoColumnPrint(string leftName, string leftValue, string rightName, string rightValue)
        {
            string left = string.Format("{0}:{1}", leftName, leftValue);
            string right = string.Format("{0}{1}", rightName, rightValue);
            StringBuilder sb = new StringBuilder();
            if (left.Length + right.Length < PrintCharCount)
            {
                sb.Append(' ', PrintCharCount - left.Length - right.Length);
            }
            Printer.Instance.PrintString(0, 1, 0, 0, 0, string.Format("{0}{1}{2}\r\n", left, sb.ToString(), right));
        }

        public string SetLeftPrint(int totalLength, string content)
        {
            StringBuilder result = new StringBuilder();
            int spaceCount = totalLength - content.Length;
            if (spaceCount > 0)
            {
                result.Append(content);
                result.Append(' ', spaceCount);
                return result.ToString();
            }
            return content;
        }
        public string SetRightPrint(int totalLength, string content)
        {
            StringBuilder result = new StringBuilder();
            int spaceCount = totalLength - content.Length;
            if (spaceCount > 0)
            {
                result.Append(' ', spaceCount);
                result.Append(content);
                return result.ToString();
            }
            return content;
        }
        public string SetCenterPrint(int totalLength, string content)
        {
            StringBuilder result = new StringBuilder();
            int spaceCount = totalLength - content.Length;
            int marginSpace = (int)(Math.Floor(spaceCount / 2.0));
            int mode = spaceCount % 2;
            if (spaceCount > 0)
            {
                if (marginSpace > 0)
                {
                    if (mode > 0)
                    {
                        result.Append(' ', marginSpace + 1);
                        result.Append(content);
                        result.Append(' ', marginSpace);
                    }
                    else
                    {
                        result.Append(' ', marginSpace);
                        result.Append(content);
                        result.Append(' ', marginSpace);
                    }
                }
                else
                {
                    result.Append(content);
                    result.Append(" ");
                }
            }
            else
            {
                result.Append(content);
            }
            return result.ToString();
        }
        #endregion
    }
}
