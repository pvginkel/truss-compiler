package truss.compiler.symbols;

import org.apache.commons.lang.Validate;
import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.MessageType;
import truss.compiler.Span;
import truss.compiler.support.ImmutableArray;
import truss.compiler.syntax.DelegateDeclarationSyntax;
import truss.compiler.syntax.EnumDeclarationSyntax;
import truss.compiler.syntax.Modifier;
import truss.compiler.syntax.TypeDeclarationSyntax;

import java.util.Collections;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class TypeSymbol extends TypeSymbolBase {
    private final TypeKind kind;
    private Set<TypeModifier> modifiers;
    private AccessModifier access;
    private boolean static_;
    private boolean abstract_;
    private boolean sealed_;
    private boolean readonly;
    private ImmutableArray<TypeParameterSymbol> typeParameters;

    private TypeSymbol(TypeKind kind, ContainerSymbol container, String name, String metadataName, Set<TypeModifier> modifiers) {
        super(name, metadataName, container);

        this.kind = kind;
        this.modifiers = modifiers;
    }

    public void parseModifiers() {
        boolean accessConflict = false;

        for (TypeModifier modifier : modifiers) {
            switch (modifier) {
                case ABSTRACT:
                    abstract_ = true;
                    break;

                case INTERNAL:
                    if (access == null) {
                        access = AccessModifier.INTERNAL;
                    } else {
                        accessConflict = true;
                    }
                    break;

                case PARTIAL:
                    // We don't care about partial. Partial already did its work, and we can loose it now.
                    break;

                case PUBLIC:
                    if (access == null) {
                        access = AccessModifier.PUBLIC;
                    } else {
                        accessConflict = true;
                    }
                    break;

                case READONLY:
                    readonly = true;
                    break;

                case SEALED:
                    sealed_ = true;
                    break;

                case STATIC:
                    static_ = true;
                    break;
            }
        }

        if (accessConflict) {
            MessageCollectionScope.addMessage(new Message(
                MessageType.DUPLICATE_ACCESS_MODIFIER,
                getSpans().get(0)
            ));
        }

        if (access == null) {
            // Default modifier on types.
            access = AccessModifier.INTERNAL;
        }

        if (abstract_ && (static_ || sealed_)) {
            MessageCollectionScope.addMessage(new Message(
                MessageType.INVALID_ABSTRACT_TYPE_COMBINATION,
                getSpans().get(0)
            ));
        }
    }

    public TypeKind getTypeKind() {
        return kind;
    }

    @Override
    public SymbolKind getKind() {
        return SymbolKind.TYPE;
    }

    @Override
    public void accept(SymbolVisitor visitor) {
        if (!visitor.isDone()) {
            visitor.visitType(this);
        }
    }

    public Set<TypeModifier> getModifiers() {
        return modifiers;
    }

    public AccessModifier getAccess() {
        return access;
    }

    public boolean isStatic() {
        return static_;
    }

    public boolean isAbstract() {
        return abstract_;
    }

    public boolean isSealed() {
        return sealed_;
    }

    public boolean isReadonly() {
        return readonly;
    }

    public static TypeSymbol fromDelegate(DelegateDeclarationSyntax syntax, ContainerSymbol container) {
        Validate.notNull(syntax, "syntax");
        Validate.notNull(container, "container");

        Set<TypeModifier> modifiers = makeDelegateModifiers(syntax.getModifiers(), syntax.getSpan());
        String name = syntax.getIdentifier().getIdentifier();
        String metadataName = NameUtils.makeMetadataName(name, syntax.getTypeParameters());

        if (container.getMemberByMetadataName(metadataName).size() > 0) {
            MessageCollectionScope.addMessage(new Message(
                MessageType.IDENTIFIER_ALREADY_DEFINED,
                syntax.getSpan(),
                name
            ));

            return null;
        }

        TypeSymbol result = new TypeSymbol(
            TypeKind.DELEGATE,
            container,
            name,
            metadataName,
            modifiers
        );

        result.addSpan(syntax.getSpan());
        container.addMember(result);

        return result;
    }

    private static Set<TypeModifier> makeDelegateModifiers(ImmutableArray<Modifier> modifiers, Span span) {
        Set<TypeModifier> result = new HashSet<>();

        for (Modifier modifier : modifiers) {
            switch (modifier) {
                case INTERNAL:
                    result.add(TypeModifier.INTERNAL);
                    break;

                case PUBLIC:
                    result.add(TypeModifier.PUBLIC);
                    break;

                default:
                    MessageCollectionScope.addMessage(new Message(
                        MessageType.DELEGATE_INVALID_MODIFIER,
                        span,
                        modifier.toString().toLowerCase()
                    ));
            }
        }

        return Collections.unmodifiableSet(result);
    }

    public static Symbol fromEnum(EnumDeclarationSyntax syntax, ContainerSymbol container) {
        Validate.notNull(syntax, "syntax");
        Validate.notNull(container, "container");

        Set<TypeModifier> modifiers = makeEnumModifiers(syntax.getModifiers(), syntax.getSpan());
        String name = syntax.getIdentifier().getIdentifier();

        FindPartial other = findOtherPartial(name, container, modifiers, syntax.getSpan());
        if (other != FindPartial.NOT_FOUND) {
            return other.other;
        }

        Symbol result = new TypeSymbol(
            TypeKind.ENUM,
            container,
            name,
            name,
            modifiers
        );

        container.addMember(result);

        return result;
    }

    private static Set<TypeModifier> makeEnumModifiers(ImmutableArray<Modifier> modifiers, Span span) {
        Set<TypeModifier> result = new HashSet<>();

        for (Modifier modifier : modifiers) {
            switch (modifier) {
                case INTERNAL:
                    result.add(TypeModifier.INTERNAL);
                    break;

                case PARTIAL:
                    result.add(TypeModifier.PARTIAL);
                    break;

                case PUBLIC:
                    result.add(TypeModifier.PUBLIC);
                    break;

                default:
                    MessageCollectionScope.addMessage(new Message(
                        MessageType.ENUM_INVALID_MODIFIER,
                        span,
                        modifier.toString().toLowerCase()
                    ));
            }
        }

        return Collections.unmodifiableSet(result);
    }

    public static TypeSymbol fromType(TypeDeclarationSyntax syntax, ContainerSymbol container) {
        Validate.notNull(syntax, "syntax");
        Validate.notNull(container, "container");

        TypeKind kind;
        switch (syntax.getType()) {
            case CLASS:
                kind = TypeKind.CLASS;
                break;
            case STRUCT:
                kind = TypeKind.STRUCT;
                break;
            default:
                kind = TypeKind.INTERFACE;
                break;
        }

        Set<TypeModifier> modifiers = makeTypeModifiers(kind, syntax.getModifiers(), syntax.getSpan());
        String name = syntax.getIdentifier().getIdentifier();
        String metadataName = NameUtils.makeMetadataName(name, syntax.getTypeParameters());

        FindPartial other = findOtherPartial(name, container, modifiers, syntax.getSpan());
        if (other != FindPartial.NOT_FOUND) {
            return other.other;
        }

        TypeSymbol result = new TypeSymbol(
            kind,
            container,
            name,
            metadataName,
            modifiers
        );

        container.addMember(result);

        return result;
    }

    private static Set<TypeModifier> makeTypeModifiers(TypeKind kind, ImmutableArray<Modifier> modifiers, Span span) {
        Set<TypeModifier> result = new HashSet<>();

        for (Modifier modifier : modifiers) {
            TypeModifier typeModifier = null;

            switch (modifier) {
                case ABSTRACT:
                    if (kind == TypeKind.CLASS) {
                        typeModifier = TypeModifier.ABSTRACT;
                    }
                    break;

                case INTERNAL:
                    typeModifier = TypeModifier.INTERNAL;
                    break;

                case PARTIAL:
                    typeModifier = TypeModifier.PARTIAL;
                    break;

                case PUBLIC:
                    typeModifier = TypeModifier.PUBLIC;
                    break;

                case READONLY:
                    if (kind == TypeKind.CLASS) {
                        typeModifier = TypeModifier.READONLY;
                    }
                    break;

                case SEALED:
                    if (kind == TypeKind.CLASS) {
                        typeModifier = TypeModifier.SEALED;
                    }
                    break;

                case STATIC:
                    if (kind == TypeKind.CLASS) {
                        typeModifier = TypeModifier.STATIC;
                    }
                    break;
            }

            if (typeModifier == null) {
                MessageType messageType;

                switch (kind) {
                    case CLASS:
                        messageType = MessageType.CLASS_INVALID_MODIFIER;
                        break;
                    case STRUCT:
                        messageType = MessageType.STRUCT_INVALID_MODIFIER;
                        break;
                    default:
                        messageType = MessageType.INTERFACE_INVALID_MODIFIER;
                        break;
                }

                MessageCollectionScope.addMessage(new Message(
                    messageType,
                    span,
                    modifier.toString().toLowerCase()
                ));
            } else {
                result.add(typeModifier);
            }
        }

        return Collections.unmodifiableSet(result);
    }

    public void setTypeParameters(ImmutableArray<TypeParameterSymbol> typeParameters) {
        this.typeParameters = typeParameters;
    }

    public ImmutableArray<TypeParameterSymbol> getTypeParameters() {
        return typeParameters;
    }

    private static class FindPartial {
        static FindPartial NOT_FOUND = new FindPartial(null);
        static FindPartial ERROR = new FindPartial(null);

        TypeSymbol other;

        private FindPartial(TypeSymbol other) {
            this.other = other;
        }
    }

    private static FindPartial findOtherPartial(String name, ContainerSymbol container, Set<TypeModifier> modifiers, Span span) {
        List<Symbol> members = container.getMemberByMetadataName(name);

        // If this is a partial type, find the other one and merge this with that.

        for (Symbol member : members) {
            if (member instanceof TypeSymbol) {
                TypeSymbol other = (TypeSymbol)member;

                // All declarations for this type must be partial for it to match.

                if (!modifiers.contains(TypeModifier.PARTIAL)) {
                    MessageCollectionScope.addMessage(new Message(
                        MessageType.IDENTIFIER_ALREADY_DEFINED,
                        span,
                        name
                    ));

                    return FindPartial.ERROR;
                }

                if (!other.getModifiers().contains(TypeModifier.PARTIAL)) {
                    MessageCollectionScope.addMessage(new Message(
                        MessageType.TYPE_OTHER_NOT_PARTIAL,
                        span,
                        name
                    ));

                    return FindPartial.ERROR;
                }

                // Add our modifiers to the existing type.
                modifiers.addAll(other.modifiers);
                other.modifiers = Collections.unmodifiableSet(modifiers);

                // Add our span.
                other.addSpan(span);

                return new FindPartial(other);
            }
        }

        return FindPartial.NOT_FOUND;
    }
}
