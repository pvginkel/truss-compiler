using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Test {
    public class TestCase {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly String _name;
        private readonly String _fileName;
        private String _code;

        public TestCase(String name, String fileName) {
            _name = name;
            _fileName = fileName;
        }

        public void BeginExpected() {
            _code = _sb.ToString();
            _sb.Clear();
        }

        public void Append(String line) {
            _sb.AppendLine(line);
        }

        public void Test(ITestCaseCallback callback) {
            String expected;
            if (_code == null) {
                _code = _sb.ToString();
                expected = _code;
            } else {
                expected = _sb.ToString();
            }

            callback.Test(_name, _fileName, _code.Trim(), expected.Trim());
        }
    }
}
