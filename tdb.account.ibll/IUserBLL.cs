using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.dto.Common;
using tdb.account.dto.User;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.ibll
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface IUserBLL : IAutofacDependency
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>token</returns>
        BaseItemRes<string> Login(LoginReq req);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <param name="oper">操作者信息</param>
        BaseItemRes<bool> AddUser(AddUserReq req, OperatorInfo oper);

        ///// <summary>
        ///// 检查用户是否存在
        ///// </summary>
        ///// <param name="req">用户编码</param>
        ///// <returns>true：已存在；false：不存在</returns>
        //bool ExistUser(ExistUserReq req);

        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <param name="req">用户编码</param>
        ///// <returns>用户信息</returns>
        //UserInfo GetUser(GetUserReq req);

        ///// <summary>
        ///// 修改用户信息
        ///// </summary>
        ///// <param name="req">用户信息</param>
        ///// <param name="oper">操作者信息</param>
        //BaseItemRes<bool> UpdateUser(UpdateUserReq req, OperatorInfo oper);
    }
}
