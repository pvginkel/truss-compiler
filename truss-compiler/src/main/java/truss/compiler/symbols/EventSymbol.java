package truss.compiler.symbols;

public class EventSymbol extends Symbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.EVENT;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitEvent(this);
        }
    }
}
