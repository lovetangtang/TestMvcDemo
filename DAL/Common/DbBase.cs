using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DbBase
    {
        private string connectionStr { get; set; }

        public DbBase(string sqlConnect)
        {
            connectionStr = sqlConnect;
        }

        public DataTable GetDataTable(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    return table;
                }
            }
        }

        public DataTable GetDataTable(string sql, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    return table;
                }
            }
        }

        public DataRow GetDataRow(string sql)
        {
            DataRow result = null;
            DataTable table = GetDataTable(sql);

            if (table != null && table.Rows.Count > 0)
            {
                result = table.Rows[0];
            }

            return result;
        }

        public DataRow GetDataRow(string sql, SqlParameter[] parameters)
        {
            DataRow result = null;
            DataTable table = GetDataTable(sql, parameters);

            if (table != null && table.Rows.Count > 0)
            {
                result = table.Rows[0];
            }

            return result;
        }

        /// <summary>
        /// 首行首列
        /// </summary>
        /// <returns></returns>
        public object GetCell(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 首行首列
        /// </summary>
        /// <returns></returns>
        public object GetCell(string sql, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql, SqlParameter[] parameters)
        {
            int result = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(parameters);

                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Helper.WriteLog("执行sql错误", ex);
            }

            return result;
        }

        /// <summary>
        /// 将null替换为DBNull
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlParameter GetSqlParameter(string paramName, object value)
        {
            if (value == null)
            {
                return new SqlParameter(paramName, DBNull.Value);
            }
            else
            {
                return new SqlParameter(paramName, value);
            }
        }
    }
}