package truss.compiler.symbols;

public class EventAccessorSymbol extends MethodSymbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.EVENT_ACCESSOR;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitEventAccessor(this);
        }
    }
}
