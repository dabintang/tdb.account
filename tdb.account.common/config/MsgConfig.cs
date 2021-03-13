﻿using System;
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
        public MsgInfo ExistUserCode { get; set; }

        /// <summary>
        /// 登录名或密码不对
        /// </summary>
        public MsgInfo ErrUserCodeOrPassword { get; set; }
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