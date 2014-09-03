using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class ListExtend
    {
        public static bool EqualsList<T>(this IList<T> lList, IList<T> rList)
        {
            if (lList != null && rList != null)
            {
                if (lList.Count != rList.Count)
                    return false;

                for (var i = 0; i < rList.Count; i++)
                {
                    var l = lList[i];
                    var r = rList[i];
                    var eq = EqualityComparer<T>.Default;
                    if (!eq.Equals(l, r))
                        return false;
                }
                return true;
            }

            return lList == rList;
        }
    }
}
