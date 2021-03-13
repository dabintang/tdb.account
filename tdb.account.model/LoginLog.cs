using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace tdb.account.model
{
    ///<summary>
    ///用户登录日志
    ///</summary>
    [SugarTable("login_log")]
    public partial class LoginLog
    {
           public LoginLog(){


           }
           /// <summary>
           /// Desc:自增ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public long ID {get;set;}

           /// <summary>
           /// Desc:用户编码/登录名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserCode {get;set;}

           /// <summary>
           /// Desc:服务编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ServiceCode {get;set;}

           /// <summary>
           /// Desc:token
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Token {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

    }
}
