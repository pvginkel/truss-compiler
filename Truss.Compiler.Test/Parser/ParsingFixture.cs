using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Truss.Compiler.Printing;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Test.Parser {
    [TestFixture]
    public class ParsingFixture : FixtureBase {
        [Test]
        public void BasicSyntax() {
            Test("BasicSyntax.trs");
        }

        [Test]
        public void Namespace() {
            Test("Namespace.trs");
        }

        [Test]
        public void Attributes() {
            Test("Attributes.trs");
        }

        [Test]
        public void Types() {
            Test("Types.trs");
        }

        [Test]
        public void TypeMembers() {
            Test("TypeMembers.trs");
        }

        [Test]
        public void Statements() {
            Test("Statements.trs");
        }

        [Test]
        public void Names() {
            Test("Names.trs");
        }

        [Test]
        public void Expressions() {
            Test("Expressions.trs");
        }

        public override void Test(String name, String fileName, String code, String expected) {
            String actual;

            using (var writer = new StringWriter()) {
                var messages = new MessageCollection();
                CompilationUnitSyntax compilationUnit;

                using (MessageCollectionScope.Create(messages)) {
                    compilationUnit = Parse(name, code);
                }

                if (messages.HasMessages) {
                    actual = messages.ToString().Trim();
                } else {
                    compilationUnit.Accept(new SyntaxPrinter(new TextPrinter(writer)));
                    actual = writer.ToString().Trim();
                }
            }

            Assert.AreEqual(expected, actual, String.Format("Parse of '{0}' failed", name));
        }
    }
}
