using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLCreatorLibrary.Models;
using UMLCreatorLibrary.Models.Decoders;

namespace UMLcreatorTestProject {
    [TestClass]
    public class MethodDeserializerTest {
        [TestMethod]
        public void deserializeShouldThrowOnNoAccessModifier() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("a(var:type):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void deserializeShouldThrowOnNoName() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+(var:type):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void deserializeShouldThrowOnNoParameterName() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+(:type):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void deserializeShouldThrowOnNoParameterType() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+(var:):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void deserializeShouldThrowOnNoReturnType() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+(var:):");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception) {
                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void deserializeShouldThrowOnInvalidAccessModifier() {
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
        public void deserializeShouldThrowOnInvalidName() {
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
        public void deserializeShouldThrowOnInvalidParameterName() {
            UML uml = new UML();
            try {
                uml.Deserialize<Method>("+abc(+:type):void");
                Assert.Inconclusive("Should've thrown exception");
            } catch (AssertInconclusiveException exception) {
                Assert.Fail(exception.Message);
            } catch (Exception e) {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void deserializeShouldThrowOnInvalidParameterType() {
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
        public void deserializeShouldThrowOnInvalidReturnType() {
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


    }
}
