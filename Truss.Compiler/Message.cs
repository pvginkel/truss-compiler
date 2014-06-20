using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Truss.Compiler.Parser;

namespace Truss.Compiler {
    public class Message {
        public const string ErrorCodeApplication = "TRS";

        public MessageType Type { get; private set; }

        public Span Span { get; private set; }

        public string Text { get; private set; }

        public Message(MessageType type, params object[] args)
            : this(type, Span.Empty, args) {
        }

        public Message(MessageType type, string fileName, params object[] args)
            : this(type, new Span(fileName), args) {
        }

        public Message(MessageType type, Span span, params object[] args) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            if (span == null) {
                throw new ArgumentNullException("span");
            }

            Type = type;
            Span = span;

            var sb = new StringBuilder();

            sb.Append(span);
            sb.Append(": ");
            sb.Append(type.Severity.ToString().ToLower());
            sb.Append(' ');
            sb.Append(ErrorCodeApplication);
            sb.Append(string.Format("{0:0000}", type.Number));
            sb.Append(": ");

            if (args != null && args.Length > 0) {
                sb.Append(string.Format(type.Message, args));
            } else {
                sb.Append(type.Message);
            }

            Text = sb.ToString();
        }

        public override string ToString() {
            return Text;
        }

        public static Message FromRecognitionException(string fileName, RecognitionException e) {
            var span = new Span(fileName, e.Line, e.CharPositionInLine);

            if (e.Token != null && e.Token.Type == TrussLexer.EOF) {
                return new Message(MessageType.UnexpectedEof, span);
            }

            return new Message(
                MessageType.UnexpectedSyntax,
                span,
                e.Token == null ? "" : TrussParser.tokenNames[e.Token.Type]
            );
        }

        public static Message FromMismatchedToken(Span span, IIntStream input, int ttype) {
            if (ttype == TrussLexer.EOF) {
                return new Message(MessageType.UnexpectedEof, span);
            }

            return new Message(MessageType.ExpectedSyntax, span, TrussParser.tokenNames[ttype]);
        }

        public static Message FromException(Exception exception) {
            if (exception is RecognitionException) {
                return FromRecognitionException(null, (RecognitionException)exception);
            }

            return new Message(MessageType.Unknown, exception.Message);
        }
    }
}
