package truss.compiler.parser;

import truss.compiler.Span;
import truss.compiler.support.ImmutableArray;
import truss.compiler.syntax.ElementAccessExpressionSyntax;
import truss.compiler.syntax.ExpressionSyntax;

class IndexSelector implements Selector {
    private final ImmutableArray<ExpressionSyntax> expressions;
    private final Span span;

    public IndexSelector(ImmutableArray<ExpressionSyntax> expressions, Span span) {
        this.expressions = expressions;
        this.span = span;
    }

    @Override
    public ExpressionSyntax build(ExpressionSyntax value) {
        return new ElementAccessExpressionSyntax(value, expressions, span);
    }
}
