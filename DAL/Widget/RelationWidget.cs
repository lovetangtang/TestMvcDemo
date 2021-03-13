using Common;
using DBModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class RelationWidget
    {
        public static DbBase handle = new DbBase(ConvertHelper.ToString(ConfigurationManager.ConnectionStrings["SysDbContext"]));
        public static SysDbContext DbContext = new SysDbContext();
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static string GetUserAuth(int? roleID)
        {
            if (!roleID.HasValue || roleID <= 0)
            {
                return string.Empty;
            }

            string sql = "Select Auth From SysRole Where RoleID = @RoleID";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = handle.GetSqlParameter("@RoleID", roleID);

            return ConvertHelper.ToString(handle.GetCell(sql, parameters));
        }

        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetUserName(int? userID)
        {
            if (!userID.HasValue || userID <= 0)
            {
                return string.Empty;
            }

            string sql = "Select UserName From SysUser Where UserID = @UserID";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = handle.GetSqlParameter("@UserID", userID);

            return ConvertHelper.ToString(handle.GetCell(sql, parameters));
        }

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>
        public static int GetDictValue(string dictCode)
        {
            if (string.IsNullOrEmpty(dictCode))
            {
                return 0;
            }

            string sql = "Select DictValue From SysDictionary Where IsActive=0 And DictCode = @DictCode";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = handle.GetSqlParameter("@DictCode", dictCode);

            return ConvertHelper.ToInt32(handle.GetCell(sql, parameters));
        }

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>
        public static string GetDictName(string dictCode,int? dictId)
        {
            if (string.IsNullOrEmpty(dictCode)||!dictId.HasValue)
            {
                return "";
            }

            string sql = "Select DictName From SysDictionary Where IsActive=0  and DictId=@DictId";

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = handle.GetSqlParameter("@DictCode", dictCode);
            parameters[1] = handle.GetSqlParameter("@DictId", dictId);

            return ConvertHelper.ToString(handle.GetCell(sql, parameters));
        }


        #region SysMenu
        /// <summary>
        /// 根据编号获取菜单名称
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public static string GetMenuName(int? menuID)
        {
            if (menuID <= 0)
            {
                return string.Empty;
            }

            string sql = "Select MenuName From SysMenu Where MenuID=@MenuID";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = handle.GetSqlParameter("@MenuID", menuID);

            return ConvertHelper.ToString(handle.GetCell(sql, parameters));
        }
        #endregion
    }
}
