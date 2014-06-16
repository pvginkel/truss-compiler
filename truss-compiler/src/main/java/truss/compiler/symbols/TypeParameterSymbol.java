package truss.compiler.symbols;

public class TypeParameterSymbol extends TypeSymbolBase {
    private TypeParameterSymbol(String name, String metadataName, ContainerSymbol parent) {
        super(name, metadataName, parent);
    }

    @Override
    public TypeKind getTypeKind() {
        return TypeKind.TYPE_PARAMETER;
    }

    @Override
    public SymbolKind getKind() {
        return SymbolKind.TYPE_PARAMETER;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitTypeParameter(this);
        }
    }
}
