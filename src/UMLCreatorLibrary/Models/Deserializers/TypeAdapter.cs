using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models.Deserializers {
    public interface TypeAdapter<out T> {
        T Deserialize(string val);
    }
}
