package truss.compiler.symbols;

public class FieldSymbol extends Symbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.FIELD;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitField(this);
        }
    }
}
