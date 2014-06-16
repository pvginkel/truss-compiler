package truss.compiler.symbols;

public class GlobalSymbol extends ContainerSymbol {
    public GlobalSymbol() {
        super(null);
    }

    @Override
    public SymbolKind getKind() {
        return SymbolKind.GLOBAL;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitGlobal(this);
        }
    }
}
