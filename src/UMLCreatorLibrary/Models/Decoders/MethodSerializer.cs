using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models.Decoders {
    public class MethodSerializer : TypeAdapter<Method> {
        private string input;
        private TypeAdapterChain parent;

        public MethodSerializer(TypeAdapterChain chain) {
            parent = chain;
        }
        public Method Deserialize(String input) {
            this.input = input;
            int typeDelimiterIndex = FindLastIndexOrThrow(Internals.TYPE_DELIMITER);
            int parameterStartDelimiterIndex = FindIndexOrThrow(Internals.PARAMETER_START_DELIMITER);
            int parameterEndDelimiterIndex = FindIndexOrThrow(Internals.PARAMETER_END_DELIMITER);
            ValidateDelimiterOrder(typeDelimiterIndex, parameterStartDelimiterIndex, parameterEndDelimiterIndex);
            //Might use Substring here
            return new Method(
                DecodeAccessModifier(),
                DecodeName(parameterStartDelimiterIndex),
                DecodeParameters(parameterStartDelimiterIndex, parameterEndDelimiterIndex),
                DecodeReturnType(typeDelimiterIndex));
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

        private AccessScope DecodeAccessModifier() {
            switch (input[0]) {
                case Internals.ACCESS_PUBLIC: return AccessScope.PUBLIC;
                case Internals.ACCESS_PROTECTED: return AccessScope.PUBLIC;
                case Internals.ACCESS_PRIVATE: return AccessScope.PUBLIC;
                default: throw new FormatException("There is no access modifier such as '" + input[0] + "'");
            }
        }
        private String DecodeName(int indexFirstParenthesis) {
            const int PREFIX_OFFSET = 1;
            int argsLength = input.Length - indexFirstParenthesis;
            string name = input.Substring(PREFIX_OFFSET, input.Length - argsLength - PREFIX_OFFSET).Replace(" ", "");
            if (name.Length == 0) throw new FormatException("Method has no name");
            if (!Internals.IsAlpha(name)) throw new FormatException("Method has invalid characters");
            return name;
        }

        private List<Argument> DecodeParameters(int indexFirstParenthesis, int indexLastParenthesis) {
            const int SINGLE_OFFSET = 1;
            List<Argument> decodedParameters = new List<Argument>();
            String argumentsInput = input.Substring(indexFirstParenthesis + SINGLE_OFFSET, indexLastParenthesis - indexFirstParenthesis - SINGLE_OFFSET);
            TypeAdapter<Argument> argumentDeserializer = parent.provide<Argument>() as TypeAdapter<Argument>;
            foreach (String part in argumentsInput.Split(Internals.VARIABLES_DELIMITER)) {
                if (!part.Trim().Equals("")) {
                    decodedParameters.Add(argumentDeserializer.Deserialize(part));
                }
            }
            return decodedParameters;
        }

        private string DecodeReturnType(int indexColon) {
            string returnType = input.Substring(indexColon + 1).Replace(" ", "");
            Internals.ValidateString(returnType,"Valid return-type expected.");
            return returnType;
        }
    }
}
