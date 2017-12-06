using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models.Deserializers;

namespace UMLCreatorLibrary.Models.Deserializers {
    public class ClassSerializer : TypeAdapter<Class> {
        private string input;
        private TypeAdapterChain parent;

        public ClassSerializer(TypeAdapterChain chain) {
            parent = chain;
        }

        public Class Deserialize(string input) {
            this.input = input;
            int parameterStartDelimiterIndex = FindIndexOrThrow(Internals.PARAMETER_START_DELIMITER);
            int parameterEndDelimiterIndex = FindIndexOrThrow(Internals.PARAMETER_END_DELIMITER);

            return new Class(
                DeserializeAccessModifier(),
                DeserializeName(parameterStartDelimiterIndex),
                DeserializeParameters(parameterStartDelimiterIndex, parameterEndDelimiterIndex));
        }

        private AccessScope DeserializeAccessModifier() {
            switch (input[0]) {
                case Internals.ACCESS_PUBLIC: return AccessScope.PUBLIC;
                case Internals.ACCESS_PROTECTED: return AccessScope.PUBLIC;
                case Internals.ACCESS_PRIVATE: return AccessScope.PUBLIC;
                default: throw new FormatException("There is no access modifier such as '" + input[0] + "'");
            }
        }
        private String DeserializeName(int indexFirstParenthesis) {
            const int PREFIX_OFFSET = 1;
            int argsLength = input.Length - indexFirstParenthesis;
            string name = input.Substring(PREFIX_OFFSET, input.Length - argsLength - PREFIX_OFFSET).Replace(" ", "");
            Internals.ValidateString(name, "Method should have a valid name");
            return name;
        }
        private List<Argument> DeserializeParameters(int indexFirstParenthesis, int indexLastParenthesis) {
            const int SINGLE_OFFSET = 1;
            List<Argument> deserializedParameters = new List<Argument>();
            String argumentsInput = input.Substring(indexFirstParenthesis + SINGLE_OFFSET, indexLastParenthesis - indexFirstParenthesis - SINGLE_OFFSET);
            TypeAdapter<Argument> argumentDeserializer = parent.provide<Argument>() as TypeAdapter<Argument>;
            foreach (String part in argumentsInput.Split(Internals.VARIABLES_DELIMITER)) {
                if (!part.Trim().Equals("")) {
                    deserializedParameters.Add(argumentDeserializer.Deserialize(part));
                }
            }
            return deserializedParameters;
        }

        private int FindIndexOrThrow(char c) {
            int indexOf = input.IndexOf(c);
            if (indexOf == -1) throw new FormatException("'" + c + "' character expected in " + input);
            return indexOf;
        }
    }
}
