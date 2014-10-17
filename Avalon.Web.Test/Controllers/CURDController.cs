using Avalon.Framework.Querys;
using Avalon.Test.Service;
using Avalon.Test.Service.Filters;
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

        /// <summary>
        /// Index视图
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 注册视图
        /// </summary>
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 创建用户信息
        /// </summary>
        [AjaxApi]
        [CustomActionName("create")]
        public object CreateUser(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            User user = new User(name, EnumField.Field1, new List<int>() { 1, 2, 3, 4 });
            this.userService.CreateUser(user);
            return new
            {
                UserId = user.UserId,
                UserName = user.UserName
            };
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        [AjaxApi]
        [CustomActionName("update")]
        public object UpdateUser(int id, string userName)
        {
            var user = this.userService.GetUser(id);
            if (user != null)
            {
                user.UserName = userName;
                user.DateDemo = DateTime.Now;
                this.userService.UpdateUser(user);
            }
            return user;

        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [AjaxApi]
        [CustomActionName("delete")]
        public void DeleteUser(int id)
        {
            this.userService.DeleteUser(new User(id));
        }
        /// <summary>
        /// 获取单个用户的信息
        /// </summary>
        /// <returns>单用户信息</returns>
        [AjaxApi]
        [CustomActionName("get")]
        public object GetUser(int id)
        {
            var user = this.userService.GetUser(id);
            return user == null ? null : new { UserId = user.UserId, UserName = user.UserName };
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        [AjaxApi]
        [CustomActionName("list")]
        public object GetUserList(string userName = "", int enumValue = -1)
        {
            IList<User> userList = new List<User>();

            if(String.IsNullOrEmpty(userName) && enumValue == -1)
            {
                userList = this.userService.GetUserList();
            }
            else
            {
                UserFilter filter = new UserFilter()
                {
                    UserName = userName,
                    EnumField = (EnumField)enumValue
                };
                userList = this.userService.GetUserList(filter);
            }
            return userList.Select(o=>new
            {
                UserId = o.UserId,
                UserName = o.UserName,
                EnField = o.EnumDemo.ToString()
            });
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
