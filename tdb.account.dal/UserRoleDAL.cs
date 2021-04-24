using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.idal;
using tdb.account.model;
using tdb.framework.webapi.Log;

namespace tdb.account.dal
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRoleDAL : Repository<UserRole>, IUserRoleDAL
    {
        #region 实现接口

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="lstUserRole">用户角色</param>
        public bool SetUserRole(string userCode, List<UserRole> lstUserRole)
        {
            try
            {
                //开启事物
                this.AsTenant().BeginTran();

                //先删除原角色
                this.AsDeleteable().Where(m => m.UserCode == userCode).ExecuteCommand();

                //插入新角色
                if (lstUserRole != null && lstUserRole.Count > 0)
                {
                    this.AsInsertable(lstUserRole).ExecuteCommand();
                }

                //提交事物
                this.AsTenant().CommitTran();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Ins.Error(ex, "设置用户角色异常");

                //回滚事物
                this.AsTenant().RollbackTran();

                return false;
            }
        }

        /// <summary>
        /// 查询指定用户的用户角色
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>用户角色</returns>
        public List<UserRole> QueryUserRole(string userCode)
        {
            return this.AsQueryable().Where(m => m.UserCode == userCode).ToList();
        }

        #endregion
    }
}
