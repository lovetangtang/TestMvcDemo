using BLL.WeChat;
using Common;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace BLL
{
    public class WeChatAPI
    {
        public static double WebCacheTime = 6000;
        private static readonly System.Web.Caching.Cache _cache;
        static WeChatAPI()
        {
            HttpContext current = HttpContext.Current;

            if (current != null)
            {
                _cache = current.Cache;//当前web中的缓存
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <returns></returns>
        public static Result GetJsApi_Ticket()
        {
            var result = new Result();
            try
            {
                string jsapi_ticket = (string)_cache["jsapi_ticket"];

                if (string.IsNullOrEmpty(jsapi_ticket))
                {
                    string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + GetToken() + "&type=jsapi";

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

                    webRequest.Method = "get";

                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                    jsapi_ticket = sr.ReadToEnd();

                    JObject obj = JObject.Parse(jsapi_ticket);
                    if ((string)obj["errmsg"] != "ok")
                    {
                        result.Status = false;
                        result.Message = (string)obj["errmsg"];
                        return result;
                    }
                    else
                    {
                        jsapi_ticket = (string)obj["ticket"];
                        result.Status = true;
                        result.Data = jsapi_ticket;
                        //添加缓存
                        _cache.Add("jsapi_ticket", jsapi_ticket, null, DateTime.Now.AddSeconds(WebCacheTime), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    }
                }
                else
                {
                    result.Status = true;
                    result.Data = jsapi_ticket;
                }
            }
            catch (Exception ce)
            {
                result.Status = false;
                result.Message = ce.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取OpenID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetOpenID(string code)
        {
            string openid = "";

            try
            {
                //获取token
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WeChatCommon.appid + "&secret=" + WeChatCommon.secret + "&code=" + code + "&grant_type=authorization_code";
                string jsonText = SendWX(url);
                Log4Helper.WriteLog(jsonText);
                JObject obj = (JObject)JsonConvert.DeserializeObject(jsonText);

                if (obj != null && !string.IsNullOrEmpty(ConvertHelper.ToString(obj["openid"])))
                {
                    openid = ConvertHelper.ToString(obj["openid"]);
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("获取OpenID失败", ex);
            }

            return openid;
        }

        public static string GetUserOpenID(string code)
        {
            string openid = "";

            try
            {
                Log4Helper.WriteLog("appid=" + WeChatCommon.appid);
                Log4Helper.WriteLog("secret=" + WeChatCommon.secret);
                //获取token
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WeChatCommon.appid + "&secret=" + WeChatCommon.secret + "&code=" + code + "&grant_type=authorization_code";
                string jsonText = SendWX(url);
                Log4Helper.WriteLog(jsonText);
                JObject obj = (JObject)JsonConvert.DeserializeObject(jsonText);

                if (obj != null && !string.IsNullOrEmpty(ConvertHelper.ToString(obj["openid"])))
                {
                    openid = ConvertHelper.ToString(obj["openid"]);
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("获取OpenID失败", ex);
            }

            return openid;
        }

        public static object GetSDKConfig(string currentUrl, out int resultCode)
        {
            resultCode = OperationWidget.Failed;

            try
            {
                //时间戳
                string timestamp = GeneralHelper.GetSecondsTimestamp();
                //随机字符串
                string nonceStr = GeneralHelper.GenerateRandomStr(16);

                //jsapi_ticket
                var result = GetJsApi_Ticket();
                if (result.Status)
                {
                    Hashtable hht = new Hashtable();
                    hht.Add("noncestr", nonceStr);
                    hht.Add("jsapi_ticket", result.Data);
                    hht.Add("timestamp", timestamp);
                    hht.Add("url", currentUrl);

                    StringBuilder sb = new StringBuilder();
                    ArrayList akeys = new ArrayList(hht.Keys);
                    akeys.Sort();

                    foreach (string k in akeys)
                    {
                        string v = (string)hht[k];

                        sb.Append(k + "=" + v + "&");
                    }

                    string tmpStr = sb.ToString().TrimEnd(new char[] { '&' });
                    tmpStr = WeChatCommon.GetSha1(tmpStr).ToLower();

                    var data = new
                    {
                        WeChatCommon.appid,
                        timestamp,
                        nonceStr,
                        signature = tmpStr
                    };

                    resultCode = OperationWidget.Success;
                    return data;
                }
                else
                {
                    return new { };
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("获取jssdk配置信息", ex);
                return new { };
            }
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            string access_token = "";
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WeChatCommon.appid + "&secret=" + WeChatCommon.secret;
            string jsonText = SendWX(url);
            JObject obj = (JObject)JsonConvert.DeserializeObject(jsonText);

            try
            {
                if (obj != null && !string.IsNullOrEmpty(ConvertHelper.ToString(obj["access_token"])))
                {
                    access_token = obj["access_token"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("GetToken错误", ex);
            }

            return access_token;
        }

        /// <summary>
        /// 获取顾客信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        //public static Customer GetCustomer(string code)
        //{
        //    string access_token = "";
        //    string openid = "";
        //    Customer user = new Customer();
        //    //获取token
        //    string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WeChatCommon.appid + "&secret=" + WeChatCommon.secret + "&code=" + code + "&grant_type=authorization_code&scope=snsapi_userinfo";
        //    string jsonText = SendWX(url);
        //    JObject obj = (JObject)JsonConvert.DeserializeObject(jsonText);

        //    if (!string.IsNullOrEmpty(obj["access_token"].ToString()))
        //    {
        //        access_token = obj["access_token"].ToString();
        //        openid = obj["openid"].ToString();
        //        url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
        //        jsonText = SendWX(url);
        //        obj = (JObject)JsonConvert.DeserializeObject(jsonText);
        //        if (!string.IsNullOrEmpty((string)obj["openid"]))
        //        {
        //            user.OpenID = obj["openid"].ToString();
        //            user.WeChatImg = obj["headimgurl"].ToString();
        //        }
        //    }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="data">发送的模板数据</param>
        /// <returns></returns>
        public static string SendTemplateMsg(string data)
        {
            string accessToken = GetToken();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accessToken);
            HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            byte[] payload;
            payload = Encoding.UTF8.GetBytes(data); //通过UTF-8编码
            hwr.ContentLength = payload.Length;
            Stream writer = hwr.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            var result = hwr.GetResponse() as HttpWebResponse; //此句是获得上面URl返回的数据
            string strMsg = WebResponseGet(result);
            return strMsg;
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="WXID"></param>
        /// <param name="Url"></param>
        /// <param name="first"></param>
        /// <param name="keyword1"></param>
        /// <param name="keyword2"></param>
        /// <returns></returns>
        public static string SendMessage(string url, string WXID, string Url, string first, string keyword1, string keyword2)
        {
            String result = null;
            XmlTextReader Reader = null;
            string param = HttpUtility.UrlEncode("WXID") + "=" + HttpUtility.UrlEncode(WXID) + "&" + HttpUtility.UrlEncode("Url") + "=" + HttpUtility.UrlEncode(Url)
                + "&" + HttpUtility.UrlEncode("first") + "=" + HttpUtility.UrlEncode(first)
                 + "&" + HttpUtility.UrlEncode("keyword1") + "=" + HttpUtility.UrlEncode(keyword1)
                  + "&" + HttpUtility.UrlEncode("keyword2") + "=" + HttpUtility.UrlEncode(keyword2);
            byte[] _buffer = Encoding.GetEncoding("utf-8").GetBytes(param);
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(url);
            _req.Method = "Post";
            _req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            _req.ContentLength = _buffer.Length;
            Stream _stream = null;
            Stream _resStream = null;
            StreamReader _resSR = null;
            try
            {
                _stream = _req.GetRequestStream();
                _stream.Write(_buffer, 0, _buffer.Length);
                _stream.Flush();
                HttpWebResponse _res = (HttpWebResponse)_req.GetResponse();

                //获取响应
                _resStream = _res.GetResponseStream();
                _resSR = new StreamReader(_resStream, Encoding.GetEncoding("utf-8"));
                Reader = new XmlTextReader(_resSR);
                Reader.MoveToContent();
                result = Reader.ReadInnerXml();
                Reader.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                if (_stream != null)
                {
                    _stream.Close();
                }
                if (Reader != null)
                {
                    Reader.Close();
                }
                if (_resStream != null)
                {
                    _resStream.Close();
                }
            }
            return result;
        }

        //    return user;
        //}
        public static string SendWX(string strUrl)
        {
            string content = "";

            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);  //用GET形式请求指定的地址
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    content = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("SendWX错误", ex);
            }

            return content;
        }
        public static string WebResponseGet(HttpWebResponse webResponse)
        {
            StreamReader responseReader = null;
            string responseData = "";
            try
            {
                responseReader = new StreamReader(webResponse.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webResponse.GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }
    }
}