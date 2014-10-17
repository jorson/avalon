using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Test.Service
{
    public class User
    {
        public User()
        {

        }

        public User(string name, EnumField enumField, List<int> list)
        {
            this.UserName = name;
            this.EnumDemo = enumField;
            this.ListDemo = list;
            this.DateDemo = DateTime.Now;
        }

        public User(int id)
        {
            this.UserId = id;
        }

        public virtual int UserId { get; protected set; }
        public virtual string UserName { get; set; }
        public virtual EnumField EnumDemo { get; set; }
        public virtual List<int> ListDemo { get; set; }
        public virtual DateTime DateDemo { get; set; }
    }

    public enum EnumField
    {
        Field1 = 1,
        Field2 = 2
    }
}
