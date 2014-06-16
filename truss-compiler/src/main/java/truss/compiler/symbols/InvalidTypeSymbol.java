package truss.compiler.symbols;

public class InvalidTypeSymbol extends TypeSymbolBase {
    public static final String INVALID_NAME = "**INVALID TYPE**";

    public InvalidTypeSymbol(ContainerSymbol parent) {
        super(INVALID_NAME, INVALID_NAME, parent);
    }

    @Override
    public TypeKind getTypeKind() {
        return TypeKind.INVALID;
    }

    @Override
    public SymbolKind getKind() {
        return SymbolKind.INVALID_TYPE;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitInvalidType(this);
        }
    }
}
