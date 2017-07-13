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

        public AccessScope AccessModifier { get; private set; }
        public string Name { get; private set; }
        public List<Argument> Parameters { get; private set; }
        public string ReturnType { get; private set; }

        public MethodDecoder(String input) {
            this.input = input;
        }
       
        public void Decode() {
            int typeDelimiterIndex = FindLastIndexOrThrowError(TYPE_DELIMITER);
            int parameterStartDelimiterIndex = FindIndexOrThrowError(PARAMETER_START_DELIMITER);
            int parameterEndDelimiterIndex = FindIndexOrThrowError(PARAMETER_END_DELIMITER);
            throwIfInvalidDelimiters(typeDelimiterIndex, parameterStartDelimiterIndex, parameterEndDelimiterIndex);

            DecodeAccessModifier(input[0]);
            DecodeName(input, parameterStartDelimiterIndex);
            DecodeParameters(input, parameterStartDelimiterIndex, parameterEndDelimiterIndex);
            DecodeReturnType(input, typeDelimiterIndex);
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
        private void DecodeAccessModifier(char c) {
            switch (c) {
                case ACCESS_PUBLIC: AccessModifier = AccessScope.PUBLIC;
                    break;
                case ACCESS_PROTECTED: AccessModifier = AccessScope.PUBLIC;
                    break;
                case ACCESS_PRIVATE: AccessModifier = AccessScope.PUBLIC;
                    break;
                default: throw new NotImplementedException("There is no access modifier such as '" + c + "'");
            }
        }
        private void DecodeName(string input, int indexFirstParenthesis) {
            const int PREFIX_OFFSET = 1;
            int argsLength = input.Length - indexFirstParenthesis;
            Name = input.Substring(PREFIX_OFFSET, input.Length - argsLength - PREFIX_OFFSET).Replace(" ", "");
        }
        private void DecodeParameters(string input, int indexFirstParenthesis, int indexLastParenthesis) {
            const int SINGLE_OFFSET = 1;
            List<Argument> decodedParameters = new List<Argument>();
            String argumentsInput = input.Substring(indexFirstParenthesis + SINGLE_OFFSET, indexLastParenthesis - indexFirstParenthesis - SINGLE_OFFSET);
            foreach (String part in argumentsInput.Split(',')) {
                if (part.Trim() != "") {
                    decodedParameters.Add(DecodeParameter(part));
                }
            }
            Parameters = decodedParameters;
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
        private void DecodeReturnType(string input, int indexColon) {
            const int NEXT_OFFSET = 1;
            ReturnType = input.Substring(indexColon + NEXT_OFFSET).Replace(" ", "");
        }
        
    }
}
