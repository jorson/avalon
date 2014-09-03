using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    /// <summary>
    /// 帐号导入模型
    /// </summary>
    public class ImportAccount:ICloneable
    {
        /// <summary>
        /// 自定义帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 自定义帐号密码，编码过的密码，密码编码函数参见 <see cref="md:PwdCoderMultiLangDemo" />
        /// </summary>
        public string Password { get; set; }
        

        object ICloneable.Clone()
        {
            return new ImportAccount
            {
                Account = this.Account,
                Password = this.Password,
            };
        }
    }

    /// <summary>
    /// 帐号导入模型集合
    /// </summary>
    public class ImportAccountList : List<ImportAccount>
    {

    }

    /// <summary>
    /// 导入结果状态
    /// </summary>
    public enum ImportResultStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        New=1,
        /// <summary>
        /// 合并
        /// </summary>
      //  Merge=2,
        /// <summary>
        /// 错误
        /// </summary>
        Error=3,
    }

    public class ImportCustomAccountResult
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
        /// 帐号信息
        /// </summary>
        public ImportAccount ImportInfo { get; set; }
    }
}
