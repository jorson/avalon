using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon.UserCenter.Models
{
    /// <summary>
    /// 验证码出现规则
    /// </summary>
    public enum VerifyCodeAppearRule
    {
        /// <summary>
        /// 验证码是必须的
        /// </summary>
        Need = 1,

        /// <summary>
        /// 验证码根据IP的出现
        /// </summary>
        Judge = 2,
    }

    /// <summary>
    /// 图片验证码显示格式
    /// </summary>
    public enum PicVerifyCodeDisplayFormat
    {
        /// <summary>
        /// 仅数字
        /// </summary>
        OnlyDigits = 0,
        /// <summary>
        /// 仅字母
        /// </summary>
        OnlyLetters = 1,
        /// <summary>
        /// 数字或字母
        /// </summary>
        DigitOrLetters = 2,
    }

    public enum PicVerifyCodeResultType : int
    {
        /// <summary>
        /// 默认返回类型 base64编码的图片数据
        /// </summary>
        Default = 0,
        /// <summary>
        /// 返回验证码的图片Url，img的src直接可用
        /// </summary>
        Url = 1,
        /// <summary>
        /// 返回带类型的 data:image/png;base64， img的src直接可用
        /// </summary>
        PngBase64 = 2,
    }
}
