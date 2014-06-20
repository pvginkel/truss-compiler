using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using NUnit.Framework;
using Truss.Compiler.Parser;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Test {
    public abstract class FixtureBase : ITestCaseCallback {
        static FixtureBase() {
            if (!Debugger.IsAttached) {
                Debug.Listeners.Clear();
                Debug.Listeners.Add(new ThrowingTraceListener());
            }
        }

        public abstract void Test(String name, String fileName, String code, String expected);

        protected void Test(String resourceName) {
            using (var stream = GetType().Assembly.GetManifestResourceStream(GetType().Namespace + "." + resourceName))
            using (var reader = new StreamReader(stream)) {
                TestCase testCase = null;
                string line;

                while ((line = reader.ReadLine()) != null) {
                    if (line == "<<<") {
                        testCase.BeginExpected();
                    } else if (line.StartsWith(">>>")) {
                        if (testCase != null) {
                            testCase.Test(this);
                        }
                        testCase = new TestCase(line.Substring(3).Trim(), resourceName);
                    } else if (testCase != null) {
                        testCase.Append(line);
                    }
                }

                if (testCase != null) {
                    testCase.Test(this);
                }
            }
        }

        protected CompilationUnitSyntax Parse(ErrorList errors, string name, string code) {
            var lexer = new TrussLexer(new ANTLRStringStream(code));
            lexer.FileName = name;
            lexer.Errors = errors;
            var parser = new TrussParser(new CommonTokenStream(lexer));
            parser.FileName = name;
            parser.Errors = errors;

            CompilationUnitSyntax compilationUnit;

            try {
                compilationUnit = parser.ParseCompilationUnit();
            } catch (Exception e) {
                errors.Add(Error.InternalError, name, e.Message);

                compilationUnit = null;
            } finally {
                if (errors.HasMessages) {
                    compilationUnit = null;
                }
            }

            return compilationUnit;
        }

        protected Stream GetSystemResource() {
            return typeof(FixtureBase).Assembly.GetManifestResourceStream(typeof(FixtureBase).Namespace + ".System.trs");
        }

        private class ThrowingTraceListener : TraceListener {
            public override void Fail(string message, string detailMessage) {
                Assert.Fail(message);
            }

            public override void Write(string message) {
                Console.Write(message);
            }

            public override void WriteLine(string message) {
                Console.WriteLine(message);
            }
        }
    }
}
