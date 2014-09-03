using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    /// <summary>
    /// 华渔通行证用户导入模型
    /// </summary>
    public class ImportUser : ICloneable
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 证件号，有效的身份证、军官证或回乡证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 登录邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 登录手机
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// 华渔通行证用户密码，编码过的密码，密码编码函数参见 <see cref="md:PwdCoderMultiLangDemo" />
        /// </summary>
        public string Password { get; set; }


        object ICloneable.Clone()
        {
            return new ImportUser
            {
                UserName = this.UserName,
                IDCard = this.IDCard,
                Email = this.Email,
                Mobile = this.Mobile,
                Password = this.Password,
            };
        }
    }

    /// <summary>
    /// 华渔通行证用户导入模型集合
    /// </summary>
    public class ImportUserList : List<ImportUser>
    {

    }

    public class ImportUserResult
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 导入结果状态
        /// </summary>
        public ImportResultStatus Status { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public UserCode ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public ImportUser ImportInfo { get; set; }
    }
}
