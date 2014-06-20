using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Truss.Compiler.Test.PreProcessor {
    [TestFixture]
    public class PreProcessorFixture : ITestCaseCallback {
        [Test]
        public void WithoutDirectives() {
            Test("WithoutDirectives.trs");
        }

        [Test]
        public void ExpressionDirectives() {
            Test("ExpressionDirectives.trs");
        }

        [Test]
        public void DefineExpressions() {
            Test("DefineExpressions.trs");
        }

        [Test]
        public void NestedDirectives() {
            Test("NestedDirectives.trs");
        }

        public void Test(String name, String fileName, String code, String expected) {
            Assert.AreEqual(expected, Parse(name, code).Trim(), String.Format("Parse of '{0}' failed", name));
        }

        private void Test(String resourceName) {
            using (var stream = GetType().Assembly.GetManifestResourceStream(GetType().Namespace + "." + resourceName)) {
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
        }

        protected String Parse(String name, String code) {
            char[] data;
            var messages = new MessageCollection();

            using (MessageCollectionScope.Create(messages)) {
                data = Compiler.PreProcessor.PreProcessor.Process(
                    code.ToCharArray(),
                    code.Length,
                    new[] { "DEFINE1", "DEFINE2" }
                );

                if (messages.HasMessages) {
                    return messages.ToString().Trim();
                }
            }

            return new String(data);
        }
    }
}
