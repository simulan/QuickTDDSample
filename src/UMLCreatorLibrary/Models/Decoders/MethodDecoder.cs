using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models.Decoders {
    public class MethodDecoder {
        private string input;
        private const char ACCESS_PUBLIC = '+';
        private const char ACCESS_PROTECTED = '#';
        private const char ACCESS_PRIVATE = '-';
        private const char TYPE_DELIMITER = ':';
        private const char PARAMETER_START_DELIMITER = '(';
        private const char PARAMETER_END_DELIMITER = ')';
        private const char VARIABLES_DELIMITER = ',';

        public MethodDecoder() {
        }

        public Method Decode(String input) {
            this.input = input;
            int typeDelimiterIndex = FindLastIndexOrThrowError(TYPE_DELIMITER);
            int parameterStartDelimiterIndex = FindIndexOrThrowError(PARAMETER_START_DELIMITER);
            int parameterEndDelimiterIndex = FindIndexOrThrowError(PARAMETER_END_DELIMITER);
            validateDelimiterOrder(typeDelimiterIndex, parameterStartDelimiterIndex, parameterEndDelimiterIndex);

            return new Method(
                DecodeAccessModifier(),
                DecodeName(parameterStartDelimiterIndex),
                DecodeParameters(parameterStartDelimiterIndex, parameterEndDelimiterIndex),
                DecodeReturnType(typeDelimiterIndex));
        }
        private void validateDelimiterOrder(int typeDelimiterIndex, int parameterStartDelimiterIndex, int parameterEndDelimiterIndex) {
            if (typeDelimiterIndex < parameterEndDelimiterIndex) {
                throw new FormatException("Must have a return type defined after last parenthesis");
            }
            bool hasSingleStartDelimiter = FindAmountOfCharInstancesWithinInput(PARAMETER_START_DELIMITER) == 1;
            bool hasSingleEndDelimiter = FindAmountOfCharInstancesWithinInput(PARAMETER_END_DELIMITER) == 1;
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
        private int FindIndexOrThrowError(char c) {
            int indexOf = input.IndexOf(c);
            if (indexOf == -1) throw new FormatException("'" + c + "' character expected in " + input);
            return indexOf;
        }
        private int FindLastIndexOrThrowError(char c) {
            int indexOf = input.LastIndexOf(c);
            if (indexOf == -1) throw new FormatException("'" + c + "' character expected in " + input);
            return indexOf;
        }
        private AccessScope DecodeAccessModifier() {
            switch (input[0]) {
                case ACCESS_PUBLIC: return AccessScope.PUBLIC;
                case ACCESS_PROTECTED: return AccessScope.PUBLIC;
                case ACCESS_PRIVATE: return AccessScope.PUBLIC;
                default: throw new FormatException("There is no access modifier such as '" + input[0] + "'");
            }
        }
        private String DecodeName(int indexFirstParenthesis) {
            const int PREFIX_OFFSET = 1;
            int argsLength = input.Length - indexFirstParenthesis;
            string name = input.Substring(PREFIX_OFFSET, input.Length - argsLength - PREFIX_OFFSET).Replace(" ", "");
            if (name.Length == 0) throw new FormatException("Method has no name");
            if (!IsAlpha(name)) throw new FormatException("Method has invalid characters");
            return name;
        }
        public bool IsAlpha(string input) {
            return Regex.IsMatch(input, "^[a-zA-Z1-9]+$");
        }
        private List<Argument> DecodeParameters(int indexFirstParenthesis, int indexLastParenthesis) {
            const int SINGLE_OFFSET = 1;
            List<Argument> decodedParameters = new List<Argument>();
            String argumentsInput = input.Substring(indexFirstParenthesis + SINGLE_OFFSET, indexLastParenthesis - indexFirstParenthesis - SINGLE_OFFSET);
            foreach (String part in argumentsInput.Split(VARIABLES_DELIMITER)) {
                if (!part.Trim().Equals("")) {
                    decodedParameters.Add(DecodeParameter(part));
                }
            }
            return decodedParameters;
        }
        private Argument DecodeParameter(string parameterInput) {
            Argument arg = new Argument();
            String[] parts = parameterInput.Split(TYPE_DELIMITER);
            if (parts.Count() == 2) {
                String variableName = parts[0].Trim();
                String variableType = parts[1].Trim();
                if (variableName.Equals("")) {
                    throw new FormatException("Argument name not specified");
                } else if(variableType.Equals("")) {
                    throw new FormatException("Agument type not specified");
                } else {
                    arg.Name = parts[0];
                    arg.ReturnType = parts[1];
                    return arg;
                }
            } else {
                throw new NotImplementedException("Parameters should have name & returntype, properly delimited");
            }
        }
        private string DecodeReturnType(int indexColon) {
            return input.Substring(indexColon + 1).Replace(" ", "");
        }

    }
}
