using Avalon.UserCenter.Models;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Avalon.UserCenter
{
    public static class RsaUtil
    {
        static RsaUtil()
        {
            rsa.FromXmlString(@"<RSAKeyValue><Modulus>u4v8XXlrGVowe4bpSQEFuO1LRyK1PnUzXl+TGeYFLgL9gZbzVOcud2Eo7zPEw74oJZBOnOwdi5nXGLOmg/LApQ==</Modulus><Exponent>AQAB</Exponent><P>4xHuZ1V51UbkH8z1EzErx96+I17k4S7Yp6oOtWu9HH8=</P><Q>03D5Lttzq/3Mu+3YzLupIH46l4g2XgcVlnh97hqkoNs=</Q><DP>z7rQwiH1QkMHYXxMXBFovwGsKomek88aj1BJAnmc2Rs=</DP><DQ>vz3TY7pBqTJpVyENkj+5/RWu0Rf2dJ1bvTlGTXHzrTk=</DQ><InverseQ>KuQNdS13ZMpfPLZIZuV2i4wcrC8ohohN5fZAWQnpxKE=</InverseQ><D>iPL/pYI0Ip8pKAqr7xNACnm8roU5tBIBILjJwe+leKKWJQQJD2wSfSEgP1Lod8Z7PQ7yXaHXuNdFKvnxxRdPQQ==</D></RSAKeyValue>");
        }

        private readonly static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512);

        public static UserCode DecryptAndCheck(string encryptString, Action<string> action)
        {
            var code = UserCode.PasswordEncryptError;
            try
            {
                if (!String.IsNullOrWhiteSpace(encryptString))
                {
                    var decryptString = DecryptFromBase64String(encryptString);
                    var pIdx = decryptString.LastIndexOf(':');

                    if (pIdx >= 0)
                    {
                        var tspStr = decryptString.Substring(pIdx + 1);
                        double timestampDouble;
                        if (double.TryParse(tspStr, out timestampDouble))
                        {
                            var timestamp = NetworkTime.Null.Date.AddMilliseconds(timestampDouble);
                            var timeSpen = NetworkTime.Now.ToUniversalTime() - timestamp;
                            var timeMinute = timeSpen.TotalMinutes;
                            if (-15 <= timeMinute && timeMinute <= 15)
                            {
                                var password = decryptString.Substring(0, pIdx);
                                code = UserCode.Success;
                                action(password);
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return code;
        }

        public static string DecryptFromBase64String(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);
            var dBytes = rsa.Decrypt(bytes, false);
            return Encoding.UTF8.GetString(dBytes);
        }
    }
}
