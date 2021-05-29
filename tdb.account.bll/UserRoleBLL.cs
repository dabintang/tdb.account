using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.common;
using tdb.account.common.Config;
using tdb.account.dto.Common;
using tdb.account.dto.UserRole;
using tdb.account.ibll;
using tdb.account.idal;
using tdb.account.model;
using tdb.framework.webapi.DTO;

namespace tdb.account.bll
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRoleBLL : BaseBLL, IUserRoleBLL
    {
        /// <summary>
        /// Autofac上下文
        /// </summary>
        private readonly IComponentContext componentContext;

        /// <summary>
        /// 用户角色
        /// </summary>
        private readonly IUserRoleDAL userRoleDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_componentContext">Autofac上下文</param>
        /// <param name="_userRoleDAL">用户角色</param>
        public UserRoleBLL(IComponentContext _componentContext, IUserRoleDAL _userRoleDAL)
        {
            this.componentContext = _componentContext;
            this.userRoleDAL = _userRoleDAL;
        }

        #region 实现接口

        /// <summary>
        /// 设置指定用户角色
        /// </summary>
        /// <param name="req">条件</param>
        /// <param name="oper">操作者信息</param>
        /// <returns></returns>
        public BaseItemRes<bool> SetUserRole(SetUserRoleReq req, OperatorInfo oper)
        {
            //验证登录名是否存在
            var userDAL = this.GetUserDAL();
            if (userDAL.ExistUser(req.LoginName) == false)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.UserNotExist, false);
            }

            //用户角色实体
            var lstUserRole = new List<UserRole>();
            if (req.LstRoleCode != null && req.LstRoleCode.Count > 0)
            {
                var now = DateTime.Now; //当前时间

                //角色
                var roleDAL = this.GetRoleDAL();
                
                //生成实体
                foreach (var roleCode in req.LstRoleCode)
                {
                    //验证角色是否存在
                    if (roleDAL.ExistRole(roleCode) == false)
                    {
                        return AccHelper.FailItemRes(AccConfig.Msg.RoleNotExist, false);
                    }

                    var userRole = new UserRole();
                    userRole.LoginName = req.LoginName;
                    userRole.RoleCode = roleCode;
                    userRole.Creater = oper.LoginName;
                    userRole.CreateTime = now;

                    lstUserRole.Add(userRole);
                }
            }

            //保存到数据库
            this.userRoleDAL.SetUserRole(req.LoginName, lstUserRole);

            return AccHelper.OkItemRes(true);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 角色
        /// </summary>
        private IRoleDAL GetRoleDAL()
        {
            return this.componentContext.Resolve<IRoleDAL>();
        }

        /// <summary>
        /// 用户
        /// </summary>
        private IUserDAL GetUserDAL()
        {
            return this.componentContext.Resolve<IUserDAL>();
        }

        #endregion
    }
}
