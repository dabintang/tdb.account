using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.common.Config
{
    /// <summary>
    /// 回报消息配置
    /// </summary>
    public class MsgConfig
    {
        /// <summary>
        /// 登录名已存在
        /// </summary>
        public MsgInfo ExistLoginName { get; set; }

        /// <summary>
        /// 登录名或密码不对
        /// </summary>
        public MsgInfo ErrLoginNameOrPassword { get; set; }

        /// <summary>
        /// 用户已禁用
        /// </summary>
        public MsgInfo DisableUser { get; set; }

        /// <summary>
        /// 用户不存在
        /// </summary>
        public MsgInfo UserNotExist { get; set; }

        /// <summary>
        /// 角色编码已存在
        /// </summary>
        public MsgInfo ExistRoleCode { get; set; }

        /// <summary>
        /// 角色不存在
        /// </summary>
        public MsgInfo RoleNotExist { get; set; }
    }

    /// <summary>
    /// 消息
    /// </summary>
    public class MsgInfo
    {
        /// <summary>
        /// 消息编码
        /// </summary>
        public string MsgID { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }
    }
}
