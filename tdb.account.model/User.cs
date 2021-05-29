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
           /// Desc:生日
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? Birthday {get;set;}

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
           /// Desc:电子邮箱
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Email {get;set;}

           /// <summary>
           /// Desc:是否启用
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool Enable {get;set;}

           /// <summary>
           /// Desc:性别（0：未知；1：男；2：女）
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public int Gender {get;set;}

           /// <summary>
           /// Desc:登录名
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string LoginName {get;set;}

           /// <summary>
           /// Desc:手机号码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string MobilePhone {get;set;}

           /// <summary>
           /// Desc:密码(MD5)
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Password {get;set;}

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

           /// <summary>
           /// Desc:用户名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserName {get;set;}

    }
}
