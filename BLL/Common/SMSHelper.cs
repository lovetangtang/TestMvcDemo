using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL
{
    public static class SMSHelper
    {
        public static string userId = ConfigurationManager.AppSettings["MsgUserID"].ToString();
        public static string pwd = ConfigurationManager.AppSettings["MsgPwd"].ToString();
        public static string sign1 = ConfigurationManager.AppSettings["MsgSign1"].ToString();

        /// <summary>
        /// 发送登录验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="operateResult"></param>
        /// <returns></returns>
        public static string SendLoginCode(string phone, out int operateResult)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                {
                    operateResult = OperationWidget.ArgumentError;
                    return "";
                }
                string sign = sign1;
                Random rd = new Random();
                string code = rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString();
                string msg = "验证码为" + code + "，您正在使用手机登录企业文化考试系统，如非本人操作，请忽略。";
                string content = "{\"userid\":\"" + userId + "\""
                                + ",\"pwd\":\"" + pwd + "\""
                                + ",\"mobile\":\"" + phone + "\""
                                + ",\"content\":\"" + HttpUtility.UrlEncode(msg, Encoding.GetEncoding("GBK")) + "\""
                                + ",\"svrtype\":\"" + sign.Trim() + "\"}";

                if (!Send(phone, content))
                {
                    operateResult = OperationWidget.Failed;
                    return "";
                }

                operateResult = OperationWidget.Success;
                return code;
            }
            catch
            {
                operateResult = OperationWidget.Failed;
                return "";
            }
        }


        /// <summary>
        /// 发送登录验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="operateResult"></param>
        /// <returns></returns>
        public static string SendBindCode(string phone, out int operateResult)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                {
                    operateResult = OperationWidget.ArgumentError;
                    return "";
                }
                string sign = sign1;
                Random rd = new Random();
                string code = rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString() + rd.Next(0, 9).ToString();
                string msg = "验证码为" + code + "，您正在使用手机绑定企业文化考试系统员工信息，如非本人操作，请忽略。";
                string content = "{\"userid\":\"" + userId + "\""
                                + ",\"pwd\":\"" + pwd + "\""
                                + ",\"mobile\":\"" + phone + "\""
                                + ",\"content\":\"" + HttpUtility.UrlEncode(msg, Encoding.GetEncoding("GBK")) + "\""
                                + ",\"svrtype\":\"" + sign.Trim() + "\"}";

                if (!Send(phone, content))
                {
                    operateResult = OperationWidget.Failed;
                    return "";
                }

                operateResult = OperationWidget.Success;
                return code;
            }
            catch
            {
                operateResult = OperationWidget.Failed;
                return "";
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool Send(string phone, string content)
        {
            try
            {
                string url = "http://61.145.229.29:7902/sms/v2/std/single_send";
                ServerHelper.SendRequest(url, content);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
