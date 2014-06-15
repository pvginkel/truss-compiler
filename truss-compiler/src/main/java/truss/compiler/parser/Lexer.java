package truss.compiler.parser;

import org.antlr.runtime.*;
import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.Span;

import java.util.ArrayDeque;
import java.util.Deque;

public abstract class Lexer extends org.antlr.runtime.Lexer {
    private String fileName;

    protected Lexer() {
    }

    protected Lexer(CharStream input) {
        super(input);
    }

    protected Lexer(CharStream input, RecognizerSharedState state) {
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
        Span span = new Span(
            fileName,
            this.input.getLine(),
            this.input.getCharPositionInLine()
        );

        MessageCollectionScope.addMessage(Message.fromMismatchedToken(span, input, ttype));

        return super.recoverFromMismatchedToken(input, ttype, follow);
    }

    @Override
    public void reportError(RecognitionException e) {
        MessageCollectionScope.addMessage(Message.fromRecognitionException(fileName, e));
    }

    Deque<Token> tokens = new ArrayDeque<>();

    @Override
    public void emit(Token token) {
        state.token = token;

        tokens.addLast(token);
    }

    @Override
    public Token nextToken() {
        super.nextToken();

        if (tokens.isEmpty())
            return getEOFToken();

        return tokens.removeFirst();
    }

    protected void emitGreaterThanGreaterThan(Token token) {
        // Split the greater than token into two tokens so they can be matched separately.

        CommonToken first = new CommonToken(token);
        first.setType(TrussLexer.OP_GREATER_THAN_GREATER_THAN_FIRST);
        emit(first);

        CommonToken second = new CommonToken(token);
        second.setType(TrussLexer.OP_GREATER_THAN_GREATER_THAN_SECOND);
        emit(second);
    }
}
