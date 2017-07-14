using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models {
    public class Argument {
        public Argument() {
        }
        public Argument(string name, string returnType) {
            Name = name;
            ReturnType = returnType;
        }

        public string Name { get; set; }
        public string ReturnType { get; set; }
    }
}
