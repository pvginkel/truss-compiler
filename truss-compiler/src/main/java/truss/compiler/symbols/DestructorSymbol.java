package truss.compiler.symbols;

public class DestructorSymbol extends MethodSymbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.DESTRUCTOR;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitDestructor(this);
        }
    }
}
