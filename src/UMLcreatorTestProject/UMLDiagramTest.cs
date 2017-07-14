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

        [TestMethod]
        public void testAddMethodShouldThrowExceptionWhenInvalidAccessModifier() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("func(var:type):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testAddMethodShouldThrowExceptionWhenInvalidName() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("++(var:type):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testAddMethodShouldThrowExceptionWhenInvalidParameterName() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+abc(+:type):void");
                Assert.Inconclusive("Should've thrown exception");
            }catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            }catch (Exception e) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testAddMethodShouldThrowExceptionWhenInvalidParameterType() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+abc(var:??):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testAddMethodShouldThrowExceptionWhenInvalidReturnType() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+abc(var:type):€€");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testGetMethodsShouldThrowExceptionWhenNoName() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            UML uml = new UML();
            try {
                diagram.AddMethod(uml.Deserialize<Method>("+(var:type):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testGetMethodsShouldThrowExceptionWhenNoParameterName() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            UML uml = new UML();
            try {
                diagram.AddMethod(uml.Deserialize<Method>("+(:type):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void testGetMethodsShouldThrowExceptionWhenNoParameterType() {
            const string NAME = "name";
            Diagram diagram = new Diagram(NAME);
            UML uml = new UML();
            try {
                diagram.AddMethod(uml.Deserialize<Method>("+(var:):void"));
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        
    }
}
