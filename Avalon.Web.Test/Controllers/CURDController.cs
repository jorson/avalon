using Avalon.Framework.Querys;
using Avalon.Test.Service;
using Avalon.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avalon.Web.Test.Controllers
{
    public class CURDController : Controller
    {
        private readonly UserService userService;
        private readonly OrderService orderService;

        public CURDController(UserService userService, OrderService orderService)
        {
            this.userService = userService;
            this.orderService = orderService;
        }

        public int CreateUser()
        {
            User user = new User("Test", EnumField.Field1, new List<int>() { 1, 2, 3, 4 });
            this.userService.CreateUser(user);
            return user.UserId;
        }
        public int CreateOrder()
        {
            Order order = new Order("Order_1", 1, DateTime.Now);
            this.orderService.CreateOrder(order);
            return order.OrderId;
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
        
        [ODataQuery(typeof(UserOrderQueryFilter), typeof(User))]
        public object SearchUseOData()
        {
            var data = ODataProcessor.Process<UserOrderQueryFilter, User>(
                this.HttpContext, null,
                null, null);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [OpenApi]
        [CustomActionName("ajax")]
        public object AjaxReturn()
        {
            User user = new User("Test", EnumField.Field1, new List<int>() { 1, 2, 3, 4 });
            return user;
        }
    }
}
