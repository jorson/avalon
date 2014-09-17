using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Test.Service
{
    public interface IUserRepository : IShardRepository<User>
    {
    }
}
