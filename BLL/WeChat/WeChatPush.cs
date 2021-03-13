using System;
using Newtonsoft.Json.Linq;
using Common;

namespace BLL
{
    public class WeChatPush
    {

        /// <summary>
        /// 验证码推送
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="message"></param>
        public static void LoginCodePush(string openID, string code)
        {
            JObject obj = JObject.FromObject(new
            {
                touser = openID,
                template_id = "2RUDu29ANeKhqFttNDRnS2nbHp63ztERPileyzmDUbs",
                data = JObject.FromObject(new
                {
                    first = JObject.FromObject(new
                    {
                        value = "收到登录验证码",
                        color = "#c20c0c"
                    }),
                    keyword1 = new
                    {
                        value = "登录验证码",
                        color = "#173177"
                    },
                    keyword2 = new
                    {
                        value = $"验证码：{code}",
                        color = "#173177"
                    },
                    remark = new
                    {
                        value = "发送时间：" + DateTime.Now.ToString(),
                        color = "#173177"
                    }
                })
            });

            Log4Helper.WriteLog(WeChatAPI.SendMessage("http://3861520.cn/Interface/MessagePush.asmx/ETPush", openID, "", "收到登录验证码", $"验证码：{code}", "发送时间：" + DateTime.Now.ToString()));
        }

        /// <summary>
        /// 消息推送
        /// </summary>
        public static void SendTips(string openID, string title, string tipsName
            , string content, string remark)
        {
            JObject obj = JObject.FromObject(new
            {
                touser = openID,
                template_id = "2RUDu29ANeKhqFttNDRnS2nbHp63ztERPileyzmDUbs",
                data = JObject.FromObject(new
                {
                    first = JObject.FromObject(new
                    {
                        value = tipsName,
                        color = "#c20c0c"
                    }),
                    keyword1 = new
                    {
                        value = tipsName,
                        color = "#173177"
                    },
                    keyword2 = new
                    {
                        value = content,
                        color = "#173177"
                    },
                    remark = new
                    {
                        value = remark,
                        color = "#173177"
                    }
                })
            });

            Log4Helper.WriteLog(WeChatAPI.SendTemplateMsg(obj.ToString()));
        }
    }
}
