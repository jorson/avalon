using Avalon.Framework;
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
        public virtual void GetUserList()
        {

        }
        public virtual void GetUserAll()
        {

        }
    }
}
