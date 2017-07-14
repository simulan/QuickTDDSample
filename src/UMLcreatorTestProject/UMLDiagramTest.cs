using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UMLcreator.models;
using System.Collections.Generic;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Decoders;

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
        public void testGetMethods() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            UML uml = new UML();
            diagram.AddMethod(uml.Deserialize<Method>("+GetPropertyA() : int"));
            diagram.AddMethod(uml.Deserialize<Method>("-SetPropertyA() : void"));
            diagram.AddMethod(uml.Deserialize<Method>("+DoSomething() : void"));
            Assert.IsTrue(diagram.Methods != null && diagram.Methods.Count != 0);
        }
        [TestMethod]
        public void testGetMethodProperties() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            UML uml = new UML();
            diagram.AddMethod(uml.Deserialize<Method>("+GetPropertyA() : int"));
            diagram.AddMethod(uml.Deserialize<Method>("-SetPropertyA() : void"));
            diagram.AddMethod(uml.Deserialize<Method>("+DoSomething() : void"));
            diagram.AddMethod(uml.Deserialize<Method>("+DoSomething2(amount:Int) : void"));
            diagram.AddMethod(uml.Deserialize<Method>("#DoSomething3(number:Int, flag:Boolean) : void"));

            List<Method> methods = diagram.Methods;
            Assert.IsFalse(methods[0].AccessModifier.Equals(AccessScope.PRIVATE));
            Assert.IsTrue(methods[1].Name.Equals("SetPropertyA"));
            Assert.IsTrue(methods[2].ReturnType.Equals("void"));
            Assert.IsNotNull(methods[3].Parameters);
            Assert.IsTrue(methods[4].Parameters.Count == 2);
        }

        
    }
}
