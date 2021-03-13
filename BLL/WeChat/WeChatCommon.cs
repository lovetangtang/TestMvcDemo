using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.WeChat
{
    public class WeChatCommon
    {
        /// <summary>
        /// 第三方用户唯一凭证 
        /// </summary>
        public static string appid = "";

        /// <summary>
        /// 第三方用户唯一凭证密钥
        /// </summary>
        public static string secret = "";

        #region 读取配置文件

        static WeChatCommon()
        {
            appid = ConfigurationManager.AppSettings["AppKey"].ToString();
            secret = ConfigurationManager.AppSettings["AppSecret"].ToString();
        }
        #endregion

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        #region 生成sha1

        /// <summary>
        /// 生成sha1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSha1(string str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash;
        }
        #endregion

        #region 读取返回码
        public static string GetReturnCode(string json)
        {
            var code = JObject.Parse(json);
            return code["errcode"].ToString();
        }
        #endregion
    }
}
