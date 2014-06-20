using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;
using Nullable = Truss.Compiler.Syntax.Nullable;

namespace Truss.Compiler.Printing {
    public class SyntaxPrinter : ISyntaxVisitor {
        private readonly IPrinter _printer;

        public bool Done {
            get { return false; }
        }

        public SyntaxPrinter(IPrinter printer) {
            if (printer == null) {
                throw new ArgumentNullException("printer");
            }

            _printer = printer;
        }

        private void Identifier(string identifier) {
            _printer.Identifier(identifier);
        }

        private void Keyword(string keyword) {
            _printer.Keyword(keyword);
        }

        private void Nl() {
            _printer.Nl();
        }

        private void Literal(string value, LiteralType type) {
            switch (type) {
                case LiteralType.True:
                case LiteralType.False:
                case LiteralType.Nil:
                    _printer.Keyword(type.ToString().ToLower());
                    break;

                default:
                    _printer.Literal(value, type);
                    break;
            }
        }

        private void UnIndent() {
            _printer.UnIndent();
        }

        private void Lead() {
            _printer.Lead();
        }

        private void Syntax(string syntax) {
            _printer.Syntax(syntax);
        }

        private void Indent() {
            _printer.Indent();
        }

        private void Ws() {
            _printer.Ws();
        }

        private void VisitAttributeListList(ImmutableArray<AttributeListSyntax> attributes, bool inline) {
            foreach (var attribute in attributes) {
                if (!inline) {
                    Lead();
                }
                attribute.Accept(this);
                if (inline) {
                    Ws();
                } else {
                    Nl();
                }
            }
        }

        private void VisitModifiers(ImmutableArray<Modifier> modifiers) {
            foreach (var modifier in modifiers) {
                if (modifier != Modifier.Tracked) {
                    Keyword(modifier.ToString().ToLower());
                    Ws();
                }
            }
        }

        private bool VisitList<T>(ImmutableArray<T> nodes)
            where T : SyntaxNode {
            foreach (var node in nodes) {
                node.Accept(this);
            }

            return nodes.Count > 0;
        }

        private void VisitCommaList<T>(ImmutableArray<T> nodes)
            where T : SyntaxNode {
            bool hadOne = false;
            foreach (var node in nodes) {
                if (hadOne) {
                    Syntax(",");
                    Ws();
                } else {
                    hadOne = true;
                }
                node.Accept(this);
            }
        }

        public void VisitAccessorDeclaration(AccessorDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            Keyword(syntax.Type.ToString().ToLower());
            VisitOuterBlock(syntax.Body);
        }

        private void VisitOuterBlock(BlockSyntax body) {
            Ws();
            body.Accept(this);
            Nl();
        }

        public void VisitAliasQualifiedName(AliasQualifiedNameSyntax syntax) {
            if (WellKnownNames.AliasGlobal == syntax.Alias.Identifier) {
                Keyword(WellKnownNames.AliasGlobal);
            } else {
                syntax.Alias.Accept(this);
            }
            Syntax("::");
            syntax.Name.Accept(this);
        }

        public void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax syntax) {
            Keyword("new");
            Ws();
            Syntax("{");
            var initializers = syntax.Initializers;
            if (initializers.Count == 0) {
                Ws();
            } else {
                Nl();
                Indent();
                for (int i = 0; i < initializers.Count; i++) {
                    Lead();
                    initializers[i].Accept(this);
                    if (i < initializers.Count - 1) {
                        Syntax(",");
                    }
                    Nl();
                }
                UnIndent();
                Lead();
                Syntax("}");
            }
        }

        public void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax syntax) {
            if (syntax.Name != null) {
                syntax.Name.Accept(this);
                Ws();
                Syntax("=");
                Ws();
            }
            syntax.Expression.Accept(this);
        }

        public void VisitArgument(ArgumentSyntax syntax) {
            VisitParameterModifiers(syntax.Modifiers);
            syntax.Expression.Accept(this);
        }

        public void VisitArrayCreationExpression(ArrayCreationExpressionSyntax syntax) {
            Keyword("new");
            Ws();
            syntax.Type.Accept(this);
            if (syntax.Initializer != null) {
                Ws();
                syntax.Initializer.Accept(this);
            }
        }

        public void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax syntax) {
            Syntax("[");
            syntax.Size.Accept(this);
            Syntax("]");
        }

        public void VisitArrayType(ArrayTypeSyntax syntax) {
            syntax.ElementType.Accept(this);
            VisitList(syntax.RankSpecifiers);
        }

        public void VisitAssertStatement(AssertStatementSyntax syntax) {
            Lead();
            Keyword("assert");
            Ws();
            syntax.Expression.Accept(this);
            Syntax(";");
            Nl();
        }

        public void VisitAttributeArgument(AttributeArgumentSyntax syntax) {
            if (syntax.Name != null) {
                syntax.Name.Accept(this);
                Ws();
                Syntax("=");
                Ws();
            }
            syntax.Expression.Accept(this);
        }

        public void VisitAttributeList(AttributeListSyntax syntax) {
            Syntax("[");
            if (syntax.Target != AttributeTarget.None) {
                Keyword(syntax.Target.ToString().ToLower());
                Syntax(":");
                Ws();
            }
            VisitCommaList(syntax.Attributes);
            Syntax("]");
        }

        public void VisitAttribute(AttributeSyntax syntax) {
            syntax.Name.Accept(this);
            if (syntax.Arguments.Count > 0) {
                Syntax("(");
                VisitCommaList(syntax.Arguments);
                Syntax(")");
            }
        }

        public void VisitAwaitExpression(AwaitExpressionSyntax syntax) {
            Keyword("await");
            Ws();
            syntax.Expression.Accept(this);
        }

        public void VisitBinaryExpression(BinaryExpressionSyntax syntax) {
            syntax.Left.Accept(this);
            Ws();
            switch (syntax.Operator) {
                case BinaryOperator.Ampersand:
                    Syntax("&");
                    break;
                case BinaryOperator.AmpersandAmpersand:
                    Syntax("&&");
                    break;
                case BinaryOperator.AmpersandEquals:
                    Syntax("&=");
                    break;
                case BinaryOperator.As:
                    Keyword("as");
                    break;
                case BinaryOperator.Asterisk:
                    Syntax("*");
                    break;
                case BinaryOperator.AsteriskEquals:
                    Syntax("*=");
                    break;
                case BinaryOperator.Bar:
                    Syntax("|");
                    break;
                case BinaryOperator.BarBar:
                    Syntax("||");
                    break;
                case BinaryOperator.BarEquals:
                    Syntax("|=");
                    break;
                case BinaryOperator.Caret:
                    Syntax("^");
                    break;
                case BinaryOperator.CaretEquals:
                    Syntax("^=");
                    break;
                case BinaryOperator.Equals:
                    Syntax("=");
                    break;
                case BinaryOperator.EqualsEquals:
                    Syntax("==");
                    break;
                case BinaryOperator.ExclamationEquals:
                    Syntax("!=");
                    break;
                case BinaryOperator.GreaterThan:
                    Syntax(">");
                    break;
                case BinaryOperator.GreaterThanEquals:
                    Syntax(">=");
                    break;
                case BinaryOperator.GreaterThanGreaterThan:
                    Syntax(">>");
                    break;
                case BinaryOperator.GreaterThanGreaterThanEquals:
                    Syntax(">>=");
                    break;
                case BinaryOperator.Is:
                    Keyword("is");
                    break;
                case BinaryOperator.LessThan:
                    Syntax("<");
                    break;
                case BinaryOperator.LessThanEquals:
                    Syntax("<=");
                    break;
                case BinaryOperator.LessThanLessThan:
                    Syntax("<<");
                    break;
                case BinaryOperator.LessThanLessThanEquals:
                    Syntax("<<=");
                    break;
                case BinaryOperator.Minus:
                    Syntax("-");
                    break;
                case BinaryOperator.MinusEquals:
                    Syntax("-=");
                    break;
                case BinaryOperator.Percent:
                    Syntax("%");
                    break;
                case BinaryOperator.PercentEquals:
                    Syntax("%=");
                    break;
                case BinaryOperator.Plus:
                    Syntax("+");
                    break;
                case BinaryOperator.PlusEquals:
                    Syntax("+=");
                    break;
                case BinaryOperator.QuestionQuestion:
                    Syntax("??");
                    break;
                case BinaryOperator.Slash:
                    Syntax("/");
                    break;
                case BinaryOperator.SlashEquals:
                    Syntax("/=");
                    break;
                default:
                    throw new InvalidOperationException();
            }
            Ws();
            syntax.Right.Accept(this);
        }

        public void VisitBlock(BlockSyntax syntax) {
            Syntax("{");
            Nl();
            Indent();
            foreach (var statement in syntax.Statements) {
                statement.Accept(this);
            }
            UnIndent();
            Lead();
            Syntax("}");
        }

        public void VisitBreakStatement(BreakStatementSyntax syntax) {
            Lead();
            Keyword("break");
            Syntax(";");
            Nl();
        }

        public void VisitCastExpression(CastExpressionSyntax syntax) {
            Syntax("(");
            syntax.TargetType.Accept(this);
            Syntax(")");
            syntax.Expression.Accept(this);
        }

        public void VisitCatchClause(CatchClauseSyntax syntax) {
            Ws();
            Keyword("catch");
            if (syntax.ExceptionType != null) {
                Ws();
                syntax.ExceptionType.Accept(this);
                if (syntax.Identifier != null) {
                    Ws();
                    syntax.Identifier.Accept(this);
                }
            } else {
                Debug.Assert(syntax.Identifier == null);
            }
            Ws();
            syntax.Block.Accept(this);
        }

        public void VisitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax) {
            switch (syntax.Family) {
                case TypeFamily.Any:
                    break;

                case TypeFamily.Class:
                    Keyword("class");
                    break;

                case TypeFamily.Struct:
                    Keyword("struct");
                    break;

                case TypeFamily.Tracked:
                    Keyword("struct");
                    Syntax("^");
                    break;
            }

            if (syntax.Nullable.HasValue) {
                PrintNullable(syntax.Nullable.Value);
            }
        }

        private void PrintNullable(Nullable nullable) {
            Syntax(nullable == Nullable.Nullable ? "?" : "!");
        }

        public void VisitCompilationUnit(CompilationUnitSyntax syntax) {
            bool hadOne;

            hadOne = VisitList(syntax.Imports);
            if (syntax.AttributeLists.Count > 0 && hadOne) {
                Nl();
            }
            VisitAttributeListList(syntax.AttributeLists, false);
            foreach (var member in syntax.Members) {
                Nl();
                member.Accept(this);
            }
        }

        public void VisitConditionalExpression(ConditionalExpressionSyntax syntax) {
            syntax.Condition.Accept(this);
            Ws();
            Syntax("?");
            Ws();
            syntax.WhenTrue.Accept(this);
            Ws();
            Syntax(":");
            Ws();
            syntax.WhenFalse.Accept(this);
        }

        public void VisitConstructorConstraint(ConstructorConstraintSyntax syntax) {
            Keyword("new");
            Syntax("()");
        }

        public void VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            syntax.Identifier.Accept(this);
            VisitParameterList(syntax.Parameters);
            if (syntax.Initializer != null) {
                Nl();
                Indent();
                Lead();
                syntax.Initializer.Accept(this);
                UnIndent();
            }
            VisitOuterBlock(syntax.Body);
        }

        public void VisitConstructorInitializer(ConstructorInitializerSyntax syntax) {
            Syntax(":");
            Ws();
            Keyword(syntax.Type.ToString().ToLower());
            VisitArgumentList(syntax.Arguments);
        }

        private void VisitArgumentList(ImmutableArray<ArgumentSyntax> arguments) {
            Syntax("(");
            VisitCommaList(arguments);
            Syntax(")");
        }

        public void VisitContinueStatement(ContinueStatementSyntax syntax) {
            Lead();
            Keyword("continue");
            Syntax(";");
            Nl();
        }

        public void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            Keyword(syntax.Type.ToString().ToLower());
            Ws();
            Keyword("operator");
            Ws();
            syntax.TargetType.Accept(this);
            VisitParameterList(syntax.Parameters);
            VisitOuterBlock(syntax.Body);
        }

        public void VisitDefaultExpression(DefaultExpressionSyntax syntax) {
            Keyword("default");
            Syntax("(");
            syntax.TargetType.Accept(this);
            Syntax(")");
        }

        public void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            Keyword("delegate");
            Ws();
            syntax.ReturnType.Accept(this);
            Ws();
            syntax.Identifier.Accept(this);
            VisitTypeParameterList(syntax.TypeParameters);
            VisitParameterList(syntax.Parameters);
            VisitTypeParameterConstraintClauseList(syntax.ConstraintClauses);
            Syntax(";");
            Nl();
        }

        public void VisitDestructorDeclaration(DestructorDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            Debug.Assert(syntax.Modifiers.Count == 0);
            Syntax("~");
            syntax.Identifier.Accept(this);
            VisitParameterList(syntax.Parameters);
            VisitOuterBlock(syntax.Body);
        }

        public void VisitDoStatement(DoStatementSyntax syntax) {
            Lead();
            Keyword("do");
            Ws();
            syntax.Block.Accept(this);
            Ws();
            Keyword("while");
            Ws();
            syntax.Condition.Accept(this);
            Syntax(";");
            Nl();
        }

        public void VisitElementAccessExpression(ElementAccessExpressionSyntax syntax) {
            syntax.Expression.Accept(this);
            Syntax("[");
            VisitCommaList(syntax.IndexExpressions);
            Syntax("]");
        }

        public void VisitElseClause(ElseClauseSyntax syntax) {
            Ws();
            Keyword(syntax.Type.ToString().ToLower());
            if (syntax.Type == ElIfOrElse.ElIf) {
                Ws();
                syntax.Condition.Accept(this);
            }
            Ws();
            syntax.Block.Accept(this);
        }

        public void VisitEmptyStatement(EmptyStatementSyntax syntax) {
            Lead();
            Syntax(";");
            Nl();
        }

        public void VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);

            Lead();
            VisitModifiers(syntax.Modifiers);
            Keyword("enum");
            Ws();
            syntax.Identifier.Accept(this);
            VisitBaseTypes(syntax.BaseTypes);
            Ws();
            Syntax("{");
            Nl();
            Indent();

            for (int i = 0; i < syntax.Members.Count; i++) {
                var member = syntax.Members[i];
                VisitAttributeListList(member.AttributeLists, false);
                Lead();
                member.Accept(this);
                if (i < syntax.Members.Count - 1) {
                    Syntax(",");
                }
                Nl();
            }

            UnIndent();
            Lead();
            Syntax("}");
            Nl();
        }

        public void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) {
            syntax.Identifier.Accept(this);
            if (syntax.Value != null) {
                Ws();
                Syntax("=");
                Ws();
                syntax.Value.Accept(this);
            }
        }

        public void VisitEventDeclaration(EventDeclarationSyntax syntax) {
            VisitBasePropertyDeclaration(syntax);
        }

        private void VisitBasePropertyDeclaration(BasePropertyDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            if (syntax.Kind == SyntaxKind.EventDeclaration) {
                Keyword("event");
                Ws();
            }
            syntax.Type.Accept(this);
            Ws();
            if (syntax.ExplicitInterfaceSpecifier != null) {
                syntax.ExplicitInterfaceSpecifier.Accept(this);
                Syntax(".");
            }
            switch (syntax.Kind) {
                case SyntaxKind.EventDeclaration:
                    ((EventDeclarationSyntax)syntax).Identifier.Accept(this);
                    break;

                case SyntaxKind.PropertyDeclaration:
                    ((PropertyDeclarationSyntax)syntax).Identifier.Accept(this);
                    break;

                case SyntaxKind.IndexerDeclaration:
                    Keyword("this");
                    Syntax("[");
                    VisitCommaList(((IndexerDeclarationSyntax)syntax).Parameters);
                    Syntax("]");
                    break;

                default:
                    throw new InvalidOperationException();
            }
            Ws();
            Syntax("{");
            Nl();
            Indent();
            VisitList(syntax.Accessors);
            UnIndent();
            Lead();
            Syntax("}");
            Nl();
        }

        public void VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) {
            VisitBaseFieldDeclaration(syntax);
        }

        private void VisitBaseFieldDeclaration(BaseFieldDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            if (syntax is EventFieldDeclarationSyntax) {
                Keyword("event");
                Ws();
            }
            syntax.Declaration.Accept(this);
            Syntax(";");
            Nl();
        }

        public void VisitExpressionStatement(ExpressionStatementSyntax syntax) {
            Lead();
            syntax.Expression.Accept(this);
            Syntax(";");
            Nl();
        }

        public void VisitFieldDeclaration(FieldDeclarationSyntax syntax) {
            VisitBaseFieldDeclaration(syntax);
        }

        public void VisitFinallyClause(FinallyClauseSyntax syntax) {
            Keyword("finally");
            Ws();
            syntax.Block.Accept(this);
        }

        public void VisitForEachStatement(ForEachStatementSyntax syntax) {
            Lead();
            Keyword("foreach");
            Ws();
            syntax.ElementType.Accept(this);
            Ws();
            syntax.Identifier.Accept(this);
            Ws();
            Keyword("in");
            Ws();
            syntax.Expression.Accept(this);
            VisitOuterBlock(syntax.Block);
        }

        public void VisitForStatement(ForStatementSyntax syntax) {
            Lead();
            Keyword("for");
            Ws();
            if (syntax.Declaration != null) {
                syntax.Declaration.Accept(this);
            }
            if (syntax.Initializers != null) {
                VisitCommaList(syntax.Initializers);
            }
            Syntax(";");
            if (syntax.Condition != null) {
                Ws();
                syntax.Condition.Accept(this);
            }
            Syntax(";");
            if (syntax.Incrementors != null) {
                Ws();
                VisitCommaList(syntax.Incrementors);
            }
            VisitOuterBlock(syntax.Block);
        }

        public void VisitGenericName(GenericNameSyntax syntax) {
            Identifier(syntax.Identifier);
            Syntax("<");
            bool hadOne = false;
            foreach (var type in syntax.TypeArguments) {
                if (hadOne) {
                    Syntax(",");
                    if (type.Kind != SyntaxKind.OmittedTypeArgument) {
                        Ws();
                    }
                } else {
                    hadOne = true;
                }
                type.Accept(this);
            }
            Syntax(">");
        }

        public void VisitIdentifierName(IdentifierNameSyntax syntax) {
            Identifier(syntax.Identifier);
        }

        public void VisitIfStatement(IfStatementSyntax syntax) {
            Lead();
            Keyword("if");
            Ws();
            syntax.Condition.Accept(this);
            Ws();
            syntax.Block.Accept(this);
            VisitList(syntax.Elses);
            Nl();
        }

        public void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax syntax) {
            Keyword("new");
            VisitList(syntax.RankSpecifiers);
            Ws();
            syntax.Initializer.Accept(this);
        }

        public void VisitImportDirective(ImportDirectiveSyntax syntax) {
            Lead();
            Keyword("import");
            Ws();
            if (syntax.IsStatic) {
                Keyword("static");
                Ws();
            }
            if (syntax.Alias != null) {
                syntax.Alias.Accept(this);
                Syntax("=");
            }
            syntax.Name.Accept(this);
            Syntax(";");
            Nl();
        }

        public void VisitIndexerDeclaration(IndexerDeclarationSyntax syntax) {
            VisitBasePropertyDeclaration(syntax);
        }

        public void VisitInitializerExpression(InitializerExpressionSyntax syntax) {
            Syntax("{");
            var expressions = syntax.Expressions;
            if (expressions.Count == 0) {
                Ws();
            } else {
                Nl();
                Indent();
                for (int i = 0; i < expressions.Count; i++) {
                    Lead();
                    expressions[i].Accept(this);
                    if (i < expressions.Count - 1) {
                        Syntax(",");
                    }
                    Nl();
                }
                UnIndent();
                Lead();
            }
            Syntax("}");
        }

        public void VisitInstanceExpression(InstanceExpressionSyntax syntax) {
            Keyword(syntax.Type == ThisOrBase.Base ? "base" : "this");
        }

        public void VisitInvocationExpression(InvocationExpressionSyntax syntax) {
            syntax.Expression.Accept(this);
            VisitArgumentList(syntax.Arguments);
        }

        public void VisitLambdaExpression(LambdaExpressionSyntax syntax) {
            VisitModifiers(syntax.Modifiers);
            // Check whether we should omit the parens.
            bool printParens = true;
            if (syntax.Parameters.Count == 1) {
                var firstParameter = syntax.Parameters[0];
                printParens =
                    firstParameter.AttributeLists.Count > 0 ||
                    firstParameter.ParameterType != null ||
                    firstParameter.Modifiers.Count > 0;
            }
            if (printParens) {
                Syntax("(");
            }
            VisitCommaList(syntax.Parameters);
            if (printParens) {
                Syntax(")");
            }
            Ws();
            Syntax("=>");
            Ws();
            syntax.Body.Accept(this);
        }

        public void VisitLiteralExpression(LiteralExpressionSyntax syntax) {
            Literal(syntax.Value, syntax.LiteralType);
        }

        public void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax) {
            Lead();
            VisitModifiers(syntax.Modifiers);
            syntax.Declaration.Accept(this);
            Syntax(";");
            Nl();
        }

        public void VisitLoopStatement(LoopStatementSyntax syntax) {
            Lead();
            Keyword("loop");
            VisitOuterBlock(syntax.Block);
        }

        public void VisitMemberAccessExpression(MemberAccessExpressionSyntax syntax) {
            syntax.Expression.Accept(this);
            Syntax(".");
            syntax.Name.Accept(this);
        }

        public void VisitMethodDeclaration(MethodDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            syntax.ReturnType.Accept(this);
            Ws();
            if (syntax.ExplicitInterfaceSpecifier != null) {
                syntax.ExplicitInterfaceSpecifier.Accept(this);
                Syntax(".");
            }
            syntax.Identifier.Accept(this);
            VisitTypeParameterList(syntax.TypeParameters);
            VisitParameterList(syntax.Parameters);
            VisitTypeParameterConstraintClauseList(syntax.ConstraintClauses);
            VisitOuterBlock(syntax.Body);
        }

        private void VisitParameterList(ImmutableArray<ParameterSyntax> parameters) {
            Syntax("(");
            VisitCommaList(parameters);
            Syntax(")");
        }

        public void VisitNakedNullableType(NakedNullableTypeSyntax syntax) {
            PrintNullable(syntax.Type);
        }

        public void VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) {
            Lead();
            Keyword("namespace");
            Ws();
            syntax.Name.Accept(this);
            Ws();
            Syntax("{");
            Nl();
            Indent();

            bool hadOne = VisitList(syntax.Imports);

            foreach (var member in syntax.Members) {
                if (hadOne) {
                    Nl();
                } else {
                    hadOne = true;
                }
                member.Accept(this);
            }

            UnIndent();
            Lead();
            Syntax("}");
            Nl();
        }

        public void VisitNullableType(NullableTypeSyntax syntax) {
            syntax.ElementType.Accept(this);
            Syntax("?");
        }

        public void VisitTrackedType(TrackedTypeSyntax syntax) {
            syntax.ElementType.Accept(this);
            Syntax("^");
        }

        public void VisitObjectCreationExpression(ObjectCreationExpressionSyntax syntax) {
            Keyword("new");
            Ws();
            syntax.TargetType.Accept(this);
            if (syntax.Arguments != null) {
                VisitArgumentList(syntax.Arguments);
            }
            if (syntax.Initializer != null) {
                Ws();
                syntax.Initializer.Accept(this);
            }
        }

        public void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax syntax) {
            // Nothing to do.
        }

        public void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax syntax) {
            // Nothing to do.
        }

        public void VisitOperatorDeclaration(OperatorDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);
            Lead();
            VisitModifiers(syntax.Modifiers);
            syntax.ReturnType.Accept(this);
            Ws();
            Keyword("operator");
            Ws();
            switch (syntax.Operator) {
                case Operator.Ampersand:
                    Syntax("&");
                    break;
                case Operator.Asterisk:
                    Syntax("*");
                    break;
                case Operator.Bar:
                    Syntax("|");
                    break;
                case Operator.Caret:
                    Syntax("^");
                    break;
                case Operator.EqualsEquals:
                    Syntax("==");
                    break;
                case Operator.Exclamation:
                    Syntax("!");
                    break;
                case Operator.ExclamationEquals:
                    Syntax("!=");
                    break;
                case Operator.False:
                    Keyword("false");
                    break;
                case Operator.GreaterThan:
                    Syntax(">");
                    break;
                case Operator.GreaterThanEquals:
                    Syntax(">=");
                    break;
                case Operator.GreaterThanGreaterThan:
                    Syntax(">>");
                    break;
                case Operator.LessThan:
                    Syntax("<");
                    break;
                case Operator.LessThanEquals:
                    Syntax("<=");
                    break;
                case Operator.LessThanLessThan:
                    Syntax("<<");
                    break;
                case Operator.Minus:
                    Syntax("-");
                    break;
                case Operator.MinusMinus:
                    Syntax("--");
                    break;
                case Operator.Percent:
                    Syntax("%");
                    break;
                case Operator.Plus:
                    Syntax("+");
                    break;
                case Operator.PlusPlus:
                    Syntax("++");
                    break;
                case Operator.Slash:
                    Syntax("/");
                    break;
                case Operator.Tilde:
                    Syntax("~");
                    break;
                case Operator.True:
                    Keyword("true");
                    break;
                default:
                    throw new InvalidOperationException();
            }
            VisitParameterList(syntax.Parameters);
            VisitOuterBlock(syntax.Body);
        }

        public void VisitParameter(ParameterSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, true);
            VisitParameterModifiers(syntax.Modifiers);
            // For implicitly typed lambdas.
            if (syntax.ParameterType != null) {
                syntax.ParameterType.Accept(this);
                Ws();
            }
            syntax.Identifier.Accept(this);
        }

        private void VisitParameterModifiers(ImmutableArray<ParameterModifier> modifiers) {
            foreach (var modifier in modifiers) {
                Keyword(modifier.ToString().ToLower());
                Ws();
            }
        }

        public void VisitParenthesizedExpression(ParenthesizedExpressionSyntax syntax) {
            Syntax("(");
            syntax.Expression.Accept(this);
            Syntax(")");
        }

        public void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax syntax) {
            syntax.Operand.Accept(this);
            switch (syntax.Operator) {
                case PostfixUnaryOperator.MinusMinus:
                    Syntax("--");
                    break;
                case PostfixUnaryOperator.PlusPlus:
                    Syntax("++");
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void VisitPredefinedType(PredefinedTypeSyntax syntax) {
            Keyword(syntax.PredefinedType.ToString().ToLower());
        }

        public void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax syntax) {
            switch (syntax.Operator) {
                case PrefixUnaryOperator.Ampersand:
                    Syntax("&");
                    break;
                case PrefixUnaryOperator.Exclamation:
                    Syntax("!");
                    break;
                case PrefixUnaryOperator.Minus:
                    Syntax("-");
                    break;
                case PrefixUnaryOperator.MinusMinus:
                    Syntax("--");
                    break;
                case PrefixUnaryOperator.Plus:
                    Syntax("+");
                    break;
                case PrefixUnaryOperator.PlusPlus:
                    Syntax("++");
                    break;
                case PrefixUnaryOperator.Tilde:
                    Syntax("~");
                    break;
                default:
                    throw new InvalidOperationException();
            }
            syntax.Operand.Accept(this);
        }

        public void VisitPropertyDeclaration(PropertyDeclarationSyntax syntax) {
            VisitBasePropertyDeclaration(syntax);
        }

        public void VisitQualifiedName(QualifiedNameSyntax syntax) {
            syntax.Left.Accept(this);
            Syntax(".");
            syntax.Right.Accept(this);
        }

        public void VisitReturnStatement(ReturnStatementSyntax syntax) {
            Lead();
            Keyword("return");
            if (syntax.Expression != null) {
                Ws();
                syntax.Expression.Accept(this);
            }
            Syntax(";");
            Nl();
        }

        public void VisitSizeOfExpression(SizeOfExpressionSyntax syntax) {
            Keyword("sizeof");
            Syntax("(");
            syntax.TargetType.Accept(this);
            Syntax(")");
        }

        public void VisitSwitchSection(SwitchSectionSyntax syntax) {
            Lead();
            Keyword(syntax.Type.ToString().ToLower());
            if (syntax.Type == CaseOrDefault.Case) {
                Ws();
                VisitCommaList(syntax.Values);
            }
            VisitOuterBlock(syntax.Block);
        }

        public void VisitSwitchStatement(SwitchStatementSyntax syntax) {
            Lead();
            Keyword("switch");
            Ws();
            syntax.Expression.Accept(this);
            Ws();
            Syntax("{");
            Nl();
            Indent();
            VisitList(syntax.Sections);
            UnIndent();
            Lead();
            Syntax("}");
            Nl();
        }

        public void VisitThrowStatement(ThrowStatementSyntax syntax) {
            Lead();
            Keyword("throw");
            if (syntax.Expression != null) {
                Ws();
                syntax.Expression.Accept(this);
            }
            Syntax(";");
            Nl();
        }

        public void VisitDeleteStatement(DeleteStatementSyntax syntax) {
            Lead();
            Keyword("delete");
            Ws();
            syntax.Expression.Accept(this);
            Syntax(";");
            Nl();
        }

        public void VisitTryStatement(TryStatementSyntax syntax) {
            Lead();
            Keyword("try");
            Ws();
            syntax.Block.Accept(this);
            VisitList(syntax.Catches);
            if (syntax.Finally != null) {
                Ws();
                syntax.Finally.Accept(this);
            }
            Nl();
        }

        public void VisitTypeConstraint(TypeConstraintSyntax syntax) {
            syntax.ConstrainedType.Accept(this);
        }

        public void VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, false);

            Lead();
            VisitModifiers(syntax.Modifiers);
            Keyword(syntax.Type.ToString().ToLower());
            if (syntax.Modifiers.Contains(Modifier.Tracked)) {
                Syntax("^");
            }
            Ws();
            syntax.Identifier.Accept(this);
            VisitTypeParameterList(syntax.TypeParameters);
            VisitBaseTypes(syntax.BaseTypes);
            VisitTypeParameterConstraintClauseList(syntax.ConstraintClauses);
            Ws();
            Syntax("{");
            Nl();
            Indent();

            bool hadOne = false;
            foreach (var member in syntax.Members) {
                if (hadOne) {
                    Nl();
                } else {
                    hadOne = true;
                }
                member.Accept(this);
            }

            UnIndent();
            Lead();
            Syntax("}");
            Nl();
        }

        private void VisitTypeParameterConstraintClauseList(ImmutableArray<TypeParameterConstraintClauseSyntax> constraintClauses) {
            Indent();
            foreach (var constraintClause in constraintClauses) {
                Nl();
                Lead();
                constraintClause.Accept(this);
            }
            UnIndent();
        }

        private void VisitBaseTypes(ImmutableArray<TypeSyntax> baseTypes) {
            if (baseTypes.Count > 0) {
                Ws();
                Syntax(":");
                Ws();
                VisitCommaList(baseTypes);
            }
        }

        private void VisitTypeParameterList(ImmutableArray<TypeParameterSyntax> typeParameters) {
            if (typeParameters.Count > 0) {
                Syntax("<");
                VisitCommaList(typeParameters);
                Syntax(">");
            }
        }

        public void VisitTypeOfExpression(TypeOfExpressionSyntax syntax) {
            Keyword("typeof");
            Syntax("(");
            syntax.TargetType.Accept(this);
            Syntax(")");
        }

        public void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax) {
            Keyword("where");
            Ws();
            syntax.Name.Accept(this);
            Ws();
            Syntax(":");
            Ws();
            VisitCommaList(syntax.Constraints);
        }

        public void VisitTypeParameter(TypeParameterSyntax syntax) {
            VisitAttributeListList(syntax.AttributeLists, true);
            if (syntax.Variance != Variance.None) {
                Keyword(syntax.Variance.ToString().ToLower());
                Ws();
            }
            syntax.Identifier.Accept(this);
        }

        public void VisitUsingStatement(UsingStatementSyntax syntax) {
            Lead();
            Keyword("using");
            Ws();
            if (syntax.Expression != null) {
                syntax.Expression.Accept(this);
            } else {
                syntax.Declaration.Accept(this);
            }
            VisitOuterBlock(syntax.Block);
        }

        public void VisitVarType(VarTypeSyntax syntax) {
            Keyword("var");
        }

        public void VisitVariableDeclarationExpression(VariableDeclarationExpressionSyntax syntax) {
            syntax.VariableType.Accept(this);
            Ws();
            syntax.Identifier.Accept(this);
        }

        public void VisitVariableDeclaration(VariableDeclarationSyntax syntax) {
            syntax.VariableType.Accept(this);
            Ws();
            VisitCommaList(syntax.Variables);
        }

        public void VisitVariableDeclarator(VariableDeclaratorSyntax syntax) {
            syntax.Identifier.Accept(this);
            if (syntax.Value != null) {
                Ws();
                Syntax("=");
                Ws();
                syntax.Value.Accept(this);
            }
        }

        public void VisitWhileStatement(WhileStatementSyntax syntax) {
            Lead();
            Keyword("while");
            Ws();
            syntax.Condition.Accept(this);
            VisitOuterBlock(syntax.Block);
        }
    }
}
