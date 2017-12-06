using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models.Deserializers {
    public abstract class TypeAdapterChain {
        public abstract TypeAdapter<Object> provide<T>() where T : class;
    }
}
