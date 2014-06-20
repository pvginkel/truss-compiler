using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.PreProcessor {
    partial class TrussPreProcessorParser {
        public string FileName { get; set; }

        public HashSet<string> Defines { get; set; }

        public IDirective ParseDirective() {
            return directive();
        }

        public override object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow) {
            MessageCollectionScope.AddMessage(Message.FromRecognitionException(FileName, e));

            return base.RecoverFromMismatchedSet(input, e, follow);
        }

        protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow) {
            var token = this.input.LT(-1);

            var span = new Span(
                FileName,
                token.Line,
                token.CharPositionInLine + token.Text.Length
            );

            MessageCollectionScope.AddMessage(Message.FromMismatchedToken(span, input, ttype));

            return base.RecoverFromMismatchedToken(input, ttype, follow);
        }

        public override void ReportError(RecognitionException e) {
            MessageCollectionScope.AddMessage(Message.FromRecognitionException(FileName, e));
        }

        private bool IsDefined(string identifier) {
            return Defines.Contains(identifier);
        }
    }
}
