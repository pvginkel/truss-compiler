using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal class MetadataSymbolBinder : SyntaxTreeWalker {
        // These modifiers specify combinations that are invalid as method modifiers.
        private const SymbolModifier InvalidStaticModifiers =
            SymbolModifier.Abstract |
            SymbolModifier.Override |
            SymbolModifier.Sealed |
            SymbolModifier.Virtual;
        private const SymbolModifier InvalidAbstractModifiers =
            SymbolModifier.Async |
            SymbolModifier.Sealed |
            SymbolModifier.Virtual;
        private const SymbolModifier InvalidExternModifiers =
            SymbolModifier.Abstract |
            SymbolModifier.Async |
            SymbolModifier.Override |
            SymbolModifier.Virtual;

        // These modifiers specify what modifiers are valid on certain constructs.
        private const SymbolModifier InheritanceModifiers =
            SymbolModifier.Abstract |
            SymbolModifier.Override |
            SymbolModifier.Virtual |
            SymbolModifier.Sealed;
        private const SymbolModifier ValidAccessorModifiers =
            SymbolModifier.AccessModifiers;
        private const SymbolModifier ValidEventModifiers =
            InheritanceModifiers |
            SymbolModifier.AccessModifiers |
            SymbolModifier.New |
            SymbolModifier.Readonly |
            SymbolModifier.Static;
        private const SymbolModifier ValidEventFieldModifiers =
            InheritanceModifiers |
            SymbolModifier.AccessModifiers |
            SymbolModifier.New |
            SymbolModifier.Static;
        private const SymbolModifier ValidFieldModifiers =
            SymbolModifier.AccessModifiers |
            SymbolModifier.New |
            SymbolModifier.Readonly |
            SymbolModifier.Static |
            SymbolModifier.Volatile;
        private const SymbolModifier ValidConstructorModifiers =
            SymbolModifier.AccessModifiers |
            SymbolModifier.Extern |
            SymbolModifier.Static;
        private const SymbolModifier ValidDestructorModifiers =
            SymbolModifier.None;
        private const SymbolModifier ValidMethodModifiers =
            InheritanceModifiers |
            SymbolModifier.AccessModifiers |
            SymbolModifier.Async |
            SymbolModifier.Extern |
            SymbolModifier.New |
            SymbolModifier.Static;
        private const SymbolModifier ValidOperatorModifiers =
            SymbolModifier.AccessModifiers |
            SymbolModifier.Extern |
            SymbolModifier.Static;
        private const SymbolModifier ValidPropertyModifiers =
            InheritanceModifiers |
            SymbolModifier.AccessModifiers |
            SymbolModifier.New |
            SymbolModifier.Static;

        // Used for validating whether conflicting access modifiers are specified.
        private static readonly Enum[] AccessModifiers = {
            SymbolModifier.Internal,
            SymbolModifier.Public,
            SymbolModifier.Private,
            SymbolModifier.Protected 
        };

        private readonly ErrorList _errors;
        private readonly SymbolManager _manager;
        private Scope _scope;
        private GlobalScope _globalScope;
        private ContainerScope _containerScope;
        private NamedTypeSymbol _objectTypeSymbol;
        private NamedTypeSymbol _methodHandleTypeSymbol;
        private NamedTypeSymbol _voidTypeSymbol;
        private NamedTypeSymbol _int32TypeSymbol;
        private NamedTypeSymbol _delegateTypeSymbol;
        private readonly HashSet<NamedTypeSymbol> _legalEnumBaseTypes = new HashSet<NamedTypeSymbol>(); 
        private readonly HashSet<NamedTypeSymbol> _illegalTypeConstraints = new HashSet<NamedTypeSymbol>(); 

        public MetadataSymbolBinder(ErrorList errors, SymbolManager manager) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
            if (manager == null) {
                throw new ArgumentNullException("manager");
            }

            _errors = errors;
            _manager = manager;
        }

        private SymbolModifier MakeModifiers(ImmutableArray<Modifier> modifiers, SymbolModifier validModifiers, Span span) {
            var result = SymbolModifier.None;

            foreach (var modifier in modifiers) {
                var symbolModifier = modifier.ToSymbolModifier();
                if ((symbolModifier & validModifiers) == 0) {
                    _errors.Add(Error.InvalidModifier, span);
                } else {
                    result |= symbolModifier;
                }
            }

            // Validate access modifiers. These are mutual exclusive.

            if (result.HasMultipleFlags(AccessModifiers)) {
                _errors.Add(Error.DuplicateAccessModifier, span);
                // Clear the access modifiers to not have the rest choke.
                result &= ~SymbolModifier.AccessModifiers;
            }

            // Set the default access modifier. These methods are not used for type modifiers, so the default is
            // private.

            Debug.Assert(validModifiers.HasFlag(SymbolModifier.Private));

            if ((result & SymbolModifier.AccessModifiers) == 0) {
                result |= SymbolModifier.Private;
            }

            return result;
        }

        private SymbolModifier MakeMethodModifiers(ImmutableArray<Modifier> modifiers, SymbolModifier validModifiers, Span span) {
            var result = MakeModifiers(modifiers, validModifiers, span);

            ValidateMethodModifiers(result, span);

            return result;
        }

        private void SetScope(Scope scope) {
            _scope = scope;
            _containerScope = scope as ContainerScope;
        }

        private void PopScope() {
            _scope = _scope.Parent;
        }

        private void PrefetchTypes() {
            _objectTypeSymbol = ResolvePredefinedTypeSymbol(PredefinedType.Object, Span.Empty);
            _voidTypeSymbol = ResolvePredefinedTypeSymbol(PredefinedType.Void, Span.Empty);
            _int32TypeSymbol = ResolvePredefinedTypeSymbol(PredefinedType.Int, Span.Empty);
            _methodHandleTypeSymbol = ResolveSpecialTypeSymbol(SpecialType.System_Reflection_MethodHandle, Span.Empty);
            _delegateTypeSymbol = ResolveSpecialTypeSymbol(SpecialType.System_Delegate, Span.Empty);

            Debug.Assert(_legalEnumBaseTypes.Count == 0, "MetadataSymbolBinder should not be reused");

            _legalEnumBaseTypes.Add(ResolvePredefinedTypeSymbol(PredefinedType.Byte, Span.Empty));
            _legalEnumBaseTypes.Add(ResolvePredefinedTypeSymbol(PredefinedType.SByte, Span.Empty));
            _legalEnumBaseTypes.Add(ResolvePredefinedTypeSymbol(PredefinedType.Short, Span.Empty));
            _legalEnumBaseTypes.Add(ResolvePredefinedTypeSymbol(PredefinedType.UShort, Span.Empty));
            _legalEnumBaseTypes.Add(_int32TypeSymbol);
            _legalEnumBaseTypes.Add(ResolvePredefinedTypeSymbol(PredefinedType.UInt, Span.Empty));
            _legalEnumBaseTypes.Add(ResolvePredefinedTypeSymbol(PredefinedType.Long, Span.Empty));
            _legalEnumBaseTypes.Add(ResolvePredefinedTypeSymbol(PredefinedType.ULong, Span.Empty));

            _illegalTypeConstraints.Add(ResolveSpecialTypeSymbol(SpecialType.System_Array, Span.Empty));
            _illegalTypeConstraints.Add(_delegateTypeSymbol);
            _illegalTypeConstraints.Add(ResolveSpecialTypeSymbol(SpecialType.System_Enum, Span.Empty));
            _illegalTypeConstraints.Add(ResolveSpecialTypeSymbol(SpecialType.System_ValueType, Span.Empty));
            _illegalTypeConstraints.Add(_objectTypeSymbol);
        }

        private List<Import> ResolveImports(IEnumerable<ImportDirectiveSyntax> imports) {
            var result = new List<Import>();

            // Resolve all imports for the global scope or a namespace. This resolves the imports and
            // results in Import instances which are attached to the namespace scope. Note that imports
            // are always matched against the global scope; not the current namespace.

            foreach (var import in imports) {
                ImportType? type = null;
                ContainerSymbol container = null;

                if (import.IsStatic) {
                    if (import.Alias != null) {
                        _errors.Add(Error.StaticImportCannotHaveAlias, import.Span);
                    } else {
                        container = _globalScope.ResolveContainer(import.Name, ResolveMode.Type);
                        type = ImportType.Static;
                    }
                } else {
                    // We only allow types in a normal import when an alias was provided. To import
                    // a type without an alias, static has to be specified.

                    container = _globalScope.ResolveContainer(
                        import.Name,
                        import.Alias == null ? ResolveMode.Namespace : ResolveMode.TypeOrNamespace
                        );
                    if (container is NamedTypeSymbol) {
                        type = ImportType.Type;
                    } else {
                        type = ImportType.Namespace;
                    }
                }

                if (container != null) {
                    result.Add(new Import(import.Alias.Identifier, container, type.Value));
                }
            }

            return result;
        }

        private Tuple<NamedTypeSymbol, ImmutableArray<TypeSymbol>> ResolveBaseTypes(ImmutableArray<TypeSyntax> types, TypeKind kind) {
            NamedTypeSymbol baseType = null;
            var builder = new ImmutableArray<TypeSymbol>.Builder();

            for (int i = 0; i < types.Count; i++) {
                var type = ResolveType(types[i]) as NamedTypeSymbol;
                if (type == null) {
                    _errors.Add(Error.ExpectedType, type.Spans);
                    continue;
                }

                if (type.TypeKind != TypeKind.Interface) {
                    if (i == 0 && (kind == TypeKind.Enum || kind == TypeKind.Class)) {
                        if (kind == TypeKind.Enum) {
                            if (!_legalEnumBaseTypes.Contains(type)) {
                                _errors.Add(Error.InvalidEnumBaseType, types[i].Span);
                            } else {
                                baseType = type;
                            }
                        } else if (type.TypeKind != TypeKind.Class) {
                            _errors.Add(Error.IllegalClassBaseType, types[i].Span);
                        } else {
                            baseType = type;
                        }
                    } else {
                        _errors.Add(Error.ExpectedInterfaceBaseType, types[i].Span);
                    }
                } else {
                    builder.Add(type);
                }
            }

            return Tuple.Create(baseType, builder.Build());
        }

        private TypeSymbol ResolveType(TypeSyntax type) {
            switch (type.Kind) {
                case SyntaxKind.PredefinedType:
                    return ResolvePredefinedTypeSymbol(type);

                case SyntaxKind.ArrayType:
                    return ResolveArrayTypeSymbol((ArrayTypeSyntax)type);

                case SyntaxKind.NullableType:
                    return ResolveNullableTypeSymbol((NullableTypeSyntax)type);

                case SyntaxKind.NakedNullableType:
                    return ((NakedNullableTypeSyntax)type).Type == Nullability.Nullable
                        ? NakedNullableTypeSymbol.Nullable
                        : NakedNullableTypeSymbol.NotNullable;

                case SyntaxKind.TrackedType:
                    return ResolveTrackedTypeSymbol((TrackedTypeSyntax)type);

                case SyntaxKind.VarType:
                    return VarTypeSymbol.Instance;

                default:
                    Debug.Assert(type is NameSyntax);

                   return (NamedTypeSymbol)_scope.ResolveContainer((NameSyntax)type, ResolveMode.Type);
            }
        }

        private TypeSymbol ResolveTrackedTypeSymbol(TrackedTypeSyntax type) {
            return ResolveTrackedTypeSymbol(ResolveType(type.ElementType));
        }

        private static TypeSymbol ResolveTrackedTypeSymbol(TypeSymbol symbol) {
            string metadataName = NameUtils.GetTrackedName(symbol, NameType.Metadata);

            TypeSymbol result;
            if (symbol.Parent.TryGetMemberByMetadataName(metadataName, out result)) {
                return result;
            }

            symbol = new TrackedTypeSymbol(
                symbol,
                NameUtils.GetTrackedName(symbol, NameType.Normal),
                metadataName,
                symbol.Parent
            );

            symbol.Parent.AddMember(symbol);

            return symbol;
        }

        private NamedTypeSymbol ResolvePredefinedTypeSymbol(TypeSyntax type) {
            return ResolvePredefinedTypeSymbol(((PredefinedTypeSyntax)type).PredefinedType, type.Span);
        }

        private NamedTypeSymbol ResolvePredefinedTypeSymbol(PredefinedType predefinedType, Span span) {
            return (NamedTypeSymbol)_globalScope.ResolveContainer(
                NameUtils.BuildPredefinedTypeName(predefinedType, span),
                ResolveMode.Type
            );
        }

        private NamedTypeSymbol ResolveSpecialTypeSymbol(SpecialType type, Span span) {
            return (NamedTypeSymbol)_globalScope.ResolveContainer(
                NameUtils.BuildSpecialTypeName(type, span),
                ResolveMode.Type
            );
        }

        private TypeSymbol ResolveArrayTypeSymbol(ArrayTypeSyntax type) {
            TypeSymbol symbol = ResolveType(type.ElementType);

            foreach (var rankSpecifier in type.RankSpecifiers) {
                if (rankSpecifier.Size.Kind != SyntaxKind.OmittedArraySizeExpression) {
                    _errors.Add(Error.UnexpectedArraySizeExpression, rankSpecifier.Span);
                }

                string metadataName = NameUtils.GetArrayName(symbol, NameType.Metadata);

                TypeSymbol result;
                if (symbol.Parent.TryGetMemberByMetadataName(metadataName, out result)) {
                    symbol = result;
                } else {
                    symbol = new ArrayTypeSymbol(
                        symbol,
                        NameUtils.GetArrayName(symbol, NameType.Normal),
                        metadataName,
                        symbol.Parent
                    );

                    symbol.Parent.AddMember(symbol);
                }

                if (rankSpecifier.IsTracked) {
                    symbol = ResolveTrackedTypeSymbol(symbol);
                }
            }

            return symbol;
        }

        private TypeSymbol ResolveNullableTypeSymbol(NullableTypeSyntax type) {
            var elementType = ResolveType(type.ElementType);
            string metadataName = NameUtils.GetNullableName(elementType, NameType.Metadata);

            TypeSymbol symbol;
            if (elementType.Parent.TryGetMemberByMetadataName(metadataName, out symbol)) {
                return symbol;
            }

            // Because of how the type system works, we cannot convert the type to a Nullable<> here already. The
            // fun part of this is that a Nullable<> type is not recognized as a nullable type.

            // TODO: Convert Nullable<> types to NullableTypeSymbol's.

            symbol = new NullableTypeSymbol(
                elementType,
                NameUtils.GetNullableName(elementType, NameType.Normal),
                metadataName,
                elementType.Parent
            );

            elementType.Parent.AddMember(symbol);

            return symbol;
        }

        private void ValidateMethodModifiers(SymbolModifier modifiers, Span span) {
            if (modifiers.HasMultipleFlags(SymbolModifier.Public, SymbolModifier.Private, SymbolModifier.Protected, SymbolModifier.Internal)) {
                _errors.Add(Error.InvalidAccessModifiers, span);
            }
            if (modifiers.HasFlag(SymbolModifier.Abstract) && (modifiers & InvalidAbstractModifiers) != 0) {
                _errors.Add(Error.InvalidAbstractModifier, span);
            }
            if (modifiers.HasFlag(SymbolModifier.Extern) && (modifiers & InvalidExternModifiers) != 0) {
                _errors.Add(Error.InvalidExternModifier, span);
            }
            if (modifiers.HasFlag(SymbolModifier.Static) && (modifiers & InvalidStaticModifiers) != 0) {
                _errors.Add(Error.InvalidStaticModifier, span);
            }
        }

        public override void VisitCompilationUnit(CompilationUnitSyntax syntax) {
            _globalScope = new GlobalScope(_errors, _manager.GlobalSymbol, ResolveImports(syntax.Imports));

            SetScope(_globalScope);

            PrefetchTypes();

            base.VisitCompilationUnit(syntax);

            PopScope();
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) {
            SetScope(new NamespaceScope(
                (NamespaceSymbol)_manager.Find(syntax),
                ResolveImports(syntax.Imports),
                _scope
            ));

            base.VisitNamespaceDeclaration(syntax);

            PopScope();
        }

        public override void VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            var scope = new TypeScope(
                (NamedTypeSymbol)_manager.Find(syntax),
                _scope
            );

            SetScope(scope);

            // Resolve the base types.

            var bases = ResolveBaseTypes(syntax.BaseTypes, scope.Symbol.TypeKind);
            var baseType = bases.Item1;
            if (scope.Symbol == _objectTypeSymbol) {
                Debug.Assert(baseType == null);
                baseType = NamedTypeSymbol.TerminatorSymbol;
            } else if (baseType == null) {
                baseType = _objectTypeSymbol;
            }

            scope.Symbol.BaseType = baseType;
            scope.Symbol.Interfaces = bases.Item2;

            // Process the type parameter constraints.

            ProcessConstraints(scope.Symbol.TypeParameters, syntax.ConstraintClauses);

            VisitList(syntax.Members);

            PopScope();
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            var scope = new TypeScope(
                (NamedTypeSymbol)_manager.Find(syntax),
                _scope
            );

            SetScope(scope);

            // Resolve the base types.

            var bases = ResolveBaseTypes(syntax.BaseTypes, scope.Symbol.TypeKind);

            scope.Symbol.BaseType = bases.Item1 ?? _int32TypeSymbol;
            scope.Symbol.Interfaces = bases.Item2;

            VisitList(syntax.Members);

            PopScope();
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_scope.Symbol;

            var enumField = new EnumFieldSymbol(
                type,
                SymbolModifier.Public | SymbolModifier.Static | SymbolModifier.Readonly,
                type.BaseType,
                syntax.Identifier.Identifier
            );

            _manager.Add(syntax, enumField);

            type.AddMember(enumField);
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            var scope = new TypeScope(
                (NamedTypeSymbol)_manager.Find(syntax),
                _scope
            );

            SetScope(scope);

            ProcessConstraints(scope.Symbol.TypeParameters, syntax.ConstraintClauses);

            // Add the constructor and invoke method.

            var constructorSymbol = new ConstructorSymbol(
                scope.Symbol,
                SymbolModifier.Public,
                scope.Symbol
            );

            constructorSymbol.Parameters = ImmutableArray<ParameterSymbol>.Create(
                new ParameterSymbol(
                    ParameterModifier.None,
                    WellKnownNames.DelegateConstructorObjectParameter,
                    _objectTypeSymbol
                ),
                new ParameterSymbol(
                    ParameterModifier.None,
                    WellKnownNames.DelegateConstructorMethodParameter,
                    _methodHandleTypeSymbol
                )
            );

            scope.Symbol.AddMember(constructorSymbol);

            var methodSymbol = new MemberMethodSymbol(
                WellKnownNames.DelegateInvokeMethod,
                WellKnownNames.DelegateInvokeMethod,
                null,
                scope.Symbol,
                SymbolModifier.Public,
                ResolveType(syntax.ReturnType)
            );

            methodSymbol.Parameters = BuildParameters(methodSymbol, syntax.Parameters);

            scope.Symbol.AddMember(methodSymbol);

            PopScope();
        }

        private void ProcessConstraints(ImmutableArray<TypeParameterSymbol> typeParameters, ImmutableArray<TypeParameterConstraintClauseSyntax> constraintClauses) {
            var seen = new HashSet<string>();

            foreach (var clause in constraintClauses) {
                var name = clause.Name.Identifier;

                if (seen.Contains(name)) {
                    _errors.Add(Error.DuplicateTypeParameterConstraint, clause.Span);
                    continue;
                }

                seen.Add(name);

                TypeParameterSymbol typeParameter = typeParameters.SingleOrDefault(p => p.Name == name);
                if (typeParameter == null) {
                    _errors.Add(Error.UnresolvedIdentifier, clause.Name.Span);
                    continue;
                }

                var family = TypeFamily.Any;
                var nullability = Nullability.NotNullable;
                bool requireDefaultConstructor = false;
                TypeSymbol baseTypeConstraint = _objectTypeSymbol;
                var interfaceConstraints = new ImmutableArray<TypeSymbol>.Builder();
                bool hadTypeConstraint = false;

                for (int i = 0; i < clause.Constraints.Count; i++) {
                    var constraint = clause.Constraints[i];

                    switch (constraint.Kind) {
                        case SyntaxKind.ConstructorConstraint:
                            if (i != clause.Constraints.Count - 1) {
                                _errors.Add(Error.InvalidConstructorConstraintPosition, constraint.Span);
                                continue;
                            }

                            requireDefaultConstructor = true;
                            break;

                        case SyntaxKind.TypeConstraint:
                            var typeConstraint = (TypeConstraintSyntax)constraint;

                            var type = ResolveType(typeConstraint.ConstrainedType);

                            if (_illegalTypeConstraints.Contains(type)) {
                                _errors.Add(Error.IllegalTypeForTypeConstraint, constraint.Span);
                            } else if (type.TypeKind != TypeKind.Class && type.TypeKind != TypeKind.Interface) {
                                _errors.Add(Error.TypeParameterConstraintNotClassOrInterface, constraint.Span);
                            } else if (type.TypeKind == TypeKind.Class) {
                                if (hadTypeConstraint) {
                                    _errors.Add(Error.InvalidClassTypeConstraintPosition, constraint.Span);
                                } else {
                                    baseTypeConstraint = type;
                                }
                            } else {
                                interfaceConstraints.Add(type);
                            }

                            hadTypeConstraint = true;
                            break;

                        case SyntaxKind.TypeFamilyConstraint:
                            if (i != 0) {
                                _errors.Add(Error.InvalidTypeFamilyPosition, constraint.Span);
                                continue;
                            }

                            var familyConstraint = (TypeFamilyConstraintSyntax)constraint;

                            family = familyConstraint.Family;
                            if (family == TypeFamily.Tracked) {
                                // Tracked type are treated as nullable. It is not possible to overrule this.
                                nullability = Nullability.Nullable;
                            } else {
                                // This is verified by TypeFamilyConstraintSyntax.
                                Debug.Assert(familyConstraint.Nullability.HasValue);
                                nullability = familyConstraint.Nullability.Value;
                            }
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }

                typeParameter.SetConstraints(
                    family,
                    nullability,
                    requireDefaultConstructor,
                    baseTypeConstraint,
                    interfaceConstraints.Build()
                );
            }

            // Setup defaults for all type parameters we didn't get specific constraints for.

            foreach (var typeParameter in typeParameters) {
                if (!seen.Contains(typeParameter.Name)) {
                    typeParameter.SetConstraints(
                        TypeFamily.Any,
                        Nullability.NotNullable,
                        false,
                        _objectTypeSymbol,
                        ImmutableArray<TypeSymbol>.Empty
                    );
                }
            }
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax) {
            var parent = (NamedTypeSymbol)_containerScope.Symbol;

            if (syntax.Identifier.Identifier != parent.Name) {
                // This isn't really a constructor, but rather a method without a return type. Bail.
                _errors.Add(Error.MethodMissingReturnType, syntax.Span);
                return;
            }

            var symbol = new ConstructorSymbol(
                parent,
                MakeMethodModifiers(syntax.Modifiers, ValidConstructorModifiers, syntax.Span),
                parent
            );

            parent.AddMember(symbol);
            _manager.Add(syntax, symbol);

            SetScope(new MethodScope(symbol, _scope));

            symbol.Parameters = BuildParameters(symbol, syntax.Parameters);

            PopScope();
        }

        public override void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) {
            var parent = (NamedTypeSymbol)_containerScope.Symbol;

            var symbol = new ConversionOperatorSymbol(
                syntax.Type,
                parent,
                MakeMethodModifiers(syntax.Modifiers, ValidOperatorModifiers, syntax.Span),
                ResolveType(syntax.TargetType)
            );

            parent.AddMember(symbol);
            _manager.Add(syntax, symbol);

            SetScope(new MethodScope(symbol, _scope));

            symbol.Parameters = BuildParameters(symbol, syntax.Parameters);

            PopScope();
        }

        public override void VisitOperatorDeclaration(OperatorDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_containerScope.Symbol;

            var symbol = new OperatorSymbol(
                syntax.Operator,
                type,
                MakeMethodModifiers(syntax.Modifiers, ValidOperatorModifiers, syntax.Span),
                ResolveType(syntax.ReturnType)
            );

            type.AddMember(symbol);
            _manager.Add(syntax, symbol);

            SetScope(new MethodScope(symbol, _scope));

            symbol.Parameters = BuildParameters(symbol, syntax.Parameters);

            PopScope();
        }

        public override void VisitDestructorDeclaration(DestructorDeclarationSyntax syntax) {
            var parent = (NamedTypeSymbol)_containerScope.Symbol;

            var symbol = new DestructorSymbol(
                parent,
                MakeMethodModifiers(syntax.Modifiers, ValidDestructorModifiers, syntax.Span),
                _voidTypeSymbol
            );

            parent.AddMember(symbol);
            _manager.Add(syntax, symbol);

            if (syntax.Parameters.Count > 0) {
                _errors.Add(Error.DestructorCannotDeclareParameters, syntax.Span);
            }
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_containerScope.Symbol;

            if (type.TypeKind == TypeKind.Interface) {
                if (syntax.Modifiers.Count > 0) {
                    _errors.Add(Error.InvalidModifier, syntax.Span);
                    return;
                }
                if (syntax.Body != null) {
                    _errors.Add(Error.UnexpectedSyntax, syntax.Body.Span);
                    return;
                }
            }

            var modifiers = MakeMethodModifiers(syntax.Modifiers, ValidMethodModifiers, syntax.Span);

            if ((syntax.Body != null) != ((modifiers & (SymbolModifier.Extern | SymbolModifier.Abstract)) != 0)) {
                if (syntax.Body == null) {
                    _errors.Add(Error.ExpectedBody, syntax.Span);
                } else {
                    _errors.Add(Error.UnexpectedSyntax, syntax.Body.Span);
                }
                return;
            }

            string name = syntax.Identifier.Identifier;

            var symbol = new MemberMethodSymbol(
                name,
                NameUtils.GetGenericMetadataName(name, syntax.TypeParameters.Count),
                syntax.ExplicitInterfaceSpecifier == null ? null : ResolveType(syntax.ExplicitInterfaceSpecifier),
                type,
                modifiers,
                ResolveType(syntax.ReturnType)
            );

            type.AddMember(symbol);
            _manager.Add(syntax, symbol);

            SetScope(new MethodScope(symbol, _scope));

            ProcessConstraints(symbol.TypeParameters, syntax.ConstraintClauses);

            symbol.Parameters = BuildParameters(symbol, syntax.Parameters);

            PopScope();
        }

        private ImmutableArray<ParameterSymbol> BuildParameters(MethodSymbol method, ImmutableArray<ParameterSyntax> parameters) {
            var builder = new ImmutableArray<ParameterSymbol>.Builder();
            var seen = new HashSet<string>();

            for (int i = 0; i < parameters.Count; i++) {
                var parameter = parameters[i];
                var name = parameter.Identifier.Identifier;

                if (seen.Contains(name)) {
                    _errors.Add(Error.DuplicateIdentifier, parameter.Span, name);
                    continue;
                }

                seen.Add(name);

                var type = ResolveType(parameter.ParameterType);

                var modifiers = MakeParameterModifiers(
                    parameter.Modifiers,
                    type,
                    i == 0 /* isFirst */,
                    i == parameters.Count - 1 /* isLast */,
                    method,
                    parameter.Span
                );

                var symbol = new ParameterSymbol(modifiers, name, type);

                _manager.Add(parameter, symbol);
                builder.Add(symbol);
            }

            return builder.Build();
        }

        private ParameterModifier MakeIndexerParameterModifiers(ImmutableArray<Syntax.ParameterModifier> modifiers, TypeSymbol type, bool isLast, Span span) {
            var result = ParameterModifier.None;

            foreach (var modifier in modifiers) {
                result |= ParseIndexerParameterModifier(type, isLast, span, modifier);
            }

            return result;
        }

        private ParameterModifier ParseIndexerParameterModifier(TypeSymbol type, bool isLast, Span span, Syntax.ParameterModifier modifier) {
            switch (modifier) {
                case Syntax.ParameterModifier.Consumes:
                    if (!type.IsTracked) {
                        _errors.Add(Error.CannotConsumeUntrackedType, span);
                    }
                    return ParameterModifier.Consumes;

                case Syntax.ParameterModifier.Out:
                    return ParameterModifier.Out;

                case Syntax.ParameterModifier.Params:
                    if (!isLast) {
                        _errors.Add(Error.ParamsMustBeLastParameter, span);
                    }
                    if (!type.IsArray) {
                        _errors.Add(Error.ParamsParameterMustBeArray, span);
                    }
                    return ParameterModifier.Params;

                case Syntax.ParameterModifier.Ref:
                    return ParameterModifier.Ref;

                default:
                    throw new InvalidOperationException();
            }
        }

        private ParameterModifier MakeParameterModifiers(ImmutableArray<Syntax.ParameterModifier> modifiers, TypeSymbol type, bool isFirst, bool isLast, MethodSymbol method, Span span) {
            var result = ParameterModifier.None;

            foreach (var modifier in modifiers) {
                switch (modifier) {
                    case Syntax.ParameterModifier.This:
                        if (!isFirst) {
                            _errors.Add(Error.ThisMustBeFirstParameter, span);
                        }
                        if (!method.IsStatic) {
                            _errors.Add(Error.ExtensionMethodMustBeStatic, span);
                        }
                        if (!method.DeclaringType.IsStatic) {
                            _errors.Add(Error.ExtensionMethodOnlyValidOnStaticMethod, span);
                        }
                        result |= ParameterModifier.This;
                        break;

                    default:
                        result |= ParseIndexerParameterModifier(type, isLast, span, modifier);
                        break;
                }
            }

            switch (method.MethodKind) {
                case MethodKind.MemberMethod:
                    // All modifiers are legal on methods.
                    break;

                case MethodKind.Constructor:
                    if (result.HasFlag(ParameterModifier.This)) {
                        _errors.Add(Error.InvalidModifier, span);
                    }
                    break;

                case MethodKind.ConversionOperator:
                case MethodKind.Operator:
                    if (result != ParameterModifier.None) {
                        _errors.Add(Error.InvalidModifier, span);
                    }
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return result;
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_scope.Symbol;

            var modifiers = MakeModifiers(syntax.Modifiers, ValidEventFieldModifiers, syntax.Span);
            var eventType = ResolveType(syntax.Declaration.VariableType);

            if (eventType.TypeKind != TypeKind.Delegate) {
                _errors.Add(Error.EventTypeMustBeDelegate, syntax.Declaration.VariableType.Span);
                return;
            }

            foreach (var variable in syntax.Declaration.Variables) {
                var symbol = new EventFieldSymbol(type, modifiers, eventType, variable.Identifier.Identifier);

                type.AddMember(symbol);
                _manager.Add(variable, symbol);
            }
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_scope.Symbol;

            var modifiers = MakeModifiers(syntax.Modifiers, ValidFieldModifiers, syntax.Span);
            var eventType = ResolveType(syntax.Declaration.VariableType);

            foreach (var variable in syntax.Declaration.Variables) {
                var symbol = new MemberFieldSymbol(type, modifiers, eventType, variable.Identifier.Identifier);

                type.AddMember(symbol);
                _manager.Add(variable, symbol);
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_scope.Symbol;

            var modifiers = MakeModifiers(syntax.Modifiers, ValidPropertyModifiers, syntax.Span);
            var propertyType = ResolveType(syntax.Type);

            var symbol = new PropertySymbol(
                type,
                modifiers,
                propertyType,
                syntax.ExplicitInterfaceSpecifier == null ? null : ResolveType(syntax.ExplicitInterfaceSpecifier),
                syntax.Identifier.Identifier,
                false /* isIndexer */
            );

            type.AddMember(symbol);
            _manager.Add(syntax, symbol);

            foreach (var accessor in syntax.Accessors) {
                switch (accessor.Type) {
                    case AccessorDeclarationType.Get:
                        if (symbol.GetMethod != null) {
                            _errors.Add(Error.DuplicateAccessorDeclaration, accessor.Span);
                        } else {
                            symbol.GetMethod = new PropertyAccessorSymbol(
                                PropertyAccessorKind.Get,
                                symbol,
                                MakeModifiers(accessor.Modifiers, ValidAccessorModifiers, accessor.Span),
                                propertyType
                            );
                        }
                        break;

                    case AccessorDeclarationType.Set:
                        if (symbol.SetMethod != null) {
                            _errors.Add(Error.DuplicateAccessorDeclaration, accessor.Span);
                        } else {
                            symbol.SetMethod = new PropertyAccessorSymbol(
                                PropertyAccessorKind.Set,
                                symbol,
                                MakeModifiers(accessor.Modifiers, ValidAccessorModifiers, accessor.Span),
                                _voidTypeSymbol
                            );
                            symbol.SetMethod.Parameters = ImmutableArray<ParameterSymbol>.Create(
                                new ParameterSymbol(
                                    ParameterModifier.None,
                                    WellKnownNames.PropertySetterValueParameter,
                                    propertyType
                                )
                            );
                        }
                        break;

                    default:
                        _errors.Add(Error.InvalidAccessorDeclarationType, accessor.Span);
                        break;
                }
            }
        }

        public override void VisitIndexerDeclaration(IndexerDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_scope.Symbol;

            var modifiers = MakeModifiers(syntax.Modifiers, ValidPropertyModifiers, syntax.Span);
            var propertyType = ResolveType(syntax.Type);

            var symbol = new PropertySymbol(
                type,
                modifiers,
                propertyType,
                syntax.ExplicitInterfaceSpecifier == null ? null : ResolveType(syntax.ExplicitInterfaceSpecifier),
                WellKnownNames.PropertyIndexer,
                true /* isIndexer */
            );

            type.AddMember(symbol);
            _manager.Add(syntax, symbol);

            var parameters = new List<ParameterSymbol>();
            var seen = new HashSet<string>();

            for (int i = 0; i < syntax.Parameters.Count; i++) {
                var parameter = syntax.Parameters[i];
                string name = parameter.Identifier.Identifier;

                if (seen.Contains(name)) {
                    _errors.Add(Error.DuplicateIdentifier, parameter.Span, name);
                }

                var parameterType = ResolveType(parameter.ParameterType);

                parameters.Add(new ParameterSymbol(
                    MakeIndexerParameterModifiers(
                        parameter.Modifiers,
                        parameterType,
                        i == syntax.Parameters.Count - 1,
                        parameter.Span
                    ),
                    name,
                    parameterType
                ));
            }

            foreach (var accessor in syntax.Accessors) {
                if (accessor.Body == null) {
                    _errors.Add(Error.ExpectedBody, accessor.Span);
                    continue;
                }

                switch (accessor.Type) {
                    case AccessorDeclarationType.Get:
                        if (symbol.GetMethod != null) {
                            _errors.Add(Error.DuplicateAccessorDeclaration, accessor.Span);
                        } else {
                            symbol.GetMethod = new PropertyAccessorSymbol(
                                PropertyAccessorKind.Get,
                                symbol,
                                MakeModifiers(accessor.Modifiers, ValidAccessorModifiers, accessor.Span),
                                propertyType
                            );
                            symbol.GetMethod.Parameters = ImmutableArray<ParameterSymbol>.Create(parameters.ToArray());
                        }
                        break;

                    case AccessorDeclarationType.Set:
                        if (symbol.SetMethod != null) {
                            _errors.Add(Error.DuplicateAccessorDeclaration, accessor.Span);
                        } else {
                            symbol.SetMethod = new PropertyAccessorSymbol(
                                PropertyAccessorKind.Set,
                                symbol,
                                MakeModifiers(accessor.Modifiers, ValidAccessorModifiers, accessor.Span),
                                _voidTypeSymbol
                            );

                            var builder = new ImmutableArray<ParameterSymbol>.Builder();

                            builder.AddRange(parameters);
                            builder.Add(
                                new ParameterSymbol(
                                    ParameterModifier.None,
                                    WellKnownNames.PropertySetterValueParameter,
                                    propertyType
                                )
                            );

                            symbol.SetMethod.Parameters = builder.Build();
                        }
                        break;

                    default:
                        _errors.Add(Error.InvalidAccessorDeclarationType, accessor.Span);
                        break;
                }
            }
        }

        public override void VisitEventDeclaration(EventDeclarationSyntax syntax) {
            var type = (NamedTypeSymbol)_scope.Symbol;

            var modifiers = MakeModifiers(syntax.Modifiers, ValidEventModifiers, syntax.Span);
            var eventType = ResolveType(syntax.Type);

            if (eventType.TypeKind != TypeKind.Delegate) {
                _errors.Add(Error.EventTypeMustBeDelegate, syntax.Type.Span);
                return;
            }

            var symbol = new EventSymbol(
                type,
                modifiers,
                eventType,
                syntax.ExplicitInterfaceSpecifier == null ? null : ResolveType(syntax.ExplicitInterfaceSpecifier),
                syntax.Identifier.Identifier
            );

            type.AddMember(symbol);
            _manager.Add(syntax, symbol);

            foreach (var accessor in syntax.Accessors) {
                if (accessor.Body == null) {
                    _errors.Add(Error.ExpectedBody, accessor.Span);
                    continue;
                }

                switch (accessor.Type) {
                    case AccessorDeclarationType.Add:
                        if (symbol.AddMethod != null) {
                            _errors.Add(Error.DuplicateAccessorDeclaration, accessor.Span);
                        } else {
                            symbol.AddMethod = BuildEventAccessor(symbol, accessor, EventAccessorKind.Add);
                        }
                        break;

                    case AccessorDeclarationType.Remove:
                        if (symbol.RemoveMethod != null) {
                            _errors.Add(Error.DuplicateAccessorDeclaration, accessor.Span);
                        } else {
                            symbol.RemoveMethod = BuildEventAccessor(symbol, accessor, EventAccessorKind.Remove);
                        }
                        break;

                    default:
                        _errors.Add(Error.InvalidAccessorDeclarationType, accessor.Span);
                        break;
                }
            }
        }

        private EventAccessorSymbol BuildEventAccessor(EventSymbol symbol, AccessorDeclarationSyntax accessor, EventAccessorKind kind) {
            return new EventAccessorSymbol(
                kind,
                symbol,
                MakeModifiers(accessor.Modifiers, ValidAccessorModifiers, accessor.Span),
                _voidTypeSymbol
            );
        }
    }
}
