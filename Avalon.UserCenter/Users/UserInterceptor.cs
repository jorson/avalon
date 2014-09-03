using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Avalon.UserCenter.Models;
using Avalon.Framework;

namespace Avalon.UserCenter
{
    public class UserInterceptor : AbstractRepositoryFrameworkInterceptor<User>
    {
        public override void PreUpdate(User entity)
        {
            UpdateUser(entity);
            base.PreUpdate(entity);
        }



        private void UpdateUser(User entity)
        {
            var userService = DependencyResolver.Resolve<UserService>();
            userService.InterceptorUpdate(entity);
        }


    }

    public class UserSecurityInterceptor : AbstractRepositoryFrameworkInterceptor<UserSecurity>
    {
        public override void PreUpdate(UserSecurity entity)
        {
            UpdateUserSecurity(entity);
            base.PreUpdate(entity);
        }



        private void UpdateUserSecurity(UserSecurity entity)
        {
            var userService = DependencyResolver.Resolve<UserService>();
            userService.InterceptorUpdate(entity);
        }


    }
}
