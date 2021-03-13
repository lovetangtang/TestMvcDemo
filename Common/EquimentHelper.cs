using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class EquimentHelper
    {
        /// <summary>
        /// 是否为移动端
        /// </summary>
        /// <returns></returns>
        public static bool ChangeEquipment()
        {
            bool result = false;
            string agent = HttpContext.Current.Request.UserAgent;
            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "UCBrowser" };
            //排除 Windows 桌面系统 ，主要标志字符串为Windows NT
            if (!agent.Contains("Windows NT") || (agent.Contains("Windows NT") && agent.Contains("compatible; MSIE 9.0;")))
            {
                //排除 苹果桌面系统 ,主要标志字符串为Macintosh
                if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
                {
                    foreach (string item in keywords)
                    {
                        if (agent.Contains(item))  //符合Android", "iPhone", "iPod", "iPad", "Windows Phone", "UCBrowser"的，返回true
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
