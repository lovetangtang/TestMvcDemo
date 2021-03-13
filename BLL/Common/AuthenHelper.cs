using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AuthenHelper
    {
        /// <summary>
        /// 获取列表操作权限
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public static Dictionary<string, bool> GetMyMenuAuth(string menuCode)
        {
            return AuthenWidget.GetMyMenuAuth(menuCode);
        }

        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public static bool HasAuth(string authCode)
        {
            return AuthenWidget.HasAuth(authCode);
        }
    }
}
