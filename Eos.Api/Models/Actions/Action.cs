using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Eos.Models
{
    public abstract class Action<T> : GenericAction, IAction<T> 
    {
        public T Data { get; set; }

        public override abstract Name Name { get; }

        [JsonIgnore]
        public override object RawData => Data;
    }
}
