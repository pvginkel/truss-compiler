package truss.compiler.preprocessor;

class ElIfDirective implements Directive {
    private final boolean expression;

    public ElIfDirective(boolean expression) {
        this.expression = expression;
    }

    public boolean isExpression() {
        return expression;
    }

    @Override
    public DirectiveKind getKind() {
        return DirectiveKind.ELIF;
    }
}
