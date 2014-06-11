package truss.compiler.preprocessor;

class IfDirective implements Directive {
    private final boolean expression;

    public IfDirective(boolean expression) {
        this.expression = expression;
    }

    public boolean isExpression() {
        return expression;
    }

    @Override
    public DirectiveKind getKind() {
        return DirectiveKind.IF;
    }
}
