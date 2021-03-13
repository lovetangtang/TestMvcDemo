using DBModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SysDictionaryDAL : DbBasic
    {
        #region read
        public List<SysDictionary> GetOptions(string dictCode)
        {
            string sql = @"Select t.* From SysDictionary t
Left Join SysDictionary d on d.DictId=t.ParentId
Where d.DictCode=@DictCode
And t.IsActive=0";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = this.DbInstance.GetSqlParameter("@DictCode", dictCode);

            return this.DbContext.SysDictionary.SqlQuery(sql, parameters).ToList();
        }
        #endregion
    }
}
