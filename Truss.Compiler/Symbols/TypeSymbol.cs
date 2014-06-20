using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public class TypeSymbol : TypeSymbolBase {
        private readonly TypeKind _kind;

        private TypeSymbol(TypeKind kind, ContainerSymbol container, string name, string metadataName, TypeModifier modifiers)
            : base(name, metadataName, container) {
            _kind = kind;
            Modifiers = modifiers;
        }

        public void ParseModifiers() {
            IsAbstract = Modifiers.HasFlag(TypeModifier.Abstract);
            IsReadonly = Modifiers.HasFlag(TypeModifier.Readonly);
            IsSealed = Modifiers.HasFlag(TypeModifier.Sealed);
            IsStatic = Modifiers.HasFlag(TypeModifier.Static);

            if (Modifiers.HasFlag(TypeModifier.Internal) && Modifiers.HasFlag(TypeModifier.Public)) {
                MessageCollectionScope.AddMessage(new Message(
                    MessageType.DuplicateAccessModifier,
                    Spans[0]
                    ));
            } else if (Modifiers.HasFlag(TypeModifier.Public)) {
                Access = AccessModifier.Public;
            } else {
                // Default modifier on types.
                Access = AccessModifier.Internal;
            }

            if (IsAbstract && (IsStatic || IsSealed)) {
                MessageCollectionScope.AddMessage(new Message(
                    MessageType.InvalidAbstractTypeCombination,
                    Spans[0]
                    ));
            }
        }

        public override TypeKind TypeKind {
            get { return _kind; }
        }

        public override SymbolKind Kind {
            get { return SymbolKind.Type; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitType(this);
            }
        }

        public TypeModifier Modifiers { get; private set; }

        public AccessModifier Access { get; private set; }

        public bool IsStatic { get; private set; }

        public bool IsAbstract { get; private set; }

        public bool IsSealed { get; private set; }

        public bool IsReadonly { get; private set; }

        public static TypeSymbol FromDelegate(DelegateDeclarationSyntax syntax, ContainerSymbol container) {
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            var modifiers = MakeDelegateModifiers(syntax.Modifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;
            string metadataName = NameUtils.MakeMetadataName(name, syntax.TypeParameters);

            if (container.GetMemberByMetadataName(metadataName).Count > 0) {
                MessageCollectionScope.AddMessage(new Message(
                    MessageType.IdentifierAlreadyDefined,
                    syntax.Span,
                    name
                    ));

                return null;
            }

            var result = new TypeSymbol(
                TypeKind.Delegate,
                container,
                name,
                metadataName,
                modifiers
                );

            result.AddSpan(syntax.Span);
            container.AddMember(result);

            return result;
        }

        private static TypeModifier MakeDelegateModifiers(ImmutableArray<Modifier> modifiers, Span span) {
            var result = TypeModifier.None;

            foreach (var modifier in modifiers) {
                switch (modifier) {
                    case Modifier.Internal:
                        result |= TypeModifier.Internal;
                        break;

                    case Modifier.Public:
                        result |= TypeModifier.Public;
                        break;

                    default:
                        MessageCollectionScope.AddMessage(new Message(
                            MessageType.DelegateInvalidModifier,
                            span,
                            modifier.ToString().ToLower()
                            ));
                        break;
                }
            }

            return result;
        }

        public static Symbol FromEnum(EnumDeclarationSyntax syntax, ContainerSymbol container) {
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            var modifiers = MakeEnumModifiers(syntax.Modifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;

            var other = FindOtherPartial(name, container, modifiers, syntax.Span);
            if (other != FindPartial.NotFound) {
                return other.Other;
            }

            Symbol result = new TypeSymbol(
                TypeKind.Enum,
                container,
                name,
                name,
                modifiers
                );

            container.AddMember(result);

            return result;
        }

        private static TypeModifier MakeEnumModifiers(ImmutableArray<Modifier> modifiers, Span span) {
            var result = TypeModifier.None;

            foreach (var modifier in modifiers) {
                switch (modifier) {
                    case Modifier.Internal:
                        result |= TypeModifier.Internal;
                        break;

                    case Modifier.Partial:
                        result |= TypeModifier.Partial;
                        break;

                    case Modifier.Public:
                        result |= TypeModifier.Public;
                        break;

                    default:
                        MessageCollectionScope.AddMessage(new Message(
                            MessageType.EnumInvalidModifier,
                            span,
                            modifier.ToString().ToLower()
                            ));
                        break;
                }
            }

            return result;
        }

        public static TypeSymbol FromType(TypeDeclarationSyntax syntax, ContainerSymbol container) {
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            TypeKind kind;
            switch (syntax.Type) {
                case TypeDeclarationType.Class:
                    kind = TypeKind.Class;
                    break;
                case TypeDeclarationType.Struct:
                    kind = TypeKind.Struct;
                    break;
                default:
                    kind = TypeKind.Interface;
                    break;
            }

            var modifiers = MakeTypeModifiers(kind, syntax.Modifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;
            string metadataName = NameUtils.MakeMetadataName(name, syntax.TypeParameters);

            var other = FindOtherPartial(name, container, modifiers, syntax.Span);
            if (other != FindPartial.NotFound) {
                return other.Other;
            }

            var result = new TypeSymbol(
                kind,
                container,
                name,
                metadataName,
                modifiers
                );

            container.AddMember(result);

            return result;
        }

        private static TypeModifier MakeTypeModifiers(TypeKind kind, ImmutableArray<Modifier> modifiers, Span span) {
            var result = TypeModifier.None;

            foreach (var modifier in modifiers) {
                TypeModifier? typeModifier = null;

                switch (modifier) {
                    case Modifier.Abstract:
                        if (kind == TypeKind.Class) {
                            typeModifier = TypeModifier.Abstract;
                        }
                        break;

                    case Modifier.Internal:
                        typeModifier = TypeModifier.Internal;
                        break;

                    case Modifier.Partial:
                        typeModifier = TypeModifier.Partial;
                        break;

                    case Modifier.Public:
                        typeModifier = TypeModifier.Public;
                        break;

                    case Modifier.Readonly:
                        if (kind == TypeKind.Class) {
                            typeModifier = TypeModifier.Readonly;
                        }
                        break;

                    case Modifier.Sealed:
                        if (kind == TypeKind.Class) {
                            typeModifier = TypeModifier.Sealed;
                        }
                        break;

                    case Modifier.Static:
                        if (kind == TypeKind.Class) {
                            typeModifier = TypeModifier.Static;
                        }
                        break;
                }

                if (typeModifier == null) {
                    MessageType messageType;

                    switch (kind) {
                        case TypeKind.Class:
                            messageType = MessageType.ClassInvalidModifier;
                            break;
                        case TypeKind.Struct:
                            messageType = MessageType.StructInvalidModifier;
                            break;
                        default:
                            messageType = MessageType.InterfaceInvalidModifier;
                            break;
                    }

                    MessageCollectionScope.AddMessage(new Message(
                        messageType,
                        span,
                        modifier.ToString().ToLower()
                        ));
                } else {
                    result |= typeModifier.Value;
                }
            }

            return result;
        }

        public ImmutableArray<TypeParameterSymbol> TypeParameters { get; set; }

        private class FindPartial {
            public static readonly FindPartial NotFound = new FindPartial(null);
            public static readonly FindPartial Error = new FindPartial(null);

            public TypeSymbol Other { get; private set; }

            public FindPartial(TypeSymbol other) {
                Other = other;
            }
        }

        private static FindPartial FindOtherPartial(string name, ContainerSymbol container, TypeModifier modifiers, Span span) {
            var members = container.GetMemberByMetadataName(name);

            // If this is a partial type, find the other one and merge this with that.

            foreach (var member in members) {
                if (!(member is TypeSymbol)) {
                    continue;
                }

                var other = (TypeSymbol)member;

                // All declarations for this type must be partial for it to match.

                if (!modifiers.HasFlag(TypeModifier.Partial)) {
                    MessageCollectionScope.AddMessage(new Message(
                        MessageType.IdentifierAlreadyDefined,
                        span,
                        name
                        ));

                    return FindPartial.Error;
                }

                if (!other.Modifiers.HasFlag(TypeModifier.Partial)) {
                    MessageCollectionScope.AddMessage(new Message(
                        MessageType.TypeOtherNotPartial,
                        span,
                        name
                        ));

                    return FindPartial.Error;
                }

                // Add our modifiers to the existing type.
                other.Modifiers |= modifiers;

                // Add our span.
                other.AddSpan(span);

                return new FindPartial(other);
            }

            return FindPartial.NotFound;
        }
    }
}
