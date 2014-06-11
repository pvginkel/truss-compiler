package truss.compiler.preprocessor;

class ElseDirective implements Directive {
    @Override
    public DirectiveKind getKind() {
        return DirectiveKind.ELSE;
    }
}
