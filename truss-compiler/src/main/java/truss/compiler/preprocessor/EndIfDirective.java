package truss.compiler.preprocessor;

class EndIfDirective implements Directive {
    @Override
    public DirectiveKind getKind() {
        return DirectiveKind.ENDIF;
    }
}
