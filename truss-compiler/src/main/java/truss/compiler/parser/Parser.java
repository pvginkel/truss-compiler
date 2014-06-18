package truss.compiler.parser;

import org.antlr.runtime.*;
import org.apache.commons.lang.Validate;
import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.MessageType;
import truss.compiler.Span;
import truss.compiler.support.ImmutableArray;
import truss.compiler.syntax.*;

public abstract class Parser extends org.antlr.runtime.Parser {
    private String fileName;

    protected Parser(TokenStream input) {
        super(input);
    }

    protected Parser(TokenStream input, RecognizerSharedState state) {
        super(input, state);
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    @Override
    public Object recoverFromMismatchedSet(IntStream input, RecognitionException e, BitSet follow) throws RecognitionException {
        MessageCollectionScope.addMessage(Message.fromRecognitionException(fileName, e));

        return super.recoverFromMismatchedSet(input, e, follow);
    }

    @Override
    protected Object recoverFromMismatchedToken(IntStream input, int ttype, BitSet follow) throws RecognitionException {
        Token token = this.input.LT(-1);

        Span span = new Span(
            fileName,
            token.getLine(),
            token.getCharPositionInLine() + token.getText().length()
        );

        MessageCollectionScope.addMessage(Message.fromMismatchedToken(span, input, ttype));

        return super.recoverFromMismatchedToken(input, ttype, follow);
    }

    @Override
    public void reportError(RecognitionException e) {
        MessageCollectionScope.addMessage(Message.fromRecognitionException(fileName, e));
    }

    protected Span span(Token start) {
        Token end = input.LT(-1);
        if (end == null) {
            end = input.get(input.size() - 1);
        }
        return span(start, end);
    }

    protected Span span(Token start, Token end) {
        return new Span(
            fileName,
            start.getLine(),
            start.getCharPositionInLine(),
            end.getLine(),
            end.getCharPositionInLine() + end.getText().length()
        );
    }

    protected Span adjustSpanStart(Token start, Span span) {
        return new Span(
            fileName,
            start.getLine(),
            start.getCharPositionInLine(),
            span.getEndLine(),
            span.getEndColumn()
        );
    }

    protected AttributeTarget parseAttributeTarget(String identifier, Span span) {
        Validate.notNull(identifier, "identifier");

        // Attribute targets are contextual keywords.

        switch (identifier) {
            case "assembly":
                return AttributeTarget.ASSEMBLY;
            case "event":
                return AttributeTarget.EVENT;
            case "field":
                return AttributeTarget.FIELD;
            case "method":
                return AttributeTarget.METHOD;
            case "param":
                return AttributeTarget.PARAM;
            case "property":
                return AttributeTarget.PROPERTY;
            case "return":
                return AttributeTarget.RETURN;
            case "type":
                return AttributeTarget.TYPE;
            default:
                MessageCollectionScope.addMessage(new Message(
                    MessageType.INVALID_ATTRIBUTE_TARGET,
                    span
                ));
                return AttributeTarget.NONE;
        }
    }

    protected AccessorDeclarationType parseAccessorDeclarationType(String identifier, Span span) {
        // Accessor declaration types are contextual keywords.

        switch (identifier) {
            case "get":
                return AccessorDeclarationType.GET;
            case "set":
                return AccessorDeclarationType.SET;
            case "add":
                return AccessorDeclarationType.ADD;
            case "remove":
                return AccessorDeclarationType.REMOVE;
            default:
                MessageCollectionScope.addMessage(new Message(
                    MessageType.INVALID_ACCESSOR_DECLARATION_TYPE,
                    span
                ));
                return AccessorDeclarationType.INVALID;
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    // TYPE PARSING                                                           //
    ////////////////////////////////////////////////////////////////////////////

    /*
     * The classes below are copies of the actual syntax node classes for type parsing.
     * Type parsing is done in two steps. First, the tokens are parsed into the classes
     * below. These classes are then converted to SyntaxNode classes when parsing
     * is complete. The point of this is that this fixes some issues with parsing
     * method names with explicit interfaces and type parameters. This is handled in
     * toMethodName which converts the matched name into a MemberName.
     */

    protected static class MemberName {
        private final NameSyntax interfaceName;
        private final IdentifierNameSyntax identifier;
        private final ImmutableArray<TypeParameterSyntax> typeParameters;

        public MemberName(NameSyntax interfaceName, IdentifierNameSyntax identifier, ImmutableArray<TypeParameterSyntax> typeParameters) {
            this.interfaceName = interfaceName;
            this.identifier = identifier;
            this.typeParameters = typeParameters;
        }

        public NameSyntax getInterfaceName() {
            return interfaceName;
        }

        public IdentifierNameSyntax getIdentifier() {
            return identifier;
        }

        public ImmutableArray<TypeParameterSyntax> getTypeParameters() {
            return typeParameters;
        }
    }

    protected abstract static class TypeParser {
        private final Span span;

        public TypeParser(Span span) {
            Validate.notNull(span, "span");

            this.span = span;
        }

        public Span getSpan() {
            return span;
        }

        public abstract TypeSyntax toType();
    }

    protected static class ArrayTypeParser extends TypeParser {
        private final TypeParser elementType;
        private final ImmutableArray<ArrayRankSpecifierSyntax> rankSpecifiers;

        public ArrayTypeParser(TypeParser elementType, ImmutableArray<ArrayRankSpecifierSyntax> rankSpecifiers, Span span) {
            super(span);

            Validate.notNull(elementType, "elementType");
            Validate.notNull(rankSpecifiers, "rankSpecifiers");

            this.elementType = elementType;
            this.rankSpecifiers = rankSpecifiers;
        }

        @Override
        public TypeSyntax toType() {
            return new ArrayTypeSyntax(elementType.toType(), rankSpecifiers, getSpan());
        }
    }

    protected static class TrackedTypeParser extends TypeParser {
        private final TypeParser elementType;

        public TrackedTypeParser(TypeParser elementType, Span span) {
            super(span);

            Validate.notNull(elementType, "elementType");

            this.elementType = elementType;
        }

        @Override
        public TypeSyntax toType() {
            return new TrackedTypeSyntax(elementType.toType(), getSpan());
        }
    }

    protected static class NullableTypeParser extends TypeParser {
        private final TypeParser elementType;

        public NullableTypeParser(TypeParser elementType, Span span) {
            super(span);

            Validate.notNull(elementType, "elementType");

            this.elementType = elementType;
        }

        @Override
        public TypeSyntax toType() {
            return new NullableTypeSyntax(elementType.toType(), getSpan());
        }
    }

    protected static class OmittedTypeArgumentParser extends TypeParser {
        public OmittedTypeArgumentParser(Span span) {
            super(span);
        }

        @Override
        public TypeSyntax toType() {
            return new OmittedTypeArgumentSyntax(getSpan());
        }
    }

    protected abstract static class NameParser extends TypeParser {
        protected NameParser(Span span) {
            super(span);
        }

        @Override
        public TypeSyntax toType() {
            return toName();
        }

        public abstract NameSyntax toName();

        public MemberName toMemberName() {
            throw new IllegalStateException();
        }
    }

    protected static class PredefinedTypeParser extends TypeParser {
        private final PredefinedType predefinedType;

        public PredefinedTypeParser(PredefinedType predefinedType, Span span) {
            super(span);

            Validate.notNull(predefinedType, "predefinedType");

            this.predefinedType = predefinedType;
        }

        @Override
        public TypeSyntax toType() {
            return new PredefinedTypeSyntax(predefinedType, getSpan());
        }
    }

    protected static class AliasQualifiedNameParser extends NameParser {
        private final IdentifierNameParser alias;
        private final SimpleNameParser name;

        public AliasQualifiedNameParser(IdentifierNameParser alias, SimpleNameParser name, Span span) {
            super(span);

            Validate.notNull(alias, "alias");
            Validate.notNull(name, "name");

            this.alias = alias;
            this.name = name;
        }

        @Override
        public NameSyntax toName() {
            return new AliasQualifiedNameSyntax(
                (IdentifierNameSyntax)alias.toSimpleName(),
                name.toSimpleName(),
                getSpan()
            );
        }
    }

    protected abstract static class SimpleNameParser extends NameParser {
        private final String identifier;

        protected SimpleNameParser(String identifier, Span span) {
            super(span);

            Validate.notNull(identifier, "identifier");

            this.identifier = identifier;
        }

        public String getIdentifier() {
            return identifier;
        }

        public abstract SimpleNameSyntax toSimpleName();

        @Override
        public NameSyntax toName() {
            return toSimpleName();
        }
    }

    protected static class GenericNameParser extends SimpleNameParser {
        private final ImmutableArray<TypeParser> typeArguments;

        public GenericNameParser(String identifier, ImmutableArray<TypeParser> typeArguments, Span span) {
            super(identifier, span);

            Validate.notNull(typeArguments, "typeArguments");

            this.typeArguments = typeArguments;
        }

        @Override
        public SimpleNameSyntax toSimpleName() {
            ImmutableArray.Builder<TypeSyntax> builder = new ImmutableArray.Builder<>();

            for (TypeParser typeArgument : typeArguments) {
                builder.add(typeArgument.toType());
            }

            return new GenericNameSyntax(
                getIdentifier(),
                builder.build(),
                getSpan()
            );
        }

        @Override
        public MemberName toMemberName() {
            // What we see here as a generic name, really is the method name with an type
            // parameters.

            ImmutableArray.Builder<TypeParameterSyntax> typeParameters = new ImmutableArray.Builder<>();
            IdentifierNameSyntax identifierName = new IdentifierNameSyntax(getIdentifier(), getSpan());

            for (TypeParser typeArgument : typeArguments) {
                if (typeArgument instanceof TypeParameterParser) {
                    typeParameters.add(((TypeParameterParser)typeArgument).toTypeParameter());
                } else {
                    MessageCollectionScope.addMessage(new Message(
                        MessageType.GENERIC_TYPE_ILLEGAL_MISSING_TYPE_PARAMETER,
                        typeArgument.getSpan()
                    ));
                }
            }

            return new MemberName(
                null,
                identifierName,
                typeParameters.build()
            );
        }
    }

    protected static class TypeParameterParser extends TypeParser {
        private final ImmutableArray<AttributeListSyntax> attributeLists;
        private final Variance variance;
        private final TypeParser type;

        public TypeParameterParser(ImmutableArray<AttributeListSyntax> attributeLists, Variance variance, TypeParser type, Span span) {
            super(span);

            Validate.notNull(type, "type");

            this.attributeLists = attributeLists;
            this.variance = variance;
            this.type = type;
        }

        @Override
        public TypeSyntax toType() {
            if (attributeLists.size() > 0) {
                MessageCollectionScope.addMessage(new Message(
                    MessageType.GENERIC_TYPE_ILLEGAL_ATTRIBUTES,
                    getSpan()
                ));
            }
            if (variance != Variance.NONE) {
                MessageCollectionScope.addMessage(new Message(
                    MessageType.GENERIC_TYPE_ILLEGAL_VARIANCE,
                    getSpan()
                ));
            }

            return type.toType();
        }

        public TypeParameterSyntax toTypeParameter() {
            IdentifierNameSyntax identifier;

            if (!(type instanceof IdentifierNameParser)) {
                MessageCollectionScope.addMessage(new Message(
                    MessageType.GENERIC_TYPE_ILLEGAL_TYPE_PARAMETER,
                    getSpan()
                ));

                identifier = new IdentifierNameSyntax("**INVALID**", getSpan());
            } else {
                identifier = (IdentifierNameSyntax)((IdentifierNameParser)type).toSimpleName();
            }

            return new TypeParameterSyntax(
                attributeLists,
                variance,
                identifier,
                getSpan()
            );
        }
    }

    protected static class IdentifierNameParser extends SimpleNameParser {
        public IdentifierNameParser(String identifier, Span span) {
            super(identifier, span);
        }

        @Override
        public SimpleNameSyntax toSimpleName() {
            return new IdentifierNameSyntax(getIdentifier(), getSpan());
        }

        @Override
        public MemberName toMemberName() {
            return new MemberName(
                null,
                (IdentifierNameSyntax)toSimpleName(),
                ImmutableArray.<TypeParameterSyntax>empty()
            );
        }
    }

    protected static class QualifiedNameParser extends NameParser {
        private final NameParser left;
        private final SimpleNameParser right;

        public QualifiedNameParser(NameParser left, SimpleNameParser right, Span span) {
            super(span);

            Validate.notNull(left, "left");
            Validate.notNull(right, "right");

            this.left = left;
            this.right = right;
        }

        @Override
        public NameSyntax toName() {
            return new QualifiedNameSyntax(
                left.toName(),
                right.toSimpleName(),
                getSpan()
            );
        }

        @Override
        public MemberName toMemberName() {
            // What we see here as a qualified name, really is the method name with an explicit
            // interface implementation. We let right create a member name, and repackage it
            // here with the explicit interface name.

            MemberName memberName = right.toMemberName();

            return new MemberName(
                left.toName(),
                memberName.getIdentifier(),
                memberName.getTypeParameters()
            );
        }
    }
}
