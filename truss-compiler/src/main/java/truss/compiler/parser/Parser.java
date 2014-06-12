package truss.compiler.parser;

import org.antlr.runtime.*;
import org.apache.commons.lang.Validate;
import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.MessageType;
import truss.compiler.Span;
import truss.compiler.support.ImmutableArray;
import truss.compiler.syntax.AccessorDeclarationType;
import truss.compiler.syntax.AttributeListSyntax;
import truss.compiler.syntax.AttributeTarget;
import truss.compiler.syntax.Modifier;

import java.util.Stack;

public abstract class Parser extends org.antlr.runtime.Parser {
    private String fileName;
    private int pendingGenericCloses;

    protected Parser(TokenStream input) {
        super(input);
    }

    protected Parser(TokenStream input, RecognizerSharedState state) {
        super(input, state);
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    @Override
    public Object recoverFromMismatchedSet(IntStream input, RecognitionException e, BitSet follow) throws RecognitionException {
        MessageCollectionScope.addMessage(Message.fromRecognitionException(fileName, e));

        return super.recoverFromMismatchedSet(input, e, follow);
    }

    @Override
    protected Object recoverFromMismatchedToken(IntStream input, int ttype, BitSet follow) throws RecognitionException {
        Token token = this.input.LT(-1);

        Span span = new Span(
            fileName,
            token.getLine(),
            token.getCharPositionInLine() + token.getText().length()
        );

        MessageCollectionScope.addMessage(Message.fromMismatchedToken(span, input, ttype));

        return super.recoverFromMismatchedToken(input, ttype, follow);
    }

    @Override
    public void reportError(RecognitionException e) {
        MessageCollectionScope.addMessage(Message.fromRecognitionException(fileName, e));
    }

    protected Span span(Token start) {
        return span(start, input.LT(-1));
    }

    protected Span span(Token start, Token end) {
        return new Span(
            fileName,
            start.getLine(),
            start.getCharPositionInLine(),
            end.getLine(),
            end.getCharPositionInLine() + end.getText().length()
        );
    }

    protected Span adjustSpanStart(Token start, Span span) {
        return new Span(
            fileName,
            start.getLine(),
            start.getCharPositionInLine(),
            span.getEndLine(),
            span.getEndColumn()
        );
    }

    protected void beginGeneric() {
        assert pendingGenericCloses == 0;

        pendingGenericCloses = 0;
    }

    protected void openGeneric() {
        pendingGenericCloses++;
    }

    protected void closeGeneric() {
        pendingGenericCloses--;

        assert pendingGenericCloses >= 0;
    }

    protected int getPendingGenericCloses() {
        return pendingGenericCloses;
    }

    protected AttributeTarget parseAttributeTarget(String identifier, Span span) {
        Validate.notNull(identifier, "identifier");

        // Attribute targets are contextual keywords.

        switch (identifier) {
            case "assembly":
                return AttributeTarget.ASSEMBLY;
            case "event":
                return AttributeTarget.EVENT;
            case "field":
                return AttributeTarget.FIELD;
            case "method":
                return AttributeTarget.METHOD;
            case "param":
                return AttributeTarget.PARAM;
            case "property":
                return AttributeTarget.PROPERTY;
            case "return":
                return AttributeTarget.RETURN;
            case "type":
                return AttributeTarget.TYPE;
            default:
                MessageCollectionScope.addMessage(new Message(
                    MessageType.INVALID_ATTRIBUTE_TARGET,
                    span
                ));
                return AttributeTarget.NONE;
        }
    }

    protected AccessorDeclarationType parseAccessorDeclarationType(String identifier, Span span) {
        // Accessor declaration types are contextual keywords.

        switch (identifier) {
            case "get":
                return AccessorDeclarationType.GET;
            case "set":
                return AccessorDeclarationType.SET;
            case "add":
                return AccessorDeclarationType.ADD;
            case "remove":
                return AccessorDeclarationType.REMOVE;
            default:
                MessageCollectionScope.addMessage(new Message(
                    MessageType.INVALID_ACCESSOR_DECLARATION_TYPE,
                    span
                ));
                return AccessorDeclarationType.INVALID;
        }
    }
}
