using Common;
using DAL;
using DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BLL
{
    public class SysMenuBLL
    {
        public readonly SysMenuDAL handle;
        public SysMenuBLL()
        {
            handle = new SysMenuDAL();
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
                    item.MenuID,
                    ParentName = RelationWidget.GetMenuName(item.ParentID),
                    item.MenuName,
                    Status = ConvertHelper.ToString(item.Status),
                    item.MenuCode,
                    item.SortNo,
                    Creator = RelationWidget.GetUserName(item.Creator),
                    CreateTime = ConvertHelper.ToString(item.CreateTime, "yyyy-MM-dd HH:mm")
                });
            }

            return list;
        }

        /// <summary>
        /// 父级菜单选项
        /// </summary>
        /// <returns></returns>
        public List<object> GetParents()
        {
            List<object> list = new List<object>();

            foreach (var item in handle.GetList(null))
            {
                list.Add(new
                {
                    value = item.MenuID,
                    label = item.MenuName
                });
            }

            return list;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public List<object> GetMenus()
        {
            List<object> list = new List<object>();

            foreach (var item in handle.GetList(null, (int)TrueOrFalse.True))
            {
                var showStatus = false;
                var children = new List<object>();

                foreach (var child in item.Children
                    .Where(p => p.IsActive == (int)IsActive.否 && p.Status == (int)TrueOrFalse.True)
                    .OrderBy(p => p.SortNo))
                {
                    if (AuthenWidget.HasAuth($"{child.MenuCode}_List"))
                    {
                        showStatus = true;
                        children.Add(new
                        {
                            label = child.MenuName,
                            name = $"{child.Parent} - { child.MenuName}",
                            url = child.Url,
                            icon = child.Icon,
                            id=child.MenuID
                        });
                    }
                }

                if (showStatus)
                {
                    list.Add(new
                    {
                        label = item.MenuName,
                        icon = item.Icon,
                        name = item.MenuName,
                        children,
                        id=item.MenuID
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        public object GetAuth(int? authCategory)
        {
            var list = new List<object>();

            foreach (var item in handle.GetList(null, (int)TrueOrFalse.True))
            {
                var listChildren = new List<object>();
                foreach (var child in item.Children.Where(p => p.IsActive == (int)IsActive.否))
                {
                    list.Add(new
                    {
                        name = $"{item.MenuName} - {child.MenuName}",
                        code = child.MenuCode,
                        authority = child.Operation
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public object Detail(int menuID, out int resultCode)
        {
            var menu = this.handle.Detail(menuID);

            if (menu == null)
            {
                resultCode = OperationWidget.NoRecord;
                return new { };
            }

            resultCode = OperationWidget.Success;
            return new
            {
                menu.MenuID,
                menu.ParentID,
                menu.MenuName,
                menu.MenuCode,
                Status = ConvertHelper.ToString(menu.Status),
                menu.Icon,
                menu.Url,
                menu.SortNo,
                menu.Operation
            };
        }
        #endregion

        #region Write
        public int Add(SysMenu entity)
        {
            if (VerificationWidget.IsExit(nameof(SysMenu), nameof(SysMenu.MenuCode), entity.MenuCode, nameof(SysMenu.MenuID), entity.MenuID) ||
                VerificationWidget.IsExit(nameof(SysMenu), nameof(SysMenu.MenuName), entity.MenuName, nameof(SysMenu.MenuID), entity.MenuID))
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

        public int Edit(SysMenu menu)
        {
            if (VerificationWidget.IsExit(nameof(SysMenu), nameof(SysMenu.MenuCode), menu.MenuCode, nameof(SysMenu.MenuID), menu.MenuID) ||
                VerificationWidget.IsExit(nameof(SysMenu), nameof(SysMenu.MenuName), menu.MenuName, nameof(SysMenu.MenuID), menu.MenuID))
            {
                return OperationWidget.IsRepeated;
            }

            SysMenu entity = handle.Detail(menu.MenuID);

            using (TransactionScope transaction = new TransactionScope())
            {
                entity.ParentID = menu.ParentID;
                entity.MenuName = menu.MenuName;
                entity.MenuCode = menu.MenuCode;
                entity.Status = menu.Status;
                entity.Icon = menu.Icon;
                entity.Url = menu.Url;
                entity.SortNo = menu.SortNo;
                entity.Operation = menu.Operation;

                if (handle.Edit(entity) != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }

                transaction.Complete();
                return OperationWidget.Success;
            }
        }

        public int Delete(int? menuID)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var entity = handle.Detail(ConvertHelper.ToInt32(menuID));

                entity.IsActive = (int)IsActive.是;
                entity.Children.ToList().ForEach(p => p.IsActive = (int)IsActive.是);

                if (handle.Edit(entity) != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }

                transaction.Complete();
                return OperationWidget.Success;
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int ChangeStatus(int? menuID, int? status)
        {
            SysMenu entity = handle.Detail(ConvertHelper.ToInt32(menuID));

            using (TransactionScope transaction = new TransactionScope())
            {
                entity.Status = status;

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
