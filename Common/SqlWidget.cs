using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class SqlWidget
    {
        #region Sql
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="overID">根据ID计算行数</param>
        /// <param name="startNo">开始行数</param>
        /// <param name="endNo">截至行数</param>
        /// <param name="columnNames">列名</param>
        /// <returns></returns>
        public static string GetMgtSql(string sql, Grid gvMgt, string columnNames = null)
        {
            //从0开始
            int startNo = Math.Max(gvMgt.PageSize * (gvMgt.PageNo - 1) + 1, 1);
            int endNo = Math.Min(gvMgt.PageSize * (gvMgt.PageNo), gvMgt.RecordCount);

            //默认查询主表
            if (string.IsNullOrEmpty(columnNames))
            {
                sql = string.Format(sql.Replace("(RowNum)", "ROW_NUMBER() over(order by {0} {1}) RowID,t.*"), gvMgt.SortFiled, gvMgt.SortDirection);
            }
            else
            {
                sql = string.Format(sql.Replace("(RowNum)", "ROW_NUMBER() over(order by {0} {1}) RowID,{2}"), gvMgt.SortFiled, gvMgt.SortDirection, columnNames);
            }

            string mgtSql = string.Format(@"Select tb.* from({0}) tb where tb.RowID Between {1} And {2}", sql, startNo, endNo);

            return mgtSql;
        }

        /// <summary>
        /// 列表数量sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="orderID"></param>
        /// <param name="dicretion"></param>
        /// <returns></returns>
        public static string GetMgtTotal(string sql, string countID)
        {
            string countStr = string.Format("Count({0})", countID);
            sql = sql.Replace("(RowNum)", countStr);

            return sql;
        }

        /// <summary>
        /// 获取筛选Sql
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static string GetFilter(Dictionary<string, string> map)
        {
            if (map == null)
            {
                return string.Empty;
            }

            string filter = string.Empty;
            foreach (var item in map)
            {
                if (!string.IsNullOrEmpty(item.Value) && item.Key != "Keyword")
                {
                    if (item.Key.Contains("Time"))
                    {
                        filter += $" And DATEDIFF(dd,{item.Key},@{item.Key.Substring(item.Key.IndexOf('.') + 1)})=0";
                    }
                    else
                    {
                        filter += $" And {item.Key} = @{item.Key.Substring(item.Key.IndexOf('.') + 1)}";
                    }
                }
            }

            return filter;
        }
        #endregion
    }
}
