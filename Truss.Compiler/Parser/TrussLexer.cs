using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.Parser {
    partial class TrussLexer {
        public string FileName { get; set; }

        public override Object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow) {
            MessageCollectionScope.AddMessage(Message.FromRecognitionException(FileName, e));

            return base.RecoverFromMismatchedSet(input, e, follow);
        }

        protected override Object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow) {
            var span = new Span(
                FileName,
                this.input.Line,
                this.input.CharPositionInLine
                );

            MessageCollectionScope.AddMessage(Message.FromMismatchedToken(span, input, ttype));

            return base.RecoverFromMismatchedToken(input, ttype, follow);
        }

        public override void ReportError(RecognitionException e) {
            MessageCollectionScope.AddMessage(Message.FromRecognitionException(FileName, e));
        }

        private readonly Queue<IToken> _tokens = new Queue<IToken>();

        public override void Emit(IToken token) {
            state.token = token;

            _tokens.Enqueue(token);
        }

        public override IToken NextToken() {
            base.NextToken();

            if (_tokens.Count == 0) {
                return GetEofToken();
            }

            return _tokens.Dequeue();
        }

        private IToken GetEofToken() {
            IToken eof = new CommonToken(input, EOF, 0, input.Index, input.Index);
            eof.Line = Line;
            eof.CharPositionInLine = CharPositionInLine;
            return eof;
        }

        private void EmitGreaterThanGreaterThan(IToken token) {
            // Split the greater than token into two tokens so they can be matched separately.

            var first = new CommonToken(token);
            first.Type = OP_GREATER_THAN_GREATER_THAN_FIRST;
            Emit(first);

            var second = new CommonToken(token);
            second.Type = OP_GREATER_THAN_GREATER_THAN_SECOND;
            Emit(second);
        }
    }
}
