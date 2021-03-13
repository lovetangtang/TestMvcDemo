using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class FileHelper
    {
        public static string SaveFile(HttpPostedFileBase file)
        {
            string result = "";

            try
            {
                string directory = HttpRuntime.AppDomainAppPath.ToString();
                string path = "/Upload";
                string extension = Path.GetExtension(file.FileName);

                CreateCatalog(directory, path);

                path = string.Format("{0}/{1}", path, DateTime.Now.ToString("yyyyMMdd"));
                CreateCatalog(directory, path);

                path = string.Format("{0}/{1}{2}{3}", path, DateTime.Now.ToString("yyyyMMddHHmmssfff"), new Random(GetRandomSeed()).Next(1, 1000), extension);
                file.SaveAs(directory + path.Replace("/", "\\"));

                if (File.Exists(directory + path.Replace("/", "\\")))
                {
                    result = path;
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("保存文件失败", ex);
            }

            return result;
        }

        public static string SaveFileForBase64ToPNG(string file)
        {
            return SaveFileForBase64(file, "demo.png");
        }

        public static string SaveFileForBase64(string file, string fileName)
        {
            string result = "";

            try
            {
                string directory = HttpRuntime.AppDomainAppPath.ToString();
                string path = "/Upload";
                byte[] content = Convert.FromBase64String(file.Substring(file.IndexOf("base64,") + 7));//切除前面那段image标识
                string extension = Path.GetExtension(fileName);

                CreateCatalog(directory, path);

                path = string.Format("{0}/{1}", path, DateTime.Now.ToString("yyyyMMdd"));
                CreateCatalog(directory, path);

                path = string.Format("{0}/{1}{2}{3}", path, DateTime.Now.ToString("yyyyMMddHHmmssfff"), new Random(GetRandomSeed()).Next(1, 1000), extension);

                using (MemoryStream ms = new MemoryStream(content))
                {
                    Bitmap bm = new Bitmap(ms);
                    bm.Save(directory + path.Replace("/", "\\"));
                }

                if (File.Exists(directory + path.Replace("/", "\\")))
                {
                    result = path;
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("保存文件失败", ex);
            }

            return result;
        }

        /// <summary>
        /// 描 述:创建加密随机数生成器 生成强随机种子
        /// </summary>
        /// <returns></returns>
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 生成目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        private static void CreateCatalog(string directory, string path)
        {

            string catalog = directory + path.Replace("/", "\\");

            if (!Directory.Exists(catalog))
            {
                Directory.CreateDirectory(catalog);
            }
        }

        public static bool DeleteFile(string path)
        {
            bool result = false;

            try
            {
                string directory = HttpRuntime.AppDomainAppPath.ToString();
                string url = string.Format("{0}{1}", directory, path.Replace("/", "\\"));

                if (File.Exists(url))
                {
                    File.SetAttributes(url, FileAttributes.Normal);
                    File.Delete(url);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("删除文件错误", ex);
            }
            Log4Helper.WriteLog("删除文件：" + path);

            return result;
        }

        public static string GetFilePath(string url)
        {
            string port = HttpContext.Current.Request.Url.Port == 80 ? "" : ":" + HttpContext.Current.Request.Url.Port;
            return string.Format("http://{0}{1}{2}", HttpContext.Current.Request.Url.Host
                     , port, url);
        }

        /// <summary>
        /// 获取本地路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetLocalPath(string url)
        {
            string directory = HttpRuntime.AppDomainAppPath.ToString();
            string result = string.Format("{0}{1}", directory, url.Replace("/", "\\"));

            return result;
        }

        /// <summary>
        /// 确认路径是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CreatePath(string url)
        {
            string result = "";

            if (!string.IsNullOrEmpty(url))
            {
                List<string> nodes = url.Split('/').ToList();
                string directory = HttpRuntime.AppDomainAppPath.ToString();

                foreach (string node in nodes)
                {
                    result += "/" + node;
                    CreateCatalog(directory, result);
                }
            }

            return result;
        }

        /// <summary>
        /// 压缩并保存图片
        /// </summary>
        /// <returns></returns>
        public static string CompressImg(HttpPostedFileBase file, int size)
        {
            var type = file.GetType();
            string directory = HttpRuntime.AppDomainAppPath.ToString();
            string path = "/Upload";
            string extension = Path.GetExtension(file.FileName);

            CreateCatalog(directory, path);

            path = string.Format("{0}/{1}", path, DateTime.Now.ToString("yyyyMMdd"));

            CreateCatalog(directory, path);

            path = string.Format("{0}/{1}{2}{3}", path, DateTime.Now.ToString("yyyyMMddHHmmssfff"), new Random(GetRandomSeed()).Next(1, 1000), extension);

            GC.Collect();

            return CreateThumbnail(GetLocalPath(path), size, file.InputStream) ? path : "";
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbnailPath"></param>
        /// <param name="size"></param>
        public static bool CopyThumbnailFile(string originalImagePath, string thumbnailPath, int size)
        {
            bool result = false;
            GC.Collect();
            FileStream fs = new FileStream(originalImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//创建文件流
            result = CreateThumbnail(thumbnailPath, size, fs);

            return result;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="thumbnailPath"></param>
        /// <param name="size"></param>
        /// <param name="fs"></param>
        /// <returns></returns>
        private static bool CreateThumbnail(string thumbnailPath, int size, Stream fs)
        {
            bool result;
            Image originalImage = Image.FromStream(fs);


            int towidth = 0;
            int toheight = 0;

            int x = 0;

            int y = 0;

            int ow = originalImage.Width;
            int oh = originalImage.Height;

            //设置图片大小
            if (size == 0 || (size >= ow && size >= oh))
            {
                //使用原始图片的尺寸
                towidth = ow;
                toheight = oh;
            }
            else
            {
                //安比例缩放
                if (originalImage.Width > originalImage.Height)
                {
                    //表示横照片
                    towidth = size;
                    toheight = originalImage.Height * size / originalImage.Width;
                }
                else
                {
                    //表示竖照片
                    toheight = size;
                    towidth = originalImage.Width * size / originalImage.Height;
                }
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(x, y, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                File.SetAttributes(thumbnailPath, FileAttributes.Normal);
                result = true;
            }
            catch (System.Exception e)
            {
                result = false;
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
                fs.Close();
                fs.Dispose();
                //强制回收资源
                GC.Collect();
            }

            return result;
        }
    }
}
