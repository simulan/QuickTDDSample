using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public MethodDecoder() {
        }

        public Method Decode(String input) {
            this.input = input;
            int typeDelimiterIndex = FindLastIndexOrThrowError(TYPE_DELIMITER);
            int parameterStartDelimiterIndex = FindIndexOrThrowError(PARAMETER_START_DELIMITER);
            int parameterEndDelimiterIndex = FindIndexOrThrowError(PARAMETER_END_DELIMITER);
            throwIfInvalidDelimiters(typeDelimiterIndex, parameterStartDelimiterIndex, parameterEndDelimiterIndex);

            return new Method(
                DecodeAccessModifier(input[0]),
                DecodeName(input, parameterStartDelimiterIndex),
                DecodeParameters(input, parameterStartDelimiterIndex, parameterEndDelimiterIndex),
                DecodeReturnType(input, typeDelimiterIndex));
        }
        private void throwIfInvalidDelimiters(int typeDelimiterIndex, int parameterStartDelimiterIndex, int parameterEndDelimiterIndex) {
            if (typeDelimiterIndex < parameterEndDelimiterIndex) {
                throw new FormatException("A UML method notation must have a return type defined after last parenthesis");
            }
            bool hasSingleStartDelimiter = FindAmountOfCharInstancesWithinInput(PARAMETER_START_DELIMITER) == 1;
            bool hasSingleEndDelimiter = FindAmountOfCharInstancesWithinInput(PARAMETER_END_DELIMITER) == 1;
            if (!hasSingleStartDelimiter && !hasSingleEndDelimiter) {
                throw new FormatException("A UML method notation must have 1 opening and 1 closing parenthesis");
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
        private AccessScope DecodeAccessModifier(char c) {
            switch (c) {
                case ACCESS_PUBLIC: return AccessScope.PUBLIC;
                case ACCESS_PROTECTED: return AccessScope.PUBLIC;
                case ACCESS_PRIVATE: return AccessScope.PUBLIC;
                default: throw new NotImplementedException("There is no access modifier such as '" + c + "'");
            }
        }
        private String DecodeName(string input, int indexFirstParenthesis) {
            const int PREFIX_OFFSET = 1;
            int argsLength = input.Length - indexFirstParenthesis;
            string name = input.Substring(PREFIX_OFFSET, input.Length - argsLength - PREFIX_OFFSET);
            return name.Replace(" ", "");
        }
        private List<Argument> DecodeParameters(string input, int indexFirstParenthesis, int indexLastParenthesis) {
            const int SINGLE_OFFSET = 1;
            List<Argument> decodedParameters = new List<Argument>();
            String argumentsInput = input.Substring(indexFirstParenthesis + SINGLE_OFFSET, indexLastParenthesis - indexFirstParenthesis - SINGLE_OFFSET);
            foreach (String part in argumentsInput.Split(',')) {
                if (part.Trim() != "") {
                    decodedParameters.Add(DecodeParameter(part));
                }
            }
            return decodedParameters;
        }
        private Argument DecodeParameter(string input) {
            Argument arg = new Argument();
            String[] parts = input.Split(TYPE_DELIMITER);
            if (parts.Count() == 2) {
                arg.Name = parts[0];
                arg.ReturnType = parts[1];
                return arg;
            } else {
                throw new NotImplementedException("Parameters should have name & returntype, properly delimited");
            }
        }
        private string DecodeReturnType(string input, int indexColon) {
            return input.Substring(indexColon + 1).Replace(" ", "");
        }

    }
}
