using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLcreator.models;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Deserializers;

namespace UMLcreatorTestProject {
    [TestClass]
    public class UMLDiagramTest {
        [TestMethod]
        public void testGetClasses() {
            UML uml = new UML();
            Diagram diagram = new Diagram();
            diagram.AddClass(uml.Deserialize<Class>("+SomeClass()"));
            Assert.IsTrue(diagram.Classes.Count == 1);
        }
    }
}
