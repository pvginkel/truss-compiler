using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class Span {
        public const string UnknownFilename = "(unknown)";
        public static readonly Span Empty = new Span(null);

        public Span(string fileName) {
            FileName = fileName;
            StartLine = -1;
            StartColumn = -1;
            EndLine = -1;
            EndColumn = -1;
        }

        public Span(string fileName, int startLine, int startColumn)
            : this(fileName, startLine, startColumn, startLine, startColumn) {
        }

        public Span(string fileName, int startLine, int startColumn, int endLine, int endColumn) {
            FileName = fileName;
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
        }

        public string FileName { get; private set; }

        public int StartLine { get; private set; }

        public int StartColumn { get; private set; }

        public int EndLine { get; private set; }

        public int EndColumn { get; private set; }

        public override string ToString() {
            var sb = new StringBuilder();

            sb.Append(FileName ?? UnknownFilename);

            if (StartLine != -1 && StartColumn != -1) {
                sb.Append('(');
                sb.Append(StartLine);
                sb.Append(',');
                sb.Append(StartColumn + 1);
                if (EndLine != -1 && EndColumn != -1 && (StartLine != EndLine || StartColumn != EndColumn)) {
                    sb.Append(',');
                    sb.Append(EndLine);
                    sb.Append(',');
                    sb.Append(EndColumn + 1);
                }
                sb.Append(')');
            }

            return sb.ToString();
        }
    }
}
