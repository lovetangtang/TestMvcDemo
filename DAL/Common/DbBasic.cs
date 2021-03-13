using Common;
using DBModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DbBasic
    {
        public SysDbContext DbContext { get; private set; }

        public DbBase DbInstance { get; private set; }

        /// <summary>
        /// 当前操作人
        /// </summary>
        public SysUser Operator { get; set; }

        public DbBasic()
        {
            string connectionStr = ConvertHelper.ToString(ConfigurationManager.ConnectionStrings["SysDbContext"]);

            this.DbInstance = new DbBase(connectionStr);
            this.DbContext = new SysDbContext();
        }
    }
}
