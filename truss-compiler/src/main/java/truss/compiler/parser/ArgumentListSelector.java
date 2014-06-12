package truss.compiler.parser;

import truss.compiler.Span;
import truss.compiler.support.ImmutableArray;
import truss.compiler.syntax.ArgumentSyntax;
import truss.compiler.syntax.ExpressionSyntax;
import truss.compiler.syntax.InvocationExpressionSyntax;

class ArgumentListSelector implements Selector {
    private final ImmutableArray<ArgumentSyntax> argumentList;
    private final Span span;

    public ArgumentListSelector(ImmutableArray<ArgumentSyntax> argumentList, Span span) {
        this.argumentList = argumentList;
        this.span = span;
    }

    @Override
    public ExpressionSyntax build(ExpressionSyntax value) {
        return new InvocationExpressionSyntax(value, argumentList, span);
    }
}
