
using Common;
using DAL;
using DBModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BLL
{
    public class SysAreaBLL
    {
        public readonly SysAreaDAL handle;
        public SysAreaBLL()
        {
            handle = new SysAreaDAL();
        }

        #region Read
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="gvMgt"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public List<object> GetMgt(Grid gvMgt, Dictionary<string, string> map)
        {
            List<object> list = new List<object>();

            foreach (var item in handle.GetMgt(gvMgt, map))
            {
                list.Add(new
                {
                    item.AreaID,
                    item.AreaName,
                    Creator = RelationWidget.GetUserName(item.Creator),
                    CreateTime = ConvertHelper.ToString(item.CreateTime, "yyyy-MM-dd HH:mm")
                });
            }

            return list;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="areaID"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public object Detail(int areaID, out int resultCode)
        {
            var entity = this.handle.Detail(areaID);

            if (entity == null)
            {
                resultCode = OperationWidget.NoRecord;
                return new { };
            }

            resultCode = OperationWidget.Success;
            return new
            {
                entity.AreaID,
                entity.AreaName,
            };
        }

        

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public SysArea GetDetail(int areaID)
        {
           return this.handle.Detail(areaID);            
        }

        /// <summary>
        /// 获取大区选项
        /// </summary>
        /// <returns></returns>
        public List<object> GetOptions()
        {
            List<object> list = new List<object>();
            foreach (var item in handle.GetList())
            {
                list.Add(new
                {
                    value = item.AreaID,
                    label = item.AreaName
                });
            }

            return list;
        }

        /// <summary>
        /// 用户端获取大区选项
        /// </summary>
        /// <returns></returns>
        public List<object> GetOptionsInCustomer()
        {
            List<object> list = new List<object>();
            foreach (var item in handle.GetList())
            {
                list.Add(new
                {
                    value = item.AreaID,
                    text = item.AreaName,
                    name = item.AreaName
                });
            }

            return list;
        }

        public object GetRecommend()
        {
            var entity = handle.GetDefault();

            return new
            {
                text = entity?.AreaName,
                value = entity?.AreaID
            };
        }

        /// <summary>
        /// 获取大区权限选项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<object> GetAreaAuth(string name)
        {
            List<object> list = new List<object>();

            SysAreaDAL areaHandle = new SysAreaDAL();
            foreach (var area in areaHandle.GetList())
            {

                list.Add(new
                {
                    id = $"{name}_Area_{area.AreaID}",
                    label = area.AreaName
                });
            }
            
            return list;
        }
        #endregion

        #region Write
        public int Add(SysArea entity)
        {
            if (VerificationWidget.IsExit(nameof(SysArea), nameof(SysArea.AreaName), entity.AreaName, nameof(SysArea.AreaID), entity.AreaID))
            {
                return OperationWidget.IsRepeated;
            }

            using (TransactionScope transaction = new TransactionScope())
            {
                if (handle.Add(entity) != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }

                transaction.Complete();
                return OperationWidget.Success;
            }
        }

        public int Edit(SysArea model)
        {
            if (VerificationWidget.IsExit(nameof(SysArea), nameof(SysArea.AreaName), model.AreaName, nameof(SysArea.AreaID), model.AreaID))
            {
                return OperationWidget.IsRepeated;
            }

            SysArea entity = handle.Detail(model.AreaID);

            using (TransactionScope transaction = new TransactionScope())
            {
                entity.AreaName = model.AreaName;

                if (handle.Edit(entity) != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }

                transaction.Complete();
                return OperationWidget.Success;
            }
        }

        public int Delete(int? areaID)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var entity = handle.Detail(ConvertHelper.ToInt32(areaID));
                entity.IsActive = (int)IsActive.是;

                if (handle.Edit(entity) != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }

                transaction.Complete();
                return OperationWidget.Success;
            }
        }
        #endregion
    }
}
