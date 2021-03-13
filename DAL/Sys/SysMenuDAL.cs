using Common;
using DBModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MenuOperation
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class SysMenuDAL : DbBasic
    {
        public SysMenuDAL()
        {
        }

        #region Read
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="gvMgt"></param>
        /// <returns></returns>
        public List<SysMenu> GetMgt(Grid gvMgt, Dictionary<string, string> map)
        {
            string sql = @"Select (RowNum) From SysMenu t Where t.IsActive = 0 And MenuName like @MenuName";

            string sqlCount = SqlWidget.GetMgtTotal(sql, "t.MenuID");
            gvMgt.RecordCount = ConvertHelper.ToInt32(this.DbInstance.GetCell(sqlCount, GetParameters(map)));

            string sqlMgt = SqlWidget.GetMgtSql(sql, gvMgt);
            return this.DbContext.SysMenu.SqlQuery(sqlMgt, GetParameters(map)).ToList();
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
        /// 列表
        /// </summary>
        /// <returns></returns>
        public List<SysMenu> GetList(int? parentID, int? status = null)
        {
            string filter = "And ParentID is NULL";
            if (parentID.HasValue)
            {
                filter = " And ParentID=@ParentID";
            }
            if (status.HasValue)
            {
                filter += " And Status=@Status";
            }

            string sql = $"Select * From SysMenu Where IsActive = 0 {filter} Order By SortNo";

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = this.DbInstance.GetSqlParameter("ParentID", parentID);
            parameters[1] = this.DbInstance.GetSqlParameter("Status", status);

            return this.DbContext.SysMenu.SqlQuery(sql, parameters).ToList();
        }

        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public List<string> GetMenuAuth(string menuCode)
        {
            List<string> list = new List<string>();
            SysMenu menu = Detail(menuCode);

            if (!string.IsNullOrEmpty(menu?.Operation))
            {
                var operation = JsonConvert.DeserializeObject<List<MenuOperation>>(menu.Operation);
                foreach (var item in operation)
                {
                    list.Add($"{menu.MenuCode}_{item.Code}");
                }
            }

            return list;
        }

        public SysMenu Detail(int menuID)
        {
            string sql = "Select * From SysMenu Where IsActive = 0 And MenuID=@MenuID";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@MenuID", menuID);

            return this.DbContext.SysMenu.SqlQuery(sql, parameters).FirstOrDefault();
        }

        public SysMenu Detail(string menuCode)
        {
            string sql = "Select * From SysMenu Where IsActive = 0 And MenuCode=@MenuCode";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@MenuCode", menuCode);

            return this.DbContext.SysMenu.SqlQuery(sql, parameters).FirstOrDefault();
        }
        #endregion

        #region Write
        public int Add(SysMenu entity)
        {
            entity.IsActive = (int)IsActive.否;
            entity.Creator = 1;// AuthenWidget.CurrentOperator?.UserID ?? AuthenWidget.CurrentCustomer?.CustomerID;
            entity.CreateTime = DateTime.Now;
            entity.Editor = 1;//AuthenWidget.CurrentOperator?.UserID ?? AuthenWidget.CurrentCustomer?.CustomerID;
            entity.EditTime = DateTime.Now;

            this.DbContext.SysMenu.Add(entity);
            this.DbContext.Entry(entity).State = EntityState.Added;

            if (this.DbContext.SaveChanges() <= 0)
            {
                return OperationWidget.Failed;
            }

            return OperationWidget.Success;
        }

        public int Edit(SysMenu entity)
        {
            entity.EditTime = DateTime.Now;
            entity.Editor = 1;// AuthenWidget.CurrentOperator?.UserID ?? AuthenWidget.CurrentCustomer?.CustomerID;

            this.DbContext.SysMenu.Attach(entity);
            this.DbContext.Entry(entity).State = EntityState.Modified;

            return this.DbContext.SaveChanges() <= 0 ? OperationWidget.Failed : OperationWidget.Success;
        }
        #endregion
    }
}
