using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ImgHelper
    {
        static private void CombinImage3(string sourceImg, string topImg)
        {
            string folder = FileHelper.GetLocalPath("/Generate");
            Image img1 = Image.FromFile(Path.Combine(sourceImg));//相框图片 
            Image img2 = Image.FromFile(Path.Combine(topImg)); //照片图片               
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics       
            Graphics g = Graphics.FromImage(img1);
            g.DrawImage(img1, 0, 0, 148, 124);// g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
                                              //g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框
                                              //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            g.DrawImage(img2, 17, 17, 112, 73);
            GC.Collect();
            img1.Save(Path.Combine(folder, $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png"));
            img1.Dispose();
        }

        /// <summary>
        /// 根据原图片和字节流二维码
        /// </summary>
        /// <param name="sourceImg"></param>
        /// <param name="topImg"></param>
        static public string CombinImageBitmap(string sourceImg, Bitmap topImg, string context)
        {
            string result = "";
            Image img1 = Image.FromFile(Path.Combine(FileHelper.GetLocalPath(sourceImg)));//相框图片 
            Image img2 = Image.FromHbitmap(topImg.GetHbitmap()); //照片图片       

            try
            {
                FileHelper.CreatePath("/Generate");
                result = $"/Generate/{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png";
                using (Graphics g = Graphics.FromImage(img1))
                {
                    g.DrawImage(img1, 0, 0, 374, 662);// g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
                                                      //g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框
                                                      //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
                    if (!string.IsNullOrEmpty(context))
                        g.DrawString(context, new Font("微软雅黑", 18, FontStyle.Bold), Brushes.White, new Point(100, 615));

                    g.DrawImage(img2, 92, 374, 190, 193);
                    //g.DrawImage(img2, 310, 875, 470, 470);
                    GC.Collect();
                    img1.Save(FileHelper.GetLocalPath(result));
                    img1.Dispose();
                }
            }
            catch (Exception ex)
            {
                result = "";
                Log4Helper.WriteLog("", ex);
            }
            finally
            {
                img1.Dispose();
                img2.Dispose();
            }

            return result;
        }
    }
}
