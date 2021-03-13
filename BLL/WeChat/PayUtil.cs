using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BLL
{
    /// <summary>
    /// TenpayUtil 的摘要说明。
    /// 配置文件
    /// </summary>
    public class PayUtil
    {
        /* 微信公众号信息配置
       * APPID：绑定支付的APPID（必须配置）
       * MCHID：商户号（必须配置）
       * KEY：商户支付密钥，参考开户邮件设置（必须配置）
       * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
       */
        public static string APPID = "";
        public static string MCHID = "";
        public static string KEY = "";
        public static string APPSECRET = "";

        public static string CollectAPPID = "";
        public static string CollectMCHID = "";
        public static string CollectKEY = "";
        public static string CollectAPPSECRET = "";

        ////=======【支付结果通知url】===================================== 
        ///* 支付结果通知回调url，用于商户接收支付结果
        //*/
        public static string NOTIFY_URL = GetUrl() + "/Common/WechatNotify";
        public static string NetNOTIFY_URL = GetUrl() + "/Common/NetPayCallBack";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public static string IP = GetUserIP();

        static PayUtil()
        {

            APPID = ConfigurationManager.AppSettings["AppKey"].ToString();
            APPSECRET = ConfigurationManager.AppSettings["AppSecret"].ToString();
            
            MCHID = "";// ConfigurationManager.AppSettings["MerchID"].ToString();
            KEY = "";//ConfigurationManager.AppSettings["MerchSecret"].ToString();

            CollectAPPID = "";//ConfigurationManager.AppSettings["CollectAppKey"].ToString();
            CollectAPPSECRET = "";//ConfigurationManager.AppSettings["CollectAppSecret"].ToString();
            CollectMCHID = "";//ConfigurationManager.AppSettings["CollectMerchID"].ToString();
            CollectKEY = "";//ConfigurationManager.AppSettings["CollectMerchSecret"].ToString();
        }

        /** 获取客户端IP地址 */
        public static string GetUserIP()
        {
            System.Web.HttpContext context = HttpContext.Current;
            string userIP = string.Empty;
            if (context.Request.ServerVariables["HTTP_VIA"] == null)
            {
                userIP = context.Request.UserHostAddress;
            }
            else
            {
                userIP = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (userIP == null) userIP = "";
            return userIP;
        }

        /** 获取大写的MD5签名结果 */
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        public static string GetNoncestr()
        {
            Random random = new Random();
            return GetMD5(random.Next(1000).ToString(), "GBK");
        }
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /** 对字符串进行URL编码 */
        public static string UrlEncode(string instr, string charset)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;
            }
        }

        /** 对字符串进行URL解码 */
        public static string UrlDecode(string instr, string charset)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;

            }
        }

        /** 取时间戳生成随即数,替换交易单号中的后10位流水号 */
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }
        /** 取随机数 */
        public static string BuildRandomStr(int length)
        {
            Random rand = new Random();

            int num = rand.Next();

            string str = num.ToString();

            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str.Insert(0, "0");
                    n--;
                }
            }

            return str;
        }

        #region 获取当前请求的域名

        /// <summary>
        ///获取当前请求的域名 
        /// </summary>
        /// <returns>http://localhost:1234</returns>
        public static string GetUrl()
        {

            string url = string.Format("{0}{1}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? "" : ":" + HttpContext.Current.Request.Url.Port.ToString());

            if (url.ToLower().IndexOf("http://") != 0)
            {
                url = "http://" + url;
            }
            return url;
        }

        #endregion

        /// <summary>
        ///生成制定位数的随机码（数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }

            return result.ToString();
        }
    }
}