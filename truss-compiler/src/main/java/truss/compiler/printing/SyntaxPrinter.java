package truss.compiler.printing;

import org.apache.commons.lang.Validate;
import truss.compiler.WellKnownNames;
import truss.compiler.support.ImmutableArray;
import truss.compiler.syntax.*;

public class SyntaxPrinter implements SyntaxVisitor {
    private final Printer printer;

    public SyntaxPrinter(Printer printer) {
        Validate.notNull(printer, "printer");

        this.printer = printer;
    }

    private void identifier(String identifier) throws Exception {
        printer.identifier(identifier);
    }

    private void keyword(String keyword) throws Exception {
        printer.keyword(keyword);
    }

    private void nl() throws Exception {
        printer.nl();
    }

    private void literal(String value, LiteralType type) throws Exception {
        switch (type) {
            case TRUE:
            case FALSE:
            case NIL:
                printer.keyword(type.toString().toLowerCase());
                break;

            default:
                printer.literal(value, type);
                break;
        }
    }

    private void unIndent() throws Exception {
        printer.unIndent();
    }

    private void lead() throws Exception {
        printer.lead();
    }

    private void syntax(String syntax) throws Exception {
        printer.syntax(syntax);
    }

    private void indent() throws Exception {
        printer.indent();
    }

    private void ws() throws Exception {
        printer.ws();
    }

    @Override
    public boolean isDone() {
        return false;
    }

    private void visitAttributeListList(ImmutableArray<AttributeListSyntax> attributes, boolean inline) throws Exception {
        for (AttributeListSyntax attribute : attributes) {
            if (!inline) {
                lead();
            }
            attribute.accept(this);
            if (inline) {
                ws();
            } else {
                nl();
            }
        }
    }

    private void visitModifiers(ImmutableArray<Modifier> modifiers) throws Exception {
        for (Modifier modifier : modifiers) {
            if (modifier != Modifier.TRACKED) {
                keyword(modifier.toString().toLowerCase());
                ws();
            }
        }
    }

    private boolean visitList(ImmutableArray<? extends SyntaxNode> nodes) throws Exception {
        for (SyntaxNode node : nodes) {
            node.accept(this);
        }

        return nodes.size() > 0;
    }

    private boolean visitCommaList(ImmutableArray<? extends SyntaxNode> nodes) throws Exception {
        boolean hadOne = false;
        for (SyntaxNode node : nodes) {
            if (hadOne) {
                syntax(",");
                ws();
            } else {
                hadOne = true;
            }
            node.accept(this);
        }

        return nodes.size() > 0;
    }

    @Override
    public void visitAccessorDeclaration(AccessorDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        keyword(syntax.getType().toString().toLowerCase());
        visitOuterBlock(syntax.getBody());
    }

    private void visitOuterBlock(BlockSyntax body) throws Exception {
        ws();
        body.accept(this);
        nl();
    }

    @Override
    public void visitAliasQualifiedName(AliasQualifiedNameSyntax syntax) throws Exception {
        if (WellKnownNames.ALIAS_GLOBAL.equals(syntax.getAlias().getIdentifier())) {
            keyword(WellKnownNames.ALIAS_GLOBAL);
        } else {
            syntax.getAlias().accept(this);
        }
        syntax("::");
        syntax.getName().accept(this);
    }

    @Override
    public void visitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax syntax) throws Exception {
        keyword("new");
        ws();
        syntax("{");
        ImmutableArray<AnonymousObjectMemberDeclaratorSyntax> initializers = syntax.getInitializers();
        if (initializers.size() == 0) {
            ws();
        } else {
            nl();
            indent();
            for (int i = 0; i < initializers.size(); i++) {
                lead();
                initializers.get(i).accept(this);
                if (i < initializers.size() - 1) {
                    syntax(",");
                }
                nl();
            }
            unIndent();
            lead();
            syntax("}");
        }
    }

    @Override
    public void visitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax syntax) throws Exception {
        if (syntax.getName() != null) {
            syntax.getName().accept(this);
            ws();
            syntax("=");
            ws();
        }
        syntax.getExpression().accept(this);
    }

    @Override
    public void visitArgument(ArgumentSyntax syntax) throws Exception {
        visitParameterModifiers(syntax.getModifiers());
        syntax.getExpression().accept(this);
    }

    @Override
    public void visitArrayCreationExpression(ArrayCreationExpressionSyntax syntax) throws Exception {
        keyword("new");
        ws();
        syntax.getType().accept(this);
        if (syntax.getInitializer() != null) {
            ws();
            syntax.getInitializer().accept(this);
        }
    }

    @Override
    public void visitArrayRankSpecifier(ArrayRankSpecifierSyntax syntax) throws Exception {
        syntax("[");
        syntax.getSize().accept(this);
        syntax("]");
    }

    @Override
    public void visitArrayType(ArrayTypeSyntax syntax) throws Exception {
        syntax.getElementType().accept(this);
        visitList(syntax.getRankSpecifiers());
    }

    @Override
    public void visitAssertStatement(AssertStatementSyntax syntax) throws Exception {
        lead();
        keyword("assert");
        ws();
        syntax.getExpression().accept(this);
        syntax(";");
        nl();
    }

    @Override
    public void visitAttributeArgument(AttributeArgumentSyntax syntax) throws Exception {
        if (syntax.getName() != null) {
            syntax.getName().accept(this);
            ws();
            syntax("=");
            ws();
        }
        syntax.getExpression().accept(this);
    }

    @Override
    public void visitAttributeList(AttributeListSyntax syntax) throws Exception {
        syntax("[");
        if (syntax.getTarget() != AttributeTarget.NONE) {
            keyword(syntax.getTarget().toString().toLowerCase());
            syntax(":");
            ws();
        }
        visitCommaList(syntax.getAttributes());
        syntax("]");
    }

    @Override
    public void visitAttribute(AttributeSyntax syntax) throws Exception {
        syntax.getName().accept(this);
        if (syntax.getArguments().size() > 0) {
            syntax("(");
            visitCommaList(syntax.getArguments());
            syntax(")");
        }
    }

    @Override
    public void visitAwaitExpression(AwaitExpressionSyntax syntax) throws Exception {
        keyword("await");
        ws();
        syntax.getExpression().accept(this);
    }

    @Override
    public void visitBinaryExpression(BinaryExpressionSyntax syntax) throws Exception {
        syntax.getLeft().accept(this);
        ws();
        switch (syntax.getOperator()) {
            case AMPERSAND:
                syntax("&");
                break;
            case AMPERSAND_AMPERSAND:
                syntax("&&");
                break;
            case AMPERSAND_EQUALS:
                syntax("&=");
                break;
            case AS:
                keyword("as");
                break;
            case ASTERISK:
                syntax("*");
                break;
            case ASTERISK_EQUALS:
                syntax("*=");
                break;
            case BAR:
                syntax("|");
                break;
            case BAR_BAR:
                syntax("||");
                break;
            case BAR_EQUALS:
                syntax("|=");
                break;
            case CARET:
                syntax("^");
                break;
            case CARET_EQUALS:
                syntax("^=");
                break;
            case EQUALS:
                syntax("=");
                break;
            case EQUALS_EQUALS:
                syntax("==");
                break;
            case EXCLAMATION_EQUALS:
                syntax("!=");
                break;
            case GREATER_THAN:
                syntax(">");
                break;
            case GREATER_THAN_EQUALS:
                syntax(">=");
                break;
            case GREATER_THAN_GREATER_THAN:
                syntax(">>");
                break;
            case GREATER_THAN_GREATER_THAN_EQUALS:
                syntax(">>=");
                break;
            case IS:
                keyword("is");
                break;
            case LESS_THAN:
                syntax("<");
                break;
            case LESS_THAN_EQUALS:
                syntax("<=");
                break;
            case LESS_THAN_LESS_THAN:
                syntax("<<");
                break;
            case LESS_THAN_LESS_THAN_EQUALS:
                syntax("<<=");
                break;
            case MINUS:
                syntax("-");
                break;
            case MINUS_EQUALS:
                syntax("-=");
                break;
            case PERCENT:
                syntax("%");
                break;
            case PERCENT_EQUALS:
                syntax("%=");
                break;
            case PLUS:
                syntax("+");
                break;
            case PLUS_EQUALS:
                syntax("+=");
                break;
            case QUESTION_QUESTION:
                syntax("??");
                break;
            case SLASH:
                syntax("/");
                break;
            case SLASH_EQUALS:
                syntax("/=");
                break;
            default:
                throw new IllegalStateException();
        }
        ws();
        syntax.getRight().accept(this);
    }

    @Override
    public void visitBlock(BlockSyntax syntax) throws Exception {
        syntax("{");
        nl();
        indent();
        for (StatementSyntax statement : syntax.getStatements()) {
            statement.accept(this);
        }
        unIndent();
        lead();
        syntax("}");
    }

    @Override
    public void visitBreakStatement(BreakStatementSyntax syntax) throws Exception {
        lead();
        keyword("break");
        syntax(";");
        nl();
    }

    @Override
    public void visitCastExpression(CastExpressionSyntax syntax) throws Exception {
        syntax("(");
        syntax.getTargetType().accept(this);
        syntax(")");
        syntax.getExpression().accept(this);
    }

    @Override
    public void visitCatchClause(CatchClauseSyntax syntax) throws Exception {
        ws();
        keyword("catch");
        if (syntax.getExceptionType() != null) {
            ws();
            syntax.getExceptionType().accept(this);
            if (syntax.getIdentifier() != null) {
                ws();
                syntax.getIdentifier().accept(this);
            }
        } else {
            assert syntax.getIdentifier() == null;
        }
        ws();
        syntax.getBlock().accept(this);
    }

    @Override
    public void visitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax) throws Exception {
        switch (syntax.getFamily()) {
            case ANY:
                break;

            case CLASS:
                keyword("class");
                break;

            case STRUCT:
                keyword("struct");
                break;

            case TRACKED:
                keyword("struct");
                syntax("^");
                return;
        }

        printNullable(syntax.getNullable());
    }

    private void printNullable(Nullable nullable) throws Exception {
        syntax(nullable == Nullable.NULLABLE ? "?" : "!");
    }

    @Override
    public void visitCompilationUnit(CompilationUnitSyntax syntax) throws Exception {
        boolean hadOne;

        hadOne = visitList(syntax.getImports());
        if (syntax.getAttributeLists().size() > 0 && hadOne) {
            nl();
        }
        visitAttributeListList(syntax.getAttributeLists(), false);
        for (MemberDeclarationSyntax member : syntax.getMembers()) {
            nl();
            member.accept(this);
        }
    }

    @Override
    public void visitConditionalExpression(ConditionalExpressionSyntax syntax) throws Exception {
        syntax.getCondition().accept(this);
        ws();
        syntax("?");
        ws();
        syntax.getWhenTrue().accept(this);
        ws();
        syntax(":");
        ws();
        syntax.getWhenFalse().accept(this);
    }

    @Override
    public void visitConstructorConstraint(ConstructorConstraintSyntax syntax) throws Exception {
        keyword("new");
        syntax("()");
    }

    @Override
    public void visitConstructorDeclaration(ConstructorDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        syntax.getIdentifier().accept(this);
        visitParameterList(syntax.getParameters());
        if (syntax.getInitializer() != null) {
            nl();
            indent();
            lead();
            syntax.getInitializer().accept(this);
            unIndent();
        }
        visitOuterBlock(syntax.getBody());
    }

    @Override
    public void visitConstructorInitializer(ConstructorInitializerSyntax syntax) throws Exception {
        syntax(":");
        ws();
        keyword(syntax.getType().toString().toLowerCase());
        visitArgumentList(syntax.getArguments());
    }

    private void visitArgumentList(ImmutableArray<ArgumentSyntax> arguments) throws Exception {
        syntax("(");
        visitCommaList(arguments);
        syntax(")");
    }

    @Override
    public void visitContinueStatement(ContinueStatementSyntax syntax) throws Exception {
        lead();
        keyword("continue");
        syntax(";");
        nl();
    }

    @Override
    public void visitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        keyword(syntax.getType().toString().toLowerCase());
        ws();
        keyword("operator");
        ws();
        syntax.getTargetType().accept(this);
        visitParameterList(syntax.getParameters());
        visitOuterBlock(syntax.getBody());
    }

    @Override
    public void visitDefaultExpression(DefaultExpressionSyntax syntax) throws Exception {
        keyword("default");
        syntax("(");
        syntax.getTargetType().accept(this);
        syntax(")");
    }

    @Override
    public void visitDelegateDeclaration(DelegateDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        keyword("delegate");
        ws();
        syntax.getReturnType().accept(this);
        ws();
        syntax.getIdentifier().accept(this);
        visitTypeParameterList(syntax.getTypeParameters());
        visitParameterList(syntax.getParameters());
        visitTypeParameterConstraintClauseList(syntax.getConstraintClauses());
        syntax(";");
        nl();
    }

    @Override
    public void visitDestructorDeclaration(DestructorDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        assert syntax.getModifiers().size() == 0;
        syntax("~");
        syntax.getIdentifier().accept(this);
        visitParameterList(syntax.getParameters());
        visitOuterBlock(syntax.getBody());
    }

    @Override
    public void visitDoStatement(DoStatementSyntax syntax) throws Exception {
        lead();
        keyword("do");
        ws();
        syntax.getBlock().accept(this);
        ws();
        keyword("while");
        ws();
        syntax.getCondition().accept(this);
        syntax(";");
        nl();
    }

    @Override
    public void visitElementAccessExpression(ElementAccessExpressionSyntax syntax) throws Exception {
        syntax.getExpression().accept(this);
        syntax("[");
        visitCommaList(syntax.getIndexExpressions());
        syntax("]");
    }

    @Override
    public void visitElseClause(ElseClauseSyntax syntax) throws Exception {
        ws();
        keyword(syntax.getType().toString().toLowerCase());
        if (syntax.getType() == ElIfOrElse.ELIF) {
            ws();
            syntax.getCondition().accept(this);
        }
        ws();
        syntax.getBlock().accept(this);
    }

    @Override
    public void visitEmptyStatement(EmptyStatementSyntax syntax) throws Exception {
        lead();
        syntax(";");
        nl();
    }

    @Override
    public void visitEnumDeclaration(EnumDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);

        lead();
        visitModifiers(syntax.getModifiers());
        keyword("enum");
        ws();
        syntax.getIdentifier().accept(this);
        visitBaseTypes(syntax.getBaseTypes());
        ws();
        syntax("{");
        nl();
        indent();

        for (int i = 0; i < syntax.getMembers().size(); i++) {
            EnumMemberDeclarationSyntax member = syntax.getMembers().get(i);
            visitAttributeListList(member.getAttributeLists(), false);
            lead();
            member.accept(this);
            if (i < syntax.getMembers().size() - 1) {
                syntax(",");
            }
            nl();
        }

        unIndent();
        lead();
        syntax("}");
        nl();
    }

    @Override
    public void visitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) throws Exception {
        syntax.getIdentifier().accept(this);
        if (syntax.getValue() != null) {
            ws();
            syntax("=");
            ws();
            syntax.getValue().accept(this);
        }
    }

    @Override
    public void visitEventDeclaration(EventDeclarationSyntax syntax) throws Exception {
        visitBasePropertyDeclaration(syntax);
    }

    private void visitBasePropertyDeclaration(BasePropertyDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        if (syntax.getKind() == SyntaxKind.EVENT_DECLARATION) {
            keyword("event");
            ws();
        }
        syntax.getType().accept(this);
        ws();
        if (syntax.getExplicitInterfaceSpecifier() != null) {
            syntax.getExplicitInterfaceSpecifier().accept(this);
            syntax(".");
        }
        switch (syntax.getKind()) {
            case EVENT_DECLARATION:
                ((EventDeclarationSyntax)syntax).getIdentifier().accept(this);
                break;

            case PROPERTY_DECLARATION:
                ((PropertyDeclarationSyntax)syntax).getIdentifier().accept(this);
                break;

            case INDEXER_DECLARATION:
                keyword("this");
                syntax("[");
                visitCommaList(((IndexerDeclarationSyntax)syntax).getParameters());
                syntax("]");
                break;

            default:
                assert false;
        }
        ws();
        syntax("{");
        nl();
        indent();
        visitList(syntax.getAccessors());
        unIndent();
        lead();
        syntax("}");
        nl();
    }

    @Override
    public void visitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) throws Exception {
        visitBaseFieldDeclaration(syntax);
    }

    private void visitBaseFieldDeclaration(BaseFieldDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        if (syntax instanceof EventFieldDeclarationSyntax) {
            keyword("event");
            ws();
        }
        syntax.getDeclaration().accept(this);
        syntax(";");
        nl();
    }

    @Override
    public void visitExpressionStatement(ExpressionStatementSyntax syntax) throws Exception {
        lead();
        syntax.getExpression().accept(this);
        syntax(";");
        nl();
    }

    @Override
    public void visitFieldDeclaration(FieldDeclarationSyntax syntax) throws Exception {
        visitBaseFieldDeclaration(syntax);
    }

    @Override
    public void visitFinallyClause(FinallyClauseSyntax syntax) throws Exception {
        keyword("finally");
        ws();
        syntax.getBlock().accept(this);
    }

    @Override
    public void visitForEachStatement(ForEachStatementSyntax syntax) throws Exception {
        lead();
        keyword("foreach");
        ws();
        syntax.getElementType().accept(this);
        ws();
        syntax.getIdentifier().accept(this);
        ws();
        keyword("in");
        ws();
        syntax.getExpression().accept(this);
        visitOuterBlock(syntax.getBlock());
    }

    @Override
    public void visitForStatement(ForStatementSyntax syntax) throws Exception {
        lead();
        keyword("for");
        ws();
        if (syntax.getDeclaration() != null) {
            syntax.getDeclaration().accept(this);
        }
        if (syntax.getInitializers() != null) {
            visitCommaList(syntax.getInitializers());
        }
        syntax(";");
        if (syntax.getCondition() != null) {
            ws();
            syntax.getCondition().accept(this);
        }
        syntax(";");
        if (syntax.getIncrementors() != null) {
            ws();
            visitCommaList(syntax.getIncrementors());
        }
        visitOuterBlock(syntax.getBlock());
    }

    @Override
    public void visitGenericName(GenericNameSyntax syntax) throws Exception {
        identifier(syntax.getIdentifier());
        syntax("<");
        boolean hadOne = false;
        for (TypeSyntax type : syntax.getTypeArguments()) {
            if (hadOne) {
                syntax(",");
                if (type.getKind() != SyntaxKind.OMITTED_TYPE_ARGUMENT) {
                    ws();
                }
            } else {
                hadOne = true;
            }
            type.accept(this);
        }
        syntax(">");
    }

    @Override
    public void visitIdentifierName(IdentifierNameSyntax syntax) throws Exception {
        identifier(syntax.getIdentifier());
    }

    @Override
    public void visitIfStatement(IfStatementSyntax syntax) throws Exception {
        lead();
        keyword("if");
        ws();
        syntax.getCondition().accept(this);
        ws();
        syntax.getBlock().accept(this);
        visitList(syntax.getElses());
        nl();
    }

    @Override
    public void visitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax syntax) throws Exception {
        keyword("new");
        visitList(syntax.getRankSpecifiers());
        ws();
        syntax.getInitializer().accept(this);
    }

    @Override
    public void visitImportDirective(ImportDirectiveSyntax syntax) throws Exception {
        lead();
        keyword("import");
        ws();
        if (syntax.isStatic()) {
            keyword("static");
            ws();
        }
        if (syntax.getAlias() != null) {
            syntax.getAlias().accept(this);
            syntax("=");
        }
        syntax.getName().accept(this);
        syntax(";");
        nl();
    }

    @Override
    public void visitIndexerDeclaration(IndexerDeclarationSyntax syntax) throws Exception {
        visitBasePropertyDeclaration(syntax);
    }

    @Override
    public void visitInitializerExpression(InitializerExpressionSyntax syntax) throws Exception {
        syntax("{");
        ImmutableArray<ExpressionSyntax> expressions = syntax.getExpressions();
        if (expressions.size() == 0) {
            ws();
        } else {
            nl();
            indent();
            for (int i = 0; i < expressions.size(); i++) {
                lead();
                expressions.get(i).accept(this);
                if (i < expressions.size() - 1) {
                    syntax(",");
                }
                nl();
            }
            unIndent();
            lead();
        }
        syntax("}");
    }

    @Override
    public void visitInstanceExpression(InstanceExpressionSyntax syntax) throws Exception {
        keyword(syntax.getType() == ThisOrBase.BASE ? "base" : "this");
    }

    @Override
    public void visitInvocationExpression(InvocationExpressionSyntax syntax) throws Exception {
        syntax.getExpression().accept(this);
        visitArgumentList(syntax.getArguments());
    }

    @Override
    public void visitLambdaExpression(LambdaExpressionSyntax syntax) throws Exception {
        visitModifiers(syntax.getModifiers());
        // Check whether we should omit the parens.
        boolean printParens = true;
        if (syntax.getParameters().size() == 1) {
            ParameterSyntax firstParameter = syntax.getParameters().get(0);
            printParens =
                firstParameter.getAttributeLists().size() > 0 ||
                    firstParameter.getParameterType() != null ||
                    firstParameter.getModifiers().size() > 0;
        }
        if (printParens) {
            syntax("(");
        }
        visitCommaList(syntax.getParameters());
        if (printParens) {
            syntax(")");
        }
        ws();
        syntax("=>");
        ws();
        syntax.getBody().accept(this);
    }

    @Override
    public void visitLiteralExpression(LiteralExpressionSyntax syntax) throws Exception {
        literal(syntax.getValue(), syntax.getLiteralType());
    }

    @Override
    public void visitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax) throws Exception {
        lead();
        visitModifiers(syntax.getModifiers());
        syntax.getDeclaration().accept(this);
        syntax(";");
        nl();
    }

    @Override
    public void visitLoopStatement(LoopStatementSyntax syntax) throws Exception {
        lead();
        keyword("loop");
        visitOuterBlock(syntax.getBlock());
    }

    @Override
    public void visitMemberAccessExpression(MemberAccessExpressionSyntax syntax) throws Exception {
        syntax.getExpression().accept(this);
        syntax(".");
        syntax.getName().accept(this);
    }

    @Override
    public void visitMethodDeclaration(MethodDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        syntax.getReturnType().accept(this);
        ws();
        if (syntax.getExplicitInterfaceSpecifier() != null) {
            syntax.getExplicitInterfaceSpecifier().accept(this);
            syntax(".");
        }
        syntax.getIdentifier().accept(this);
        visitTypeParameterList(syntax.getTypeParameters());
        visitParameterList(syntax.getParameters());
        visitTypeParameterConstraintClauseList(syntax.getConstraintClauses());
        visitOuterBlock(syntax.getBody());
    }

    private void visitParameterList(ImmutableArray<ParameterSyntax> parameters) throws Exception {
        syntax("(");
        visitCommaList(parameters);
        syntax(")");
    }

    @Override
    public void visitNakedNullableType(NakedNullableTypeSyntax syntax) throws Exception {
        printNullable(syntax.getType());
    }

    @Override
    public void visitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) throws Exception {
        lead();
        keyword("namespace");
        ws();
        syntax.getName().accept(this);
        ws();
        syntax("{");
        nl();
        indent();

        boolean hadOne = visitList(syntax.getImports());

        for (MemberDeclarationSyntax member : syntax.getMembers()) {
            if (hadOne) {
                nl();
            } else {
                hadOne = true;
            }
            member.accept(this);
        }

        unIndent();
        lead();
        syntax("}");
        nl();
    }

    @Override
    public void visitNullableType(NullableTypeSyntax syntax) throws Exception {
        syntax.getElementType().accept(this);
        syntax("?");
    }

    @Override
    public void visitTrackedType(TrackedTypeSyntax syntax) throws Exception {
        syntax.getElementType().accept(this);
        syntax("^");
    }

    @Override
    public void visitObjectCreationExpression(ObjectCreationExpressionSyntax syntax) throws Exception {
        keyword("new");
        ws();
        syntax.getTargetType().accept(this);
        if (syntax.getArguments() != null) {
            visitArgumentList(syntax.getArguments());
        }
        if (syntax.getInitializer() != null) {
            ws();
            syntax.getInitializer().accept(this);
        }
    }

    @Override
    public void visitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax syntax) throws Exception {
        // Nothing to do.
    }

    @Override
    public void visitOmittedTypeArgument(OmittedTypeArgumentSyntax syntax) throws Exception {
        // Nothing to do.
    }

    @Override
    public void visitOperatorDeclaration(OperatorDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);
        lead();
        visitModifiers(syntax.getModifiers());
        syntax.getReturnType().accept(this);
        ws();
        keyword("operator");
        ws();
        switch (syntax.getOperator()) {
            case AMPERSAND:
                syntax("&");
                break;
            case ASTERISK:
                syntax("*");
                break;
            case BAR:
                syntax("|");
                break;
            case CARET:
                syntax("^");
                break;
            case EQUALS_EQUALS:
                syntax("==");
                break;
            case EXCLAMATION:
                syntax("!");
                break;
            case EXCLAMATION_EQUALS:
                syntax("!=");
                break;
            case FALSE:
                keyword("false");
                break;
            case GREATER_THAN:
                syntax(">");
                break;
            case GREATER_THAN_EQUALS:
                syntax(">=");
                break;
            case GREATER_THAN_GREATER_THAN:
                syntax(">>");
                break;
            case LESS_THAN:
                syntax("<");
                break;
            case LESS_THAN_EQUALS:
                syntax("<=");
                break;
            case LESS_THAN_LESS_THAN:
                syntax("<<");
                break;
            case MINUS:
                syntax("-");
                break;
            case MINUS_MINUS:
                syntax("--");
                break;
            case PERCENT:
                syntax("%");
                break;
            case PLUS:
                syntax("+");
                break;
            case PLUS_PLUS:
                syntax("++");
                break;
            case SLASH:
                syntax("/");
                break;
            case TILDE:
                syntax("~");
                break;
            case TRUE:
                keyword("true");
                break;
            default:
                assert false;
        }
        visitParameterList(syntax.getParameters());
        visitOuterBlock(syntax.getBody());
    }

    @Override
    public void visitParameter(ParameterSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), true);
        visitParameterModifiers(syntax.getModifiers());
        // For implicitly typed lambdas.
        if (syntax.getParameterType() != null) {
            syntax.getParameterType().accept(this);
            ws();
        }
        syntax.getIdentifier().accept(this);
    }

    private void visitParameterModifiers(ImmutableArray<ParameterModifier> modifiers) throws Exception {
        for (ParameterModifier modifier : modifiers) {
            keyword(modifier.toString().toLowerCase());
            ws();
        }
    }

    @Override
    public void visitParenthesizedExpression(ParenthesizedExpressionSyntax syntax) throws Exception {
        syntax("(");
        syntax.getExpression().accept(this);
        syntax(")");
    }

    @Override
    public void visitPostfixUnaryExpression(PostfixUnaryExpressionSyntax syntax) throws Exception {
        syntax.getOperand().accept(this);
        switch (syntax.getOperator()) {
            case MINUS_MINUS:
                syntax("--");
                break;
            case PLUS_PLUS:
                syntax("++");
                break;
            default:
                throw new IllegalStateException();
        }
    }

    @Override
    public void visitPredefinedType(PredefinedTypeSyntax syntax) throws Exception {
        keyword(syntax.getPredefinedType().toString().toLowerCase());
    }

    @Override
    public void visitPrefixUnaryExpression(PrefixUnaryExpressionSyntax syntax) throws Exception {
        switch (syntax.getOperator()) {
            case AMPERSAND:
                syntax("&");
                break;
            case EXCLAMATION:
                syntax("!");
                break;
            case MINUS:
                syntax("-");
                break;
            case MINUS_MINUS:
                syntax("--");
                break;
            case PLUS:
                syntax("+");
                break;
            case PLUS_PLUS:
                syntax("++");
                break;
            case TILDE:
                syntax("~");
                break;
            default:
                throw new IllegalStateException();
        }
        syntax.getOperand().accept(this);
    }

    @Override
    public void visitPropertyDeclaration(PropertyDeclarationSyntax syntax) throws Exception {
        visitBasePropertyDeclaration(syntax);
    }

    @Override
    public void visitQualifiedName(QualifiedNameSyntax syntax) throws Exception {
        syntax.getLeft().accept(this);
        syntax(".");
        syntax.getRight().accept(this);
    }

    @Override
    public void visitReturnStatement(ReturnStatementSyntax syntax) throws Exception {
        lead();
        keyword("return");
        if (syntax.getExpression() != null) {
            ws();
            syntax.getExpression().accept(this);
        }
        syntax(";");
        nl();
    }

    @Override
    public void visitSizeOfExpression(SizeOfExpressionSyntax syntax) throws Exception {
        keyword("sizeof");
        syntax("(");
        syntax.getTargetType().accept(this);
        syntax(")");
    }

    @Override
    public void visitSwitchSection(SwitchSectionSyntax syntax) throws Exception {
        lead();
        keyword(syntax.getType().toString().toLowerCase());
        if (syntax.getType() == CaseOrDefault.CASE) {
            ws();
            visitCommaList(syntax.getValues());
        }
        visitOuterBlock(syntax.getBlock());
    }

    @Override
    public void visitSwitchStatement(SwitchStatementSyntax syntax) throws Exception {
        lead();
        keyword("switch");
        ws();
        syntax.getExpression().accept(this);
        ws();
        syntax("{");
        nl();
        indent();
        visitList(syntax.getSections());
        unIndent();
        lead();
        syntax("}");
        nl();
    }

    @Override
    public void visitThrowStatement(ThrowStatementSyntax syntax) throws Exception {
        lead();
        keyword("throw");
        if (syntax.getExpression() != null) {
            ws();
            syntax.getExpression().accept(this);
        }
        syntax(";");
        nl();
    }

    @Override
    public void visitDeleteStatement(DeleteStatementSyntax syntax) throws Exception {
        lead();
        keyword("delete");
        ws();
        syntax.getExpression().accept(this);
        syntax(";");
        nl();
    }

    @Override
    public void visitTryStatement(TryStatementSyntax syntax) throws Exception {
        lead();
        keyword("try");
        ws();
        syntax.getBlock().accept(this);
        visitList(syntax.getCatches());
        if (syntax.getFinally() != null) {
            ws();
            syntax.getFinally().accept(this);
        }
        nl();
    }

    @Override
    public void visitTypeConstraint(TypeConstraintSyntax syntax) throws Exception {
        syntax.getConstrainedType().accept(this);
    }

    @Override
    public void visitTypeDeclaration(TypeDeclarationSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), false);

        lead();
        visitModifiers(syntax.getModifiers());
        keyword(syntax.getType().toString().toLowerCase());
        if (syntax.getModifiers().contains(Modifier.TRACKED)) {
            syntax("^");
        }
        ws();
        syntax.getIdentifier().accept(this);
        visitTypeParameterList(syntax.getTypeParameters());
        visitBaseTypes(syntax.getBaseTypes());
        visitTypeParameterConstraintClauseList(syntax.getConstraintClauses());
        ws();
        syntax("{");
        nl();
        indent();

        boolean hadOne = false;
        for (MemberDeclarationSyntax member : syntax.getMembers()) {
            if (hadOne) {
                nl();
            } else {
                hadOne = true;
            }
            member.accept(this);
        }

        unIndent();
        lead();
        syntax("}");
        nl();
    }

    private void visitTypeParameterConstraintClauseList(ImmutableArray<TypeParameterConstraintClauseSyntax> constraintClauses) throws Exception {
        indent();
        for (TypeParameterConstraintClauseSyntax constraintClause : constraintClauses) {
            nl();
            lead();
            constraintClause.accept(this);
        }
        unIndent();
    }

    private void visitBaseTypes(ImmutableArray<TypeSyntax> baseTypes) throws Exception {
        if (baseTypes.size() > 0) {
            ws();
            syntax(":");
            ws();
            visitCommaList(baseTypes);
        }
    }

    private void visitTypeParameterList(ImmutableArray<TypeParameterSyntax> typeParameters) throws Exception {
        if (typeParameters.size() > 0) {
            syntax("<");
            visitCommaList(typeParameters);
            syntax(">");
        }
    }

    @Override
    public void visitTypeOfExpression(TypeOfExpressionSyntax syntax) throws Exception {
        keyword("typeof");
        syntax("(");
        syntax.getTargetType().accept(this);
        syntax(")");
    }

    @Override
    public void visitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax) throws Exception {
        keyword("where");
        ws();
        syntax.getName().accept(this);
        ws();
        syntax(":");
        ws();
        visitCommaList(syntax.getConstraints());
    }

    @Override
    public void visitTypeParameter(TypeParameterSyntax syntax) throws Exception {
        visitAttributeListList(syntax.getAttributeLists(), true);
        if (syntax.getVariance() != Variance.NONE) {
            keyword(syntax.getVariance().toString().toLowerCase());
            ws();
        }
        syntax.getIdentifier().accept(this);
    }

    @Override
    public void visitUsingStatement(UsingStatementSyntax syntax) throws Exception {
        lead();
        keyword("using");
        ws();
        if (syntax.getExpression() != null) {
            syntax.getExpression().accept(this);
        } else {
            syntax.getDeclaration().accept(this);
        }
        visitOuterBlock(syntax.getBlock());
    }

    @Override
    public void visitVarType(VarTypeSyntax syntax) throws Exception {
        keyword("var");
    }

    @Override
    public void visitVariableDeclarationExpression(VariableDeclarationExpressionSyntax syntax) throws Exception {
        syntax.getVariableType().accept(this);
        ws();
        syntax.getIdentifier().accept(this);
    }

    @Override
    public void visitVariableDeclaration(VariableDeclarationSyntax syntax) throws Exception {
        syntax.getVariableType().accept(this);
        ws();
        visitCommaList(syntax.getVariables());
    }

    @Override
    public void visitVariableDeclarator(VariableDeclaratorSyntax syntax) throws Exception {
        syntax.getIdentifier().accept(this);
        if (syntax.getValue() != null) {
            ws();
            syntax("=");
            ws();
            syntax.getValue().accept(this);
        }
    }

    @Override
    public void visitWhileStatement(WhileStatementSyntax syntax) throws Exception {
        lead();
        keyword("while");
        ws();
        syntax.getCondition().accept(this);
        visitOuterBlock(syntax.getBlock());
    }
}
