package truss.compiler.binding;

import org.apache.commons.lang.Validate;
import truss.compiler.Message;
import truss.compiler.MessageCollectionScope;
import truss.compiler.MessageType;
import truss.compiler.support.ImmutableArray;
import truss.compiler.symbols.ContainerSymbol;
import truss.compiler.symbols.NamespaceSymbol;
import truss.compiler.symbols.TypeSymbol;
import truss.compiler.syntax.*;

import java.util.ArrayList;
import java.util.List;

class MetadataSymbolBinder extends SyntaxTreeWalker {
    private final SymbolManager manager;
    private Scope scope;
    private GlobalScope globalScope;
    private ContainerScope containerScope;

    public MetadataSymbolBinder(SymbolManager manager) {
        Validate.notNull(manager, "manager");

        this.manager = manager;
    }

    private void setScope(Scope scope) {
        this.scope = scope;
        if (scope instanceof ContainerScope) {
            containerScope = (ContainerScope)scope;
        } else {
            containerScope = null;
        }
    }

    private void popScope() {
        scope = scope.getParent();
    }

    private List<Import> resolveImports(ImmutableArray<ImportDirectiveSyntax> imports) {
        List<Import> result = new ArrayList<>();

        // Resolve all imports for the global scope or a namespace. This resolves the imports and
        // results in Import instances which are attached to the namespace scope. Note that imports
        // are always matched against the global scope; not the current namespace.

        for (ImportDirectiveSyntax import_ : imports) {
            ImportType type = null;
            ContainerSymbol container = null;

            if (import_.isStatic()) {
                if (import_.getAlias() != null) {
                    MessageCollectionScope.addMessage(new Message(
                        MessageType.STATIC_IMPORT_CANNOT_HAVE_ALIAS,
                        import_.getSpan()
                    ));
                } else {
                    container = globalScope.resolveContainer(import_.getName(), ResolveMode.TYPE);
                    type = ImportType.STATIC;
                }
            } else {
                // We only allow types in a normal import when an alias was provided. To import
                // a type without an alias, static has to be specified.

                container = globalScope.resolveContainer(
                    import_.getName(),
                    import_.getAlias() == null ? ResolveMode.NAMESPACE : ResolveMode.TYPE_OR_NAMESPACE
                );
                if (container instanceof TypeSymbol) {
                    type = ImportType.TYPE;
                } else {
                    type = ImportType.NAMESPACE;
                }
            }

            if (container != null) {
                result.add(new Import(import_.getAlias().getIdentifier(), container, type));
            }
        }
        return result;
    }

    @Override
    public void visitAccessorDeclaration(AccessorDeclarationSyntax syntax) throws Exception {
        super.visitAccessorDeclaration(syntax);
    }

    @Override
    public void visitAttributeArgument(AttributeArgumentSyntax syntax) throws Exception {
        super.visitAttributeArgument(syntax);
    }

    @Override
    public void visitAttributeList(AttributeListSyntax syntax) throws Exception {
        super.visitAttributeList(syntax);
    }

    @Override
    public void visitAttribute(AttributeSyntax syntax) throws Exception {
        super.visitAttribute(syntax);
    }

    @Override
    public void visitCompilationUnit(CompilationUnitSyntax syntax) throws Exception {
        globalScope = new GlobalScope(
            manager.getGlobalSymbol(),
            resolveImports(syntax.getImports())
        );

        setScope(globalScope);

        super.visitCompilationUnit(syntax);

        popScope();
    }

    @Override
    public void visitConstructorConstraint(ConstructorConstraintSyntax syntax) throws Exception {
        super.visitConstructorConstraint(syntax);
    }

    @Override
    public void visitConstructorDeclaration(ConstructorDeclarationSyntax syntax) throws Exception {
        super.visitConstructorDeclaration(syntax);
    }

    @Override
    public void visitConstructorInitializer(ConstructorInitializerSyntax syntax) throws Exception {
        super.visitConstructorInitializer(syntax);
    }

    @Override
    public void visitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) throws Exception {
        super.visitConversionOperatorDeclaration(syntax);
    }

    @Override
    public void visitDelegateDeclaration(DelegateDeclarationSyntax syntax) throws Exception {
        super.visitDelegateDeclaration(syntax);
    }

    @Override
    public void visitDestructorDeclaration(DestructorDeclarationSyntax syntax) throws Exception {
        super.visitDestructorDeclaration(syntax);
    }

    @Override
    public void visitEnumDeclaration(EnumDeclarationSyntax syntax) throws Exception {
        super.visitEnumDeclaration(syntax);
    }

    @Override
    public void visitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) throws Exception {
        super.visitEnumMemberDeclaration(syntax);
    }

    @Override
    public void visitEventDeclaration(EventDeclarationSyntax syntax) throws Exception {
        super.visitEventDeclaration(syntax);
    }

    @Override
    public void visitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) throws Exception {
        super.visitEventFieldDeclaration(syntax);
    }

    @Override
    public void visitFieldDeclaration(FieldDeclarationSyntax syntax) throws Exception {
        super.visitFieldDeclaration(syntax);
    }

    @Override
    public void visitIndexerDeclaration(IndexerDeclarationSyntax syntax) throws Exception {
        super.visitIndexerDeclaration(syntax);
    }

    @Override
    public void visitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax) throws Exception {
        super.visitLocalDeclarationStatement(syntax);
    }

    @Override
    public void visitMethodDeclaration(MethodDeclarationSyntax syntax) throws Exception {
        super.visitMethodDeclaration(syntax);
    }

    @Override
    public void visitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) throws Exception {
        setScope(new NamespaceScope(
            (NamespaceSymbol)manager.find(syntax),
            resolveImports(syntax.getImports()),
            scope
        ));

        super.visitNamespaceDeclaration(syntax);

        popScope();
    }

    @Override
    public void visitOperatorDeclaration(OperatorDeclarationSyntax syntax) throws Exception {
        super.visitOperatorDeclaration(syntax);
    }

    @Override
    public void visitParameter(ParameterSyntax syntax) throws Exception {
        super.visitParameter(syntax);
    }

    @Override
    public void visitPredefinedType(PredefinedTypeSyntax syntax) throws Exception {
        super.visitPredefinedType(syntax);
    }

    @Override
    public void visitPropertyDeclaration(PropertyDeclarationSyntax syntax) throws Exception {
        super.visitPropertyDeclaration(syntax);
    }

    @Override
    public void visitTypeConstraint(TypeConstraintSyntax syntax) throws Exception {
        super.visitTypeConstraint(syntax);
    }

    @Override
    public void visitTypeDeclaration(TypeDeclarationSyntax syntax) throws Exception {
        super.visitTypeDeclaration(syntax);
    }

    @Override
    public void visitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax) throws Exception {
        super.visitTypeParameterConstraintClause(syntax);
    }

    @Override
    public void visitTypeParameter(TypeParameterSyntax syntax) throws Exception {
        super.visitTypeParameter(syntax);
    }

    @Override
    public void visitTrackedType(TrackedTypeSyntax syntax) throws Exception {
        super.visitTrackedType(syntax);
    }

    @Override
    public void visitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax) throws Exception {
        super.visitTypeFamilyConstraint(syntax);
    }
}
