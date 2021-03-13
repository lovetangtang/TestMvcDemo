
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
    public class SysUserDAL : DbBasic
    {
        public SysUserDAL()
        {
        }

        #region Read
        /// <summary>
        /// ∑÷“≥¡–±Ì
        /// </summary>
        /// <param name="gvMgt"></param>
        /// <returns></returns>
        public List<SysUser> GetMgt(Grid gvMgt, Dictionary<string, string> map)
        {
            string filter = SqlWidget.GetFilter(map);
            string sql = $"Select (RowNum) From SysUser t Where t.IsActive = 0 and UserName like @Keyword {filter}";

            string sqlCount = SqlWidget.GetMgtTotal(sql, "t.UserId");
            gvMgt.RecordCount = ConvertHelper.ToInt32(this.DbInstance.GetCell(sqlCount, GetParameters(map)));

            string sqlMgt = SqlWidget.GetMgtSql(sql, gvMgt);
            return this.DbContext.SysUser.SqlQuery(sqlMgt, GetParameters(map)).ToList();
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

        public SysUser Detail(int userId)
        {
            string sql = "Select * From SysUser Where IsActive = 0 And UserId=@UserId";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@UserId", userId);

            return this.DbContext.SysUser.SqlQuery(sql, parameters).FirstOrDefault();
        }
        public SysUser Detail(string phone)
        {
            string sql = "Select * From SysUser Where IsActive = 0 And phone=@phone";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@phone", phone);

            return this.DbContext.SysUser.SqlQuery(sql, parameters).FirstOrDefault();
        }

        public SysUser GetDetailByName(string userName)
        {
            string sql = "Select * From SysUser Where IsActive = 0 And userName=@userName";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@userName", userName);

            return this.DbContext.SysUser.SqlQuery(sql, parameters).FirstOrDefault();
        }

        public SysUser DetailByOpenID(string openId)
        {
            string sql = "Select * From SysUser Where IsActive = 0 And OpenId=@OpenId";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@OpenId", openId);

            return this.DbContext.SysUser.SqlQuery(sql, parameters).FirstOrDefault();
        }

        public object GetPhone(int UserID)
        {
            string sql = $@"SELECT Phone from SysUser where UserId={UserID}";
            return this.DbInstance.GetCell(sql);
        }

        public object GetUserName(int UserID)
        {
            string sql = $@"SELECT UserName from SysUser where UserId={UserID}";
            return this.DbInstance.GetCell(sql);
        }

        public object CheckLogin(string OpenId)
        {
            string sql = $@"SELECT count(*) from SysUser where OpenId='{OpenId}'";
            return this.DbInstance.GetCell(sql);
        }
        #endregion

        #region Write
        public int Add(SysUser entity)
        {
            entity.IsActive = (int)IsActive.∑Ò;
            entity.Creator = AuthenWidget.CurrentOperator?.UserId ?? 0;
            entity.CreateTime = DateTime.Now;
            entity.Editor = AuthenWidget.CurrentOperator?.UserId ?? 0;
            entity.EditTime = DateTime.Now;

            this.DbContext.SysUser.Add(entity);
            this.DbContext.Entry(entity).State = EntityState.Added;

            if (this.DbContext.SaveChanges() <= 0)
            {
                return OperationWidget.Failed;
            }
            return OperationWidget.Success;
        }

        public int Edit(SysUser entity)
        {
            entity.EditTime = DateTime.Now;
            entity.Editor = AuthenWidget.CurrentOperator?.UserId ?? 0;

            this.DbContext.SysUser.Attach(entity);
            this.DbContext.Entry(entity).State = EntityState.Modified;

            return this.DbContext.SaveChanges() <= 0 ? OperationWidget.Failed : OperationWidget.Success;
        }
        #endregion
    }
}