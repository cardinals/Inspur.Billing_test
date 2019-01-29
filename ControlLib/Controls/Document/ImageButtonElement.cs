using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ControlLib.Document
{
    /// <summary>
    /// 模块编号：公共类库
    /// 作用：定义图片按钮公用的属性
    /// 作者：丁纪名
    /// 编写日期：2017-12-19
    /// </summary>
    public class ImageButtonElement : DependencyObject
    {
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
        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(ImageSource), typeof(ImageButtonElement), new PropertyMetadata(null));
        /// <summary>
        /// 鼠标滑过图片
        /// </summary>
        public ImageSource MouseOverImage
        {
            get { return (ImageSource)GetValue(MouseOverImageProperty); }
            set { SetValue(MouseOverImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverImageProperty =
            DependencyProperty.Register("MouseOverImage", typeof(ImageSource), typeof(ImageButtonElement), new PropertyMetadata(null));
        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register("PressedImage", typeof(ImageSource), typeof(ImageButtonElement), new PropertyMetadata(null));
        #endregion
    }
}
