using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.PreProcessor {
    partial class TrussPreProcessorLexer {
        public string FileName { get; set; }

        public ErrorList Errors { get; set; }

        public override object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow) {
            Errors.Add(Error.FromRecognitionException(FileName, e));

            return base.RecoverFromMismatchedSet(input, e, follow);
        }

        protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow) {
            var span = new Span(
                FileName,
                this.input.Line,
                this.input.CharPositionInLine
            );

            Errors.Add(Error.FromMismatchedToken(span, input, ttype));

            return base.RecoverFromMismatchedToken(input, ttype, follow);
        }

        public override void ReportError(RecognitionException e) {
            Errors.Add(Error.FromRecognitionException(FileName, e));
        }
    }
}
