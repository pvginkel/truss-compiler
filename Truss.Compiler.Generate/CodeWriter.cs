using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Generate {
    public class CodeWriter {
        private bool hadIndent = false;
        private int indent;
        private readonly StringBuilder sb = new StringBuilder();

        public void Indent() {
            indent++;
        }

        public void UnIndent() {
            indent--;
        }

        public void Write(string format, params Object[] args) {
            if (!hadIndent && indent > 0) {
                sb.Append(new string(' ', indent * 4));
                hadIndent = true;
            }

            if (args != null && args.Length > 0) {
                format = string.Format(format, args);
            }

            sb.Append(format);
        }

        public void WriteLine() {
            hadIndent = false;
            sb.Append(Environment.NewLine);
        }

        public void WriteLine(string format, params Object[] args) {
            Write(format, args);
            WriteLine();
        }

        public override string ToString() {
            return sb.ToString();
        }

        public void WriteRaw(string validation) {
            sb.Append(validation);
        }
    }
}
