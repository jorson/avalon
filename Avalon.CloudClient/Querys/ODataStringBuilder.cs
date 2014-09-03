using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public class ODataStringBuilder
    {
        public ODataStringBuilder()
        {
            Filter = new StringBuilder();
            Orderby = new StringBuilder();
            Select = new StringBuilder();
        }

        public StringBuilder Filter { get; private set; }

        public StringBuilder Orderby { get; private set; }

        public int Skip { get; set; }

        public int Top { get; set; }

        public StringBuilder Select { get; private set; }

        public NameValueCollection ToNameValueCollection()
        {
            NameValueCollection datas = new NameValueCollection();
            if(Filter.Length > 0)
                datas.Add("$filter", Filter.ToString());
            if(Orderby.Length > 0)
                datas.Add("$order", Orderby.ToString());
            if (Skip > 0)
                datas.Add("$skip", Skip.ToString());
            if (Top > 0)
                datas.Add("$top", Top.ToString());

            return datas;
        }
    }
}
