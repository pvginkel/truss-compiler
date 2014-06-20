using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Parser {
    partial class TrussParser {
        public string FileName { get; set; }

        public ErrorList Errors { get; set; }

        public CompilationUnitSyntax ParseCompilationUnit() {
            return compilationUnit();
        }

        public override object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow) {
            Errors.Add(Error.FromRecognitionException(FileName, e));

            return base.RecoverFromMismatchedSet(input, e, follow);
        }

        protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow) {
            var token = this.input.LT(-1);

            var span = new Span(
                FileName,
                token.Line,
                token.CharPositionInLine + token.Text.Length
                );

            Errors.Add(Error.FromMismatchedToken(span, input, ttype));

            return base.RecoverFromMismatchedToken(input, ttype, follow);
        }

        public override void ReportError(RecognitionException e) {
            Errors.Add(Error.FromRecognitionException(FileName, e));
        }

        private Span Span(IToken start) {
            var end = input.LT(-1) ?? input.Get(input.Count - 1);
            return Span(start, end);
        }

        private Span Span(IToken start, IToken end) {
            return new Span(
                FileName,
                start.Line,
                start.CharPositionInLine,
                end.Line,
                end.CharPositionInLine + end.Text.Length
                );
        }

        private AttributeTarget ParseAttributeTarget(string identifier, Span span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            // Attribute targets are contextual keywords.

            switch (identifier) {
                case "assembly":
                    return AttributeTarget.Assembly;
                case "event":
                    return AttributeTarget.Event;
                case "field":
                    return AttributeTarget.Field;
                case "method":
                    return AttributeTarget.Method;
                case "param":
                    return AttributeTarget.Param;
                case "property":
                    return AttributeTarget.Property;
                case "return":
                    return AttributeTarget.Return;
                case "type":
                    return AttributeTarget.Type;
                default:
                    Errors.Add(Error.InvalidAttributeTarget, span);
                    return AttributeTarget.None;
            }
        }

        protected AccessorDeclarationType ParseAccessorDeclarationType(string identifier, Span span) {
            // Accessor declaration types are contextual keywords.

            switch (identifier) {
                case "get":
                    return AccessorDeclarationType.Get;
                case "set":
                    return AccessorDeclarationType.Set;
                case "add":
                    return AccessorDeclarationType.Add;
                case "remove":
                    return AccessorDeclarationType.Remove;
                default:
                    Errors.Add(Error.InvalidAccessorDeclarationType, span);
                    return AccessorDeclarationType.Invalid;
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

        protected class MemberName {
            public MemberName(NameSyntax interfaceName, IdentifierNameSyntax identifier, ImmutableArray<TypeParameterSyntax> typeParameters) {
                InterfaceName = interfaceName;
                Identifier = identifier;
                TypeParameters = typeParameters;
            }

            public NameSyntax InterfaceName { get; private set; }

            public IdentifierNameSyntax Identifier { get; private set; }

            public ImmutableArray<TypeParameterSyntax> TypeParameters { get; private set; }
        }

        protected abstract class TypeParser {
            protected TypeParser(Span span) {
                if (span == null) {
                    throw new ArgumentNullException("span");
                }

                Span = span;
            }

            public Span Span { get; private set; }

            public abstract TypeSyntax ToType();
        }

        protected class ArrayTypeParser : TypeParser {
            private readonly TypeParser _elementType;
            private readonly ImmutableArray<ArrayRankSpecifierSyntax> _rankSpecifiers;

            public ArrayTypeParser(TypeParser elementType, ImmutableArray<ArrayRankSpecifierSyntax> rankSpecifiers, Span span)
                : base(span) {
                if (elementType == null) {
                    throw new ArgumentNullException("elementType");
                }
                if (rankSpecifiers == null) {
                    throw new ArgumentNullException("rankSpecifiers");
                }

                _elementType = elementType;
                _rankSpecifiers = rankSpecifiers;
            }

            public override TypeSyntax ToType() {
                return new ArrayTypeSyntax(_elementType.ToType(), _rankSpecifiers, Span);
            }
        }

        protected class TrackedTypeParser : TypeParser {
            private readonly TypeParser _elementType;

            public TrackedTypeParser(TypeParser elementType, Span span)
                : base(span) {
                if (elementType == null) {
                    throw new ArgumentNullException("elementType");
                }

                _elementType = elementType;
            }

            public override TypeSyntax ToType() {
                return new TrackedTypeSyntax(_elementType.ToType(), Span);
            }
        }

        protected class NullableTypeParser : TypeParser {
            private readonly TypeParser _elementType;

            public NullableTypeParser(TypeParser elementType, Span span)
                : base(span) {
                if (elementType == null) {
                    throw new ArgumentNullException("elementType");
                }

                _elementType = elementType;
            }

            public override TypeSyntax ToType() {
                return new NullableTypeSyntax(_elementType.ToType(), Span);
            }
        }

        protected class OmittedTypeArgumentParser : TypeParser {
            public OmittedTypeArgumentParser(Span span)
                : base(span) {
            }

            public override TypeSyntax ToType() {
                return new OmittedTypeArgumentSyntax(Span);
            }
        }

        protected abstract class NameParser : TypeParser {
            protected NameParser(Span span)
                : base(span) {
            }

            public override TypeSyntax ToType() {
                return ToName();
            }

            public abstract NameSyntax ToName();

            public virtual MemberName ToMemberName() {
                throw new InvalidOperationException();
            }
        }

        protected class PredefinedTypeParser : TypeParser {
            private readonly PredefinedType _predefinedType;

            public PredefinedTypeParser(PredefinedType predefinedType, Span span)
                : base(span) {
                _predefinedType = predefinedType;
            }

            public override TypeSyntax ToType() {
                return new PredefinedTypeSyntax(_predefinedType, Span);
            }
        }

        protected class AliasQualifiedNameParser : NameParser {
            private readonly IdentifierNameParser _alias;
            private readonly SimpleNameParser _name;

            public AliasQualifiedNameParser(IdentifierNameParser alias, SimpleNameParser name, Span span)
                : base(span) {
                if (alias == null) {
                    throw new ArgumentNullException("alias");
                }
                if (name == null) {
                    throw new ArgumentNullException("name");
                }

                _alias = alias;
                _name = name;
            }

            public override NameSyntax ToName() {
                return new AliasQualifiedNameSyntax(
                    (IdentifierNameSyntax)_alias.ToSimpleName(),
                    _name.ToSimpleName(),
                    Span
                    );
            }
        }

        protected abstract class SimpleNameParser : NameParser {
            protected SimpleNameParser(string identifier, Span span)
                : base(span) {
                if (identifier == null) {
                    throw new ArgumentNullException("identifier");
                }

                Identifier = identifier;
            }

            public string Identifier { get; private set; }

            public abstract SimpleNameSyntax ToSimpleName();

            public override NameSyntax ToName() {
                return ToSimpleName();
            }
        }

        protected class GenericNameParser : SimpleNameParser {
            private readonly ErrorList _errors;
            private readonly ImmutableArray<TypeParser> _typeArguments;

            public GenericNameParser(ErrorList errors, string identifier, ImmutableArray<TypeParser> typeArguments, Span span)
                : base(identifier, span) {
                if (typeArguments == null) {
                    throw new ArgumentNullException("typeArguments");
                }

                _errors = errors;
                _typeArguments = typeArguments;
            }

            public override SimpleNameSyntax ToSimpleName() {
                var builder = new ImmutableArray<TypeSyntax>.Builder();

                foreach (var typeArgument in _typeArguments) {
                    builder.Add(typeArgument.ToType());
                }

                return new GenericNameSyntax(
                    Identifier,
                    builder.Build(),
                    Span
                    );
            }

            public override MemberName ToMemberName() {
                // What we see here as a generic name, really is the method name with an type
                // parameters.

                var typeParameters = new ImmutableArray<TypeParameterSyntax>.Builder();
                var identifierName = new IdentifierNameSyntax(Identifier, Span);

                foreach (var typeArgument in _typeArguments) {
                    if (typeArgument is TypeParameterParser) {
                        typeParameters.Add(((TypeParameterParser)typeArgument).ToTypeParameter());
                    } else {
                        _errors.Add(Error.GenericTypeIllegalMissingTypeParameter, typeArgument.Span);
                    }
                }

                return new MemberName(
                    null,
                    identifierName,
                    typeParameters.Build()
                    );
            }
        }

        protected class TypeParameterParser : TypeParser {
            private readonly ErrorList _errors;
            private readonly ImmutableArray<AttributeListSyntax> _attributeLists;
            private readonly Variance _variance;
            private readonly TypeParser _type;

            public TypeParameterParser(ErrorList errors, ImmutableArray<AttributeListSyntax> attributeLists, Variance variance, TypeParser type, Span span)
                : base(span) {
                if (type == null) {
                    throw new ArgumentNullException("type");
                }

                _errors = errors;
                _attributeLists = attributeLists;
                _variance = variance;
                _type = type;
            }

            public override TypeSyntax ToType() {
                if (_attributeLists.Count > 0) {
                    _errors.Add(Error.GenericTypeIllegalAttributes, Span);
                }
                if (_variance != Variance.None) {
                    _errors.Add(Error.GenericTypeIllegalVariance, Span);
                }

                return _type.ToType();
            }

            public TypeParameterSyntax ToTypeParameter() {
                IdentifierNameSyntax identifier;

                if (!(_type is IdentifierNameParser)) {
                    _errors.Add(Error.GenericTypeIllegalTypeParameter, Span);

                    identifier = new IdentifierNameSyntax("**INVALID**", Span);
                } else {
                    identifier = (IdentifierNameSyntax)((IdentifierNameParser)_type).ToSimpleName();
                }

                return new TypeParameterSyntax(
                    _attributeLists,
                    _variance,
                    identifier,
                    Span
                    );
            }
        }

        protected class IdentifierNameParser : SimpleNameParser {
            public IdentifierNameParser(string identifier, Span span)
                : base(identifier, span) {
            }

            public override SimpleNameSyntax ToSimpleName() {
                return new IdentifierNameSyntax(Identifier, Span);
            }

            public override MemberName ToMemberName() {
                return new MemberName(
                    null,
                    (IdentifierNameSyntax)ToSimpleName(),
                    ImmutableArray<TypeParameterSyntax>.Empty
                    );
            }
        }

        protected class QualifiedNameParser : NameParser {
            private readonly NameParser _left;
            private readonly SimpleNameParser _right;

            public QualifiedNameParser(NameParser left, SimpleNameParser right, Span span)
                : base(span) {
                if (left == null) {
                    throw new ArgumentNullException("left");
                }
                if (right == null) {
                    throw new ArgumentNullException("right");
                }

                _left = left;
                _right = right;
            }

            public override NameSyntax ToName() {
                return new QualifiedNameSyntax(
                    _left.ToName(),
                    _right.ToSimpleName(),
                    Span
                    );
            }

            public override MemberName ToMemberName() {
                // What we see here as a qualified name, really is the method name with an explicit
                // interface implementation. We let right create a member name, and repackage it
                // here with the explicit interface name.

                var memberName = _right.ToMemberName();

                return new MemberName(
                    _left.ToName(),
                    memberName.Identifier,
                    memberName.TypeParameters
                    );
            }
        }
    }
}
