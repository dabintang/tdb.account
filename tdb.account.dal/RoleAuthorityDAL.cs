using Autofac.Extras.DynamicProxy;
using SqlSugar;
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
    /// 角色权限
    /// </summary>
    [Intercept(typeof(TdbCacheInterceptor))]
    public class RoleAuthorityDAL : Repository<RoleAuthority>, IRoleAuthorityDAL
    {
        #region 实现接口

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="lstRoleAuthority">角色权限</param>
        [TdbRemoveCacheHash(CstCache.RoleAuthority)]
        [TdbCacheKey(0)]
        public bool SetRoleAuthority(string roleCode, List<RoleAuthority> lstRoleAuthority)
        {
            try
            {
                //开启事物
                this.AsTenant().BeginTran();

                //先删除原权限
                this.AsDeleteable().Where(m => m.RoleCode == roleCode).ExecuteCommand();

                //插入新权限
                if (lstRoleAuthority != null && lstRoleAuthority.Count > 0)
                {
                    this.AsInsertable(lstRoleAuthority).ExecuteCommand();
                }

                //提交事物
                this.AsTenant().CommitTran();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Ins.Error(ex, "设置角色权限异常");

                //回滚事物
                this.AsTenant().RollbackTran();

                return false;
            }
        }

        /// <summary>
        /// 查询指定角色的权限
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>角色权限</returns>
        [TdbReadCacheHash(CstCache.RoleAuthority)]
        [TdbCacheKey(0)]
        public List<RoleAuthority> QueryRoleAuthority(string roleCode)
        {
            var list = DbScoped.Sugar.Queryable<RoleAuthority, Authority, Role>((ra, a, r) => new object[] {
                                        JoinType.Inner, ra.AuthorityCode == a.AuthorityCode,
                                        JoinType.Inner, ra.RoleCode == r.RoleCode})
                                     .Where((ra, a, r) => ra.RoleCode == roleCode && a.Enable == true && r.Enable == true)
                                     .Select((ra, a, r) => ra)
                                     .ToList();
            return list;
        }

        ///// <summary>
        ///// 查询指定角色的权限
        ///// </summary>
        ///// <param name="lstRoleCode">角色编码</param>
        ///// <returns>角色权限</returns>
        //public List<RoleAuthority> QueryRoleAuthority(List<string> lstRoleCode)
        //{
        //    var list = DbScoped.Sugar.Queryable<RoleAuthority, Authority, Role>((ra, a, r) => new object[] {
        //                                JoinType.Inner, ra.AuthorityCode == a.AuthorityCode,
        //                                JoinType.Inner, ra.RoleCode == r.RoleCode})
        //                             .Where((ra, a, r) => lstRoleCode.Contains(ra.RoleCode) && a.Enable == true && r.Enable == true)
        //                             .Select((ra, a, r) => ra)
        //                             .ToList();
        //    return list;
        //}

        #endregion
    }
}
