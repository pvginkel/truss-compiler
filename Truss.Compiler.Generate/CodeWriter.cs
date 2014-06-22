using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Truss.Compiler.Generate {
    public class CodeWriter {
        private bool _hadIndent;
        private int _indent;
        private readonly StringBuilder _sb = new StringBuilder();

        public void Indent() {
            _indent++;
        }

        public void UnIndent() {
            _indent--;
        }

        public void Write(string format) {
            Write(format, null);
        }

        [StringFormatMethod("format")]
        public void Write(string format, params Object[] args) {
            if (!_hadIndent && _indent > 0) {
                _sb.Append(new string(' ', _indent * 4));
                _hadIndent = true;
            }

            if (args != null && args.Length > 0) {
                format = string.Format(format, args);
            }

            _sb.Append(format);
        }

        public void WriteLine() {
            _hadIndent = false;
            _sb.Append(Environment.NewLine);
        }

        public void WriteLine(string format) {
            WriteLine(format, null);
        }

        [StringFormatMethod("format")]
        public void WriteLine(string format, params Object[] args) {
            Write(format, args);
            WriteLine();
        }

        public override string ToString() {
            return _sb.ToString();
        }

        public void WriteRaw(string validation) {
            _sb.Append(validation);
        }
    }
}
