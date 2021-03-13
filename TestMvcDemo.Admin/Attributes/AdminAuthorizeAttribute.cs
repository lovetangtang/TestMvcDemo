using BLL;
using Common;
using DBModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestMvcDemo.Admin
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public bool isPc = true;
        public string operateCode = string.Empty;
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            var user = (SysUser)filterContext.HttpContext.Session["user"];

            if (user == null)
            {
                string code = filterContext.HttpContext.Request["code"];

                if (string.IsNullOrEmpty(code) && isPc)
                {
                    filterContext.Result = new RedirectResult("/Home/Login");
                }
                else
                {
                    var request = filterContext.HttpContext.Request;
                    SysUserBLL handle = new SysUserBLL();
                    int environmentMode = ConvertHelper.ToInt32(ConfigurationManager.AppSettings["EnvironmentMode"]);
                    string openID = ConvertHelper.ToString(request.QueryString["OpenID"]);
                    if (!string.IsNullOrEmpty(openID) && environmentMode == (int)EnvironmentMode.Development) // 开发模式
                    {
                        user = handle.DetailByOpenID(openID);
                    }
                    else
                    {
                        string openId = WeChatAPI.GetOpenID(code);
                        if (string.IsNullOrEmpty(openId))
                        {
                            string toUrl = FileHelper.GetFilePath("/Home/Index");
                            string url = @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={1}&redirect_uri={0}&response_type=code&scope=snsapi_userinfo&state=1&connect_redirect=1#wechat_redirect";
                            string redirectUrl = string.Format(url, toUrl, ConfigurationManager.AppSettings["AppKey"]);
                            filterContext.Result = new RedirectResult(redirectUrl);
                        }
                        else
                        {
                            user = handle.DetailByOpenID(openId);
                        }
                    }
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(operateCode) && !AuthenHelper.HasAuth(operateCode))
                        {
                            filterContext.Result = new RedirectResult("/Home/NoRole");
                        }
                        else
                        {
                            filterContext.HttpContext.Session["user"] = user;
                            filterContext.HttpContext.Session.Timeout = 30;
                        }
                    }
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(operateCode) && !AuthenHelper.HasAuth(operateCode))
                {
                    filterContext.Result = new RedirectResult("/Home/NoAuth");
                }
                else
                {
                    filterContext.HttpContext.Session["user"] = user;
                    filterContext.HttpContext.Session.Timeout = 30;
                }
                filterContext.HttpContext.Session.Timeout = 30;
            }
        }
    }
}