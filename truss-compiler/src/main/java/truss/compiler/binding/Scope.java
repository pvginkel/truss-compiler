package truss.compiler.binding;

import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.MessageType;
import truss.compiler.WellKnownNames;
import truss.compiler.symbols.*;
import truss.compiler.syntax.*;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

enum ResolveMode {
    TYPE,
    NAMESPACE,
    TYPE_OR_NAMESPACE;

    public boolean allowType() {
        return this == TYPE || this == TYPE_OR_NAMESPACE;
    }

    public boolean allowNamespace() {
        return this == NAMESPACE || this == TYPE_OR_NAMESPACE;
    }
}

abstract class Scope {
    private final Scope parent;

    protected Scope(Scope parent) {
        this.parent = parent;
    }

    public Scope getParent() {
        return parent;
    }

    public ContainerSymbol resolveContainer(NameSyntax name, ResolveMode type) {
        return parent.resolveContainer(name, type);
    }

    public GlobalSymbol getGlobalSymbol() {
        return parent.getGlobalSymbol();
    }
}

abstract class ContainerScope extends Scope {
    private final ContainerSymbol symbol;
    private final List<Import> imports;

    protected ContainerScope(ContainerSymbol symbol, List<Import> imports, Scope parent) {
        super(parent);
        this.symbol = symbol;
        this.imports = imports;
    }

    @Override
    public ContainerSymbol resolveContainer(NameSyntax name, ResolveMode mode) {
        List<ContainerSymbol> results = resolveContainersForSymbol(symbol, name, mode, imports);

        if (results != null) {
            if (results.size() > 1) {
                MessageCollectionScope.addMessage(new Message(
                    MessageType.AMBIGUOUS_CONTAINER_SYMBOL_MATCH,
                    name.getSpan(),
                    NameUtils.prettyPrint(name)
                ));

                return null;
            }

            return results.get(0);
        }

        return super.resolveContainer(name, mode);
    }

    protected List<ContainerSymbol> resolveContainersForSymbol(ContainerSymbol symbol, NameSyntax name, ResolveMode mode, List<Import> imports) {
        return resolveContainersForSymbol(symbol, name, mode, imports, true);
    }

    private List<ContainerSymbol> resolveContainersForSymbol(ContainerSymbol symbol, NameSyntax name, ResolveMode mode, List<Import> imports, boolean isTail) {
        List<ContainerSymbol> parents;

        if (name instanceof QualifiedNameSyntax) {
            // If this level is a qualified name, first resolve the left part.

            QualifiedNameSyntax qualifiedName = (QualifiedNameSyntax)name;
            parents = resolveContainersForSymbol(symbol, qualifiedName.getLeft(), mode, imports, false);
            if (parents.size() == 0) {
                return null;
            }

            // Extract the tail out of the qualified name and continue with that.

            name = qualifiedName.getRight();

            // Import matching is only for the top level.

            imports = null;
        } else if (name instanceof AliasQualifiedNameSyntax) {
            AliasQualifiedNameSyntax aliasQualifiedName = (AliasQualifiedNameSyntax)name;

            // Currently we only support the global alias. Later we could add support for linking libraries
            // under an alias.

            if (!WellKnownNames.ALIAS_GLOBAL.equals(aliasQualifiedName.getAlias().getIdentifier())) {
                MessageCollectionScope.addMessage(new Message(
                    MessageType.INVALID_ALIAS,
                    aliasQualifiedName.getAlias().getSpan(),
                    aliasQualifiedName.getAlias().getIdentifier()
                ));

                return null;
            }

            name = aliasQualifiedName.getName();

            parents = Arrays.asList((ContainerSymbol)getGlobalSymbol());
        } else {
            parents = Arrays.asList(symbol);
        }

        // By now we should have ended up with a simple name.

        assert name instanceof SimpleNameSyntax;

        // Find all matching container symbols in all matching children.

        SimpleNameSyntax simpleName = (SimpleNameSyntax)name;
        String metadataName = NameUtils.getMetadataName(simpleName);

        List<ContainerSymbol> result = new ArrayList<>();

        for (ContainerSymbol parent : parents) {
            for (Symbol member : parent.getMemberByMetadataName(metadataName)) {
                if (isSymbolValidForMode(member, mode, isTail)) {
                    result.add((ContainerSymbol)member);
                }
            }
        }

        // If we have imports, match those too.

        if (imports != null) {
            String identifier = simpleName.getIdentifier();

            for (Import import_ : imports) {
                ContainerSymbol member = null;

                if (import_.getAlias() != null) {
                    // If we're matching an alias, the alias will be against the full name. So, when the
                    // alias is against a generic type, the symbol of the alias will be the generic
                    // type. This means that the matching name may not be a generic type and we can
                    // only match identifier names here. Identifier names in this context will be
                    // either a type name without generic arguments, or the first part of a namespace.
                    // Both are fine (isSymbolValidForMode will make sure that we only add valid symbols).

                    if (
                        simpleName instanceof IdentifierNameSyntax &&
                        identifier.equals(import_.getAlias())
                    ) {
                        member = import_.getSymbol();
                    }
                } else {
                    if (metadataName.equals(import_.getSymbol().getMetadataName())) {
                        member = import_.getSymbol();
                    }
                }

                if (member != null && isSymbolValidForMode(member, mode, isTail)) {
                    result.add(member);
                }
            }
        }

        if (result.size() > 0) {
            return result;
        }

        return null;
    }

    private boolean isSymbolValidForMode(Symbol member, ResolveMode mode, boolean isTail) {
        // If we're at the tail, we need to return something that is valid for the mode.
        // Otherwise, we can just add all matching containers.

        if (!isTail) {
            return member instanceof ContainerSymbol;
        }

        return
            (member.getKind() == SymbolKind.TYPE && mode.allowType()) ||
            (member.getKind() == SymbolKind.NAMESPACE && mode.allowNamespace());
    }
}

class GlobalScope extends ContainerScope {
    private final GlobalSymbol symbol;

    GlobalScope(GlobalSymbol symbol, List<Import> imports) {
        super(symbol, imports, null);
        this.symbol = symbol;
    }

    @Override
    public GlobalSymbol getGlobalSymbol() {
        return symbol;
    }

    @Override
    public ContainerSymbol resolveContainer(NameSyntax name, ResolveMode mode) {
        ContainerSymbol result = super.resolveContainer(name, mode);

        if (result == null) {
            MessageCollectionScope.addMessage(new Message(
                MessageType.CANNOT_RESOLVE_NAME,
                name.getSpan(),
                NameUtils.prettyPrint(name)
            ));

            if (mode.allowType()) {
                return new InvalidTypeSymbol(symbol);
            } else if (mode.allowNamespace()) {
                return new InvalidNamespaceSymbol(symbol);
            } else {
                throw new IllegalStateException();
            }
        }

        return result;
    }
}

class NamespaceScope extends ContainerScope {
    private final NamespaceSymbol symbol;

    NamespaceScope(NamespaceSymbol symbol, List<Import> imports, Scope parent) {
        super(symbol, imports, parent);
        this.symbol = symbol;
    }
}

class Import {
    private final String alias;
    private final ContainerSymbol symbol;
    private final ImportType type;

    Import(String alias, ContainerSymbol symbol, ImportType type) {
        this.alias = alias;
        this.symbol = symbol;
        this.type = type;
    }

    public String getAlias() {
        return alias;
    }

    public ContainerSymbol getSymbol() {
        return symbol;
    }

    public ImportType getType() {
        return type;
    }
}

enum ImportType {
    NAMESPACE,
    TYPE,
    STATIC
}

class TypeScope extends ContainerScope {
    private final TypeSymbol symbol;

    TypeScope(TypeSymbol symbol, Scope parent) {
        super(symbol, null, parent);
        this.symbol = symbol;
    }
}
