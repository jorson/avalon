using Avalon.Framework;
using Avalon.Test.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Test.Repository
{
    public class UserRepository : AbstractNoShardRepository<User>, IUserRepository
    {
    }
}
