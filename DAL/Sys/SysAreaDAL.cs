
using Common;
using DBModel;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SysAreaDAL : DbBasic
    {
        public SysAreaDAL()
        {
        }

        #region Read
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="gvMgt"></param>
        /// <returns></returns>
        public List<SysArea> GetMgt(Grid gvMgt, Dictionary<string, string> map)
        {
            string filter = SqlWidget.GetFilter(map);
            string sql = $"Select (RowNum) From SysArea t Where t.IsActive = 0 And AreaName like @Keyword {filter}";

            string sqlCount = SqlWidget.GetMgtTotal(sql, "t.AreaID");
            gvMgt.RecordCount = ConvertHelper.ToInt32(this.DbInstance.GetCell(sqlCount, GetParameters(map)));

            string sqlMgt = SqlWidget.GetMgtSql(sql, gvMgt);
            return this.DbContext.SysArea.SqlQuery(sqlMgt, GetParameters(map)).ToList();
        }

        public SqlParameter[] GetParameters(Dictionary<string, string> map)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (var item in map)
            {
                parameters.Add(this.DbInstance.GetSqlParameter($"@{item.Key.Substring(item.Key.IndexOf('.') + 1)}", item.Value));
            }

            return parameters.ToArray();
        }

        /// <summary>
        /// 获取大区
        /// </summary>
        /// <returns></returns>
        public List<SysArea> GetList()
        {
            string sql = "Select * From SysArea Where IsActive = 0";

            return this.DbContext.SysArea.SqlQuery(sql).ToList();
        }

        public SysArea Detail(int areaID)
        {
            string sql = "Select * From SysArea Where IsActive = 0 And AreaID=@AreaID";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@AreaID", areaID);

            return this.DbContext.SysArea.SqlQuery(sql, parameters).FirstOrDefault();
        }

        public SysArea GetDefault()
        {
            string sql = $"Select * From SysArea Where IsActive = 0 And IsDefault = {(int)TrueOrFalse.True}";

            return this.DbContext.SysArea.SqlQuery(sql).FirstOrDefault();
        }

        /// <summary>
        /// 根据大区名称获取详情
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public SysArea Detail(string areaName)
        {
            string sql = "Select * From SysArea Where IsActive = 0 And AreaName=@AreaName";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@AreaName", areaName);

            return this.DbContext.SysArea.SqlQuery(sql, parameters).FirstOrDefault();
        }
        
        #endregion

        #region Write
        public int Add(SysArea entity)
        {
            entity.IsActive = (int)IsActive.否;
            entity.Creator = AuthenWidget.CurrentOperator?.UserId;
            entity.CreateTime = DateTime.Now;
            entity.Editor = AuthenWidget.CurrentOperator?.UserId;
            entity.EditTime = DateTime.Now;

            this.DbContext.SysArea.Add(entity);
            this.DbContext.Entry(entity).State = EntityState.Added;

            if (this.DbContext.SaveChanges() <= 0)
            {
                return OperationWidget.Failed;
            }

            return OperationWidget.Success;
        }

        public int Edit(SysArea entity)
        {
            entity.EditTime = DateTime.Now;
            entity.Editor = AuthenWidget.CurrentOperator?.UserId;

            this.DbContext.SysArea.Attach(entity);
            this.DbContext.Entry(entity).State = EntityState.Modified;

            return this.DbContext.SaveChanges() <= 0 ? OperationWidget.Failed : OperationWidget.Success;
        }
        #endregion
    }
}