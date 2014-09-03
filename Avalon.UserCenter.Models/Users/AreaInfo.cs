using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
   public class AreaInfo:ICloneable
    {

       public int ProviceId { get; set; }

       public int CityId { get; set; }

       object ICloneable.Clone()
       {
           return new AreaInfo
           {
               ProviceId = this.CityId,
               CityId = this.CityId,
           };
       }

       public override bool Equals(object obj)
       {
           var areaInfo = obj as AreaInfo;
           if (areaInfo == null)
               return false;

           return ProviceId == areaInfo.ProviceId && CityId == areaInfo.CityId;
       }
    }
}
