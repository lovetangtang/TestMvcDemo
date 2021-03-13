using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SysDictionaryBLL
    {
        public readonly SysDictionaryDAL handle;

        public SysDictionaryBLL()
        {
            this.handle = new SysDictionaryDAL();
        }

        #region read
        /// <summary>
        /// 根据父级编码获取选项
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>
        public List<object> GetOptions(string dictCode)
        {
            List<object> list = new List<object>();
            
            foreach (var item in this.handle.GetOptions(dictCode))
            {
                list.Add(new
                {
                    value = item.DictId,
                    text = item.DictName,
                    label = item.DictName
                });
                
            }

            return list;
        }
        #endregion
    }
}
