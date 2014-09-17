using Avalon.Test.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avalon.Web.Test.Controllers
{
    public class CURDController : Controller
    {
        private UserService userService;

        public CURDController(UserService userService)
        {
            this.userService = userService;
        }

        public int CreateUser()
        {
            User user = new User("Test", EnumField.Field1, new List<int>() { 1, 2, 3, 4 });
            this.userService.CreateUser(user);
            return user.UserId;
        }
        public int UpdateUser()
        {
            var user = this.userService.GetUser(1);
            if (user != null)
            {
                user.UserName = "TestChange";
                user.DateDemo = DateTime.Now;
                this.userService.UpdateUser(user);
                return user.UserId;
            }
            return -1;

        }
        public string GetUser()
        {
            var user = this.userService.GetUser(1);
            return user.UserName;
        }
        public bool GetUserList()
        {
            return false;
        }
    }
}
