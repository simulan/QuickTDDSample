using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models {
    public class Class {
        public string Name { get; set; }
        public List<Method> Methods { get; set; }
    }
}
