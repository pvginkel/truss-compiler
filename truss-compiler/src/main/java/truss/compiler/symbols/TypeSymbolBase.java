package truss.compiler.symbols;

public abstract class TypeSymbolBase extends ContainerSymbol {
    private final String name;
    private final String metadataName;

    protected TypeSymbolBase(String name, String metadataName, ContainerSymbol parent) {
        super(parent);

        this.name = name;
        this.metadataName = metadataName;
    }

    @Override
    public String getMetadataName() {
        return metadataName;
    }

    @Override
    public String getName() {
        return name;
    }

    public abstract TypeKind getTypeKind();
}
