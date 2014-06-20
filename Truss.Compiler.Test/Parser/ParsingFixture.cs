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
                var errors = new ErrorList();

                CompilationUnitSyntax compilationUnit = Parse(errors, name, code);

                if (errors.HasMessages) {
                    actual = errors.ToString().Trim();
                } else {
                    compilationUnit.Accept(new SyntaxPrinter(new TextPrinter(writer)));
                    actual = writer.ToString().Trim();
                }
            }

            if (expected != actual) {
                Console.WriteLine(">>> Expected");
                Console.WriteLine(expected);
                Console.WriteLine(">>> Actual");
                Console.WriteLine(actual);
                Assert.Fail("Parse of '{0}' failed", name);
            }
        }
    }
}
