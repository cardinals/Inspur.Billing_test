using ControlLib.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlLib.Controls
{
    /// <summary>
    /// 模块编号：公用控件库
    /// 作用：图片单选按钮
    /// 作者：丁纪名
    /// 编写日期：2017-12-19
    /// </summary>
    public class ImageRadioButton : RadioButton
    {
        #region 构造函数
        static ImageRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageRadioButton), new FrameworkPropertyMetadata(typeof(ImageRadioButton)));
        }
        #endregion

        #region 属性
        /// <summary>
        /// 正常时候的图片
        /// </summary>
        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalImageProperty = ImageButtonElement.NormalImageProperty.AddOwner(typeof(ImageRadioButton));
        /// <summary>
        /// 鼠标滑过图片
        /// </summary>
        public ImageSource MouseOverImage
        {
            get { return (ImageSource)GetValue(MouseOverImageProperty); }
            set { SetValue(MouseOverImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverImageProperty = ImageButtonElement.MouseOverImageProperty.AddOwner(typeof(ImageRadioButton));
        #endregion
    }
}
