package truss.compiler.parser;

import truss.compiler.Span;
import truss.compiler.syntax.ExpressionSyntax;
import truss.compiler.syntax.MemberAccessExpressionSyntax;
import truss.compiler.syntax.SimpleNameSyntax;

class MemberAccessSelector implements Selector {
    private final SimpleNameSyntax memberName;
    private final Span span;

    public MemberAccessSelector(SimpleNameSyntax memberName, Span span) {
        this.memberName = memberName;
        this.span = span;
    }

    @Override
    public ExpressionSyntax build(ExpressionSyntax value) {
        return new MemberAccessExpressionSyntax(value, memberName, span);
    }
}
