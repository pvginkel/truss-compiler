using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Binding;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public class NamedTypeSymbol : TypeSymbol {
        public static readonly NamedTypeSymbol TerminatorSymbol = new NamedTypeSymbol(TypeKind.Terminator, SymbolModifier.Public, null, null, null);

        private const SymbolModifier ValidAccessModifiers =
            SymbolModifier.Internal |
            SymbolModifier.Public |
            SymbolModifier.Private |
            SymbolModifier.Protected;
        private const SymbolModifier ValidDelegateModifiers =
            ValidAccessModifiers;
        private const SymbolModifier ValidEnumModifiers =
            ValidAccessModifiers |
            SymbolModifier.Partial;
        private const SymbolModifier ValidTypeModifiers =
            ValidAccessModifiers |
            SymbolModifier.Partial;
        private const SymbolModifier ValidClassModifiers =
            ValidTypeModifiers |
            SymbolModifier.Abstract |
            SymbolModifier.Sealed |
            SymbolModifier.Static |
            SymbolModifier.Readonly;
        private static readonly Enum[] AccessModifiers = {
            SymbolModifier.Public,
            SymbolModifier.Internal
        };

        private static SymbolModifier MakeModifiers(ErrorList errors, ImmutableArray<Modifier> modifiers, SymbolModifier validModifiers, Span span) {
            var result = SymbolModifier.None;

            foreach (var modifier in modifiers) {
                var symbolModifier = modifier.ToSymbolModifier();
                
                if ((symbolModifier & validModifiers) != 0) {
                    result |= symbolModifier;
                } else {
                    errors.Add(Error.InvalidModifier, span, modifier.ToString().ToLower());
                }
            }

            return result;
        }

        private readonly TypeKind _kind;

        private NamedTypeSymbol(TypeKind kind, SymbolModifier modifiers, string name, string metadataName, ContainerSymbol container)
            : base(name, metadataName, container) {
            _kind = kind;
            Modifiers = modifiers;
        }

        public void ParseModifiers(ErrorList errors) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }

            if (Modifiers.HasMultipleFlags(AccessModifiers)) {
                errors.Add(Error.DuplicateAccessModifier, Spans[0]);
                // Reset the access modifiers to not confuse the rest of the code.
                Modifiers &= ~ValidAccessModifiers;
            }

            // Private and protected modifiers are only allowed for nested types.

            if (
                !IsNested &&
                (Access == SymbolModifier.Private || Access == SymbolModifier.Protected)
            ) {
                errors.Add(Error.InvalidAccessModifiers, Spans[0]);
            }

            if ((Modifiers & ValidAccessModifiers) == 0) {
                // Default modifier.
                Modifiers |= IsNested ? SymbolModifier.Private : SymbolModifier.Internal;
            }

            if (IsAbstract && (IsStatic || IsSealed)) {
                errors.Add(Error.InvalidAbstractTypeCombination, Spans[0]);
            }
        }

        public override TypeKind TypeKind {
            get { return _kind; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNamedType(this);
            }
        }

        public SymbolModifier Modifiers { get; private set; }

        public SymbolModifier Access {
            get { return Modifiers & SymbolModifier.AccessModifiers; }
        }

        public bool IsStatic {
            get { return Modifiers.HasFlag(SymbolModifier.Static); }
        }

        public bool IsAbstract {
            get { return Modifiers.HasFlag(SymbolModifier.Abstract); }
        }

        public bool IsSealed {
            get { return Modifiers.HasFlag(SymbolModifier.Sealed); }
        }

        public bool IsReadonly {
            get { return Modifiers.HasFlag(SymbolModifier.Readonly); }
        }

        public override bool IsTracked {
            get { return Modifiers.HasFlag(SymbolModifier.Tracked); }
        }

        public bool IsNested {
            get { return Parent is NamedTypeSymbol; }
        }

        public ImmutableArray<TypeParameterSymbol> TypeParameters { get; private set; }

        public NamedTypeSymbol BaseType { get; internal set; }

        public ImmutableArray<TypeSymbol> Interfaces { get; internal set; } 

        public static NamedTypeSymbol FromDelegate(ErrorList errors, DelegateDeclarationSyntax syntax, ContainerSymbol container) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            var modifiers = MakeModifiers(errors, syntax.Modifiers, ValidDelegateModifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;
            string metadataName = NameUtils.GetGenericMetadataName(name, syntax.TypeParameters.Count);

            if (container.GetMemberByMetadataName(metadataName).Count > 0) {
                errors.Add(Error.IdentifierAlreadyDefined, syntax.Span, name);

                return null;
            }

            var result = new NamedTypeSymbol(TypeKind.Delegate, modifiers, name, metadataName, container);

            result.TypeParameters = MakeTypeParameters(errors, result, syntax.TypeParameters);

            container.AddMember(result);

            return result;
        }

        private static ImmutableArray<TypeParameterSymbol> MakeTypeParameters(ErrorList errors, NamedTypeSymbol container, ImmutableArray<TypeParameterSyntax> typeParameters) {
            var builder = new ImmutableArray<TypeParameterSymbol>.Builder();
            var seen = new HashSet<string>();

            foreach (var typeParameter in typeParameters) {
                string name = typeParameter.Identifier.Identifier;
                if (seen.Contains(name)) {
                    errors.Add(Error.DuplicateIdentifier, typeParameter.Span, name);
                    continue;
                }

                seen.Add(name);

                var symbol = new TypeParameterSymbol(typeParameter.Variance, name, name, container);

                container.AddMember(symbol);
                builder.Add(symbol);
            }

            return builder.Build();
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

            var modifiers = MakeModifiers(errors, syntax.Modifiers, ValidEnumModifiers, syntax.Span);
            string name = syntax.Identifier.Identifier;

            var other = FindOtherPartial(errors, name, container, modifiers, syntax.Span);
            if (other != FindPartial.NotFound) {
                return other.Other;
            }

            Symbol result = new NamedTypeSymbol(
                TypeKind.Enum,
                modifiers, name, name, container);

            container.AddMember(result);

            return result;
        }

        public static NamedTypeSymbol FromType(ErrorList errors, TypeDeclarationSyntax syntax, ContainerSymbol container) {
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

            var modifiers = MakeModifiers(
                errors,
                syntax.Modifiers,
                kind == TypeKind.Class ? ValidClassModifiers : ValidTypeModifiers,
                syntax.Span
            );
            string name = syntax.Identifier.Identifier;
            string metadataName = NameUtils.GetGenericMetadataName(name, syntax.TypeParameters.Count);

            NamedTypeSymbol result;

            var other = FindOtherPartial(errors, name, container, modifiers, syntax.Span);
            if (other == FindPartial.Error) {
                return null;
            }
            if (other == FindPartial.NotFound) {
                result = new NamedTypeSymbol(kind, modifiers, name, metadataName, container);
            
                container.AddMember(result);
            } else {
                result = other.Other;
            }

            if (result.TypeParameters != null && result.TypeParameters.Count > 0 && syntax.TypeParameters.Count > 0) {
                errors.Add(Error.TypeParametersAlreadyDeclared, syntax.Span);
            } else {
                result.TypeParameters = MakeTypeParameters(errors, result, syntax.TypeParameters);
            }

            return result;
        }

        private class FindPartial {
            public static readonly FindPartial NotFound = new FindPartial(null);
            public static readonly FindPartial Error = new FindPartial(null);

            public NamedTypeSymbol Other { get; private set; }

            public FindPartial(NamedTypeSymbol other) {
                Other = other;
            }
        }

        private static FindPartial FindOtherPartial(ErrorList errors, string name, ContainerSymbol container, SymbolModifier modifiers, Span span) {
            NamedTypeSymbol other;
            if (!container.TryGetMemberByMetadataName(name, out other)) {
                return FindPartial.NotFound;
            }

            // If this is a partial type, find the other one and merge this with that.
            // All declarations for this type must be partial for it to match.

            if (!modifiers.HasFlag(SymbolModifier.Partial)) {
                errors.Add(Error.IdentifierAlreadyDefined, span, name);

                return FindPartial.Error;
            }

            if (!other.Modifiers.HasFlag(SymbolModifier.Partial)) {
                errors.Add(Error.TypeOtherNotPartial, span, name);

                return FindPartial.Error;
            }

            // Add our modifiers to the existing type.
            other.Modifiers |= modifiers;

            return new FindPartial(other);
        }
    }
}
