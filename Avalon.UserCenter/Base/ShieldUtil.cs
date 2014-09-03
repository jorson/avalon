using System.Linq;

namespace Avalon.UserCenter
{
    public class ShieldUtil
    {
        public static string Mobile(string mobile)
        {
            if (mobile.IsNullOrWhiteSpace())
                return mobile;
            var rs = mobile.Substring(0, 3) + "****" + mobile.Substring(mobile.Length - 3, 3);
            return rs;
        }

        public static string Email(string email)
        {
            if (email.IsNullOrWhiteSpace())
                return email;
            var split1 = email.IndexOf("@");
            var firstString = email.Substring(0, split1);
            var endString = email.Substring(split1, email.Length - split1);

            if (firstString.Length > 3)
            {
                var firstString1 = firstString.Substring(0, 2);
                var firstString2 = firstString.Substring(firstString.Length - 1, 1);
                firstString = firstString1 + "****" + firstString2;
            }
            else
            {
                firstString = firstString + "****";
            }

            var rs = firstString + endString;

            return rs;
        }

        public static string IdCard(string idCard)
        {
            if (idCard.IsNullOrWhiteSpace())
                return idCard;
            if (UserValid.GetIdCardType(idCard) == IdCardType.Army)
            {
                var num = idCard.IndexOf("第");
                var temp = idCard.Substring(num + 6);
                var headtemp = idCard.Substring(0, num + 2);
                return headtemp + "****" + temp;
            }
            var rs = idCard.Substring(0, 4) + "****" + idCard.Substring(idCard.Length - 3, 3);
            return rs;
        }
    }
}