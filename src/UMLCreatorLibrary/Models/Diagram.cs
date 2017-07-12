using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UMLcreator.models
{
    public class Diagram
    {
        public string Name { get; private set; }

        public Diagram(String name) {
            Name = name;
        }
    }
}
