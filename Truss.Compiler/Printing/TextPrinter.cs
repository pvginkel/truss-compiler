using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Printing {
    public class TextPrinter : IPrinter {
        private readonly TextWriter _writer;
        private int _indent;

        public TextPrinter(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }

            _writer = writer;
            Indentation = "    ";
        }

        public string Indentation { get; set; }

        public void Identifier(string identifier) {
            _writer.Write(identifier);
        }

        public void Indent() {
            _indent++;
        }

        public void Keyword(string keyword) {
            _writer.Write(keyword);
        }

        public void Lead() {
            for (int i = 0; i < _indent; i++) {
                _writer.Write(Indentation);
            }
        }

        public void Literal(string value, LiteralType type) {
            _writer.Write(value);
        }

        public void Nl() {
            _writer.Write(Environment.NewLine);
        }

        public void Ws() {
            _writer.Write(" ");
        }

        public void Syntax(string syntax) {
            _writer.Write(syntax);
        }

        public void UnIndent() {
            _indent--;
        }
    }
}
