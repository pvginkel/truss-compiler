package truss.compiler.symbols;

public class PropertySymbol extends Symbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.PROPERTY;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitProperty(this);
        }
    }
}
