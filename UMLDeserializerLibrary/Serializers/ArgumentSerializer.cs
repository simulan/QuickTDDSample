using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Decoders;

namespace UMLSerializerLibrary.Serializers {
    public class ArgumentSerializer : TypeAdapter<Argument> {
        private String input;

        public Argument Deserialize(String input) {
            this.input = input;
            String[] parts = input.Split(Internals.TYPE_DELIMITER);
            if (parts.Count() == 2) {
                return DeserializeArgument(parts[0].Trim(), parts[1].Trim());
            } else {
                throw new NotImplementedException("Parameters should have name & returntype, delimited by "+Internals.TYPE_DELIMITER);
            }
        }
        private Argument DeserializeArgument(String variableName,String variableType) {
            Internals.ValidateString(variableName, "Parameter name is invalid.");
            Internals.ValidateString(variableType, "Parameter type is invalid.");
            return new Argument(variableName,variableType);
        }
    }
}
