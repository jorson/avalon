using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Test.Service
{
    public class Order
    {
        protected Order()
        {

        }
        public Order(string number, int userId, DateTime date)
        {
            this.UserId = userId;
            this.OrderNumber = number;
            this.OrderDate = date;
        }

        public virtual int OrderId { get; set; }
        public virtual int UserId { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual DateTime OrderDate { get; set; }
    }
}
