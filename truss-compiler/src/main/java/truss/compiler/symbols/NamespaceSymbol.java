package truss.compiler.symbols;

import org.apache.commons.lang.Validate;
import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.MessageType;
import truss.compiler.syntax.IdentifierNameSyntax;
import truss.compiler.syntax.NameSyntax;
import truss.compiler.syntax.NamespaceDeclarationSyntax;
import truss.compiler.syntax.QualifiedNameSyntax;

public class NamespaceSymbol extends ContainerSymbol {
    private final String name;

    protected NamespaceSymbol(String name, ContainerSymbol container) {
        super(container);

        this.name = name;
    }

    @Override
    public String getName() {
        return name;
    }

    @Override
    public SymbolKind getKind() {
        return SymbolKind.NAMESPACE;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitNamespace(this);
        }
    }

    public static NamespaceSymbol fromDeclaration(NamespaceDeclarationSyntax syntax, ContainerSymbol container) {
        Validate.notNull(syntax, "syntax");
        Validate.notNull(container, "container");

        NamespaceSymbol symbol = createChain(syntax.getName(), container);

        symbol.addSpan(syntax.getSpan());

        return symbol;
    }

    private static NamespaceSymbol createChain(NameSyntax name, ContainerSymbol container) {
        if (name instanceof QualifiedNameSyntax) {
            QualifiedNameSyntax qualifiedName = (QualifiedNameSyntax)name;
            container = createChain(qualifiedName.getLeft(), container);
            name = qualifiedName.getRight();
        }

        if (!(name instanceof IdentifierNameSyntax)) {
            MessageCollectionScope.addMessage(new Message(
                MessageType.INVALID_NAMESPACE_IDENTIFIER,
                name.getSpan()
            ));

            return null;
        }

        String identifier = ((IdentifierNameSyntax)name).getIdentifier();

        for (Symbol member : container.getMemberByMetadataName(identifier)) {
            if (member instanceof NamespaceSymbol) {
                return (NamespaceSymbol)member;
            }
        }

        NamespaceSymbol symbol = new NamespaceSymbol(
            identifier,
            container
        );

        container.addMember(symbol);

        return symbol;
    }
}
