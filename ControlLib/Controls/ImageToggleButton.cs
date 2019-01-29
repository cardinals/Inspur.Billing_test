using ControlLib.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ControlLib.Controls
{
    /// <summary>
    /// 模块编号：公共控件库
    /// 作用：图片tooglebutton
    /// 作者：丁纪名
    /// 编写日期：2017-12-05
    /// </summary>
    public class ImageToggleButton : ToggleButton
    {
        #region 构造函数
        static ImageToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageToggleButton), new FrameworkPropertyMetadata(typeof(ImageToggleButton)));
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
        public static readonly DependencyProperty NormalImageProperty = ImageButtonElement.NormalImageProperty.AddOwner(typeof(ImageToggleButton));
        /// <summary>
        /// 鼠标滑过图片
        /// </summary>
        public ImageSource MouseOverImage
        {
            get { return (ImageSource)GetValue(MouseOverImageProperty); }
            set { SetValue(MouseOverImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverImageProperty = ImageButtonElement.MouseOverImageProperty.AddOwner(typeof(ImageToggleButton));


        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageProperty = ImageButtonElement.PressedImageProperty.AddOwner(typeof(ImageToggleButton));

        #endregion
    }
}
