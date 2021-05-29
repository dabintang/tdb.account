using Autofac;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using tdb.account.common.Config;
using tdb.account.common.Const;
using tdb.account.dto.Common;
using tdb.account.dto.Enums;
using tdb.account.ibll;
using tdb.account.idal;
using tdb.account.model;
using tdb.common;
using tdb.framework.webapi.Config;
using tdb.framework.webapi.Log;

namespace tdb.account.bll
{
    /// <summary>
    /// 管理工具
    /// </summary>
    public class ManageTools : IManageTools
    {
        /// <summary>
        /// Autofac上下文
        /// </summary>
        private IComponentContext componentContext;

        /// <summary>
        /// 用户
        /// </summary>
        private IUserDAL userDAL
        {
            get
            {
                return this.componentContext.Resolve<IUserDAL>();
            }
        }

        /// <summary>
        /// 角色
        /// </summary>
        private IRoleDAL roleDAL
        {
            get
            {
                return this.componentContext.Resolve<IRoleDAL>();
            }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        private IUserRoleDAL userRoleDAL
        {
            get
            {
                return this.componentContext.Resolve<IUserRoleDAL>();
            }
        }

        /// <summary>
        /// 权限
        /// </summary>
        private IAuthorityDAL authorityDAL
        {
            get
            {
                return this.componentContext.Resolve<IAuthorityDAL>();
            }
        }

        /// <summary>
        /// 角色权限
        /// </summary>
        private IRoleAuthorityDAL roleAuthorityDAL
        {
            get
            {
                return this.componentContext.Resolve<IRoleAuthorityDAL>();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_componentContext">Autofac上下文</param>
        public ManageTools(IComponentContext _componentContext)
        {
            this.componentContext = _componentContext;
        }

        #region 实现接口

        /// <summary>
        /// 备份consul配置
        /// </summary>
        /// <returns>备份配置完整文件名</returns>
        public string BackupConsulConfig()
        {
            var fullFileName = DistributedConfigurator.Ins.BackupConfig<ConsulConfig>();
            return fullFileName;
        }

        /// <summary>
        /// 还原consul配置
        /// </summary>
        /// <param name="file">配置文件（.json文件）</param>
        public void RestoreConsulConfig(IFormFile file)
        {
            using (var ms = file.OpenReadStream())
            {
                var buf = new byte[ms.Length];
                ms.Read(buf, 0, (int)ms.Length);

                var jsonTxt = Encoding.Default.GetString(buf);
                var config = JsonConvert.DeserializeObject<ConsulConfig>(jsonTxt);

                DistributedConfigurator.Ins.RestoreConfig<ConsulConfig>(config);
            }
        }

        /// <summary>
        /// 根据数据库表结构生成实体类
        /// </summary>
        public void CreateDBModels()
        {
            // 连接数据库
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = AccConfig.Consul.DBConnStr,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true//开启自动释放模式
            });

            //表映射
            db.MappingTables.Add("LoginLog", "login_log");
            db.MappingTables.Add("User", "user");
            db.MappingTables.Add("Role", "role");
            db.MappingTables.Add("Authority", "authority");
            db.MappingTables.Add("RoleAuthority", "role_authority");
            db.MappingTables.Add("UserRole", "user_role");

            // 生成路径
            var classDirectory = @"..\..\..\..\tdb.account.model";
            // 实体命名空间
            string classNamespace = "tdb.account.model";
            // 生成实体类
            db.DbFirst.IsCreateAttribute().CreateClassFile(classDirectory, classNamespace);
        }

        ///// <summary>
        ///// 初始化数据库
        ///// </summary>
        ///// <returns>是否成功</returns>
        //public bool InitDB()
        //{
        //    try
        //    {
        //        //开始事物
        //        this.userDAL.AsTenant().BeginTran();

        //        #region 添加超级管理员账号

        //        var userSuperAdmin = new User();
        //        userSuperAdmin.LoginName = CstUser.SuperAdmin;
        //        userSuperAdmin.UserName = "超级管理员";
        //        userSuperAdmin.Password = EncryptHelper.Md5(AccConfig.Consul.DefaultPassword);
        //        userSuperAdmin.Gender = (int)EnumGender.Unknown;
        //        userSuperAdmin.Birthday = null;
        //        userSuperAdmin.MobilePhone = "";
        //        userSuperAdmin.Email = "";
        //        userSuperAdmin.Enable = true;
        //        userSuperAdmin.Creater = "0";
        //        userSuperAdmin.CreateTime = DateTime.Now;
        //        userSuperAdmin.Updater = "0";
        //        userSuperAdmin.UpdateTime = DateTime.Now;

        //        if (this.userDAL.ExistUser(userSuperAdmin.LoginName) == false)
        //        {
        //            this.userDAL.AddUser(userSuperAdmin);
        //        }

        //        #endregion

        //        #region 添加角色

        //        var roleSuperAdmin = new Role();
        //        roleSuperAdmin.RoleCode = CstRole.SuperAdmin;
        //        roleSuperAdmin.RoleName = "超级管理员";
        //        roleSuperAdmin.Enable = true;
        //        roleSuperAdmin.Remark = "超级管理员角色";

        //        if (this.roleDAL.ExistRole(roleSuperAdmin.RoleCode) == false)
        //        {
        //            this.roleDAL.AddRole(roleSuperAdmin);
        //        }

        //        #endregion

        //        #region 添加权限

        //        var authManageUser = new Authority();
        //        authManageUser.AuthorityCode = CstAuthority.ManageUser;
        //        authManageUser.AuthorityName = "管理用户";
        //        authManageUser.Enable = true;
        //        authManageUser.Remark = "增删查改用户的权限";

        //        if (this.authorityDAL.ExistAuthority(authManageUser.AuthorityCode) == false)
        //        {
        //            this.authorityDAL.AddAuthority(authManageUser);
        //        }

        //        #endregion

        //        #region 添加角色权限

        //        var roleAuthManageUser = new RoleAuthority();
        //        roleAuthManageUser.RoleCode = CstRole.SuperAdmin;
        //        roleAuthManageUser.AuthorityCode = CstAuthority.ManageUser;
        //        roleAuthManageUser.Creater = "0";
        //        roleAuthManageUser.CreateTime = DateTime.Now;

        //        this.roleAuthorityDAL.SetRoleAuthority(roleAuthManageUser.RoleCode, new List<RoleAuthority>() { roleAuthManageUser });

        //        #endregion

        //        #region 添加超级管理员账号角色

        //        var userRoleSuperAdmin = new UserRole();
        //        userRoleSuperAdmin.LoginName = userSuperAdmin.LoginName;
        //        userRoleSuperAdmin.RoleCode = roleSuperAdmin.RoleCode;
        //        userRoleSuperAdmin.Creater = "0";
        //        userRoleSuperAdmin.CreateTime = DateTime.Now;

        //        this.userRoleDAL.SetUserRole(userRoleSuperAdmin.LoginName, new List<UserRole>() { userRoleSuperAdmin });

        //        #endregion

        //        //提交事物
        //        this.userDAL.AsTenant().CommitTran();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Ins.Error(ex, "初始化数据库异常");

        //        //回滚事物
        //        this.userDAL.AsTenant().RollbackTran();

        //        return false;
        //    }
        //}

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns>是否成功</returns>
        public bool InitDB()
        {
            try
            {
                //消息配置
                var fullFileName = CommHelper.GetFullFileName("initData.json");
                var jsonStr = File.ReadAllText(fullFileName);
                var initData = JsonConvert.DeserializeObject<InitDataInfo>(jsonStr);

                //开始事物
                this.userDAL.AsTenant().BeginTran();

                #region 用户

                foreach (var user in initData.Users)
                {
                    user.Password = EncryptHelper.Md5(user.Password);
                    if (this.userDAL.ExistUser(user.LoginName) == false)
                    {
                        this.userDAL.AddUser(user);
                    }
                }

                #endregion

                #region 添加角色

                foreach (var role in initData.Roles)
                {
                    if (this.roleDAL.ExistRole(role.RoleCode) == false)
                    {
                        this.roleDAL.AddRole(role);
                    }
                }

                #endregion

                #region 添加权限

                foreach (var authority in initData.Authoritys)
                {
                    if (this.authorityDAL.ExistAuthority(authority.AuthorityCode) == false)
                    {
                        this.authorityDAL.AddAuthority(authority);
                    }
                }

                #endregion

                #region 添加角色权限

                var lstRoleCode = initData.RoleAuthoritys.Select(m => m.RoleCode).Distinct().ToList();
                foreach (var roleCode in lstRoleCode)
                {
                    var lstRoleAuthority = initData.RoleAuthoritys.FindAll(m => m.RoleCode == roleCode);
                    this.roleAuthorityDAL.SetRoleAuthority(roleCode, lstRoleAuthority);
                }

                #endregion

                #region 添加超级管理员账号角色

                var lstLoginName = initData.UserRoles.Select(m => m.LoginName).Distinct().ToList();
                foreach (var loginName in lstLoginName)
                {
                    var lstUserRole = initData.UserRoles.FindAll(m => m.LoginName == loginName);
                    this.userRoleDAL.SetUserRole(loginName, lstUserRole);
                }

                #endregion

                //提交事物
                this.userDAL.AsTenant().CommitTran();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Ins.Error(ex, "初始化数据库异常");

                //回滚事物
                this.userDAL.AsTenant().RollbackTran();

                return false;
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        public void Test()
        {
            //this.userDAL.AsTenant().BeginTran();

            //var user1 = this.userDAL.GetUser("admin");
            //user1.Email = "12345";
            //this.userDAL.UpdateUser(user1);
            //var user2 = this.userDAL.GetUser("admin");

            //this.userDAL.AsTenant().RollbackTran();
        }

        #endregion
    }
}
