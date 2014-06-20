using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;

namespace Truss.Compiler.Syntax {
    public class AccessorDeclarationSyntax : SyntaxNode {
        public AccessorDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, AccessorDeclarationType type, BlockSyntax body, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            Type = type;
            Body = body;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public AccessorDeclarationType Type { get; private set; }

        public BlockSyntax Body { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AccessorDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAccessorDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAccessorDeclaration(this);
        }
    }

    public class AliasQualifiedNameSyntax : NameSyntax {
        public AliasQualifiedNameSyntax(IdentifierNameSyntax alias, SimpleNameSyntax name, Span span)
            : base(span) {
            if (alias == null) {
                throw new ArgumentNullException("alias");
            }
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            Alias = alias;
            Name = name;
        }

        public IdentifierNameSyntax Alias { get; private set; }

        public SimpleNameSyntax Name { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AliasQualifiedName; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAliasQualifiedName(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAliasQualifiedName(this);
        }
    }

    public class AnonymousObjectCreationExpressionSyntax : ExpressionSyntax {
        public AnonymousObjectCreationExpressionSyntax(ImmutableArray<AnonymousObjectMemberDeclaratorSyntax> initializers, Span span)
            : base(span) {
            if (initializers == null) {
                throw new ArgumentNullException("initializers");
            }

            Initializers = initializers;
        }

        public ImmutableArray<AnonymousObjectMemberDeclaratorSyntax> Initializers { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AnonymousObjectCreationExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAnonymousObjectCreationExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAnonymousObjectCreationExpression(this);
        }
    }

    public class AnonymousObjectMemberDeclaratorSyntax : SyntaxNode {
        public AnonymousObjectMemberDeclaratorSyntax(IdentifierNameSyntax name, ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Name = name;
            Expression = expression;
        }

        public IdentifierNameSyntax Name { get; private set; }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AnonymousObjectMemberDeclarator; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAnonymousObjectMemberDeclarator(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAnonymousObjectMemberDeclarator(this);
        }
    }

    public class ArgumentSyntax : SyntaxNode {
        public ArgumentSyntax(ImmutableArray<ParameterModifier> modifiers, ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Modifiers = modifiers;
            Expression = expression;
        }

        public ImmutableArray<ParameterModifier> Modifiers { get; private set; }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.Argument; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArgument(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitArgument(this);
        }
    }

    public class ArrayCreationExpressionSyntax : ExpressionSyntax {
        public ArrayCreationExpressionSyntax(ArrayTypeSyntax type, InitializerExpressionSyntax initializer, Span span)
            : base(span) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            Type = type;
            Initializer = initializer;
        }

        public ArrayTypeSyntax Type { get; private set; }

        public InitializerExpressionSyntax Initializer { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ArrayCreationExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArrayCreationExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitArrayCreationExpression(this);
        }
    }

    public class ArrayRankSpecifierSyntax : SyntaxNode {
        public ArrayRankSpecifierSyntax(ExpressionSyntax size, bool isTracked, Span span)
            : base(span) {
            if (size == null) {
                throw new ArgumentNullException("size");
            }

            Size = size;
            IsTracked = isTracked;
        }

        public ExpressionSyntax Size { get; private set; }

        public bool IsTracked { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ArrayRankSpecifier; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArrayRankSpecifier(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitArrayRankSpecifier(this);
        }
    }

    public class ArrayTypeSyntax : TypeSyntax {
        public ArrayTypeSyntax(TypeSyntax elementType, ImmutableArray<ArrayRankSpecifierSyntax> rankSpecifiers, Span span)
            : base(span) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }
            if (rankSpecifiers == null) {
                throw new ArgumentNullException("rankSpecifiers");
            }

            ElementType = elementType;
            RankSpecifiers = rankSpecifiers;
        }

        public TypeSyntax ElementType { get; private set; }

        public ImmutableArray<ArrayRankSpecifierSyntax> RankSpecifiers { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ArrayType; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArrayType(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitArrayType(this);
        }
    }

    public class AssertStatementSyntax : StatementSyntax {
        public AssertStatementSyntax(ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AssertStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAssertStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAssertStatement(this);
        }
    }

    public class AttributeArgumentSyntax : SyntaxNode {
        public AttributeArgumentSyntax(IdentifierNameSyntax name, ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Name = name;
            Expression = expression;
        }

        public IdentifierNameSyntax Name { get; private set; }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AttributeArgument; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAttributeArgument(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAttributeArgument(this);
        }
    }

    public class AttributeListSyntax : SyntaxNode {
        public AttributeListSyntax(AttributeTarget target, ImmutableArray<AttributeSyntax> attributes, Span span)
            : base(span) {
            if (attributes == null) {
                throw new ArgumentNullException("attributes");
            }

            Target = target;
            Attributes = attributes;
        }

        public AttributeTarget Target { get; private set; }

        public ImmutableArray<AttributeSyntax> Attributes { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AttributeList; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAttributeList(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAttributeList(this);
        }
    }

    public class AttributeSyntax : SyntaxNode {
        public AttributeSyntax(NameSyntax name, ImmutableArray<AttributeArgumentSyntax> arguments, Span span)
            : base(span) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (arguments == null) {
                throw new ArgumentNullException("arguments");
            }

            Name = name;
            Arguments = arguments;
        }

        public NameSyntax Name { get; private set; }

        public ImmutableArray<AttributeArgumentSyntax> Arguments { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.Attribute; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAttribute(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAttribute(this);
        }
    }

    public class AwaitExpressionSyntax : ExpressionSyntax {
        public AwaitExpressionSyntax(ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.AwaitExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAwaitExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitAwaitExpression(this);
        }
    }

    public abstract class BaseFieldDeclarationSyntax : MemberDeclarationSyntax {
        protected BaseFieldDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, VariableDeclarationSyntax declaration, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (declaration == null) {
                throw new ArgumentNullException("declaration");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            Declaration = declaration;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public VariableDeclarationSyntax Declaration { get; private set; }
    }

    public abstract class BaseMethodDeclarationSyntax : MemberDeclarationSyntax {
        protected BaseMethodDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, ImmutableArray<ParameterSyntax> parameters, BlockSyntax body, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            Parameters = parameters;
            Body = body;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public ImmutableArray<ParameterSyntax> Parameters { get; private set; }

        public BlockSyntax Body { get; private set; }
    }

    public abstract class BasePropertyDeclarationSyntax : MemberDeclarationSyntax {
        protected BasePropertyDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, TypeSyntax type, NameSyntax explicitInterfaceSpecifier, ImmutableArray<AccessorDeclarationSyntax> accessors, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            if (accessors == null) {
                throw new ArgumentNullException("accessors");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            Type = type;
            ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            Accessors = accessors;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public TypeSyntax Type { get; private set; }

        public NameSyntax ExplicitInterfaceSpecifier { get; private set; }

        public ImmutableArray<AccessorDeclarationSyntax> Accessors { get; private set; }
    }

    public abstract class BaseTypeDeclarationSyntax : MemberDeclarationSyntax {
        protected BaseTypeDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, IdentifierNameSyntax identifier, ImmutableArray<TypeSyntax> baseTypes, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }
            if (baseTypes == null) {
                throw new ArgumentNullException("baseTypes");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            Identifier = identifier;
            BaseTypes = baseTypes;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public ImmutableArray<TypeSyntax> BaseTypes { get; private set; }
    }

    public class BinaryExpressionSyntax : ExpressionSyntax {
        public BinaryExpressionSyntax(BinaryOperator operator_, ExpressionSyntax left, ExpressionSyntax right, Span span)
            : base(span) {
            if (left == null) {
                throw new ArgumentNullException("left");
            }
            if (right == null) {
                throw new ArgumentNullException("right");
            }

            Operator = operator_;
            Left = left;
            Right = right;
        }

        public BinaryOperator Operator { get; private set; }

        public ExpressionSyntax Left { get; private set; }

        public ExpressionSyntax Right { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.BinaryExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitBinaryExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitBinaryExpression(this);
        }
    }

    public class BlockSyntax : StatementSyntax {
        public BlockSyntax(ImmutableArray<StatementSyntax> statements, Span span)
            : base(span) {
            if (statements == null) {
                throw new ArgumentNullException("statements");
            }

            Statements = statements;
        }

        public ImmutableArray<StatementSyntax> Statements { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.Block; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitBlock(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitBlock(this);
        }
    }

    public class BreakStatementSyntax : StatementSyntax {
        public BreakStatementSyntax(Span span)
            : base(span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.BreakStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitBreakStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitBreakStatement(this);
        }
    }

    public class CastExpressionSyntax : ExpressionSyntax {
        public CastExpressionSyntax(ExpressionSyntax expression, TypeSyntax targetType, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            Expression = expression;
            TargetType = targetType;
        }

        public ExpressionSyntax Expression { get; private set; }

        public TypeSyntax TargetType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.CastExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitCastExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitCastExpression(this);
        }
    }

    public class CatchClauseSyntax : SyntaxNode {
        public CatchClauseSyntax(TypeSyntax exceptionType, IdentifierNameSyntax identifier, BlockSyntax block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            if (identifier != null && exceptionType == null) {
                throw new ArgumentException("Exception type is mandatory when an identifier is provided");
            }
        

            ExceptionType = exceptionType;
            Identifier = identifier;
            Block = block;
        }

        public TypeSyntax ExceptionType { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.CatchClause; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitCatchClause(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitCatchClause(this);
        }
    }

    public class CompilationUnitSyntax : SyntaxNode {
        public CompilationUnitSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<ImportDirectiveSyntax> imports, ImmutableArray<MemberDeclarationSyntax> members, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (imports == null) {
                throw new ArgumentNullException("imports");
            }
            if (members == null) {
                throw new ArgumentNullException("members");
            }

            AttributeLists = attributeLists;
            Imports = imports;
            Members = members;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<ImportDirectiveSyntax> Imports { get; private set; }

        public ImmutableArray<MemberDeclarationSyntax> Members { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.CompilationUnit; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitCompilationUnit(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitCompilationUnit(this);
        }
    }

    public class ConditionalExpressionSyntax : ExpressionSyntax {
        public ConditionalExpressionSyntax(ExpressionSyntax condition, ExpressionSyntax whenTrue, ExpressionSyntax whenFalse, Span span)
            : base(span) {
            if (condition == null) {
                throw new ArgumentNullException("condition");
            }
            if (whenTrue == null) {
                throw new ArgumentNullException("whenTrue");
            }
            if (whenFalse == null) {
                throw new ArgumentNullException("whenFalse");
            }

            Condition = condition;
            WhenTrue = whenTrue;
            WhenFalse = whenFalse;
        }

        public ExpressionSyntax Condition { get; private set; }

        public ExpressionSyntax WhenTrue { get; private set; }

        public ExpressionSyntax WhenFalse { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ConditionalExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConditionalExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitConditionalExpression(this);
        }
    }

    public class ConstructorConstraintSyntax : TypeParameterConstraintSyntax {
        public ConstructorConstraintSyntax(Span span)
            : base(span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ConstructorConstraint; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructorConstraint(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitConstructorConstraint(this);
        }
    }

    public class ConstructorDeclarationSyntax : BaseMethodDeclarationSyntax {
        public ConstructorDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, IdentifierNameSyntax identifier, ConstructorInitializerSyntax initializer, ImmutableArray<ParameterSyntax> parameters, BlockSyntax body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
            Initializer = initializer;
        }

        public IdentifierNameSyntax Identifier { get; private set; }

        public ConstructorInitializerSyntax Initializer { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ConstructorDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructorDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitConstructorDeclaration(this);
        }
    }

    public class ConstructorInitializerSyntax : SyntaxNode {
        public ConstructorInitializerSyntax(ThisOrBase type, ImmutableArray<ArgumentSyntax> arguments, Span span)
            : base(span) {
            if (arguments == null) {
                throw new ArgumentNullException("arguments");
            }

            Type = type;
            Arguments = arguments;
        }

        public ThisOrBase Type { get; private set; }

        public ImmutableArray<ArgumentSyntax> Arguments { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ConstructorInitializer; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructorInitializer(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitConstructorInitializer(this);
        }
    }

    public class ContinueStatementSyntax : StatementSyntax {
        public ContinueStatementSyntax(Span span)
            : base(span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ContinueStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitContinueStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitContinueStatement(this);
        }
    }

    public class ConversionOperatorDeclarationSyntax : BaseMethodDeclarationSyntax {
        public ConversionOperatorDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, ImplicitOrExplicit type, TypeSyntax targetType, ImmutableArray<ParameterSyntax> parameters, BlockSyntax body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            Type = type;
            TargetType = targetType;
        }

        public ImplicitOrExplicit Type { get; private set; }

        public TypeSyntax TargetType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ConversionOperatorDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConversionOperatorDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitConversionOperatorDeclaration(this);
        }
    }

    public class DefaultExpressionSyntax : ExpressionSyntax {
        public DefaultExpressionSyntax(TypeSyntax targetType, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
        }

        public TypeSyntax TargetType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.DefaultExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDefaultExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitDefaultExpression(this);
        }
    }

    public class DelegateDeclarationSyntax : MemberDeclarationSyntax {
        public DelegateDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, TypeSyntax returnType, IdentifierNameSyntax identifier, ImmutableArray<TypeParameterSyntax> typeParameters, ImmutableArray<ParameterSyntax> parameters, ImmutableArray<TypeParameterConstraintClauseSyntax> constraintClauses, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (returnType == null) {
                throw new ArgumentNullException("returnType");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }
            if (typeParameters == null) {
                throw new ArgumentNullException("typeParameters");
            }
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            if (constraintClauses == null) {
                throw new ArgumentNullException("constraintClauses");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            ReturnType = returnType;
            Identifier = identifier;
            TypeParameters = typeParameters;
            Parameters = parameters;
            ConstraintClauses = constraintClauses;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public TypeSyntax ReturnType { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public ImmutableArray<TypeParameterSyntax> TypeParameters { get; private set; }

        public ImmutableArray<ParameterSyntax> Parameters { get; private set; }

        public ImmutableArray<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.DelegateDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDelegateDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitDelegateDeclaration(this);
        }
    }

    public class DeleteStatementSyntax : StatementSyntax {
        public DeleteStatementSyntax(ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.DeleteStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDeleteStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitDeleteStatement(this);
        }
    }

    public class DestructorDeclarationSyntax : BaseMethodDeclarationSyntax {
        public DestructorDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, IdentifierNameSyntax identifier, ImmutableArray<ParameterSyntax> parameters, BlockSyntax body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public IdentifierNameSyntax Identifier { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.DestructorDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDestructorDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitDestructorDeclaration(this);
        }
    }

    public class DoStatementSyntax : StatementSyntax {
        public DoStatementSyntax(ExpressionSyntax condition, BlockSyntax block, Span span)
            : base(span) {
            if (condition == null) {
                throw new ArgumentNullException("condition");
            }
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Condition = condition;
            Block = block;
        }

        public ExpressionSyntax Condition { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.DoStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDoStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitDoStatement(this);
        }
    }

    public class ElementAccessExpressionSyntax : ExpressionSyntax {
        public ElementAccessExpressionSyntax(ExpressionSyntax expression, ImmutableArray<ExpressionSyntax> indexExpressions, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            if (indexExpressions == null) {
                throw new ArgumentNullException("indexExpressions");
            }

            Expression = expression;
            IndexExpressions = indexExpressions;
        }

        public ExpressionSyntax Expression { get; private set; }

        public ImmutableArray<ExpressionSyntax> IndexExpressions { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ElementAccessExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitElementAccessExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitElementAccessExpression(this);
        }
    }

    public class ElseClauseSyntax : SyntaxNode {
        public ElseClauseSyntax(ElIfOrElse type, ExpressionSyntax condition, BlockSyntax block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Type = type;
            Condition = condition;
            Block = block;
        }

        public ElIfOrElse Type { get; private set; }

        public ExpressionSyntax Condition { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ElseClause; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitElseClause(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitElseClause(this);
        }
    }

    public class EmptyStatementSyntax : StatementSyntax {
        public EmptyStatementSyntax(Span span)
            : base(span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.EmptyStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEmptyStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitEmptyStatement(this);
        }
    }

    public class EnumDeclarationSyntax : BaseTypeDeclarationSyntax {
        public EnumDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, IdentifierNameSyntax identifier, ImmutableArray<EnumMemberDeclarationSyntax> members, ImmutableArray<TypeSyntax> baseTypes, Span span)
            : base(attributeLists, modifiers, identifier, baseTypes, span) {
            if (members == null) {
                throw new ArgumentNullException("members");
            }

            Members = members;
        }

        public ImmutableArray<EnumMemberDeclarationSyntax> Members { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.EnumDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEnumDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitEnumDeclaration(this);
        }
    }

    public class EnumMemberDeclarationSyntax : MemberDeclarationSyntax {
        public EnumMemberDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, IdentifierNameSyntax identifier, ExpressionSyntax value, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            AttributeLists = attributeLists;
            Identifier = identifier;
            Value = value;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public ExpressionSyntax Value { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.EnumMemberDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEnumMemberDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitEnumMemberDeclaration(this);
        }
    }

    public class EventDeclarationSyntax : BasePropertyDeclarationSyntax {
        public EventDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, TypeSyntax type, NameSyntax explicitInterfaceSpecifier, IdentifierNameSyntax identifier, ImmutableArray<AccessorDeclarationSyntax> accessors, Span span)
            : base(attributeLists, modifiers, type, explicitInterfaceSpecifier, accessors, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public IdentifierNameSyntax Identifier { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.EventDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEventDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitEventDeclaration(this);
        }
    }

    public class EventFieldDeclarationSyntax : BaseFieldDeclarationSyntax {
        public EventFieldDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, VariableDeclarationSyntax declaration, Span span)
            : base(attributeLists, modifiers, declaration, span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.EventFieldDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEventFieldDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitEventFieldDeclaration(this);
        }
    }

    public class ExpressionStatementSyntax : StatementSyntax {
        public ExpressionStatementSyntax(ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ExpressionStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitExpressionStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitExpressionStatement(this);
        }
    }

    public abstract class ExpressionSyntax : SyntaxNode {
        protected ExpressionSyntax(Span span)
            : base(span) {

        }
    }

    public class FieldDeclarationSyntax : BaseFieldDeclarationSyntax {
        public FieldDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, VariableDeclarationSyntax declaration, Span span)
            : base(attributeLists, modifiers, declaration, span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.FieldDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitFieldDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitFieldDeclaration(this);
        }
    }

    public class FinallyClauseSyntax : SyntaxNode {
        public FinallyClauseSyntax(BlockSyntax block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Block = block;
        }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.FinallyClause; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitFinallyClause(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitFinallyClause(this);
        }
    }

    public class ForEachStatementSyntax : StatementSyntax {
        public ForEachStatementSyntax(TypeSyntax elementType, IdentifierNameSyntax identifier, ExpressionSyntax expression, BlockSyntax block, Span span)
            : base(span) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            ElementType = elementType;
            Identifier = identifier;
            Expression = expression;
            Block = block;
        }

        public TypeSyntax ElementType { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public ExpressionSyntax Expression { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ForEachStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitForEachStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitForEachStatement(this);
        }
    }

    public class ForStatementSyntax : StatementSyntax {
        public ForStatementSyntax(VariableDeclarationSyntax declaration, ImmutableArray<ExpressionSyntax> initializers, ExpressionSyntax condition, ImmutableArray<ExpressionSyntax> incrementors, BlockSyntax block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            if (declaration != null && initializers != null) {
                throw new ArgumentException("Provide either a declaration or initializers");
            }
        

            Declaration = declaration;
            Initializers = initializers;
            Condition = condition;
            Incrementors = incrementors;
            Block = block;
        }

        public VariableDeclarationSyntax Declaration { get; private set; }

        public ImmutableArray<ExpressionSyntax> Initializers { get; private set; }

        public ExpressionSyntax Condition { get; private set; }

        public ImmutableArray<ExpressionSyntax> Incrementors { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ForStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitForStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitForStatement(this);
        }
    }

    public class GenericNameSyntax : SimpleNameSyntax {
        public GenericNameSyntax(string identifier, ImmutableArray<TypeSyntax> typeArguments, Span span)
            : base(identifier, span) {
            if (typeArguments == null) {
                throw new ArgumentNullException("typeArguments");
            }

            TypeArguments = typeArguments;
        }

        public ImmutableArray<TypeSyntax> TypeArguments { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.GenericName; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitGenericName(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitGenericName(this);
        }

            public bool IsUnboundGenericName {
                get {
                    return TypeArguments.Any(p => p is OmittedTypeArgumentSyntax);
                }
            }
        
    }

    public class IdentifierNameSyntax : SimpleNameSyntax {
        public IdentifierNameSyntax(string identifier, Span span)
            : base(identifier, span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.IdentifierName; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitIdentifierName(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitIdentifierName(this);
        }
    }

    public class IfStatementSyntax : StatementSyntax {
        public IfStatementSyntax(ExpressionSyntax condition, BlockSyntax block, ImmutableArray<ElseClauseSyntax> elses, Span span)
            : base(span) {
            if (condition == null) {
                throw new ArgumentNullException("condition");
            }
            if (block == null) {
                throw new ArgumentNullException("block");
            }
            if (elses == null) {
                throw new ArgumentNullException("elses");
            }

            Condition = condition;
            Block = block;
            Elses = elses;
        }

        public ExpressionSyntax Condition { get; private set; }

        public BlockSyntax Block { get; private set; }

        public ImmutableArray<ElseClauseSyntax> Elses { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.IfStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitIfStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitIfStatement(this);
        }
    }

    public class ImplicitArrayCreationExpressionSyntax : ExpressionSyntax {
        public ImplicitArrayCreationExpressionSyntax(ImmutableArray<ArrayRankSpecifierSyntax> rankSpecifiers, InitializerExpressionSyntax initializer, Span span)
            : base(span) {
            if (rankSpecifiers == null) {
                throw new ArgumentNullException("rankSpecifiers");
            }
            if (initializer == null) {
                throw new ArgumentNullException("initializer");
            }

            RankSpecifiers = rankSpecifiers;
            Initializer = initializer;
        }

        public ImmutableArray<ArrayRankSpecifierSyntax> RankSpecifiers { get; private set; }

        public InitializerExpressionSyntax Initializer { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ImplicitArrayCreationExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitImplicitArrayCreationExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitImplicitArrayCreationExpression(this);
        }
    }

    public class ImportDirectiveSyntax : SyntaxNode {
        public ImportDirectiveSyntax(bool isStatic, IdentifierNameSyntax alias, NameSyntax name, Span span)
            : base(span) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            IsStatic = isStatic;
            Alias = alias;
            Name = name;
        }

        public bool IsStatic { get; private set; }

        public IdentifierNameSyntax Alias { get; private set; }

        public NameSyntax Name { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ImportDirective; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitImportDirective(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitImportDirective(this);
        }
    }

    public class IndexerDeclarationSyntax : BasePropertyDeclarationSyntax {
        public IndexerDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, TypeSyntax type, NameSyntax explicitInterfaceSpecifier, ImmutableArray<ParameterSyntax> parameters, ImmutableArray<AccessorDeclarationSyntax> accessors, Span span)
            : base(attributeLists, modifiers, type, explicitInterfaceSpecifier, accessors, span) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }

            Parameters = parameters;
        }

        public ImmutableArray<ParameterSyntax> Parameters { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.IndexerDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitIndexerDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitIndexerDeclaration(this);
        }
    }

    public class InitializerExpressionSyntax : ExpressionSyntax {
        public InitializerExpressionSyntax(ImmutableArray<ExpressionSyntax> expressions, Span span)
            : base(span) {
            if (expressions == null) {
                throw new ArgumentNullException("expressions");
            }

            Expressions = expressions;
        }

        public ImmutableArray<ExpressionSyntax> Expressions { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.InitializerExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitInitializerExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitInitializerExpression(this);
        }
    }

    public class InstanceExpressionSyntax : ExpressionSyntax {
        public InstanceExpressionSyntax(ThisOrBase type, Span span)
            : base(span) {

            Type = type;
        }

        public ThisOrBase Type { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.InstanceExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitInstanceExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitInstanceExpression(this);
        }
    }

    public class InvocationExpressionSyntax : ExpressionSyntax {
        public InvocationExpressionSyntax(ExpressionSyntax expression, ImmutableArray<ArgumentSyntax> arguments, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            if (arguments == null) {
                throw new ArgumentNullException("arguments");
            }

            Expression = expression;
            Arguments = arguments;
        }

        public ExpressionSyntax Expression { get; private set; }

        public ImmutableArray<ArgumentSyntax> Arguments { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.InvocationExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitInvocationExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitInvocationExpression(this);
        }
    }

    public class LambdaExpressionSyntax : ExpressionSyntax {
        public LambdaExpressionSyntax(ImmutableArray<Modifier> modifiers, ImmutableArray<ParameterSyntax> parameters, SyntaxNode body, Span span)
            : base(span) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            if (body == null) {
                throw new ArgumentNullException("body");
            }

            Modifiers = modifiers;
            Parameters = parameters;
            Body = body;
        }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public ImmutableArray<ParameterSyntax> Parameters { get; private set; }

        public SyntaxNode Body { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.LambdaExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLambdaExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitLambdaExpression(this);
        }
    }

    public class LiteralExpressionSyntax : ExpressionSyntax {
        public LiteralExpressionSyntax(LiteralType literalType, string value, Span span)
            : base(span) {

            LiteralType = literalType;
            Value = value;
        }

        public LiteralType LiteralType { get; private set; }

        public string Value { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.LiteralExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLiteralExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitLiteralExpression(this);
        }
    }

    public class LocalDeclarationStatementSyntax : StatementSyntax {
        public LocalDeclarationStatementSyntax(ImmutableArray<Modifier> modifiers, VariableDeclarationSyntax declaration, Span span)
            : base(span) {
            if (declaration == null) {
                throw new ArgumentNullException("declaration");
            }

            Modifiers = modifiers;
            Declaration = declaration;
        }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public VariableDeclarationSyntax Declaration { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.LocalDeclarationStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLocalDeclarationStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitLocalDeclarationStatement(this);
        }
    }

    public class LoopStatementSyntax : StatementSyntax {
        public LoopStatementSyntax(BlockSyntax block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Block = block;
        }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.LoopStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLoopStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitLoopStatement(this);
        }
    }

    public class MemberAccessExpressionSyntax : ExpressionSyntax {
        public MemberAccessExpressionSyntax(ExpressionSyntax expression, SimpleNameSyntax name, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            Expression = expression;
            Name = name;
        }

        public ExpressionSyntax Expression { get; private set; }

        public SimpleNameSyntax Name { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.MemberAccessExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitMemberAccessExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitMemberAccessExpression(this);
        }
    }

    public abstract class MemberDeclarationSyntax : SyntaxNode {
        protected MemberDeclarationSyntax(Span span)
            : base(span) {

        }
    }

    public class MethodDeclarationSyntax : BaseMethodDeclarationSyntax {
        public MethodDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, TypeSyntax returnType, NameSyntax explicitInterfaceSpecifier, IdentifierNameSyntax identifier, ImmutableArray<TypeParameterSyntax> typeParameters, ImmutableArray<TypeParameterConstraintClauseSyntax> constraintClauses, ImmutableArray<ParameterSyntax> parameters, BlockSyntax body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (returnType == null) {
                throw new ArgumentNullException("returnType");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }
            if (typeParameters == null) {
                throw new ArgumentNullException("typeParameters");
            }
            if (constraintClauses == null) {
                throw new ArgumentNullException("constraintClauses");
            }

            ReturnType = returnType;
            ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            Identifier = identifier;
            TypeParameters = typeParameters;
            ConstraintClauses = constraintClauses;
        }

        public TypeSyntax ReturnType { get; private set; }

        public NameSyntax ExplicitInterfaceSpecifier { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public ImmutableArray<TypeParameterSyntax> TypeParameters { get; private set; }

        public ImmutableArray<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.MethodDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitMethodDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitMethodDeclaration(this);
        }
    }

    public class NakedNullableTypeSyntax : TypeSyntax {
        public NakedNullableTypeSyntax(Nullability type, Span span)
            : base(span) {

            Type = type;
        }

        public Nullability Type { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.NakedNullableType; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNakedNullableType(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitNakedNullableType(this);
        }
    }

    public class NamespaceDeclarationSyntax : MemberDeclarationSyntax {
        public NamespaceDeclarationSyntax(NameSyntax name, ImmutableArray<ImportDirectiveSyntax> imports, ImmutableArray<MemberDeclarationSyntax> members, Span span)
            : base(span) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (imports == null) {
                throw new ArgumentNullException("imports");
            }
            if (members == null) {
                throw new ArgumentNullException("members");
            }

            Name = name;
            Imports = imports;
            Members = members;
        }

        public NameSyntax Name { get; private set; }

        public ImmutableArray<ImportDirectiveSyntax> Imports { get; private set; }

        public ImmutableArray<MemberDeclarationSyntax> Members { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.NamespaceDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNamespaceDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitNamespaceDeclaration(this);
        }
    }

    public abstract class NameSyntax : TypeSyntax {
        protected NameSyntax(Span span)
            : base(span) {

        }
    }

    public class NullableTypeSyntax : TypeSyntax {
        public NullableTypeSyntax(TypeSyntax elementType, Span span)
            : base(span) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }

            ElementType = elementType;
        }

        public TypeSyntax ElementType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.NullableType; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNullableType(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitNullableType(this);
        }
    }

    public class ObjectCreationExpressionSyntax : ExpressionSyntax {
        public ObjectCreationExpressionSyntax(TypeSyntax targetType, ImmutableArray<ArgumentSyntax> arguments, InitializerExpressionSyntax initializer, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
            Arguments = arguments;
            Initializer = initializer;
        }

        public TypeSyntax TargetType { get; private set; }

        public ImmutableArray<ArgumentSyntax> Arguments { get; private set; }

        public InitializerExpressionSyntax Initializer { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ObjectCreationExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitObjectCreationExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitObjectCreationExpression(this);
        }
    }

    public class OmittedArraySizeExpressionSyntax : ExpressionSyntax {
        public OmittedArraySizeExpressionSyntax(Span span)
            : base(span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.OmittedArraySizeExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOmittedArraySizeExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitOmittedArraySizeExpression(this);
        }
    }

    public class OmittedTypeArgumentSyntax : TypeSyntax {
        public OmittedTypeArgumentSyntax(Span span)
            : base(span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.OmittedTypeArgument; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOmittedTypeArgument(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitOmittedTypeArgument(this);
        }
    }

    public class OperatorDeclarationSyntax : BaseMethodDeclarationSyntax {
        public OperatorDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, TypeSyntax returnType, Operator operator_, ImmutableArray<ParameterSyntax> parameters, BlockSyntax body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (returnType == null) {
                throw new ArgumentNullException("returnType");
            }

            ReturnType = returnType;
            Operator = operator_;
        }

        public TypeSyntax ReturnType { get; private set; }

        public Operator Operator { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.OperatorDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOperatorDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitOperatorDeclaration(this);
        }
    }

    public class ParameterSyntax : SyntaxNode {
        public ParameterSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<ParameterModifier> modifiers, TypeSyntax parameterType, IdentifierNameSyntax identifier, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            ParameterType = parameterType;
            Identifier = identifier;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public ImmutableArray<ParameterModifier> Modifiers { get; private set; }

        public TypeSyntax ParameterType { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.Parameter; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitParameter(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitParameter(this);
        }
    }

    public class ParenthesizedExpressionSyntax : ExpressionSyntax {
        public ParenthesizedExpressionSyntax(ExpressionSyntax expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ParenthesizedExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitParenthesizedExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitParenthesizedExpression(this);
        }
    }

    public class PostfixUnaryExpressionSyntax : ExpressionSyntax {
        public PostfixUnaryExpressionSyntax(PostfixUnaryOperator operator_, ExpressionSyntax operand, Span span)
            : base(span) {
            if (operand == null) {
                throw new ArgumentNullException("operand");
            }

            Operator = operator_;
            Operand = operand;
        }

        public PostfixUnaryOperator Operator { get; private set; }

        public ExpressionSyntax Operand { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.PostfixUnaryExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPostfixUnaryExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitPostfixUnaryExpression(this);
        }
    }

    public class PredefinedTypeSyntax : TypeSyntax {
        public PredefinedTypeSyntax(PredefinedType predefinedType, Span span)
            : base(span) {

            PredefinedType = predefinedType;
        }

        public PredefinedType PredefinedType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.PredefinedType; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPredefinedType(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitPredefinedType(this);
        }
    }

    public class PrefixUnaryExpressionSyntax : ExpressionSyntax {
        public PrefixUnaryExpressionSyntax(PrefixUnaryOperator operator_, ExpressionSyntax operand, Span span)
            : base(span) {
            if (operand == null) {
                throw new ArgumentNullException("operand");
            }

            Operator = operator_;
            Operand = operand;
        }

        public PrefixUnaryOperator Operator { get; private set; }

        public ExpressionSyntax Operand { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.PrefixUnaryExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPrefixUnaryExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitPrefixUnaryExpression(this);
        }
    }

    public class PropertyDeclarationSyntax : BasePropertyDeclarationSyntax {
        public PropertyDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, TypeSyntax type, NameSyntax explicitInterfaceSpecifier, IdentifierNameSyntax identifier, ImmutableArray<AccessorDeclarationSyntax> accessors, Span span)
            : base(attributeLists, modifiers, type, explicitInterfaceSpecifier, accessors, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public IdentifierNameSyntax Identifier { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.PropertyDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPropertyDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitPropertyDeclaration(this);
        }
    }

    public class QualifiedNameSyntax : NameSyntax {
        public QualifiedNameSyntax(NameSyntax left, SimpleNameSyntax right, Span span)
            : base(span) {
            if (left == null) {
                throw new ArgumentNullException("left");
            }
            if (right == null) {
                throw new ArgumentNullException("right");
            }

            Left = left;
            Right = right;
        }

        public NameSyntax Left { get; private set; }

        public SimpleNameSyntax Right { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.QualifiedName; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitQualifiedName(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitQualifiedName(this);
        }
    }

    public class ReturnStatementSyntax : StatementSyntax {
        public ReturnStatementSyntax(ExpressionSyntax expression, Span span)
            : base(span) {

            Expression = expression;
        }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ReturnStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitReturnStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitReturnStatement(this);
        }
    }

    public abstract class SimpleNameSyntax : NameSyntax {
        protected SimpleNameSyntax(string identifier, Span span)
            : base(span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public string Identifier { get; private set; }
    }

    public class SizeOfExpressionSyntax : ExpressionSyntax {
        public SizeOfExpressionSyntax(TypeSyntax targetType, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
        }

        public TypeSyntax TargetType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.SizeOfExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitSizeOfExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitSizeOfExpression(this);
        }
    }

    public abstract class StatementSyntax : SyntaxNode {
        protected StatementSyntax(Span span)
            : base(span) {

        }
    }

    public class SwitchSectionSyntax : SyntaxNode {
        public SwitchSectionSyntax(CaseOrDefault type, ImmutableArray<ExpressionSyntax> values, BlockSyntax block, Span span)
            : base(span) {
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Type = type;
            Values = values;
            Block = block;
        }

        public CaseOrDefault Type { get; private set; }

        public ImmutableArray<ExpressionSyntax> Values { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.SwitchSection; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitSwitchSection(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitSwitchSection(this);
        }
    }

    public class SwitchStatementSyntax : StatementSyntax {
        public SwitchStatementSyntax(ExpressionSyntax expression, ImmutableArray<SwitchSectionSyntax> sections, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }
            if (sections == null) {
                throw new ArgumentNullException("sections");
            }

            Expression = expression;
            Sections = sections;
        }

        public ExpressionSyntax Expression { get; private set; }

        public ImmutableArray<SwitchSectionSyntax> Sections { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.SwitchStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitSwitchStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitSwitchStatement(this);
        }
    }

    public class ThrowStatementSyntax : StatementSyntax {
        public ThrowStatementSyntax(ExpressionSyntax expression, Span span)
            : base(span) {

            Expression = expression;
        }

        public ExpressionSyntax Expression { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.ThrowStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitThrowStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitThrowStatement(this);
        }
    }

    public class TrackedTypeSyntax : TypeSyntax {
        public TrackedTypeSyntax(TypeSyntax elementType, Span span)
            : base(span) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }

            ElementType = elementType;
        }

        public TypeSyntax ElementType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TrackedType; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTrackedType(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTrackedType(this);
        }
    }

    public class TryStatementSyntax : StatementSyntax {
        public TryStatementSyntax(BlockSyntax block, ImmutableArray<CatchClauseSyntax> catches, FinallyClauseSyntax finally_, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }
            if (catches == null) {
                throw new ArgumentNullException("catches");
            }

            Block = block;
            Catches = catches;
            Finally = finally_;
        }

        public BlockSyntax Block { get; private set; }

        public ImmutableArray<CatchClauseSyntax> Catches { get; private set; }

        public FinallyClauseSyntax Finally { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TryStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTryStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTryStatement(this);
        }
    }

    public class TypeConstraintSyntax : TypeParameterConstraintSyntax {
        public TypeConstraintSyntax(TypeSyntax constrainedType, Span span)
            : base(span) {
            if (constrainedType == null) {
                throw new ArgumentNullException("constrainedType");
            }

            ConstrainedType = constrainedType;
        }

        public TypeSyntax ConstrainedType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TypeConstraint; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeConstraint(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTypeConstraint(this);
        }
    }

    public class TypeDeclarationSyntax : BaseTypeDeclarationSyntax {
        public TypeDeclarationSyntax(ImmutableArray<AttributeListSyntax> attributeLists, ImmutableArray<Modifier> modifiers, IdentifierNameSyntax identifier, TypeDeclarationType type, ImmutableArray<TypeParameterSyntax> typeParameters, ImmutableArray<TypeParameterConstraintClauseSyntax> constraintClauses, ImmutableArray<MemberDeclarationSyntax> members, ImmutableArray<TypeSyntax> baseTypes, Span span)
            : base(attributeLists, modifiers, identifier, baseTypes, span) {
            if (typeParameters == null) {
                throw new ArgumentNullException("typeParameters");
            }
            if (constraintClauses == null) {
                throw new ArgumentNullException("constraintClauses");
            }
            if (members == null) {
                throw new ArgumentNullException("members");
            }

            Type = type;
            TypeParameters = typeParameters;
            ConstraintClauses = constraintClauses;
            Members = members;
        }

        public TypeDeclarationType Type { get; private set; }

        public ImmutableArray<TypeParameterSyntax> TypeParameters { get; private set; }

        public ImmutableArray<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; private set; }

        public ImmutableArray<MemberDeclarationSyntax> Members { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TypeDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTypeDeclaration(this);
        }
    }

    public class TypeFamilyConstraintSyntax : TypeParameterConstraintSyntax {
        public TypeFamilyConstraintSyntax(TypeFamily family, Nullability? nullability, Span span)
            : base(span) {

            if (family != TypeFamily.Tracked && nullability == null) {
                throw new ArgumentException("Nullability is mandatory when family is not tracked");
            }
        

            Family = family;
            Nullability = nullability;
        }

        public TypeFamily Family { get; private set; }

        public Nullability? Nullability { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TypeFamilyConstraint; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeFamilyConstraint(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTypeFamilyConstraint(this);
        }
    }

    public class TypeOfExpressionSyntax : ExpressionSyntax {
        public TypeOfExpressionSyntax(TypeSyntax targetType, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
        }

        public TypeSyntax TargetType { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TypeOfExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeOfExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTypeOfExpression(this);
        }
    }

    public class TypeParameterConstraintClauseSyntax : SyntaxNode {
        public TypeParameterConstraintClauseSyntax(IdentifierNameSyntax name, ImmutableArray<TypeParameterConstraintSyntax> constraints, Span span)
            : base(span) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (constraints == null) {
                throw new ArgumentNullException("constraints");
            }

            Name = name;
            Constraints = constraints;
        }

        public IdentifierNameSyntax Name { get; private set; }

        public ImmutableArray<TypeParameterConstraintSyntax> Constraints { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TypeParameterConstraintClause; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeParameterConstraintClause(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTypeParameterConstraintClause(this);
        }
    }

    public abstract class TypeParameterConstraintSyntax : SyntaxNode {
        protected TypeParameterConstraintSyntax(Span span)
            : base(span) {

        }
    }

    public class TypeParameterSyntax : SyntaxNode {
        public TypeParameterSyntax(ImmutableArray<AttributeListSyntax> attributeLists, Variance variance, IdentifierNameSyntax identifier, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            AttributeLists = attributeLists;
            Variance = variance;
            Identifier = identifier;
        }

        public ImmutableArray<AttributeListSyntax> AttributeLists { get; private set; }

        public Variance Variance { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.TypeParameter; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeParameter(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitTypeParameter(this);
        }
    }

    public abstract class TypeSyntax : ExpressionSyntax {
        protected TypeSyntax(Span span)
            : base(span) {

        }
    }

    public class UsingStatementSyntax : StatementSyntax {
        public UsingStatementSyntax(VariableDeclarationSyntax declaration, ExpressionSyntax expression, BlockSyntax block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            if ((declaration != null) == (expression != null)) {
                throw new ArgumentException("Provide either a declaration or expression");
            }
        

            Declaration = declaration;
            Expression = expression;
            Block = block;
        }

        public VariableDeclarationSyntax Declaration { get; private set; }

        public ExpressionSyntax Expression { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.UsingStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitUsingStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitUsingStatement(this);
        }
    }

    public class VariableDeclarationExpressionSyntax : ExpressionSyntax {
        public VariableDeclarationExpressionSyntax(TypeSyntax variableType, IdentifierNameSyntax identifier, Span span)
            : base(span) {
            if (variableType == null) {
                throw new ArgumentNullException("variableType");
            }
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            VariableType = variableType;
            Identifier = identifier;
        }

        public TypeSyntax VariableType { get; private set; }

        public IdentifierNameSyntax Identifier { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.VariableDeclarationExpression; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVariableDeclarationExpression(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitVariableDeclarationExpression(this);
        }
    }

    public class VariableDeclarationSyntax : SyntaxNode {
        public VariableDeclarationSyntax(TypeSyntax variableType, ImmutableArray<VariableDeclaratorSyntax> variables, Span span)
            : base(span) {
            if (variableType == null) {
                throw new ArgumentNullException("variableType");
            }
            if (variables == null) {
                throw new ArgumentNullException("variables");
            }

            VariableType = variableType;
            Variables = variables;
        }

        public TypeSyntax VariableType { get; private set; }

        public ImmutableArray<VariableDeclaratorSyntax> Variables { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.VariableDeclaration; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVariableDeclaration(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitVariableDeclaration(this);
        }
    }

    public class VariableDeclaratorSyntax : SyntaxNode {
        public VariableDeclaratorSyntax(IdentifierNameSyntax identifier, ExpressionSyntax value, Span span)
            : base(span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
            Value = value;
        }

        public IdentifierNameSyntax Identifier { get; private set; }

        public ExpressionSyntax Value { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.VariableDeclarator; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVariableDeclarator(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitVariableDeclarator(this);
        }
    }

    public class VarTypeSyntax : TypeSyntax {
        public VarTypeSyntax(Span span)
            : base(span) {

        }

        public override SyntaxKind Kind {
            get { return SyntaxKind.VarType; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVarType(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitVarType(this);
        }
    }

    public class WhileStatementSyntax : StatementSyntax {
        public WhileStatementSyntax(ExpressionSyntax condition, BlockSyntax block, Span span)
            : base(span) {
            if (condition == null) {
                throw new ArgumentNullException("condition");
            }
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Condition = condition;
            Block = block;
        }

        public ExpressionSyntax Condition { get; private set; }

        public BlockSyntax Block { get; private set; }

        public override SyntaxKind Kind {
            get { return SyntaxKind.WhileStatement; }
        }

        public override void Accept(ISyntaxVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitWhileStatement(this);
            }
        }

        public override T Accept<T>(ISyntaxVisitor<T> visitor) {
            return visitor.VisitWhileStatement(this);
        }
    }

    public enum AccessorDeclarationType {
        Invalid,
        Get,
        Set,
        Add,
        Remove,
    }

    public enum AttributeTarget {
        None,
        Assembly,
        Event,
        Field,
        Method,
        Param,
        Property,
        Return,
        Type,
    }

    public enum BinaryOperator {
        Ampersand,
        AmpersandAmpersand,
        AmpersandEquals,
        As,
        Asterisk,
        AsteriskEquals,
        Bar,
        BarBar,
        BarEquals,
        Caret,
        CaretEquals,
        Equals,
        EqualsEquals,
        ExclamationEquals,
        GreaterThan,
        GreaterThanEquals,
        GreaterThanGreaterThan,
        GreaterThanGreaterThanEquals,
        Is,
        LessThan,
        LessThanEquals,
        LessThanLessThan,
        LessThanLessThanEquals,
        Minus,
        MinusEquals,
        Percent,
        PercentEquals,
        Plus,
        PlusEquals,
        QuestionQuestion,
        Slash,
        SlashEquals,
    }

    public enum CaseOrDefault {
        Case,
        Default,
    }

    public enum TypeFamily {
        Any,
        Class,
        Struct,
        Tracked,
    }

    public enum ElIfOrElse {
        ElIf,
        Else,
    }

    public enum ImplicitOrExplicit {
        Explicit,
        Implicit,
    }

    public enum LiteralType {
        Char,
        False,
        Float,
        Hex,
        Integer,
        Nil,
        String,
        True,
    }

    public enum Modifier {
        Abstract,
        Async,
        Extern,
        Internal,
        New,
        Override,
        Partial,
        Private,
        Protected,
        Public,
        Readonly,
        Sealed,
        Static,
        Tracked,
        Virtual,
        Volatile,
    }

    public enum Nullability {
        Nullable,
        NotNullable,
    }

    public enum Operator {
        Ampersand,
        Asterisk,
        Bar,
        Caret,
        EqualsEquals,
        Exclamation,
        ExclamationEquals,
        False,
        GreaterThan,
        GreaterThanEquals,
        GreaterThanGreaterThan,
        LessThan,
        LessThanEquals,
        LessThanLessThan,
        Minus,
        MinusMinus,
        Percent,
        Plus,
        PlusPlus,
        Slash,
        Tilde,
        True,
    }

    public enum ParameterModifier {
        This,
        Ref,
        Out,
        Params,
        Consumes,
    }

    public enum PostfixUnaryOperator {
        MinusMinus,
        PlusPlus,
    }

    public enum PredefinedType {
        Bool,
        Byte,
        Char,
        Decimal,
        Double,
        Float,
        Int,
        Long,
        Object,
        SByte,
        Short,
        String,
        UInt,
        ULong,
        UShort,
        Void,
    }

    public enum PrefixUnaryOperator {
        Ampersand,
        Exclamation,
        Minus,
        MinusMinus,
        Plus,
        PlusPlus,
        Tilde,
    }

    public enum ThisOrBase {
        Base,
        This,
    }

    public enum TypeDeclarationType {
        Class,
        Interface,
        Struct,
    }

    public enum Variance {
        None,
        In,
        Out,
    }

    public interface ISyntaxVisitor {
        bool Done { get; }

        void VisitAccessorDeclaration(AccessorDeclarationSyntax syntax);

        void VisitAliasQualifiedName(AliasQualifiedNameSyntax syntax);

        void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax syntax);

        void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax syntax);

        void VisitArgument(ArgumentSyntax syntax);

        void VisitArrayCreationExpression(ArrayCreationExpressionSyntax syntax);

        void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax syntax);

        void VisitArrayType(ArrayTypeSyntax syntax);

        void VisitAssertStatement(AssertStatementSyntax syntax);

        void VisitAttributeArgument(AttributeArgumentSyntax syntax);

        void VisitAttributeList(AttributeListSyntax syntax);

        void VisitAttribute(AttributeSyntax syntax);

        void VisitAwaitExpression(AwaitExpressionSyntax syntax);

        void VisitBinaryExpression(BinaryExpressionSyntax syntax);

        void VisitBlock(BlockSyntax syntax);

        void VisitBreakStatement(BreakStatementSyntax syntax);

        void VisitCastExpression(CastExpressionSyntax syntax);

        void VisitCatchClause(CatchClauseSyntax syntax);

        void VisitCompilationUnit(CompilationUnitSyntax syntax);

        void VisitConditionalExpression(ConditionalExpressionSyntax syntax);

        void VisitConstructorConstraint(ConstructorConstraintSyntax syntax);

        void VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax);

        void VisitConstructorInitializer(ConstructorInitializerSyntax syntax);

        void VisitContinueStatement(ContinueStatementSyntax syntax);

        void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax);

        void VisitDefaultExpression(DefaultExpressionSyntax syntax);

        void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax);

        void VisitDeleteStatement(DeleteStatementSyntax syntax);

        void VisitDestructorDeclaration(DestructorDeclarationSyntax syntax);

        void VisitDoStatement(DoStatementSyntax syntax);

        void VisitElementAccessExpression(ElementAccessExpressionSyntax syntax);

        void VisitElseClause(ElseClauseSyntax syntax);

        void VisitEmptyStatement(EmptyStatementSyntax syntax);

        void VisitEnumDeclaration(EnumDeclarationSyntax syntax);

        void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax);

        void VisitEventDeclaration(EventDeclarationSyntax syntax);

        void VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax);

        void VisitExpressionStatement(ExpressionStatementSyntax syntax);

        void VisitFieldDeclaration(FieldDeclarationSyntax syntax);

        void VisitFinallyClause(FinallyClauseSyntax syntax);

        void VisitForEachStatement(ForEachStatementSyntax syntax);

        void VisitForStatement(ForStatementSyntax syntax);

        void VisitGenericName(GenericNameSyntax syntax);

        void VisitIdentifierName(IdentifierNameSyntax syntax);

        void VisitIfStatement(IfStatementSyntax syntax);

        void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax syntax);

        void VisitImportDirective(ImportDirectiveSyntax syntax);

        void VisitIndexerDeclaration(IndexerDeclarationSyntax syntax);

        void VisitInitializerExpression(InitializerExpressionSyntax syntax);

        void VisitInstanceExpression(InstanceExpressionSyntax syntax);

        void VisitInvocationExpression(InvocationExpressionSyntax syntax);

        void VisitLambdaExpression(LambdaExpressionSyntax syntax);

        void VisitLiteralExpression(LiteralExpressionSyntax syntax);

        void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax);

        void VisitLoopStatement(LoopStatementSyntax syntax);

        void VisitMemberAccessExpression(MemberAccessExpressionSyntax syntax);

        void VisitMethodDeclaration(MethodDeclarationSyntax syntax);

        void VisitNakedNullableType(NakedNullableTypeSyntax syntax);

        void VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax);

        void VisitNullableType(NullableTypeSyntax syntax);

        void VisitObjectCreationExpression(ObjectCreationExpressionSyntax syntax);

        void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax syntax);

        void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax syntax);

        void VisitOperatorDeclaration(OperatorDeclarationSyntax syntax);

        void VisitParameter(ParameterSyntax syntax);

        void VisitParenthesizedExpression(ParenthesizedExpressionSyntax syntax);

        void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax syntax);

        void VisitPredefinedType(PredefinedTypeSyntax syntax);

        void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax syntax);

        void VisitPropertyDeclaration(PropertyDeclarationSyntax syntax);

        void VisitQualifiedName(QualifiedNameSyntax syntax);

        void VisitReturnStatement(ReturnStatementSyntax syntax);

        void VisitSizeOfExpression(SizeOfExpressionSyntax syntax);

        void VisitSwitchSection(SwitchSectionSyntax syntax);

        void VisitSwitchStatement(SwitchStatementSyntax syntax);

        void VisitThrowStatement(ThrowStatementSyntax syntax);

        void VisitTrackedType(TrackedTypeSyntax syntax);

        void VisitTryStatement(TryStatementSyntax syntax);

        void VisitTypeConstraint(TypeConstraintSyntax syntax);

        void VisitTypeDeclaration(TypeDeclarationSyntax syntax);

        void VisitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax);

        void VisitTypeOfExpression(TypeOfExpressionSyntax syntax);

        void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax);

        void VisitTypeParameter(TypeParameterSyntax syntax);

        void VisitUsingStatement(UsingStatementSyntax syntax);

        void VisitVariableDeclarationExpression(VariableDeclarationExpressionSyntax syntax);

        void VisitVariableDeclaration(VariableDeclarationSyntax syntax);

        void VisitVariableDeclarator(VariableDeclaratorSyntax syntax);

        void VisitVarType(VarTypeSyntax syntax);

        void VisitWhileStatement(WhileStatementSyntax syntax);
    }

    public interface ISyntaxVisitor<T> {
        T VisitAccessorDeclaration(AccessorDeclarationSyntax syntax);

        T VisitAliasQualifiedName(AliasQualifiedNameSyntax syntax);

        T VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax syntax);

        T VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax syntax);

        T VisitArgument(ArgumentSyntax syntax);

        T VisitArrayCreationExpression(ArrayCreationExpressionSyntax syntax);

        T VisitArrayRankSpecifier(ArrayRankSpecifierSyntax syntax);

        T VisitArrayType(ArrayTypeSyntax syntax);

        T VisitAssertStatement(AssertStatementSyntax syntax);

        T VisitAttributeArgument(AttributeArgumentSyntax syntax);

        T VisitAttributeList(AttributeListSyntax syntax);

        T VisitAttribute(AttributeSyntax syntax);

        T VisitAwaitExpression(AwaitExpressionSyntax syntax);

        T VisitBinaryExpression(BinaryExpressionSyntax syntax);

        T VisitBlock(BlockSyntax syntax);

        T VisitBreakStatement(BreakStatementSyntax syntax);

        T VisitCastExpression(CastExpressionSyntax syntax);

        T VisitCatchClause(CatchClauseSyntax syntax);

        T VisitCompilationUnit(CompilationUnitSyntax syntax);

        T VisitConditionalExpression(ConditionalExpressionSyntax syntax);

        T VisitConstructorConstraint(ConstructorConstraintSyntax syntax);

        T VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax);

        T VisitConstructorInitializer(ConstructorInitializerSyntax syntax);

        T VisitContinueStatement(ContinueStatementSyntax syntax);

        T VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax);

        T VisitDefaultExpression(DefaultExpressionSyntax syntax);

        T VisitDelegateDeclaration(DelegateDeclarationSyntax syntax);

        T VisitDeleteStatement(DeleteStatementSyntax syntax);

        T VisitDestructorDeclaration(DestructorDeclarationSyntax syntax);

        T VisitDoStatement(DoStatementSyntax syntax);

        T VisitElementAccessExpression(ElementAccessExpressionSyntax syntax);

        T VisitElseClause(ElseClauseSyntax syntax);

        T VisitEmptyStatement(EmptyStatementSyntax syntax);

        T VisitEnumDeclaration(EnumDeclarationSyntax syntax);

        T VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax);

        T VisitEventDeclaration(EventDeclarationSyntax syntax);

        T VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax);

        T VisitExpressionStatement(ExpressionStatementSyntax syntax);

        T VisitFieldDeclaration(FieldDeclarationSyntax syntax);

        T VisitFinallyClause(FinallyClauseSyntax syntax);

        T VisitForEachStatement(ForEachStatementSyntax syntax);

        T VisitForStatement(ForStatementSyntax syntax);

        T VisitGenericName(GenericNameSyntax syntax);

        T VisitIdentifierName(IdentifierNameSyntax syntax);

        T VisitIfStatement(IfStatementSyntax syntax);

        T VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax syntax);

        T VisitImportDirective(ImportDirectiveSyntax syntax);

        T VisitIndexerDeclaration(IndexerDeclarationSyntax syntax);

        T VisitInitializerExpression(InitializerExpressionSyntax syntax);

        T VisitInstanceExpression(InstanceExpressionSyntax syntax);

        T VisitInvocationExpression(InvocationExpressionSyntax syntax);

        T VisitLambdaExpression(LambdaExpressionSyntax syntax);

        T VisitLiteralExpression(LiteralExpressionSyntax syntax);

        T VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax);

        T VisitLoopStatement(LoopStatementSyntax syntax);

        T VisitMemberAccessExpression(MemberAccessExpressionSyntax syntax);

        T VisitMethodDeclaration(MethodDeclarationSyntax syntax);

        T VisitNakedNullableType(NakedNullableTypeSyntax syntax);

        T VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax);

        T VisitNullableType(NullableTypeSyntax syntax);

        T VisitObjectCreationExpression(ObjectCreationExpressionSyntax syntax);

        T VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax syntax);

        T VisitOmittedTypeArgument(OmittedTypeArgumentSyntax syntax);

        T VisitOperatorDeclaration(OperatorDeclarationSyntax syntax);

        T VisitParameter(ParameterSyntax syntax);

        T VisitParenthesizedExpression(ParenthesizedExpressionSyntax syntax);

        T VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax syntax);

        T VisitPredefinedType(PredefinedTypeSyntax syntax);

        T VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax syntax);

        T VisitPropertyDeclaration(PropertyDeclarationSyntax syntax);

        T VisitQualifiedName(QualifiedNameSyntax syntax);

        T VisitReturnStatement(ReturnStatementSyntax syntax);

        T VisitSizeOfExpression(SizeOfExpressionSyntax syntax);

        T VisitSwitchSection(SwitchSectionSyntax syntax);

        T VisitSwitchStatement(SwitchStatementSyntax syntax);

        T VisitThrowStatement(ThrowStatementSyntax syntax);

        T VisitTrackedType(TrackedTypeSyntax syntax);

        T VisitTryStatement(TryStatementSyntax syntax);

        T VisitTypeConstraint(TypeConstraintSyntax syntax);

        T VisitTypeDeclaration(TypeDeclarationSyntax syntax);

        T VisitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax);

        T VisitTypeOfExpression(TypeOfExpressionSyntax syntax);

        T VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax);

        T VisitTypeParameter(TypeParameterSyntax syntax);

        T VisitUsingStatement(UsingStatementSyntax syntax);

        T VisitVariableDeclarationExpression(VariableDeclarationExpressionSyntax syntax);

        T VisitVariableDeclaration(VariableDeclarationSyntax syntax);

        T VisitVariableDeclarator(VariableDeclaratorSyntax syntax);

        T VisitVarType(VarTypeSyntax syntax);

        T VisitWhileStatement(WhileStatementSyntax syntax);
    }

    public class AbstractSyntaxVisitor : ISyntaxVisitor {
        public bool Done { get; set; }

        public void DefaultVisit(SyntaxNode syntax) {
        }

        public void VisitAccessorDeclaration(AccessorDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAliasQualifiedName(AliasQualifiedNameSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitArgument(ArgumentSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitArrayCreationExpression(ArrayCreationExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitArrayType(ArrayTypeSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAssertStatement(AssertStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAttributeArgument(AttributeArgumentSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAttributeList(AttributeListSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAttribute(AttributeSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitAwaitExpression(AwaitExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitBinaryExpression(BinaryExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitBlock(BlockSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitBreakStatement(BreakStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitCastExpression(CastExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitCatchClause(CatchClauseSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitCompilationUnit(CompilationUnitSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitConditionalExpression(ConditionalExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitConstructorConstraint(ConstructorConstraintSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitConstructorInitializer(ConstructorInitializerSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitContinueStatement(ContinueStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitDefaultExpression(DefaultExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitDeleteStatement(DeleteStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitDestructorDeclaration(DestructorDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitDoStatement(DoStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitElementAccessExpression(ElementAccessExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitElseClause(ElseClauseSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitEmptyStatement(EmptyStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitEventDeclaration(EventDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitExpressionStatement(ExpressionStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitFieldDeclaration(FieldDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitFinallyClause(FinallyClauseSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitForEachStatement(ForEachStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitForStatement(ForStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitGenericName(GenericNameSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitIdentifierName(IdentifierNameSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitIfStatement(IfStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitImportDirective(ImportDirectiveSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitIndexerDeclaration(IndexerDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitInitializerExpression(InitializerExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitInstanceExpression(InstanceExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitInvocationExpression(InvocationExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitLambdaExpression(LambdaExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitLiteralExpression(LiteralExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitLoopStatement(LoopStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitMemberAccessExpression(MemberAccessExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitMethodDeclaration(MethodDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitNakedNullableType(NakedNullableTypeSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitNullableType(NullableTypeSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitObjectCreationExpression(ObjectCreationExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitOperatorDeclaration(OperatorDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitParameter(ParameterSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitParenthesizedExpression(ParenthesizedExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitPredefinedType(PredefinedTypeSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitPropertyDeclaration(PropertyDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitQualifiedName(QualifiedNameSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitReturnStatement(ReturnStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitSizeOfExpression(SizeOfExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitSwitchSection(SwitchSectionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitSwitchStatement(SwitchStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitThrowStatement(ThrowStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTrackedType(TrackedTypeSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTryStatement(TryStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTypeConstraint(TypeConstraintSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTypeOfExpression(TypeOfExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitTypeParameter(TypeParameterSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitUsingStatement(UsingStatementSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitVariableDeclarationExpression(VariableDeclarationExpressionSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitVariableDeclaration(VariableDeclarationSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitVariableDeclarator(VariableDeclaratorSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitVarType(VarTypeSyntax syntax) {
            DefaultVisit(syntax);
        }

        public void VisitWhileStatement(WhileStatementSyntax syntax) {
            DefaultVisit(syntax);
        }
    }

    public class AbstractSyntaxVisitor<T> : ISyntaxVisitor<T> {
        public T DefaultVisit(SyntaxNode syntax) {
            return default(T);
        }
        public T VisitAccessorDeclaration(AccessorDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAliasQualifiedName(AliasQualifiedNameSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitArgument(ArgumentSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitArrayCreationExpression(ArrayCreationExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitArrayRankSpecifier(ArrayRankSpecifierSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitArrayType(ArrayTypeSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAssertStatement(AssertStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAttributeArgument(AttributeArgumentSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAttributeList(AttributeListSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAttribute(AttributeSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitAwaitExpression(AwaitExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitBinaryExpression(BinaryExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitBlock(BlockSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitBreakStatement(BreakStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitCastExpression(CastExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitCatchClause(CatchClauseSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitCompilationUnit(CompilationUnitSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitConditionalExpression(ConditionalExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitConstructorConstraint(ConstructorConstraintSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitConstructorInitializer(ConstructorInitializerSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitContinueStatement(ContinueStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitDefaultExpression(DefaultExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitDeleteStatement(DeleteStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitDestructorDeclaration(DestructorDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitDoStatement(DoStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitElementAccessExpression(ElementAccessExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitElseClause(ElseClauseSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitEmptyStatement(EmptyStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitEventDeclaration(EventDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitExpressionStatement(ExpressionStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitFieldDeclaration(FieldDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitFinallyClause(FinallyClauseSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitForEachStatement(ForEachStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitForStatement(ForStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitGenericName(GenericNameSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitIdentifierName(IdentifierNameSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitIfStatement(IfStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitImportDirective(ImportDirectiveSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitIndexerDeclaration(IndexerDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitInitializerExpression(InitializerExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitInstanceExpression(InstanceExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitInvocationExpression(InvocationExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitLambdaExpression(LambdaExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitLiteralExpression(LiteralExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitLoopStatement(LoopStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitMemberAccessExpression(MemberAccessExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitMethodDeclaration(MethodDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitNakedNullableType(NakedNullableTypeSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitNullableType(NullableTypeSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitObjectCreationExpression(ObjectCreationExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitOmittedTypeArgument(OmittedTypeArgumentSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitOperatorDeclaration(OperatorDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitParameter(ParameterSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitParenthesizedExpression(ParenthesizedExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitPredefinedType(PredefinedTypeSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitPropertyDeclaration(PropertyDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitQualifiedName(QualifiedNameSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitReturnStatement(ReturnStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitSizeOfExpression(SizeOfExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitSwitchSection(SwitchSectionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitSwitchStatement(SwitchStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitThrowStatement(ThrowStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTrackedType(TrackedTypeSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTryStatement(TryStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTypeConstraint(TypeConstraintSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTypeOfExpression(TypeOfExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitTypeParameter(TypeParameterSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitUsingStatement(UsingStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitVariableDeclarationExpression(VariableDeclarationExpressionSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitVariableDeclaration(VariableDeclarationSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitVariableDeclarator(VariableDeclaratorSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitVarType(VarTypeSyntax syntax) {
            return DefaultVisit(syntax);
        }

        public T VisitWhileStatement(WhileStatementSyntax syntax) {
            return DefaultVisit(syntax);
        }
    }

    public class SyntaxTreeWalker : ISyntaxVisitor {
        public bool Done { get; set; }

        public virtual void VisitList<T>(ImmutableArray<T> list)
            where T : SyntaxNode {
            foreach (var node in list) {
                Visit(node);
            }
        }

        public virtual void Visit(SyntaxNode syntax) {
            if (syntax != null) {
                syntax.Accept(this);
            }
        }

        public virtual void VisitAccessorDeclaration(AccessorDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Body);
        }

        public virtual void VisitAliasQualifiedName(AliasQualifiedNameSyntax syntax) {
            Visit(syntax.Alias);
            Visit(syntax.Name);
        }

        public virtual void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax syntax) {
            VisitList(syntax.Initializers);
        }

        public virtual void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax syntax) {
            Visit(syntax.Name);
            Visit(syntax.Expression);
        }

        public virtual void VisitArgument(ArgumentSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitArrayCreationExpression(ArrayCreationExpressionSyntax syntax) {
            Visit(syntax.Type);
            Visit(syntax.Initializer);
        }

        public virtual void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax syntax) {
            Visit(syntax.Size);
        }

        public virtual void VisitArrayType(ArrayTypeSyntax syntax) {
            Visit(syntax.ElementType);
            VisitList(syntax.RankSpecifiers);
        }

        public virtual void VisitAssertStatement(AssertStatementSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitAttributeArgument(AttributeArgumentSyntax syntax) {
            Visit(syntax.Name);
            Visit(syntax.Expression);
        }

        public virtual void VisitAttributeList(AttributeListSyntax syntax) {
            VisitList(syntax.Attributes);
        }

        public virtual void VisitAttribute(AttributeSyntax syntax) {
            Visit(syntax.Name);
            VisitList(syntax.Arguments);
        }

        public virtual void VisitAwaitExpression(AwaitExpressionSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitBinaryExpression(BinaryExpressionSyntax syntax) {
            Visit(syntax.Left);
            Visit(syntax.Right);
        }

        public virtual void VisitBlock(BlockSyntax syntax) {
            VisitList(syntax.Statements);
        }

        public virtual void VisitBreakStatement(BreakStatementSyntax syntax) {
        }

        public virtual void VisitCastExpression(CastExpressionSyntax syntax) {
            Visit(syntax.Expression);
            Visit(syntax.TargetType);
        }

        public virtual void VisitCatchClause(CatchClauseSyntax syntax) {
            Visit(syntax.ExceptionType);
            Visit(syntax.Identifier);
            Visit(syntax.Block);
        }

        public virtual void VisitCompilationUnit(CompilationUnitSyntax syntax) {
            VisitList(syntax.AttributeLists);
            VisitList(syntax.Imports);
            VisitList(syntax.Members);
        }

        public virtual void VisitConditionalExpression(ConditionalExpressionSyntax syntax) {
            Visit(syntax.Condition);
            Visit(syntax.WhenTrue);
            Visit(syntax.WhenFalse);
        }

        public virtual void VisitConstructorConstraint(ConstructorConstraintSyntax syntax) {
        }

        public virtual void VisitConstructorDeclaration(ConstructorDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Identifier);
            Visit(syntax.Initializer);
            VisitList(syntax.Parameters);
            Visit(syntax.Body);
        }

        public virtual void VisitConstructorInitializer(ConstructorInitializerSyntax syntax) {
            VisitList(syntax.Arguments);
        }

        public virtual void VisitContinueStatement(ContinueStatementSyntax syntax) {
        }

        public virtual void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.TargetType);
            VisitList(syntax.Parameters);
            Visit(syntax.Body);
        }

        public virtual void VisitDefaultExpression(DefaultExpressionSyntax syntax) {
            Visit(syntax.TargetType);
        }

        public virtual void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.ReturnType);
            Visit(syntax.Identifier);
            VisitList(syntax.TypeParameters);
            VisitList(syntax.Parameters);
            VisitList(syntax.ConstraintClauses);
        }

        public virtual void VisitDeleteStatement(DeleteStatementSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitDestructorDeclaration(DestructorDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Identifier);
            VisitList(syntax.Parameters);
            Visit(syntax.Body);
        }

        public virtual void VisitDoStatement(DoStatementSyntax syntax) {
            Visit(syntax.Condition);
            Visit(syntax.Block);
        }

        public virtual void VisitElementAccessExpression(ElementAccessExpressionSyntax syntax) {
            Visit(syntax.Expression);
            VisitList(syntax.IndexExpressions);
        }

        public virtual void VisitElseClause(ElseClauseSyntax syntax) {
            Visit(syntax.Condition);
            Visit(syntax.Block);
        }

        public virtual void VisitEmptyStatement(EmptyStatementSyntax syntax) {
        }

        public virtual void VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Identifier);
            VisitList(syntax.Members);
            VisitList(syntax.BaseTypes);
        }

        public virtual void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Identifier);
            Visit(syntax.Value);
        }

        public virtual void VisitEventDeclaration(EventDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Type);
            Visit(syntax.ExplicitInterfaceSpecifier);
            Visit(syntax.Identifier);
            VisitList(syntax.Accessors);
        }

        public virtual void VisitEventFieldDeclaration(EventFieldDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Declaration);
        }

        public virtual void VisitExpressionStatement(ExpressionStatementSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitFieldDeclaration(FieldDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Declaration);
        }

        public virtual void VisitFinallyClause(FinallyClauseSyntax syntax) {
            Visit(syntax.Block);
        }

        public virtual void VisitForEachStatement(ForEachStatementSyntax syntax) {
            Visit(syntax.ElementType);
            Visit(syntax.Identifier);
            Visit(syntax.Expression);
            Visit(syntax.Block);
        }

        public virtual void VisitForStatement(ForStatementSyntax syntax) {
            Visit(syntax.Declaration);
            VisitList(syntax.Initializers);
            Visit(syntax.Condition);
            VisitList(syntax.Incrementors);
            Visit(syntax.Block);
        }

        public virtual void VisitGenericName(GenericNameSyntax syntax) {
            VisitList(syntax.TypeArguments);
        }

        public virtual void VisitIdentifierName(IdentifierNameSyntax syntax) {
        }

        public virtual void VisitIfStatement(IfStatementSyntax syntax) {
            Visit(syntax.Condition);
            Visit(syntax.Block);
            VisitList(syntax.Elses);
        }

        public virtual void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax syntax) {
            VisitList(syntax.RankSpecifiers);
            Visit(syntax.Initializer);
        }

        public virtual void VisitImportDirective(ImportDirectiveSyntax syntax) {
            Visit(syntax.Alias);
            Visit(syntax.Name);
        }

        public virtual void VisitIndexerDeclaration(IndexerDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Type);
            Visit(syntax.ExplicitInterfaceSpecifier);
            VisitList(syntax.Parameters);
            VisitList(syntax.Accessors);
        }

        public virtual void VisitInitializerExpression(InitializerExpressionSyntax syntax) {
            VisitList(syntax.Expressions);
        }

        public virtual void VisitInstanceExpression(InstanceExpressionSyntax syntax) {
        }

        public virtual void VisitInvocationExpression(InvocationExpressionSyntax syntax) {
            Visit(syntax.Expression);
            VisitList(syntax.Arguments);
        }

        public virtual void VisitLambdaExpression(LambdaExpressionSyntax syntax) {
            VisitList(syntax.Parameters);
        }

        public virtual void VisitLiteralExpression(LiteralExpressionSyntax syntax) {
        }

        public virtual void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax syntax) {
            Visit(syntax.Declaration);
        }

        public virtual void VisitLoopStatement(LoopStatementSyntax syntax) {
            Visit(syntax.Block);
        }

        public virtual void VisitMemberAccessExpression(MemberAccessExpressionSyntax syntax) {
            Visit(syntax.Expression);
            Visit(syntax.Name);
        }

        public virtual void VisitMethodDeclaration(MethodDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.ReturnType);
            Visit(syntax.ExplicitInterfaceSpecifier);
            Visit(syntax.Identifier);
            VisitList(syntax.TypeParameters);
            VisitList(syntax.ConstraintClauses);
            VisitList(syntax.Parameters);
            Visit(syntax.Body);
        }

        public virtual void VisitNakedNullableType(NakedNullableTypeSyntax syntax) {
        }

        public virtual void VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) {
            Visit(syntax.Name);
            VisitList(syntax.Imports);
            VisitList(syntax.Members);
        }

        public virtual void VisitNullableType(NullableTypeSyntax syntax) {
            Visit(syntax.ElementType);
        }

        public virtual void VisitObjectCreationExpression(ObjectCreationExpressionSyntax syntax) {
            Visit(syntax.TargetType);
            VisitList(syntax.Arguments);
            Visit(syntax.Initializer);
        }

        public virtual void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax syntax) {
        }

        public virtual void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax syntax) {
        }

        public virtual void VisitOperatorDeclaration(OperatorDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.ReturnType);
            VisitList(syntax.Parameters);
            Visit(syntax.Body);
        }

        public virtual void VisitParameter(ParameterSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.ParameterType);
            Visit(syntax.Identifier);
        }

        public virtual void VisitParenthesizedExpression(ParenthesizedExpressionSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax syntax) {
            Visit(syntax.Operand);
        }

        public virtual void VisitPredefinedType(PredefinedTypeSyntax syntax) {
        }

        public virtual void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax syntax) {
            Visit(syntax.Operand);
        }

        public virtual void VisitPropertyDeclaration(PropertyDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Type);
            Visit(syntax.ExplicitInterfaceSpecifier);
            Visit(syntax.Identifier);
            VisitList(syntax.Accessors);
        }

        public virtual void VisitQualifiedName(QualifiedNameSyntax syntax) {
            Visit(syntax.Left);
            Visit(syntax.Right);
        }

        public virtual void VisitReturnStatement(ReturnStatementSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitSizeOfExpression(SizeOfExpressionSyntax syntax) {
            Visit(syntax.TargetType);
        }

        public virtual void VisitSwitchSection(SwitchSectionSyntax syntax) {
            VisitList(syntax.Values);
            Visit(syntax.Block);
        }

        public virtual void VisitSwitchStatement(SwitchStatementSyntax syntax) {
            Visit(syntax.Expression);
            VisitList(syntax.Sections);
        }

        public virtual void VisitThrowStatement(ThrowStatementSyntax syntax) {
            Visit(syntax.Expression);
        }

        public virtual void VisitTrackedType(TrackedTypeSyntax syntax) {
            Visit(syntax.ElementType);
        }

        public virtual void VisitTryStatement(TryStatementSyntax syntax) {
            Visit(syntax.Block);
            VisitList(syntax.Catches);
            Visit(syntax.Finally);
        }

        public virtual void VisitTypeConstraint(TypeConstraintSyntax syntax) {
            Visit(syntax.ConstrainedType);
        }

        public virtual void VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Identifier);
            VisitList(syntax.TypeParameters);
            VisitList(syntax.ConstraintClauses);
            VisitList(syntax.Members);
            VisitList(syntax.BaseTypes);
        }

        public virtual void VisitTypeFamilyConstraint(TypeFamilyConstraintSyntax syntax) {
        }

        public virtual void VisitTypeOfExpression(TypeOfExpressionSyntax syntax) {
            Visit(syntax.TargetType);
        }

        public virtual void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax syntax) {
            Visit(syntax.Name);
            VisitList(syntax.Constraints);
        }

        public virtual void VisitTypeParameter(TypeParameterSyntax syntax) {
            VisitList(syntax.AttributeLists);
            Visit(syntax.Identifier);
        }

        public virtual void VisitUsingStatement(UsingStatementSyntax syntax) {
            Visit(syntax.Declaration);
            Visit(syntax.Expression);
            Visit(syntax.Block);
        }

        public virtual void VisitVariableDeclarationExpression(VariableDeclarationExpressionSyntax syntax) {
            Visit(syntax.VariableType);
            Visit(syntax.Identifier);
        }

        public virtual void VisitVariableDeclaration(VariableDeclarationSyntax syntax) {
            Visit(syntax.VariableType);
            VisitList(syntax.Variables);
        }

        public virtual void VisitVariableDeclarator(VariableDeclaratorSyntax syntax) {
            Visit(syntax.Identifier);
            Visit(syntax.Value);
        }

        public virtual void VisitVarType(VarTypeSyntax syntax) {
        }

        public virtual void VisitWhileStatement(WhileStatementSyntax syntax) {
            Visit(syntax.Condition);
            Visit(syntax.Block);
        }
    }

    public enum SyntaxKind {
        AccessorDeclaration,
        AliasQualifiedName,
        AnonymousObjectCreationExpression,
        AnonymousObjectMemberDeclarator,
        Argument,
        ArrayCreationExpression,
        ArrayRankSpecifier,
        ArrayType,
        AssertStatement,
        AttributeArgument,
        AttributeList,
        Attribute,
        AwaitExpression,
        BinaryExpression,
        Block,
        BreakStatement,
        CastExpression,
        CatchClause,
        CompilationUnit,
        ConditionalExpression,
        ConstructorConstraint,
        ConstructorDeclaration,
        ConstructorInitializer,
        ContinueStatement,
        ConversionOperatorDeclaration,
        DefaultExpression,
        DelegateDeclaration,
        DeleteStatement,
        DestructorDeclaration,
        DoStatement,
        ElementAccessExpression,
        ElseClause,
        EmptyStatement,
        EnumDeclaration,
        EnumMemberDeclaration,
        EventDeclaration,
        EventFieldDeclaration,
        ExpressionStatement,
        FieldDeclaration,
        FinallyClause,
        ForEachStatement,
        ForStatement,
        GenericName,
        IdentifierName,
        IfStatement,
        ImplicitArrayCreationExpression,
        ImportDirective,
        IndexerDeclaration,
        InitializerExpression,
        InstanceExpression,
        InvocationExpression,
        LambdaExpression,
        LiteralExpression,
        LocalDeclarationStatement,
        LoopStatement,
        MemberAccessExpression,
        MethodDeclaration,
        NakedNullableType,
        NamespaceDeclaration,
        NullableType,
        ObjectCreationExpression,
        OmittedArraySizeExpression,
        OmittedTypeArgument,
        OperatorDeclaration,
        Parameter,
        ParenthesizedExpression,
        PostfixUnaryExpression,
        PredefinedType,
        PrefixUnaryExpression,
        PropertyDeclaration,
        QualifiedName,
        ReturnStatement,
        SizeOfExpression,
        SwitchSection,
        SwitchStatement,
        ThrowStatement,
        TrackedType,
        TryStatement,
        TypeConstraint,
        TypeDeclaration,
        TypeFamilyConstraint,
        TypeOfExpression,
        TypeParameterConstraintClause,
        TypeParameter,
        UsingStatement,
        VariableDeclarationExpression,
        VariableDeclaration,
        VariableDeclarator,
        VarType,
        WhileStatement,
    }
}
