
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

    public class SysUserBLL
    {
        public readonly SysUserDAL handle;
        public SysUserBLL()
        {
            handle = new SysUserDAL();
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
            SysAreaDAL sysAreaDAL = new SysAreaDAL();

            foreach (var item in handle.GetMgt(gvMgt, map))
            {
                var sysArea = sysAreaDAL.Detail((int)item.AreaID);
                list.Add(new
                {
                    item.UserId,
                    item.UserName,
                    item.Phone,
                    item.AreaID,
                    sysArea.AreaName,
                    item.OpenId,
                    item.OUId,
                    item.RoleId,
                    item.PersonalAuth,
                    Creator = RelationWidget.GetUserName(item.Creator),
                    CreateTime = ConvertHelper.ToString(item.CreateTime, "yyyy-MM-dd HH:mm")
                });
            }

            return list;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public SysUser GetDetail(int customerId)
        {
            return this.handle.Detail(customerId);
        }

        /// <summary>
        /// 根据电话号码查询详情
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public SysUser GetDetail(string phone)
        {
            return this.handle.Detail(phone);
        }


        /// <summary>
        /// 根据用户获取电话号码
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public SysUser GetDetailByName(string userName)
        {
            return this.handle.GetDetailByName(userName);
        }

        /// <summary>
        /// 根据openID获取详情
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public SysUser DetailByOpenID(string openId)
        {
            return this.handle.DetailByOpenID(openId);
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public object Detail(int customerId, out int resultCode)
        {
            var entity = this.handle.Detail(customerId);

            if (entity == null)
            {
                resultCode = OperationWidget.NoRecord;
                return new { };
            }

            resultCode = OperationWidget.Success;
            return new
            {
                entity.UserId,
                entity.UserName,
                entity.Phone,
                entity.AreaID,
                entity.OpenId,
                entity.OUId,
                entity.RoleId,
                entity.PersonalAuth,
                Creator = RelationWidget.GetUserName(entity.Creator),
                CreateTime = ConvertHelper.ToString(entity.CreateTime, "yyyy-MM-dd HH:mm")
            };
        }

        public int CheckLogin()
        {
            int CheckLogin = 0;
            string OpenId = AuthenWidget.CurrentOperator?.OpenId ?? "";
            CheckLogin = string.IsNullOrWhiteSpace(OpenId) ? 0 : 1;
            if (CheckLogin == 0)
                return CheckLogin;
            return Convert.ToInt32(this.handle.CheckLogin(OpenId));
        }

        /// <summary>
        /// 获取员工权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public int GetAuth(int userID, out List<string> operateAuth, out List<string> dataAuth)
        {
            operateAuth = new List<string>();
            dataAuth = new List<string>();
            var user = handle.Detail(userID);

            if (user == null)
            {
                return OperationWidget.NoRecord;
            }

            user.PersonalAuth = user.PersonalAuth == null ? string.Empty : user.PersonalAuth;

            operateAuth = user.PersonalAuth.Split('|').ToList();
            return OperationWidget.Success;
        }
        #endregion

        #region Write
        public int Add(SysUser entity)
        {
            if (VerificationWidget.IsExit(nameof(SysUser), nameof(SysUser.Phone), entity.Phone, nameof(SysUser.UserId), null))
            {
                return OperationWidget.IsRepeated;
            }
            using (TransactionScope transaction = new TransactionScope())
            {
                int result = handle.Add(entity);
                if (result != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }
                transaction.Complete();
                return OperationWidget.Success;
            }
        }
        /// <summary>
        /// 绑定员工
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public SysUser UserBinding(string phone, string openID, out int resultCode)
        {
            var entity = handle.Detail(phone);
            if (entity == null)
            {
                resultCode = OperationWidget.NoRecord;
                return null;
            }

            entity.OpenId = openID;
            if (handle.Edit(entity) != OperationWidget.Success)
            {
                resultCode = OperationWidget.Failed;
            }

            resultCode = OperationWidget.Success;
            return entity;
        }
        public int Edit(SysUser model)
        {
            if (VerificationWidget.IsExit(nameof(SysUser), nameof(SysUser.Phone), model.Phone, nameof(SysUser.UserId), model.UserId))
            {
                return OperationWidget.IsRepeated;
            }
            SysUser entity = handle.Detail(model.UserId);

            using (TransactionScope transaction = new TransactionScope())
            {
                entity.UserName = model.UserName;
                entity.Phone = model.Phone;
                entity.AreaID = model.AreaID;

                if (handle.Edit(entity) != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }
                transaction.Complete();
                return OperationWidget.Success;
            }
        }

        public int Delete(int? userId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var entity = handle.Detail(ConvertHelper.ToInt32(userId));
                entity.IsActive = (int)IsActive.是;

                if (handle.Edit(entity) != OperationWidget.Success)
                {
                    return OperationWidget.Failed;
                }
                transaction.Complete();
                return OperationWidget.Success;
            }
        }
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="operateAuth"></param>
        /// <returns></returns>
        public int UpdateAuth(int userID, List<string> personalAuth, List<string> dataAuth)
        {
            SysUser entity = handle.Detail(userID);

            using (TransactionScope transaction = new TransactionScope())
            {
                entity.PersonalAuth = personalAuth != null ? string.Join("|", personalAuth) : string.Empty;

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
