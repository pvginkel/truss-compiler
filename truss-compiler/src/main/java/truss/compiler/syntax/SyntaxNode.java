package truss.compiler.syntax;

import truss.compiler.Span;
import org.apache.commons.lang.Validate;

import java.util.Collections;
import java.util.List;

public abstract class SyntaxNode {
    private final Span span;

    protected SyntaxNode(Span span) {
        Validate.notNull(span, "span");

        this.span = span;
    }

    public abstract SyntaxKind getKind();

    public Span getSpan() {
        return span;
    }

    public abstract void accept(SyntaxVisitor visitor) throws Exception;

    public abstract <T> T accept(SyntaxActionVisitor<T> visitor) throws Exception;

    public List<? extends SyntaxNode> getChildren() {
        return Collections.emptyList();
    }
}
