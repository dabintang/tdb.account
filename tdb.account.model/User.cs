using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace tdb.account.model
{
    ///<summary>
    ///用户
    ///</summary>
    [SugarTable("user")]
    public partial class User
    {
           public User(){


           }
           /// <summary>
           /// Desc:用户编码/登录名
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string UserCode {get;set;}

           /// <summary>
           /// Desc:用户名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserName {get;set;}

           /// <summary>
           /// Desc:密码(MD5)
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Password {get;set;}

           /// <summary>
           /// Desc:是否管理员
           /// Default:b'0'
           /// Nullable:False
           /// </summary>           
           public bool IsAdmin {get;set;}

           /// <summary>
           /// Desc:是否启用
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool Enable {get;set;}

           /// <summary>
           /// Desc:创建者
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Creater {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:更新者
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Updater {get;set;}

           /// <summary>
           /// Desc:更新时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime UpdateTime {get;set;}

    }
}
