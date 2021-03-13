using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Common
{
    public struct ServerHelper
    {
        public static void SetCache(string name, object val, int expired = 20)
        {
            if (!string.IsNullOrEmpty(name) && HttpRuntime.Cache != null)
            {
                if (HttpRuntime.Cache.Get(name) != null)
                {
                    HttpRuntime.Cache.Remove(name);
                }
                HttpRuntime.Cache.Insert(name, val, null, DateTime.Now.AddMinutes(expired), Cache.NoSlidingExpiration);
            }
        }

        public static object GetCache(string name)
        {
            object result = null;
            if (!string.IsNullOrEmpty(name) && HttpRuntime.Cache != null && HttpRuntime.Cache.Get(name) != null)
            {
                result = HttpRuntime.Cache.Get(name);
            }
            return result;
        }

        public static void RemoveCache(string name)
        {
            if (!string.IsNullOrEmpty(name) && HttpRuntime.Cache != null)
            {
                HttpRuntime.Cache.Remove(name);
            }
        }

        public static void SetCookie(string name, string val, int expired = 20)
        {
            if (!string.IsNullOrEmpty(name) && HttpContext.Current != null)
            {
                HttpCookie httpCookie = new HttpCookie(name, val);
                httpCookie.Expires = DateTime.Now.AddMinutes(expired);
                if (HttpContext.Current.Response.Cookies.Get(name) != null)
                {
                    HttpContext.Current.Response.Cookies.Remove(name);
                }
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
        }

        public static void SetCookie(string name, Dictionary<string, string> map, int expired = 20)
        {
            if (!string.IsNullOrEmpty(name) && HttpContext.Current != null)
            {
                HttpCookie httpCookie = new HttpCookie(name);
                httpCookie.Expires = DateTime.Now.AddMinutes(expired);
                foreach (KeyValuePair<string, string> item in map)
                {
                    httpCookie.Values.Add(item.Key, item.Value);
                }
                if (HttpContext.Current.Response.Cookies.Get(name) != null)
                {
                    HttpContext.Current.Response.Cookies.Remove(name);
                }
                HttpContext.Current.Response.Cookies.Add(httpCookie);
            }
        }

        public static void SetCookie(string name, string key, string val, int expired = 20)
        {
            if (!string.IsNullOrEmpty(key) && HttpContext.Current != null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary[key] = val;
                SetCookie(name, dictionary, expired);
            }
        }

        public static string GetCookie(string name)
        {
            string result = null;
            if (!string.IsNullOrEmpty(name) && HttpContext.Current != null)
            {
                HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get(name);
                if (httpCookie != null)
                {
                    result = httpCookie.Value;
                }
            }
            return result;
        }

        public static string GetCookie(string name, string key)
        {
            string result = null;
            if (!string.IsNullOrEmpty(name) && HttpContext.Current != null)
            {
                HttpCookie httpCookie = HttpContext.Current.Request.Cookies.Get(name);
                if (httpCookie != null && httpCookie.Values.Count > 0)
                {
                    result = httpCookie.Values.Get(key);
                }
            }
            return result;
        }

        public static void RemoveCookie(string name)
        {
            if (!string.IsNullOrEmpty(name) && HttpContext.Current != null)
            {
                HttpContext.Current.Response.Cookies.Remove(name);
            }
        }

        public static void SetSession(string name, object val)
        {
            if (!string.IsNullOrEmpty(name) && HttpContext.Current != null)
            {
                if (HttpContext.Current.Session[name] != null)
                {
                    HttpContext.Current.Session.Remove(name);
                }
                HttpContext.Current.Session.Add(name, val);
            }
        }

        public static object GetSession(string name)
        {
            object result = null;
            if (!string.IsNullOrEmpty(name) && HttpContext.Current != null)
            {
                result = HttpContext.Current.Session[name];
            }
            return result;
        }

        public static void RemoveSession(string name)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[name] != null)
            {
                HttpContext.Current.Session.Remove(name);
            }
        }

        public static string GetClientIP()
        {
            string text = string.Empty;
            if (HttpContext.Current != null)
            {
                text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (text == null || text == string.Empty)
                {
                    text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (text == null || text == string.Empty)
                {
                    text = HttpContext.Current.Request.UserHostAddress;
                }
            }
            return text;
        }

        public static void Write(string outputString, string mimeType = "application/json")
        {
            if (HttpContext.Current != null)
            {
                HttpResponse response = HttpContext.Current.Response;
                response.ClearContent();
                response.ContentType = mimeType;
                response.Write(outputString);
                response.End();
            }
        }

        public static string SendRequest(string url, string data)
        {
            string result = null;
            byte[] _buffer = Encoding.GetEncoding("utf-8").GetBytes(data);
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(url);
            _req.Method = "Post";
            _req.ContentType = "application/json;charset=utf-8";
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
                result = _resSR.ReadToEnd();
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
                if (_resSR != null)
                {
                    _resSR.Close();
                }
                if (_resStream != null)
                {
                    _resStream.Close();
                }
            }

            return result;
        }
    }
}
