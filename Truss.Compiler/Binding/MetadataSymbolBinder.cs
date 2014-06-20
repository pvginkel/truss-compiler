using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal class MetadataSymbolBinder : SyntaxTreeWalker {
        private readonly SymbolManager _manager;
        private Scope _scope;
        private GlobalScope _globalScope;
        private ContainerScope _containerScope;

        public MetadataSymbolBinder(SymbolManager manager) {
            if (manager == null) {
                throw new ArgumentNullException("manager");
            }

            _manager = manager;
        }

        private void SetScope(Scope scope) {
            _scope = scope;
            _containerScope = scope as ContainerScope;
        }

        private void PopScope() {
            _scope = _scope.Parent;
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
                        MessageCollectionScope.AddMessage(new Message(
                            MessageType.StaticImportCannotHaveAlias,
                            import.Span
                            ));
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
                    if (container is TypeSymbol) {
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

        public override void VisitAccessorDeclaration(AccessorDeclarationSyntax syntax) {
            base.VisitAccessorDeclaration(syntax);
        }

        public override void VisitAttributeArgument(AttributeArgumentSyntax syntax) {
            base.VisitAttributeArgument(syntax);
        }

        public override void VisitAttributeList(AttributeListSyntax syntax) {
            base.VisitAttributeList(syntax);
        }

        public override void VisitAttribute(AttributeSyntax syntax) {
            base.VisitAttribute(syntax);
        }

        public override void VisitCompilationUnit(CompilationUnitSyntax syntax) {
            _globalScope = new GlobalScope(_manager.GlobalSymbol, ResolveImports(syntax.Imports));

            SetScope(_globalScope);

            base.VisitCompilationUnit(syntax);

            PopScope();
        }

        public override void VisitConstructorConstraint(ConstructorConstraintSyntax syntax) {
            base.VisitConstructorConstraint(syntax);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax) {
            base.VisitConstructorDeclaration(syntax);
        }

        public override void VisitConstructorInitializer(ConstructorInitializerSyntax syntax) {
            base.VisitConstructorInitializer(syntax);
        }

        public override void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) {
            base.VisitConversionOperatorDeclaration(syntax);
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            base.VisitDelegateDeclaration(syntax);
        }

        public override void VisitDestructorDeclaration(DestructorDeclarationSyntax syntax) {
            base.VisitDestructorDeclaration(syntax);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            base.VisitEnumDeclaration(syntax);
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) {
            base.VisitEnumMemberDeclaration(syntax);
        }

        public override void VisitEventDeclaration(EventDeclarationSyntax syntax) {
            base.VisitEventDeclaration(syntax);
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) {
            base.VisitEventFieldDeclaration(syntax);
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax syntax) {
            base.VisitFieldDeclaration(syntax);
        }

        public override void VisitIndexerDeclaration(IndexerDeclarationSyntax syntax) {
            base.VisitIndexerDeclaration(syntax);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax) {
            base.VisitLocalDeclarationStatement(syntax);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax syntax) {
            base.VisitMethodDeclaration(syntax);
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

        public override void VisitOperatorDeclaration(OperatorDeclarationSyntax syntax) {
            base.VisitOperatorDeclaration(syntax);
        }

        public override void VisitParameter(ParameterSyntax syntax) {
            base.VisitParameter(syntax);
        }

        public override void VisitPredefinedType(PredefinedTypeSyntax syntax) {
            base.VisitPredefinedType(syntax);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax syntax) {
            base.VisitPropertyDeclaration(syntax);
        }

        public override void VisitTypeConstraint(TypeConstraintSyntax syntax) {
            base.VisitTypeConstraint(syntax);
        }

        public override void VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            base.VisitTypeDeclaration(syntax);
        }

        public override void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax) {
            base.VisitTypeParameterConstraintClause(syntax);
        }

        public override void VisitTypeParameter(TypeParameterSyntax syntax) {
            base.VisitTypeParameter(syntax);
        }

        public override void VisitTrackedType(TrackedTypeSyntax syntax) {
            base.VisitTrackedType(syntax);
        }

        public override void VisitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax) {
            base.VisitTypeFamilyConstraint(syntax);
        }
    }
}
