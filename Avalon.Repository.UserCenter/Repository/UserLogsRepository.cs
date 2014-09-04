using Avalon.Framework;
using Avalon.UserCenter;
using Avalon.NHibernateAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Avalon.Repository.UserCenter.Repository
{
    public class UserRegisterLogRepository : AbstractNoShardRepository<UserRegisterLog>, IUserRegisterLogRepository
    {


    }
    
    public class UserLoginLogRepository : AbstractNoShardRepository<UserLoginLog>, IUserLoginLogRepository
    {
         /// <summary>
        /// 登陆前五信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IList<UserLoginLog> GetLoginStaList(long userid, DateTime createTime)
        {
            this.CreateSpecification().Where(a => a.UserId == userid);
            var session = this.GetSession();
            var conn = session.Connection;
            IList<UserLoginLog> list = new List<UserLoginLog>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText =  @" Select  IpCityId,IpCityIdNum,LoginTime 
from (SELECT count(IpCityId) as IpCityIdNum,IpCityId,max(LoginTime) as LoginTime FROM log_userlogin where userid=@userid and LoginTime<=@createTime group by IpCityId) a 
order by IpCityIdNum desc,LoginTime desc limit 5";
                
                IDbDataParameter paraUserid = cmd.CreateParameter();
                paraUserid.ParameterName = "@userid";
                paraUserid.DbType = DbType.Int32;
                paraUserid.Value = userid;
                IDbDataParameter paracreateTime = cmd.CreateParameter();
                paracreateTime.ParameterName = "@createTime";
                paracreateTime.DbType = DbType.DateTime;
                paracreateTime.Value = createTime;
                cmd.Parameters.Add(paraUserid);
                cmd.Parameters.Add(paracreateTime);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())   //一列一列读 读完返回false
                {

                    list.Add(new UserLoginLog
                    {
                        IpCityId = Convert.ToInt32(reader["IpCityId"]),
                        LoginTime = Convert.ToDateTime(reader["LoginTime"]),
                        IpCityIdNum = Convert.ToInt32(reader["IpCityIdNum"])
                    });
                }
                reader.Close();

            }
            return list;

        }
    }

    public class UserMainHistoryLogRepository : AbstractNoShardRepository<UserMainHistoryLog>, IUserMainHistoryLogRepository
    {

    }

    public class UserAccountCreateInfoRepository : AbstractNoShardRepository<UserAccountCreateInfo>, IUserAccountCreateInfoRepository
    {


    }
}
