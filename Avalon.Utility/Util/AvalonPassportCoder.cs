using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Avalon.Utility
{
    public static class AvalonPassportCoder
    {
        /// <summary>
        /// 对给定的明文密码按照91安全中心的模式进行签名加密
        /// </summary>
        public static string EncryptPassword(string password)
        {
            string content = password + "\xa3\xac\xa1\xa3" + "fdjf,jkgfkl";
            MD5 md5 = new MD5CryptoServiceProvider();

            //28591 Latin1 ISO-8859-1
            var buffer = Encoding.GetEncoding(28591).GetBytes(content);
            byte[] hashBuffer = md5.ComputeHash(buffer);
            return StringUtil.ToHex(hashBuffer);
        }
    }
}
