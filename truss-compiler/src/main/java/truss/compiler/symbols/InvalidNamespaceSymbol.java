package truss.compiler.symbols;

public class InvalidNamespaceSymbol extends NamespaceSymbol {
    public static final String INVALID_NAME = "**INVALID NAMESPACE**";

    public InvalidNamespaceSymbol(ContainerSymbol container) {
        super(INVALID_NAME, container);
    }
}
