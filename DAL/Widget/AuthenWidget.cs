using DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DAL
{
    public class AuthenWidget
    {
        /// <summary>
        /// 当前操作人
        /// </summary>
        public static SysUser CurrentOperator
        {
            get
            {
                try
                {
                    var user = ServerHelper.GetSession("user") as SysUser;
                    return user?.IsActive == (int)IsActive.否 ? user : null;
                }
                catch
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// 获取列表操作权限
        /// </summary>
        /// <param name="menuCode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Dictionary<string, bool> GetMyMenuAuth(string menuCode)
        {
            Dictionary<string, bool> list = new Dictionary<string, bool>();
            SysMenuDAL menuhandle = new SysMenuDAL();
            SysUserDAL userHandle = new SysUserDAL();
            var user = userHandle.Detail(ConvertHelper.ToInt32(AuthenWidget.CurrentOperator?.UserId ?? 0));

            foreach (var item in menuhandle.GetMenuAuth(menuCode))
            {
                bool hasAuth = ConvertHelper.StrIn(user.PersonalAuth, item);
                list.Add(item.Split('_')[1], hasAuth);
            }

            return list;
        }

        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public static bool HasAuth(string authCode)
        {
            bool result = false;
            SysUserDAL userHandle = new SysUserDAL();
            var user = userHandle.Detail(ConvertHelper.ToInt32(AuthenWidget.CurrentOperator?.UserId ?? 0));

            result = ConvertHelper.StrIn(user.PersonalAuth, authCode);

            return result;
        }

        public static bool HasAuth(int? userID, string operateCode)
        {
            string auth = RelationWidget.GetUserAuth(userID);
            return ConvertHelper.StrIn(auth, operateCode);
        }




    }
}
