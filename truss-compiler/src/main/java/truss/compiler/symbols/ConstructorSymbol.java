package truss.compiler.symbols;

public class ConstructorSymbol extends MethodSymbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.CONSTRUCTOR;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitConstructor(this);
        }
    }
}
