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

        public void ParseModifiers(ErrorList errors) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }

            IsAbstract = Modifiers.HasFlag(TypeModifier.Abstract);
            IsReadonly = Modifiers.HasFlag(TypeModifier.Readonly);
            IsSealed = Modifiers.HasFlag(TypeModifier.Sealed);
            IsStatic = Modifiers.HasFlag(TypeModifier.Static);

            if (Modifiers.HasFlag(TypeModifier.Internal) && Modifiers.HasFlag(TypeModifier.Public)) {
                errors.Add(Error.DuplicateAccessModifier, Spans[0]);
            } else if (Modifiers.HasFlag(TypeModifier.Public)) {
                Access = AccessModifier.Public;
            } else {
                // Default modifier on types.
                Access = AccessModifier.Internal;
            }

            if (IsAbstract && (IsStatic || IsSealed)) {
                errors.Add(Error.InvalidAbstractTypeCombination, Spans[0]);
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

        public static TypeSymbol FromDelegate(ErrorList errors, DelegateDeclarationSyntax syntax, ContainerSymbol container) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            var modifiers = MakeDelegateModifiers(errors, syntax.Modifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;
            string metadataName = NameUtils.MakeMetadataName(name, syntax.TypeParameters);

            if (container.GetMemberByMetadataName(metadataName).Count > 0) {
                errors.Add(Error.IdentifierAlreadyDefined, syntax.Span, name);

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

        private static TypeModifier MakeDelegateModifiers(ErrorList errors, ImmutableArray<Modifier> modifiers, Span span) {
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
                        errors.Add(Error.DelegateInvalidModifier, span, modifier.ToString().ToLower());
                        break;
                }
            }

            return result;
        }

        public static Symbol FromEnum(ErrorList errors, EnumDeclarationSyntax syntax, ContainerSymbol container) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            var modifiers = MakeEnumModifiers(errors, syntax.Modifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;

            var other = FindOtherPartial(errors, name, container, modifiers, syntax.Span);
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

        private static TypeModifier MakeEnumModifiers(ErrorList errors, ImmutableArray<Modifier> modifiers, Span span) {
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
                        errors.Add(Error.EnumInvalidModifier, span, modifier.ToString().ToLower());
                        break;
                }
            }

            return result;
        }

        public static TypeSymbol FromType(ErrorList errors, TypeDeclarationSyntax syntax, ContainerSymbol container) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
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

            var modifiers = MakeTypeModifiers(errors, kind, syntax.Modifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;
            string metadataName = NameUtils.MakeMetadataName(name, syntax.TypeParameters);

            var other = FindOtherPartial(errors, name, container, modifiers, syntax.Span);
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

        private static TypeModifier MakeTypeModifiers(ErrorList errors, TypeKind kind, ImmutableArray<Modifier> modifiers, Span span) {
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
                    ErrorType error;

                    switch (kind) {
                        case TypeKind.Class:
                            error = Error.ClassInvalidModifier;
                            break;
                        case TypeKind.Struct:
                            error = Error.StructInvalidModifier;
                            break;
                        default:
                            error = Error.InterfaceInvalidModifier;
                            break;
                    }

                    errors.Add(error, span, modifier.ToString().ToLower());
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

        private static FindPartial FindOtherPartial(ErrorList errors, string name, ContainerSymbol container, TypeModifier modifiers, Span span) {
            var members = container.GetMemberByMetadataName(name);

            // If this is a partial type, find the other one and merge this with that.

            foreach (var member in members) {
                if (!(member is TypeSymbol)) {
                    continue;
                }

                var other = (TypeSymbol)member;

                // All declarations for this type must be partial for it to match.

                if (!modifiers.HasFlag(TypeModifier.Partial)) {
                    errors.Add(Error.IdentifierAlreadyDefined, span, name);

                    return FindPartial.Error;
                }

                if (!other.Modifiers.HasFlag(TypeModifier.Partial)) {
                    errors.Add(Error.TypeOtherNotPartial, span, name);

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
