﻿using Avalon.Framework;
using Avalon.Test.Service.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Test.Service
{
    public class UserService : IService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public virtual void CreateUser(User entry)
        {
            this.userRepository.Create(entry);
        }
        public virtual void UpdateUser(User entry)
        {
            this.userRepository.Update(entry);
        }
        public virtual void DeleteUser(User entry)
        {
            this.userRepository.Delete(entry);
        }
        public virtual User GetUser(int id)
        {
            return this.userRepository.Get(id);
        }
        public virtual IList<User> GetUserList(UserFilter filter = null)
        {
            var spec = this.userRepository.CreateSpecification().Where(o => o.UserId > 0);
            if (filter != null)
            {
                if (!String.IsNullOrEmpty(filter.UserName))
                {
                    spec = spec.And(o => o.UserName.Contains(filter.UserName));
                }
                if (filter.EnumValue != 0)
                {
                    spec = spec.And(o => o.EnumDemo == (EnumField)filter.EnumValue);
                }
            }
            return this.userRepository.FindAll(spec);
        }
    }
}
