using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Decoders;
using UMLSerializerLibrary.Serializers;

namespace UMLDeserializerLibrary.Serializers {
    //todo change to suit classes
    public class ClassSerializer : TypeAdapter<Class> {
        private string input;
        private TypeAdapterChain parent;

        public ClassSerializer(TypeAdapterChain chain) {
            parent = chain;
        }
        public Class Deserialize(string input) {
            this.input = input;
            int typeDelimiterIndex = FindLastIndexOrThrow(Internals.TYPE_DELIMITER);
            int parameterStartDelimiterIndex = FindIndexOrThrow(Internals.PARAMETER_START_DELIMITER);
            int parameterEndDelimiterIndex = FindIndexOrThrow(Internals.PARAMETER_END_DELIMITER);
            ValidateDelimiterOrder(typeDelimiterIndex, parameterStartDelimiterIndex, parameterEndDelimiterIndex);

            return new Method(
                DeserializeAccessModifier(),
                DeserializeName(parameterStartDelimiterIndex),
                DeserializeParameters(parameterStartDelimiterIndex, parameterEndDelimiterIndex),
                DeserializeReturnType(typeDelimiterIndex));
        }

        private void ValidateDelimiterOrder(int typeDelimiterIndex, int parameterStartDelimiterIndex, int parameterEndDelimiterIndex) {
            if (typeDelimiterIndex < parameterEndDelimiterIndex) {
                throw new FormatException("Must have a return type defined after last parenthesis");
            }
            bool hasSingleStartDelimiter = FindAmountOfCharInstancesWithinInput(Internals.PARAMETER_START_DELIMITER) == 1;
            bool hasSingleEndDelimiter = FindAmountOfCharInstancesWithinInput(Internals.PARAMETER_END_DELIMITER) == 1;
            if (!hasSingleStartDelimiter && !hasSingleEndDelimiter) {
                throw new FormatException("Must have 1 opening and 1 closing parenthesis");
            }

        }
        private int FindAmountOfCharInstancesWithinInput(Char c) {
            int amountOfCharInstances = 0;
            Char[] chars = input.ToCharArray();
            foreach (Char iteratedChar in chars) {
                if (iteratedChar == c) amountOfCharInstances++;
            }
            return amountOfCharInstances;
        }
        private int FindIndexOrThrow(char c) {
            int indexOf = input.IndexOf(c);
            if (indexOf == -1) throw new FormatException("'" + c + "' character expected in " + input);
            return indexOf;
        }
        private int FindLastIndexOrThrow(char c) {
            int indexOf = input.LastIndexOf(c);
            if (indexOf == -1) throw new FormatException("'" + c + "' character expected in " + input);
            return indexOf;
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
        private string DeserializeReturnType(int indexColon) {
            string returnType = input.Substring(indexColon + 1).Replace(" ", "");
            Internals.ValidateString(returnType, "Valid return-type expected.");
            return returnType;
        }

        
    }
}
