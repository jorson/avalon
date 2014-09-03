using System;
using System.Collections.Generic;
using System.Linq;

namespace Avalon.UserCenter
{
    public class Validor<Code> where Code : struct
    {
        public Code OkCode { get; private set; }

        public Validor(Code okCode)
        {
            OkCode = okCode;
        }

        private readonly IList<dynamic> validDataList = new List<dynamic>();

        public void AppendValidAndSet(Func<Code> validFunc, Action action = null)
        {
            var data = new ValidData {ValidFunc = validFunc, Action = action};
            validDataList.Add(data);
        }


        public Code Valid()
        {
            foreach (var valid in validDataList)
            {
                var rsCode = valid.ValidFunc();
                if (rsCode.Equals(OkCode))
                {
                    if (valid.Action != null)
                        valid.Action();

                    continue;
                }
                return rsCode;
            }
            return OkCode;
        }

        protected class ValidData
        {
            public Func<Code> ValidFunc { get; set; }

            public Action Action { get; set; }
        }

    }

    public class Validor<TEntity, TCode> where TCode : struct
    {
        public TCode OkCode { get; private set; }

        public TEntity Entity { get; private set; }

        public Validor(TEntity entity, TCode okCode)
        {
            OkCode = okCode;
            Entity = entity;
        }

        private readonly IList<ValidData> validDataList = new List<ValidData>();

        public void AppendValidAndSet(Func<TEntity, TCode> validFunc, Action<TEntity> thenAction = null)
        {
            var data = new ValidData { ValidFunc = validFunc, Action = thenAction };
            validDataList.Add(data);
        }

        public void AppendValidAndSet<TValue>(Func<TEntity, TValue> property, Func<TValue, TCode> valid,
            Action<TEntity> thenAction = null)
        {
            AppendValidAndSet(o => valid(property(o)), thenAction);
        }

        public TCode Valid()
        {
            foreach (var valid in validDataList)
            {
                var rsCode = valid.ValidFunc(Entity);
                if (rsCode.Equals(OkCode))
                {
                    if (valid.Action != null)
                        valid.Action(Entity);
                    continue;
                }
                return rsCode;
            }
            return OkCode;
        }

        class ValidData
        {
            public Func<TEntity, TCode> ValidFunc { get; set; }

            public Action<TEntity> Action { get; set; }
        }
    }
}