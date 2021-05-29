using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.common;
using tdb.account.common.Config;
using tdb.account.dto;
using tdb.account.dto.Common;
using tdb.account.dto.Role;
using tdb.account.ibll;
using tdb.account.idal;
using tdb.account.model;
using tdb.framework.webapi.DTO;

namespace tdb.account.bll
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleBLL : BaseBLL, IRoleBLL
    {
        /// <summary>
        /// 角色
        /// </summary>
        private readonly IRoleDAL roleDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleDAL">角色</param>
        public RoleBLL(IRoleDAL _roleDAL)
        {
            this.roleDAL = _roleDAL;
        }

        #region 实现接口

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="req">角色信息</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> AddRole(AddRoleReq req, OperatorInfo oper)
        {
            //判断角色是否已存在
            if (this.roleDAL.ExistRole(req.RoleCode))
            {
                return AccHelper.FailItemRes(AccConfig.Msg.ExistRoleCode, false);
            }

            //用户实体
            var model = new Role();
            model.RoleCode = req.RoleCode;
            model.RoleName = req.RoleName;
            model.Enable = true;
            model.Remark = req.Remark;

            //插入数据库
            this.roleDAL.AddRole(model);

            return AccHelper.OkItemRes(true);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>角色信息</returns>
        public RoleInfo GetRole(GetRoleReq req)
        {
            var model = this.roleDAL.GetRole(req.RoleCode);
            var roleInfo = AccMapper.Ins.Map<RoleInfo>(model);

            return roleInfo;
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="req">角色信息</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> UpdateRole(UpdateRoleReq req, OperatorInfo oper)
        {
            //判断角色是否存在
            var model = this.roleDAL.GetRole(req.RoleCode);
            if (model == null)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.RoleNotExist, false);
            }

            model.RoleCode = req.RoleCode;
            model.RoleName = req.RoleName;
            model.Remark = req.Remark;

            //更新数据库
            this.roleDAL.UpdateRole(model);

            return AccHelper.OkItemRes(true);
        }

        /// <summary>
        /// 修改角色启用状态
        /// </summary>
        /// <param name="req">条件</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> UpdateRoleEnable(UpdateRoleEnableReq req, OperatorInfo oper)
        {
            //判断角色是否存在
            var model = this.roleDAL.GetRole(req.RoleCode);
            if (model == null)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.RoleNotExist, false);
            }

            model.Enable = req.Enable;

            //更新数据库
            this.roleDAL.UpdateRole(model);

            return AccHelper.OkItemRes(true);
        }

        #endregion
    }
}
