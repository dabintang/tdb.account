using Autofac.Extras.DynamicProxy;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.common.Const;
using tdb.account.idal;
using tdb.account.model;
using tdb.framework.webapi.IocAutofac.CacheAOP;
using tdb.framework.webapi.Log;

namespace tdb.account.dal
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Intercept(typeof(TdbCacheInterceptor))]
    public class UserRoleDAL : Repository<UserRole>, IUserRoleDAL
    {
        #region 实现接口

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="lstUserRole">用户角色</param>
        [TdbRemoveCacheHash(CstCache.UserRole)]
        [TdbCacheKey(0)]
        public bool SetUserRole(string loginName, List<UserRole> lstUserRole)
        {
            try
            {
                //开启事物
                this.AsTenant().BeginTran();

                //先删除原角色
                this.AsDeleteable().Where(m => m.LoginName == loginName).ExecuteCommand();

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
        /// <param name="loginName">登录名</param>
        /// <returns>用户角色</returns>
        [TdbReadCacheHash(CstCache.UserRole)]
        [TdbCacheKey(0)]
        public List<UserRole> QueryUserRole(string loginName)
        {
            var list = DbScoped.Sugar.Queryable<UserRole, Role>((ur, r) => ur.RoleCode == r.RoleCode)
                                     .Where((ur, r) => ur.LoginName == loginName && r.Enable == true)
                                     .Select((ur, r) => ur)
                                     .ToList();
            return list;
        }

        #endregion
    }
}
