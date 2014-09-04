using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Avalon.Repository.UserCenter.Mappinig;
using Avalon.Framework;
using Avalon.UserCenter;
using Avalon.Utility;
using Avalon.MongoAccess;

namespace Avalon.Repository.UserCenter.Repository
{
    public class PicVerifyIpStatRepository : AbstractNoShardRepository<PicVerifyIpStat>, IPicVerifyIpStatRepository
    {
        private MongoCollection table;
        private MongoCollection Table
        {
            get {
                return table ??
                       (table =
                           this.GetMongoSession(ShardParams.Empty).GetCollection<PicVerifyIpStat>(ShardParams.Empty));
            }
        }

        void IPicVerifyIpStatRepository.Remove(long ipAddress, PicVerifyIpStatType type)
        {
            
            var intType = (int) type;
            var q = Query.And(
                Query.EQ("IPAddress",ipAddress),
                Query.EQ("OptType",intType)
                );
            //var tab = db.GetCollection(PicVerifyIpStatDefine.TABLENAME);
            Table.Remove(q);
        }

        public int Count(long ipAddress, DateTime dateTime, PicVerifyIpStatType type)
        {
            var intType = (int)type;
            var q = Query.And(
                Query.EQ("IPAddress", ipAddress),
                Query.EQ("OptType", intType),
                Query.GT("RecordTime", new BsonDateTime(dateTime))
                );
            //var tab = db.GetCollection(PicVerifyIpStatDefine.TABLENAME);
            return (int)Table.Count(q);
        }
    }

    public class MobileVerifyCodeLogRepository : AbstractNoShardRepository<MobileVerifyCodeStat>, IMobileVerifyCodeStatRepository
    {
        
    }

    public class UserVeriyCodeRepeateSendStatRepository : AbstractNoShardRepository<UserVeriyCodeRepeateSendStat>, IUserVeriyCodeRepeateSendStatRepository
    {

    }
}
