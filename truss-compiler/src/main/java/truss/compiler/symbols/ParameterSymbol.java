package truss.compiler.symbols;

public class ParameterSymbol extends Symbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.PARAMETER;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitParameter(this);
        }
    }
}
