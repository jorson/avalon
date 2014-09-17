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
        public bool UpdateUser()
        {
            return false;
        }
        public bool GetUser()
        {
            return false;
        }
        public bool GetUserList()
        {
            return false;
        }
    }
}
