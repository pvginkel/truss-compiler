package truss.compiler.parser;

import truss.compiler.Span;
import truss.compiler.syntax.ExpressionSyntax;
import truss.compiler.syntax.ElementAccessExpressionSyntax;

class IndexSelector implements Selector {
    private final ExpressionSyntax expression;
    private final Span span;

    public IndexSelector(ExpressionSyntax expression, Span span) {
        this.expression = expression;
        this.span = span;
    }

    @Override
    public ExpressionSyntax build(ExpressionSyntax value) {
        return new ElementAccessExpressionSyntax(value, expression, span);
    }
}
