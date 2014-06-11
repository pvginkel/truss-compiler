package truss.compiler.preprocessor;

import org.apache.commons.lang.Validate;

class DefineDirective implements Directive {
    private final String identifier;
    private final boolean define;

    public DefineDirective(String identifier, boolean define) {
        Validate.notNull(identifier, "identifier");

        this.identifier = identifier;
        this.define = define;
    }

    public String getIdentifier() {
        return identifier;
    }

    public boolean isDefine() {
        return define;
    }

    @Override
    public DirectiveKind getKind() {
        return DirectiveKind.DEFINE;
    }
}
