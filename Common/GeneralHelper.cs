using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace Common
{
    public class GeneralHelper
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="qrCodeContent"></param>
        /// <returns></returns>
        public static Bitmap GenerateQRCode(string qrCodeContent)
        {
            QRCodeEncoder encoder = new QRCodeEncoder
            {
                QRCodeVersion = 0
            };
            Bitmap img = encoder.Encode(qrCodeContent, Encoding.UTF8);

            return img;
        }
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateCode(int length = 4)
        {
            Random rd = new Random();
            string code = string.Empty;

            for (int i = 0; i < length; i++)
            {
                code += ConvertHelper.ToString(rd.Next(0, 9));
            }

            return code;
        }

        /// <summary>
        /// 获取秒级时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetSecondsTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GenerateRandomStr(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }

            return result.ToString();
        }

        /// <summary>
        /// 获取本地地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocal()
        {
            string port = HttpContext.Current.Request.Url.Port == 80 ? "" : ":" + HttpContext.Current.Request.Url.Port;
            return string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host
                     , port);
        }

        #region 消息模板
        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get
            {
                return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>";
            }
        }

        /// <summary>
        /// 图文消息
        /// </summary>
        public static string Message_PicText
        {
            get
            {
                return @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[news]]></MsgType>
                                <ArticleCount>1</ArticleCount>
                                <Articles>
                                <item>
                                <Title><![CDATA[{3}]]></Title> 
                                <Description><![CDATA[{4}]]></Description>
                                <PicUrl><![CDATA[{5}]]></PicUrl>
                                <Url><![CDATA[{6}]]></Url>
                                </item>
                                </Articles>
                        </xml>";
            }
        }
        #endregion
    }
}
