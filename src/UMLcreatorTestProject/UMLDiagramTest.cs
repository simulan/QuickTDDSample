using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMLcreator.models;

namespace UMLcreatorTestProject {
    [TestClass]
    public class UMLDiagramTest {
        [TestMethod]
        public void testGetName() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            Assert.IsTrue(diagram.Name.Equals(NAME));
        }
        [TestMethod]
        public void testGetFunctions() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            diagram.AddMethod("+GetPropertyA : int");
            diagram.AddMethod("-SetPropertyA : void");
            diagram.AddMethod("+DoSomething : void");
            Assert.IsTrue(diagram.Methods != null && diagram.Methods.Count != 0);
        }
    }
}
