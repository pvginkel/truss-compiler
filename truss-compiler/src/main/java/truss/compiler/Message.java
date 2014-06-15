package truss.compiler;

import org.antlr.runtime.IntStream;
import org.antlr.runtime.RecognitionException;
import org.apache.commons.lang.Validate;
import truss.compiler.parser.TrussLexer;
import truss.compiler.parser.TrussParser;

public class Message {
    public static final String ERROR_CODE_APPLICATION = "DTL";

    private final MessageType type;
    private final Span span;
    private final String message;

    public Message(MessageType type, Object... args) {
        this(type, Span.EMPTY, args);
    }

    public Message(MessageType type, String fileName, Object... args) {
        this(type, new Span(fileName), args);
    }

    public Message(MessageType type, Span span, Object... args) {
        Validate.notNull(type, "type");
        Validate.notNull(span, "span");

        this.type = type;
        this.span = span;

        StringBuilder sb = new StringBuilder();

        sb.append(span.toString());
        sb.append(": ");
        sb.append(type.getSeverity().getCode());
        sb.append(' ');
        sb.append(ERROR_CODE_APPLICATION);
        sb.append(String.format("%04d", type.getNumber()));
        sb.append(": ");

        if (args != null && args.length > 0) {
            sb.append(String.format(type.getMessage(), args));
        } else {
            sb.append(type.getMessage());
        }

        message = sb.toString();
    }

    public MessageType getType() {
        return type;
    }

    public Span getSpan() {
        return span;
    }

    public String getMessage() {
        return message;
    }

    @Override
    public String toString() {
        return message;
    }

    public static Message fromRecognitionException(String fileName, RecognitionException e) {
        Span span = new Span(fileName, e.line, e.charPositionInLine);

        if (e.token != null && e.token.getType() == TrussLexer.EOF) {
            return new Message(MessageType.UNEXPECTED_EOF, span);
        }

        return new Message(
            MessageType.UNEXPECTED_SYNTAX,
            span,
            e.token == null ? "" : TrussParser.tokenNames[e.token.getType()]
        );
    }

    public static Message fromMismatchedToken(Span span, IntStream input, int ttype) {
        if (ttype == TrussLexer.EOF) {
            return new Message(MessageType.UNEXPECTED_EOF, span);
        }

        return new Message(MessageType.EXPECTED_SYNTAX, span, TrussParser.tokenNames[ttype]);
    }

    public static Message fromException(Exception exception) {
        if (exception instanceof RecognitionException) {
            return fromRecognitionException(null, (RecognitionException)exception);
        }
        return new Message(MessageType.UNKNOWN, exception.getMessage());
    }
}
