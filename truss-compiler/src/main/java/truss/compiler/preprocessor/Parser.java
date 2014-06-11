package truss.compiler.preprocessor;

import org.antlr.runtime.*;
import org.apache.commons.lang.Validate;
import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.Span;

import java.util.Set;

public abstract class Parser extends org.antlr.runtime.Parser {
    private String fileName;
    private Set<String> defines;

    protected Parser(TokenStream input) {
        super(input);
    }

    protected Parser(TokenStream input, RecognizerSharedState state) {
        super(input, state);
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    public void setDefines(Set<String> defines) {
        Validate.notNull(defines, "defines");

        this.defines = defines;
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

    protected boolean isDefined(String identifier) {
        return defines.contains(identifier);
    }
}
