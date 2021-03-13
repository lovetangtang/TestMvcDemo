using BLL;
using BLL.WeChat;
using Common;
using DBModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestMvcDemo.Admin.Controllers
{
    public class HomeController : Controller
    {
        public readonly SysUserBLL suhandle;
        public HomeController()
        {
            suhandle = new SysUserBLL();
        }

        [AdminAuthorize]
        public ActionResult Index()
        {
            ViewBag.UserName = (Session["user"] as SysUser).UserName;
            return View();
        }

        public ActionResult Login(string code)
        {
            ViewBag.OpenID = WeChatAPI.GetOpenID(code);
            return View();
        }

        public ActionResult Bind(string code, int? state)
        {
            string url = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={WeChatCommon.appid}&redirect_uri={GeneralHelper.GetLocal()}/Home/Bind&response_type=code&scope=snsapi_userinfo&agentid=1000084&state=1#wechat_redirect";
            return Redirect(url);
        }

        public ActionResult NoAuth()
        {
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }
      

        #region 登录

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public ActionResult SendLoginCode(string phone)
        {
            var userM = suhandle.GetDetail(phone);
            if (userM == null)
            {
                return Json(new { ResultCode = OperationWidget.NoRecord });
            }

            if (string.IsNullOrEmpty(userM.OpenId))
            {
                return Json(new { ResultCode = OperationWidget.LoginExpired });
            }

            string code = GeneralHelper.GenerateCode();//"123456";// 
            Session["code"] = code;

            WeChatPush.LoginCodePush(userM.OpenId, code);

            return Json(new { ResultCode = OperationWidget.Success });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public ActionResult UserLogin(string phone, string code)
        {
            string correctCode = ConvertHelper.ToString(Session["code"]);
            if (code != correctCode)
            {
                return Json(new { ResultCode = OperationWidget.CodeError });
            }

            var userM = suhandle.GetDetail(phone);

            Session["user"] = userM;
            Session["code"] = null;

            return Json(new { ResultCode = OperationWidget.Success });
        }

        /// <summary>
        /// 生成登录二维码
        /// </summary>
        public ActionResult GenerateQRCode()
        {
            //string url = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={WeChatCommon.appid}&redirect_uri={GeneralHelper.GetLocal()}/AppHome/Bind&response_type=code&scope=snsapi_userinfo&agentid=1000084&state=1#wechat_redirect";
            string url = $"http://3861520.cn/Culture/Index";
            Bitmap bitmap = GeneralHelper.GenerateQRCode(url);
            var qrCode = ConvertHelper.ToBase64String(bitmap);
            return Json(new { data = qrCode }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session["user"] = null;

            return Json(new
            {
                ResultCode = OperationWidget.Success
            });
        }
        #endregion
    }
}