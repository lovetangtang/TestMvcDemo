using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
    public static class VerificationWidget
    {
        public static DbBase handle = new DbBase(ConvertHelper.ToString(ConfigurationManager.ConnectionStrings["SysDbContext"]));

        /// <summary>
        /// 根据编码判断是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">查询字段</param>
        /// <param name="value">字段值</param>
        /// <param name="key">主键</param>
        /// <param name="id">编号值</param>
        /// <returns></returns>
        public static bool IsExit(string tableName, string column, string value, string key, int? id)
        {
            string sql = $@"Select COUNT(*) From {tableName} where IsActive=0 and {column}=@Value";

            if (id.HasValue && id.Value > 0)
            {
                sql += $" and {key} <> {id}";
            }

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = handle.GetSqlParameter("@Value", value);

            return ConvertHelper.ToInt32(handle.GetCell(sql, parameters)) > 0 ? true : false;
        }
      
    }
}
