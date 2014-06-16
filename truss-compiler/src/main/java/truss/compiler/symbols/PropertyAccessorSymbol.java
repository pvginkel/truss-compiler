package truss.compiler.symbols;

public class PropertyAccessorSymbol extends MethodSymbol {
    @Override
    public SymbolKind getKind() {
        return SymbolKind.PROPERTY_ACCESSOR;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitPropertyAccessor(this);
        }
    }
}
