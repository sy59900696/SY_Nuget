using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text; 

namespace StaticCode
{
    /// <summary>
    /// 图像合成、压缩、缩放、格式转换等相关操作。
    /// </summary>
    public class ImgHelper
    {

        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="_sImgBase">来源图片地址</param>        
        /// <param name="_iX">从偏移X坐标位置开始截取</param>
        /// <param name="_iY">从偏移Y坐标位置开始截取</param> 
        /// <param name="_iWidth">保存图片的宽度</param>
        /// <param name="_iHeight">保存图片的高度</param>
        /// <returns></returns>
        public static Image M_CaptureImage(string _sImgBase, int _iX, int _iY, int _iWidth, int _iHeight)
        {
            //原图片文件
            Image _imgBase = Image.FromFile(_sImgBase);
            return M_CaptureImage(_imgBase, _iX, _iY, _iWidth, _iHeight);
        }
        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="_imgBase">来源图片地址</param>        
        /// <param name="_iX">从偏移X坐标位置开始截取</param>
        /// <param name="_iY">从偏移Y坐标位置开始截取</param> 
        /// <param name="_iWidth">保存图片的宽度</param>
        /// <param name="_iHeight">保存图片的高度</param>
        /// <returns></returns>
        public static Bitmap M_CaptureImage(Image _imgBase, int _iX, int _iY, int _iWidth, int _iHeight)
        {
            //原图片文件
            //创建新图位图
            Bitmap bitmap = new Bitmap(_iWidth, _iHeight);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            //graphic.DrawImage(_imgBase, 0, 0, new Rectangle(_iX, _iY, _iWidth, _iHeight), GraphicsUnit.Pixel);
            graphic.DrawImage(_imgBase, new Rectangle(0, 0, _iWidth, _iHeight), new Rectangle(_iX, _iY, _iWidth, _iHeight), GraphicsUnit.Pixel);

            return bitmap;
            ////从作图区生成新图
            //Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            ////保存图片
            ////saveImage.Save(toImagePath, ImageFormat.Png);
            ////释放资源   
            ////saveImage.Dispose();
            ////graphic.Dispose();
            ////bitmap.Dispose();
            //return saveImage;
        }

        public static Image M_ChangeColor(Image imgFace, Color cTarget)
        {
            Bitmap bitBack = new Bitmap((Image)imgFace.Clone());

            Bitmap bitModel = new Bitmap(imgFace.Width, imgFace.Height);// 背景为透明的空白画板
            Graphics graphBitModel = Graphics.FromImage(bitModel);
            graphBitModel.Clear(Color.Transparent);
            //graphBitModel.Save();

            for (int y = 1; y < bitBack.Height; y++)                // 遍历图像的每个像素。
            {
                for (int x = 1; x < bitBack.Width; x++)
                {
                    Color _color = bitBack.GetPixel(x, y);
                    if (_color != Color.Transparent)
                    {
                        bitModel.SetPixel(x, y, cTarget);
                    }
                    //if (_color.A == 255 && _color.B == 255 && _color.G == 255 && _color.R == 255) continue;                    // 透明度为0，则跳过。

                    //bitModel.SetPixel(x, y, _color); 
                }
            }
            graphBitModel.Save();
            return bitModel;
        }

        /// <summary>
        /// 将两个图像合成。
        /// </summary>
        /// <param name="imgBack">背景图像</param>
        /// <param name="imgFace">上浮图像</param>
        /// <returns></returns>
        public static Bitmap Compose(Image imgBack, Image imgFace)
        {
            return Compose(imgBack, imgBack.Size, imgFace, imgFace.Size, new Point(0, 0));
        }

        /// <summary>
        /// 将两个图像合成。
        /// </summary>
        /// <param name="imgBack">背景图像</param>
        /// <param name="imgFace">上浮图像</param>
        /// <param name="point">imgFace在背景图像的起始坐标</param>
        /// <returns></returns>
        public static Bitmap Compose(Image imgBack, Image imgFace, Point point)
        {
            return Compose(imgBack, imgBack.Size, imgFace, imgFace.Size, point);
        }

        /// <summary>
        /// 将两个图像合成。
        /// </summary>
        /// <param name="imgBack">背景图像</param>
        /// <param name="imgFace">上浮图像</param>
        /// <param name="sizeImgFace">上浮图像大小</param>
        /// <param name="point">imgFace在背景图像的起始坐标</param>
        /// <returns></returns>
        public static Bitmap Compose(Image imgBack, Image imgFace, Size sizeImgFace, Point point)
        {
            return Compose(imgBack, imgBack.Size, imgFace, sizeImgFace, point);
        }

        /// <summary>
        /// 将两个图像合成。
        /// </summary>
        /// <param name="imgBack">背景图像</param>
        /// <param name="sizeImgBack">背景图像大小</param>
        /// <param name="imgFace">上浮图像</param>
        /// <param name="sizeImgFace">上浮图像大小</param>
        /// <param name="point">imgFace在背景图像的起始坐标</param>
        /// <returns></returns>
        public static Bitmap Compose(Image imgBack, Size sizeImgBack, Image imgFace, Size sizeImgFace, Point point)
        {
            if (sizeImgBack == Size.Empty) sizeImgBack = imgBack.Size;
            if (sizeImgFace == Size.Empty) sizeImgFace = imgFace.Size;
            if (point == null)
            {
                point.X = 0;
                point.Y = 0;
            }
            Bitmap bmpBack = new Bitmap((Image)imgBack.Clone(), sizeImgBack);
            Graphics graph = Graphics.FromImage(bmpBack);
            graph.DrawImage(imgFace, point.X, point.Y, sizeImgFace.Width, sizeImgFace.Height);

            graph.Save();
            return bmpBack;
        }
        /// <summary>
        /// 非占用方式读取文件
        /// </summary>
        /// <param name="sImgFullName"></param>
        /// <returns></returns>
        public static Image GetImageShare(string sImgFullName)
        {
            Image img;
            using (StreamReader sr = new StreamReader(sImgFullName))
            {
                img = Image.FromStream(sr.BaseStream);
                sr.Close();
            }
            return img;
        }

        /// <summary>
        /// 遍历图像每个像素。用于将文字转成图片。原理：将白色(即全是4色值全为255的)设置为透明
        /// </summary>
        /// <param name="imgFace"></param>
        /// <returns></returns>
        public static Image M_ClearWhiteBack(Image imgFace)
        {
            Bitmap bitBack = new Bitmap((Image)imgFace.Clone());

            Bitmap bitModel = new Bitmap(imgFace.Width, imgFace.Height);// 背景为透明的空白画板
            Graphics graphBitModel = Graphics.FromImage(bitModel);
            graphBitModel.Clear(Color.Transparent);
            //graphBitModel.Save();

            for (int y = 1; y < bitBack.Height; y++)                // 遍历图像的每个像素。
            {
                for (int x = 1; x < bitBack.Width; x++)
                {
                    Color _color = bitBack.GetPixel(x, y);
                    if (_color.A == 255 && _color.B == 255 && _color.G == 255 && _color.R == 255) continue;                    // 透明度为0，则跳过。

                    bitModel.SetPixel(x, y, _color);
                    //graphBitModel.DrawImage(_color, x, y, 1, 1);
                }
            }
            graphBitModel.Save();
            return bitModel;
        }

        /// <summary>
        /// width、height 其中任意一个不大于零时，则返回根据另一值等比例缩放的图像。
        /// 异常：width、height同时小于0则异常
        /// </summary>
        /// <param name="imgSource">原图</param>
        /// <param name="width">新图像宽度</param>
        /// <param name="height">新图像高度</param>
        /// <returns></returns>
        public static Bitmap ReSize(Image imgSource, int width, int height)
        {
            if (width <= 0 && height <= 0) throw new Exception("输入的width、height不能同时<=0");
            else if (width <= 0 && height > 0)
            {
                width = Convert.ToInt32(height * imgSource.Width / (float)imgSource.Height);
                Bitmap _bmpNew = new Bitmap(imgSource, new Size(width, height));
                return _bmpNew;
            }
            else if (width > 0 && height <= 0)
            {
                height = Convert.ToInt32(width * imgSource.Height / (float)imgSource.Width);
                Bitmap _bmpNew = new Bitmap(imgSource, new Size(width, height));
                return _bmpNew;
            }
            else
            {
                Bitmap _bmpNew = new Bitmap(imgSource, new Size(width, height));
                return _bmpNew;
            }
        }

        /// <summary>
        /// width、height 其中都不大于零时，则返回原图。其中任意一个不大于零时，则返回根据另一值等比例缩放的图像。
        /// </summary>
        /// <param name="imgSource">原图</param>
        /// <param name="width">新图像宽度</param>
        /// <param name="height">新图像高度</param>
        /// <returns></returns>
        public static Bitmap ReSizeZoom(Image imgSource, int width, int height)
        {
            double _dReal = 1.0;
            int _iWidth = imgSource.Width;
            int _iHeight = imgSource.Height;

            //先计算原图需要缩放的比例
            if ((width <= 0 && imgSource.Height == height) || (height <= 0 && imgSource.Width == width) || (width <= 0 && height <= 0))
            {
                return new Bitmap(imgSource);
            }
            else if (width > 0 && height <= 0)
            {
                height = Convert.ToInt32(width * imgSource.Height / (float)imgSource.Width);
                Bitmap _bmpNew = new Bitmap(imgSource, new Size(width, height));
                return _bmpNew;
            }
            else if (height > 0 && width <= 0)
            {
                width = Convert.ToInt32(height * imgSource.Width / (float)imgSource.Height);
                Bitmap _bmpNew = new Bitmap(imgSource, new Size(width, height));
                return _bmpNew;
            }
            else
            {
                if (imgSource.Width / imgSource.Height > width / height)
                {
                    //则以Width的比例计算real
                    _dReal = width / (1.0 * imgSource.Width);
                    _iWidth = width;
                    _iHeight = (int)(imgSource.Height * _dReal);
                }
                else
                {
                    //则以Height的比例计算real
                    _dReal = height / (1.0 * imgSource.Height);
                    _iHeight = height;
                    _iWidth = (int)(imgSource.Width * _dReal);
                }
            }

            //将原图等比例缩放至合适
            Bitmap _bmpTmp = ImgHelper.ReSize(imgSource, (int)(imgSource.Width * _dReal), (int)(imgSource.Height * _dReal));

            //合成
            Bitmap bitBack = new Bitmap(width, height); // 背景
            Graphics graphBit1 = Graphics.FromImage(bitBack);
            graphBit1.Clear(System.Drawing.Color.Transparent);
            //graphBit1.FillRectangle(new SolidBrush(Color.White), 0, 0, _iWidth, _iHeight);
            graphBit1.DrawImage(_bmpTmp, 0, 0);
            graphBit1.Save(); ;

            return bitBack;
        }
   
        /// <summary>
        /// 将8位的颜色名转为颜色。如：80ff0000 ，可转为透明度为128的红色。出现任何异常后均返回为黑色。
        /// </summary>
        /// <param name="_sColor"></param>
        /// <returns></returns>
        public static Color M_ConvertArgbToColor(string _sColor)
        {
            try
            {
                if (_sColor.Length != 8) throw new Exception("_sColor.Length != 8");
                int _iA = int.Parse(_sColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int _iR = int.Parse(_sColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int _iG = int.Parse(_sColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                int _iB = int.Parse(_sColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(_iA, _iR, _iG, _iB);
            }
            catch (Exception ex)
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// 判断是否为描述Argb的8位字符串。将8位的颜色名转为颜色。如：80ff0000 ，可转为透明度为128的红色。
        /// </summary>
        /// <param name="_sColor"></param>
        /// <returns></returns>
        public static bool M_IsArgbToColor(string _sColor)
        {
            try
            {
                if (_sColor.Length != 8) throw new Exception("_sColor.Length != 8");
                int _iA = int.Parse(_sColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int _iR = int.Parse(_sColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int _iG = int.Parse(_sColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                int _iB = int.Parse(_sColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                Color _c0 = Color.FromArgb(_iA, _iR, _iG, _iB);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 将颜色转为8位的颜色名。如：透明度为128的红色，可转为80ff0000 。出现任何异常后均返回为FFFFFFFF。
        /// </summary>
        /// <param name="_sColor"></param>
        /// <returns></returns>
        public static string M_ConvertColorToArgb(Color _sColor)
        {
            try
            {
                StringBuilder _sb0 = new StringBuilder();
                _sb0.Append(_sColor.A.ToString("x2"));
                _sb0.Append(_sColor.R.ToString("x2"));
                _sb0.Append(_sColor.G.ToString("x2"));
                _sb0.Append(_sColor.B.ToString("x2"));
                return _sb0.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                return "FFFFFFFF";
            }
        }

        ///// <summary>
        ///// 创建支持位图区域的控件（目前有button和form）
        ///// </summary>
        ///// <param name="control">待应用的控件</param>
        ///// <param name="bitmap">原图</param>
        //public static void CreateControlRegion(Control control, Bitmap bitmap)
        //{
        //    // Return if control and bitmap are null
        //    //判断是否存在控件和位图
        //    if (control == null || bitmap == null)
        //        return;
        //    // Set our control''s size to be the same as the bitmap
        //    //设置控件大小为位图大小
        //    control.Width = bitmap.Width;
        //    control.Height = bitmap.Height;
        //    // Check if we are dealing with Form here
        //    //当控件是form时
        //    if (control is System.Windows.Forms.Form)
        //    {
        //        // Cast to a Form object
        //        //强制转换为FORM
        //        Form form = (Form)control;
        //        // Set our form''s size to be a little larger that the bitmap just
        //        // in case the form''s border style is not set to none in the first place
        //        //当FORM的边界FormBorderStyle不为NONE时，应将FORM的大小设置成比位图大小稍大一点
        //        form.Width = control.Width;
        //        form.Height = control.Height;
        //        // No border
        //        //没有边界
        //        form.FormBorderStyle = FormBorderStyle.None;
        //        // Set bitmap as the background image
        //        //将位图设置成窗体背景图片
        //        form.BackgroundImage = bitmap;
        //        // Calculate the graphics path based on the bitmap supplied
        //        //计算位图中不透明部分的边界
        //        GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap);
        //        // Apply new region
        //        //应用新的区域
        //        form.Region = new Region(graphicsPath);
        //    }
        //    // Check if we are dealing with Button here
        //    //当控件是button时
        //    else if (control is System.Windows.Forms.Button)
        //    {
        //        // Cast to a button object
        //        //强制转换为 button
        //        Button button = (Button)control;
        //        // Do not show button text
        //        //不显示button text
        //        button.Text = "";
        //        // Change cursor to hand when over button
        //        //改变 cursor的style
        //        //button.Cursor = Cursors.Hand;
        //        // Set background image of button
        //        //设置button的背景图片
        //        button.BackgroundImage = bitmap;
        //        // Calculate the graphics path based on the bitmap supplied
        //        //计算位图中不透明部分的边界
        //        GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap);
        //        // Apply new region
        //        //应用新的区域
        //        button.Region = new Region(graphicsPath);
        //    }
        //    else if (control is System.Windows.Forms.PictureBox)
        //    {
        //        // Cast to a button object
        //        //强制转换为 button
        //        PictureBox pic = (PictureBox)control;

        //        // Change cursor to hand when over button
        //        //改变 cursor的style
        //        //button.Cursor = Cursors.Hand;
        //        // Set background image of button
        //        //设置button的背景图片
        //        //pic.BackgroundImage = bitmap;
        //        pic.Image = bitmap;
        //        // Calculate the graphics path based on the bitmap supplied
        //        //计算位图中不透明部分的边界
        //        GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap);
        //        // Apply new region
        //        //应用新的区域
        //        pic.Region = new Region(graphicsPath);
        //    }
        //}
        /////
        ///// Calculate the graphics path that representing the figure in the bitmap
        ///// excluding the transparent color which is the top left pixel.
        ///// //计算位图中不透明部分的边界 
        ///// The Bitmap object to calculate our graphics path from
        ///// Calculated graphics path


        /// <summary>
        /// Calculate the graphics path that representing the figure in the bitmap
        /// excluding the transparent color which is the top left pixel.
        /// 计算位图中不透明部分的边界 
        /// The Bitmap object to calculate our graphics path from
        /// Calculated graphics path
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <returns></returns>
        private static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap)
        {
            // Create GraphicsPath for our bitmap calculation
            //创建 GraphicsPath
            GraphicsPath graphicsPath = new GraphicsPath();
            // Use the top left pixel as our transparent color
            //使用左上角的一点的颜色作为我们透明色
            Color colorTransparent = bitmap.GetPixel(0, 0);
            // This is to store the column value where an opaque pixel is first found.
            // This value will determine where we start scanning for trailing opaque pixels.
            //第一个找到点的X
            int colOpaquePixel = 0;
            // Go through all rows (Y axis)
            // 偏历所有行（Y方向）
            for (int row = 0; row < bitmap.Height; row++)
            {
                // Reset value
                //重设
                colOpaquePixel = 0;
                // Go through all columns (X axis)
                //偏历所有列（X方向）
                for (int col = 0; col < bitmap.Width; col++)
                {
                    // If this is an opaque pixel, mark it and search for anymore trailing behind
                    //如果是不需要透明处理的点则标记，然后继续偏历
                    if (bitmap.GetPixel(col, row) != colorTransparent)
                    {
                        // Opaque pixel found, mark current position
                        //记录当前
                        colOpaquePixel = col;
                        // Create another variable to set the current pixel position
                        //建立新变量来记录当前点
                        int colNext = col;
                        // Starting from current found opaque pixel, search for anymore opaque pixels
                        // trailing behind, until a transparent   pixel is found or minimum width is reached
                        ///从找到的不透明点开始，继续寻找不透明点,一直到找到或则达到图片宽度
                        for (colNext = colOpaquePixel; colNext < bitmap.Width; colNext++)
                            if (bitmap.GetPixel(colNext, row) == colorTransparent)
                                break;
                        // Form a rectangle for line of opaque   pixels found and add it to our graphics path
                        //将不透明点加到graphics path
                        graphicsPath.AddRectangle(new Rectangle(colOpaquePixel, row, colNext - colOpaquePixel, 1));
                        // No need to scan the line of opaque pixels just found
                        col = colNext;
                    }
                }
            }
            // Return calculated graphics path
            return graphicsPath;
        }

        /// <summary>
        /// 生成MT系统在Form_Map中使用的图例
        /// </summary>
        /// <param name="myColor"></param>
        /// <param name="iType">0正圆形; 1正方形; </param>
        /// <param name="iWidth">若iHeight不大于0，则为正型。</param>
        /// <param name="iHeight"></param>
        public static Bitmap M_CreateLegend(Color myColor, int iType = 0, int iWidth = 30, int iHeight = 0)
        {
            //myColor = Color.Red;
            //iWidth = 50; 
            try
            {
                if (iHeight <= 0) iHeight = iWidth;
                //Size _mySize = new Size(iWidth, iHeight);
                System.Drawing.Bitmap _bmp0 = new System.Drawing.Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(_bmp0);
                g.Clear(Color.Transparent);
                switch (iType)
                {
                    case 0:
                        g.FillEllipse(new SolidBrush(myColor), 0, 0, iWidth, iHeight);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
                        break;
                    case 1:
                    default:
                        g.FillRectangle(new SolidBrush(myColor), new Rectangle(iWidth / 20, iWidth / 20, iWidth - iWidth / 10, iHeight - iHeight / 10));
                        break;
                }
                return _bmp0;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("方法StringToPic，异常：{0}", ex.Message.ToString()));
                return null;
            }
        } 


        private static string G_sFontName = "宋体";// "宋体";// "微软雅黑";// "锐字锐线梦想黑简1.0";
        public static Font G_Font;
        public static Font G_Font_MainTitle;
         
    }

}
