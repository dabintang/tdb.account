using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.common.Const;
using tdb.account.idal;
using tdb.account.model;
using tdb.framework.webapi.IocAutofac.CacheAOP;

namespace tdb.account.dal
{
    /// <summary>
    /// 用户
    /// </summary>
    [Intercept(typeof(TdbCacheInterceptor))]
    public class UserDAL : Repository<User>, IUserDAL
    {
        #region 实现接口

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户信息</param>
        public void AddUser(User user)
        {
            this.AsInsertable(user).ExecuteCommand();
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>true：已存在；false：不存在</returns>
        public bool ExistUser(string loginName)
        {
            var count = this.AsQueryable().Count(m => m.LoginName == loginName);
            return count > 0;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>用户信息</returns>
        [TdbReadCacheHash(CstCache.User)]
        [TdbCacheKey(0)]
        public User GetUser(string loginName)
        {
            return this.GetSingle(m => m.LoginName == loginName);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        [TdbRemoveCacheHash(CstCache.User)]
        [TdbCacheKey(0, FromPropertyName = Cst.FieldName.LoginName)]
        public void UpdateUser(User user)
        {
            this.AsUpdateable(user).ExecuteCommand();
        }

        #endregion
    }
}
