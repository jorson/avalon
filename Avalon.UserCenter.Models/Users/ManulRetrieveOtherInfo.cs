using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;

namespace Avalon.UserCenter.Models
{
    /// <summary>
    /// 人工申诉:帐号核心信息,其他补充信息
    /// </summary>
    public class ManulRetrieveOtherInfo : ICloneable
    {
       /// <summary>
       /// 曾经使用过的密码
       /// </summary>
       public IList<string> OldPassWord { get; set; }
       /// <summary>
       /// 常用的登录地点
       /// </summary>
       public IList<AreaInfo> LoginArea { get; set; }
       /// <summary>
       /// 注册地点
       /// </summary>
       public AreaInfo RegisArea { get; set; }
       /// <summary>
       /// 登录手机
       /// </summary>
       public string LoginMobile { get; set; }
       /// <summary>
       /// 登录邮箱
       /// </summary>
       public string LoginMail { get; set; }
       /// <summary>
       /// 登录证件号
       /// </summary>
       public string LoginIDCard { get; set; }
       /// <summary>
       /// 密保手机
       /// </summary>
       public IList<string> ProtectMobile { get; set; }
       /// <summary>
       /// 密保邮箱
       /// </summary>
       public IList<string> ProtectMail { get; set; }
       /// <summary>
       /// 用户自述
       /// </summary>
       public string UserRemarks { get; set; }
       /// <summary>
       /// 不通过原因
       /// </summary>
       public string Reason { get; set; }

       object ICloneable.Clone()
       {
           return new ManulRetrieveOtherInfo
           {
               OldPassWord = OldPassWord,
               LoginArea = LoginArea,
               RegisArea = RegisArea,
               LoginMobile = LoginMobile,
               LoginMail = LoginMail,
               LoginIDCard = LoginIDCard,
               ProtectMobile = ProtectMobile,
               ProtectMail = ProtectMail,
               UserRemarks = UserRemarks,
               Reason = Reason
           };
       }

       public override bool Equals(object obj)
       {
           var outObj = obj as ManulRetrieveOtherInfo;
           if (outObj == null)
               return false;

           return outObj.LoginIDCard == LoginIDCard
                      && outObj.LoginMail == LoginMail
                      && outObj.LoginMobile == LoginMobile
                      && outObj.UserRemarks == UserRemarks
                      && outObj.Reason == Reason
                      && outObj.OldPassWord.EqualsList(OldPassWord)
                      && outObj.LoginArea.EqualsList(LoginArea)
                      && outObj.ProtectMobile.EqualsList(ProtectMobile)
                      && outObj.ProtectMail.EqualsList(ProtectMail);

       }
    }
}
