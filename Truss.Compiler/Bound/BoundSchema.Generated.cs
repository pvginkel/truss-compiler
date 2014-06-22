using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;

namespace Truss.Compiler.Bound {
    public class BoundAccessorDeclaration : BoundNode {
        public BoundAccessorDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, AccessorDeclarationType type, BoundBlock body, Span span)
            : base(span) {
            if (attributeLists == null) {
                throw new ArgumentNullException("attributeLists");
            }

            AttributeLists = attributeLists;
            Modifiers = modifiers;
            Type = type;
            Body = body;
        }

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public AccessorDeclarationType Type { get; private set; }

        public BoundBlock Body { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AccessorDeclaration; }
        }

        public BoundAccessorDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, AccessorDeclarationType type, BoundBlock body, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Type != type || Body != body || Span != span) {
                return new BoundAccessorDeclaration(attributeLists, modifiers, type, body, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAccessorDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAccessorDeclaration(this);
        }
    }

    public class BoundAliasQualifiedName : BoundName {
        public BoundAliasQualifiedName(BoundIdentifierName alias, BoundSimpleName name, Span span)
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

        public BoundIdentifierName Alias { get; private set; }

        public BoundSimpleName Name { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AliasQualifiedName; }
        }

        public BoundAliasQualifiedName Update(BoundIdentifierName alias, BoundSimpleName name, Span span) {
            if (Alias != alias || Name != name || Span != span) {
                return new BoundAliasQualifiedName(alias, name, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAliasQualifiedName(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAliasQualifiedName(this);
        }
    }

    public class BoundAnonymousObjectCreationExpression : BoundExpression {
        public BoundAnonymousObjectCreationExpression(ImmutableArray<BoundAnonymousObjectMemberDeclarator> initializers, Span span)
            : base(span) {
            if (initializers == null) {
                throw new ArgumentNullException("initializers");
            }

            Initializers = initializers;
        }

        public ImmutableArray<BoundAnonymousObjectMemberDeclarator> Initializers { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AnonymousObjectCreationExpression; }
        }

        public BoundAnonymousObjectCreationExpression Update(ImmutableArray<BoundAnonymousObjectMemberDeclarator> initializers, Span span) {
            if (Initializers != initializers || Span != span) {
                return new BoundAnonymousObjectCreationExpression(initializers, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAnonymousObjectCreationExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAnonymousObjectCreationExpression(this);
        }
    }

    public class BoundAnonymousObjectMemberDeclarator : BoundNode {
        public BoundAnonymousObjectMemberDeclarator(BoundIdentifierName name, BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Name = name;
            Expression = expression;
        }

        public BoundIdentifierName Name { get; private set; }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AnonymousObjectMemberDeclarator; }
        }

        public BoundAnonymousObjectMemberDeclarator Update(BoundIdentifierName name, BoundExpression expression, Span span) {
            if (Name != name || Expression != expression || Span != span) {
                return new BoundAnonymousObjectMemberDeclarator(name, expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAnonymousObjectMemberDeclarator(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAnonymousObjectMemberDeclarator(this);
        }
    }

    public class BoundArgument : BoundNode {
        public BoundArgument(ImmutableArray<ParameterModifier> modifiers, BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Modifiers = modifiers;
            Expression = expression;
        }

        public ImmutableArray<ParameterModifier> Modifiers { get; private set; }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.Argument; }
        }

        public BoundArgument Update(ImmutableArray<ParameterModifier> modifiers, BoundExpression expression, Span span) {
            if (Modifiers != modifiers || Expression != expression || Span != span) {
                return new BoundArgument(modifiers, expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArgument(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitArgument(this);
        }
    }

    public class BoundArrayCreationExpression : BoundExpression {
        public BoundArrayCreationExpression(BoundArrayType type, BoundInitializerExpression initializer, Span span)
            : base(span) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }

            Type = type;
            Initializer = initializer;
        }

        public BoundArrayType Type { get; private set; }

        public BoundInitializerExpression Initializer { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ArrayCreationExpression; }
        }

        public BoundArrayCreationExpression Update(BoundArrayType type, BoundInitializerExpression initializer, Span span) {
            if (Type != type || Initializer != initializer || Span != span) {
                return new BoundArrayCreationExpression(type, initializer, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArrayCreationExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitArrayCreationExpression(this);
        }
    }

    public class BoundArrayRankSpecifier : BoundNode {
        public BoundArrayRankSpecifier(BoundExpression size, bool isTracked, Span span)
            : base(span) {
            if (size == null) {
                throw new ArgumentNullException("size");
            }

            Size = size;
            IsTracked = isTracked;
        }

        public BoundExpression Size { get; private set; }

        public bool IsTracked { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ArrayRankSpecifier; }
        }

        public BoundArrayRankSpecifier Update(BoundExpression size, bool isTracked, Span span) {
            if (Size != size || IsTracked != isTracked || Span != span) {
                return new BoundArrayRankSpecifier(size, isTracked, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArrayRankSpecifier(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitArrayRankSpecifier(this);
        }
    }

    public class BoundArrayType : BoundType {
        public BoundArrayType(BoundType elementType, ImmutableArray<BoundArrayRankSpecifier> rankSpecifiers, Span span)
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

        public BoundType ElementType { get; private set; }

        public ImmutableArray<BoundArrayRankSpecifier> RankSpecifiers { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ArrayType; }
        }

        public BoundArrayType Update(BoundType elementType, ImmutableArray<BoundArrayRankSpecifier> rankSpecifiers, Span span) {
            if (ElementType != elementType || RankSpecifiers != rankSpecifiers || Span != span) {
                return new BoundArrayType(elementType, rankSpecifiers, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitArrayType(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitArrayType(this);
        }
    }

    public class BoundAssertStatement : BoundStatement {
        public BoundAssertStatement(BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AssertStatement; }
        }

        public BoundAssertStatement Update(BoundExpression expression, Span span) {
            if (Expression != expression || Span != span) {
                return new BoundAssertStatement(expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAssertStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAssertStatement(this);
        }
    }

    public class BoundAttribute : BoundNode {
        public BoundAttribute(BoundName name, ImmutableArray<BoundAttributeArgument> arguments, Span span)
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

        public BoundName Name { get; private set; }

        public ImmutableArray<BoundAttributeArgument> Arguments { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.Attribute; }
        }

        public BoundAttribute Update(BoundName name, ImmutableArray<BoundAttributeArgument> arguments, Span span) {
            if (Name != name || Arguments != arguments || Span != span) {
                return new BoundAttribute(name, arguments, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAttribute(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAttribute(this);
        }
    }

    public class BoundAttributeArgument : BoundNode {
        public BoundAttributeArgument(BoundIdentifierName name, BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Name = name;
            Expression = expression;
        }

        public BoundIdentifierName Name { get; private set; }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AttributeArgument; }
        }

        public BoundAttributeArgument Update(BoundIdentifierName name, BoundExpression expression, Span span) {
            if (Name != name || Expression != expression || Span != span) {
                return new BoundAttributeArgument(name, expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAttributeArgument(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAttributeArgument(this);
        }
    }

    public class BoundAttributeList : BoundNode {
        public BoundAttributeList(AttributeTarget target, ImmutableArray<BoundAttribute> attributes, Span span)
            : base(span) {
            if (attributes == null) {
                throw new ArgumentNullException("attributes");
            }

            Target = target;
            Attributes = attributes;
        }

        public AttributeTarget Target { get; private set; }

        public ImmutableArray<BoundAttribute> Attributes { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AttributeList; }
        }

        public BoundAttributeList Update(AttributeTarget target, ImmutableArray<BoundAttribute> attributes, Span span) {
            if (Target != target || Attributes != attributes || Span != span) {
                return new BoundAttributeList(target, attributes, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAttributeList(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAttributeList(this);
        }
    }

    public class BoundAwaitExpression : BoundExpression {
        public BoundAwaitExpression(BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.AwaitExpression; }
        }

        public BoundAwaitExpression Update(BoundExpression expression, Span span) {
            if (Expression != expression || Span != span) {
                return new BoundAwaitExpression(expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitAwaitExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitAwaitExpression(this);
        }
    }

    public abstract class BoundBaseFieldDeclaration : BoundMemberDeclaration {
        protected BoundBaseFieldDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundVariableDeclaration declaration, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public BoundVariableDeclaration Declaration { get; private set; }
    }

    public abstract class BoundBaseMethodDeclaration : BoundMemberDeclaration {
        protected BoundBaseMethodDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public ImmutableArray<BoundParameter> Parameters { get; private set; }

        public BoundBlock Body { get; private set; }
    }

    public abstract class BoundBasePropertyDeclaration : BoundMemberDeclaration {
        protected BoundBasePropertyDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType type, BoundName explicitInterfaceSpecifier, ImmutableArray<BoundAccessorDeclaration> accessors, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public BoundType Type { get; private set; }

        public BoundName ExplicitInterfaceSpecifier { get; private set; }

        public ImmutableArray<BoundAccessorDeclaration> Accessors { get; private set; }
    }

    public abstract class BoundBaseTypeDeclaration : BoundMemberDeclaration {
        protected BoundBaseTypeDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, ImmutableArray<BoundType> baseTypes, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public ImmutableArray<BoundType> BaseTypes { get; private set; }
    }

    public class BoundBinaryExpression : BoundExpression {
        public BoundBinaryExpression(BinaryOperator operator_, BoundExpression left, BoundExpression right, Span span)
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

        public BoundExpression Left { get; private set; }

        public BoundExpression Right { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.BinaryExpression; }
        }

        public BoundBinaryExpression Update(BinaryOperator operator_, BoundExpression left, BoundExpression right, Span span) {
            if (Operator != operator_ || Left != left || Right != right || Span != span) {
                return new BoundBinaryExpression(operator_, left, right, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitBinaryExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitBinaryExpression(this);
        }
    }

    public class BoundBlock : BoundStatement {
        public BoundBlock(ImmutableArray<BoundStatement> statements, Span span)
            : base(span) {
            if (statements == null) {
                throw new ArgumentNullException("statements");
            }

            Statements = statements;
        }

        public ImmutableArray<BoundStatement> Statements { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.Block; }
        }

        public BoundBlock Update(ImmutableArray<BoundStatement> statements, Span span) {
            if (Statements != statements || Span != span) {
                return new BoundBlock(statements, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitBlock(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitBlock(this);
        }
    }

    public class BoundBreakStatement : BoundStatement {
        public BoundBreakStatement(Span span)
            : base(span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.BreakStatement; }
        }

        public BoundBreakStatement Update(Span span) {
            if (Span != span) {
                return new BoundBreakStatement(span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitBreakStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitBreakStatement(this);
        }
    }

    public class BoundCastExpression : BoundExpression {
        public BoundCastExpression(BoundExpression expression, BoundType targetType, Span span)
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

        public BoundExpression Expression { get; private set; }

        public BoundType TargetType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.CastExpression; }
        }

        public BoundCastExpression Update(BoundExpression expression, BoundType targetType, Span span) {
            if (Expression != expression || TargetType != targetType || Span != span) {
                return new BoundCastExpression(expression, targetType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitCastExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitCastExpression(this);
        }
    }

    public class BoundCatchClause : BoundNode {
        public BoundCatchClause(BoundType exceptionType, BoundIdentifierName identifier, BoundBlock block, Span span)
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

        public BoundType ExceptionType { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.CatchClause; }
        }

        public BoundCatchClause Update(BoundType exceptionType, BoundIdentifierName identifier, BoundBlock block, Span span) {
            if (ExceptionType != exceptionType || Identifier != identifier || Block != block || Span != span) {
                return new BoundCatchClause(exceptionType, identifier, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitCatchClause(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitCatchClause(this);
        }
    }

    public class BoundCompilationUnit : BoundNode {
        public BoundCompilationUnit(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<BoundImportDirective> imports, ImmutableArray<BoundMemberDeclaration> members, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<BoundImportDirective> Imports { get; private set; }

        public ImmutableArray<BoundMemberDeclaration> Members { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.CompilationUnit; }
        }

        public BoundCompilationUnit Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<BoundImportDirective> imports, ImmutableArray<BoundMemberDeclaration> members, Span span) {
            if (AttributeLists != attributeLists || Imports != imports || Members != members || Span != span) {
                return new BoundCompilationUnit(attributeLists, imports, members, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitCompilationUnit(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitCompilationUnit(this);
        }
    }

    public class BoundConditionalExpression : BoundExpression {
        public BoundConditionalExpression(BoundExpression condition, BoundExpression whenTrue, BoundExpression whenFalse, Span span)
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

        public BoundExpression Condition { get; private set; }

        public BoundExpression WhenTrue { get; private set; }

        public BoundExpression WhenFalse { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ConditionalExpression; }
        }

        public BoundConditionalExpression Update(BoundExpression condition, BoundExpression whenTrue, BoundExpression whenFalse, Span span) {
            if (Condition != condition || WhenTrue != whenTrue || WhenFalse != whenFalse || Span != span) {
                return new BoundConditionalExpression(condition, whenTrue, whenFalse, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConditionalExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitConditionalExpression(this);
        }
    }

    public class BoundConstructorConstraint : BoundTypeParameterConstraint {
        public BoundConstructorConstraint(Span span)
            : base(span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.ConstructorConstraint; }
        }

        public BoundConstructorConstraint Update(Span span) {
            if (Span != span) {
                return new BoundConstructorConstraint(span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructorConstraint(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitConstructorConstraint(this);
        }
    }

    public class BoundConstructorDeclaration : BoundBaseMethodDeclaration {
        public BoundConstructorDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, BoundConstructorInitializer initializer, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
            Initializer = initializer;
        }

        public BoundIdentifierName Identifier { get; private set; }

        public BoundConstructorInitializer Initializer { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ConstructorDeclaration; }
        }

        public BoundConstructorDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, BoundConstructorInitializer initializer, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Identifier != identifier || Initializer != initializer || Parameters != parameters || Body != body || Span != span) {
                return new BoundConstructorDeclaration(attributeLists, modifiers, identifier, initializer, parameters, body, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructorDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitConstructorDeclaration(this);
        }
    }

    public class BoundConstructorInitializer : BoundNode {
        public BoundConstructorInitializer(ThisOrBase type, ImmutableArray<BoundArgument> arguments, Span span)
            : base(span) {
            if (arguments == null) {
                throw new ArgumentNullException("arguments");
            }

            Type = type;
            Arguments = arguments;
        }

        public ThisOrBase Type { get; private set; }

        public ImmutableArray<BoundArgument> Arguments { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ConstructorInitializer; }
        }

        public BoundConstructorInitializer Update(ThisOrBase type, ImmutableArray<BoundArgument> arguments, Span span) {
            if (Type != type || Arguments != arguments || Span != span) {
                return new BoundConstructorInitializer(type, arguments, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructorInitializer(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitConstructorInitializer(this);
        }
    }

    public class BoundContinueStatement : BoundStatement {
        public BoundContinueStatement(Span span)
            : base(span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.ContinueStatement; }
        }

        public BoundContinueStatement Update(Span span) {
            if (Span != span) {
                return new BoundContinueStatement(span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitContinueStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitContinueStatement(this);
        }
    }

    public class BoundConversionOperatorDeclaration : BoundBaseMethodDeclaration {
        public BoundConversionOperatorDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, ImplicitOrExplicit type, BoundType targetType, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            Type = type;
            TargetType = targetType;
        }

        public ImplicitOrExplicit Type { get; private set; }

        public BoundType TargetType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ConversionOperatorDeclaration; }
        }

        public BoundConversionOperatorDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, ImplicitOrExplicit type, BoundType targetType, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Type != type || TargetType != targetType || Parameters != parameters || Body != body || Span != span) {
                return new BoundConversionOperatorDeclaration(attributeLists, modifiers, type, targetType, parameters, body, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConversionOperatorDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitConversionOperatorDeclaration(this);
        }
    }

    public class BoundDefaultExpression : BoundExpression {
        public BoundDefaultExpression(BoundType targetType, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
        }

        public BoundType TargetType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.DefaultExpression; }
        }

        public BoundDefaultExpression Update(BoundType targetType, Span span) {
            if (TargetType != targetType || Span != span) {
                return new BoundDefaultExpression(targetType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDefaultExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitDefaultExpression(this);
        }
    }

    public class BoundDelegateDeclaration : BoundMemberDeclaration {
        public BoundDelegateDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType returnType, BoundIdentifierName identifier, ImmutableArray<BoundTypeParameter> typeParameters, ImmutableArray<BoundParameter> parameters, ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public BoundType ReturnType { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public ImmutableArray<BoundTypeParameter> TypeParameters { get; private set; }

        public ImmutableArray<BoundParameter> Parameters { get; private set; }

        public ImmutableArray<BoundTypeParameterConstraintClause> ConstraintClauses { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.DelegateDeclaration; }
        }

        public BoundDelegateDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType returnType, BoundIdentifierName identifier, ImmutableArray<BoundTypeParameter> typeParameters, ImmutableArray<BoundParameter> parameters, ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || ReturnType != returnType || Identifier != identifier || TypeParameters != typeParameters || Parameters != parameters || ConstraintClauses != constraintClauses || Span != span) {
                return new BoundDelegateDeclaration(attributeLists, modifiers, returnType, identifier, typeParameters, parameters, constraintClauses, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDelegateDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitDelegateDeclaration(this);
        }
    }

    public class BoundDeleteStatement : BoundStatement {
        public BoundDeleteStatement(BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.DeleteStatement; }
        }

        public BoundDeleteStatement Update(BoundExpression expression, Span span) {
            if (Expression != expression || Span != span) {
                return new BoundDeleteStatement(expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDeleteStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitDeleteStatement(this);
        }
    }

    public class BoundDestructorDeclaration : BoundBaseMethodDeclaration {
        public BoundDestructorDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public BoundIdentifierName Identifier { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.DestructorDeclaration; }
        }

        public BoundDestructorDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Identifier != identifier || Parameters != parameters || Body != body || Span != span) {
                return new BoundDestructorDeclaration(attributeLists, modifiers, identifier, parameters, body, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDestructorDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitDestructorDeclaration(this);
        }
    }

    public class BoundDoStatement : BoundStatement {
        public BoundDoStatement(BoundExpression condition, BoundBlock block, Span span)
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

        public BoundExpression Condition { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.DoStatement; }
        }

        public BoundDoStatement Update(BoundExpression condition, BoundBlock block, Span span) {
            if (Condition != condition || Block != block || Span != span) {
                return new BoundDoStatement(condition, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDoStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitDoStatement(this);
        }
    }

    public class BoundElementAccessExpression : BoundExpression {
        public BoundElementAccessExpression(BoundExpression expression, ImmutableArray<BoundExpression> indexExpressions, Span span)
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

        public BoundExpression Expression { get; private set; }

        public ImmutableArray<BoundExpression> IndexExpressions { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ElementAccessExpression; }
        }

        public BoundElementAccessExpression Update(BoundExpression expression, ImmutableArray<BoundExpression> indexExpressions, Span span) {
            if (Expression != expression || IndexExpressions != indexExpressions || Span != span) {
                return new BoundElementAccessExpression(expression, indexExpressions, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitElementAccessExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitElementAccessExpression(this);
        }
    }

    public class BoundElseClause : BoundNode {
        public BoundElseClause(ElIfOrElse type, BoundExpression condition, BoundBlock block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Type = type;
            Condition = condition;
            Block = block;
        }

        public ElIfOrElse Type { get; private set; }

        public BoundExpression Condition { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ElseClause; }
        }

        public BoundElseClause Update(ElIfOrElse type, BoundExpression condition, BoundBlock block, Span span) {
            if (Type != type || Condition != condition || Block != block || Span != span) {
                return new BoundElseClause(type, condition, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitElseClause(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitElseClause(this);
        }
    }

    public class BoundEmptyStatement : BoundStatement {
        public BoundEmptyStatement(Span span)
            : base(span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.EmptyStatement; }
        }

        public BoundEmptyStatement Update(Span span) {
            if (Span != span) {
                return new BoundEmptyStatement(span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEmptyStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitEmptyStatement(this);
        }
    }

    public class BoundEnumDeclaration : BoundBaseTypeDeclaration {
        public BoundEnumDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, ImmutableArray<BoundEnumMemberDeclaration> members, ImmutableArray<BoundType> baseTypes, Span span)
            : base(attributeLists, modifiers, identifier, baseTypes, span) {
            if (members == null) {
                throw new ArgumentNullException("members");
            }

            Members = members;
        }

        public ImmutableArray<BoundEnumMemberDeclaration> Members { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.EnumDeclaration; }
        }

        public BoundEnumDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, ImmutableArray<BoundEnumMemberDeclaration> members, ImmutableArray<BoundType> baseTypes, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Identifier != identifier || Members != members || BaseTypes != baseTypes || Span != span) {
                return new BoundEnumDeclaration(attributeLists, modifiers, identifier, members, baseTypes, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEnumDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitEnumDeclaration(this);
        }
    }

    public class BoundEnumMemberDeclaration : BoundMemberDeclaration {
        public BoundEnumMemberDeclaration(ImmutableArray<BoundAttributeList> attributeLists, BoundIdentifierName identifier, BoundExpression value, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public BoundExpression Value { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.EnumMemberDeclaration; }
        }

        public BoundEnumMemberDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, BoundIdentifierName identifier, BoundExpression value, Span span) {
            if (AttributeLists != attributeLists || Identifier != identifier || Value != value || Span != span) {
                return new BoundEnumMemberDeclaration(attributeLists, identifier, value, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEnumMemberDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitEnumMemberDeclaration(this);
        }
    }

    public class BoundEventDeclaration : BoundBasePropertyDeclaration {
        public BoundEventDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType type, BoundName explicitInterfaceSpecifier, BoundIdentifierName identifier, ImmutableArray<BoundAccessorDeclaration> accessors, Span span)
            : base(attributeLists, modifiers, type, explicitInterfaceSpecifier, accessors, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public BoundIdentifierName Identifier { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.EventDeclaration; }
        }

        public BoundEventDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType type, BoundName explicitInterfaceSpecifier, BoundIdentifierName identifier, ImmutableArray<BoundAccessorDeclaration> accessors, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Type != type || ExplicitInterfaceSpecifier != explicitInterfaceSpecifier || Identifier != identifier || Accessors != accessors || Span != span) {
                return new BoundEventDeclaration(attributeLists, modifiers, type, explicitInterfaceSpecifier, identifier, accessors, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEventDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitEventDeclaration(this);
        }
    }

    public class BoundEventFieldDeclaration : BoundBaseFieldDeclaration {
        public BoundEventFieldDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundVariableDeclaration declaration, Span span)
            : base(attributeLists, modifiers, declaration, span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.EventFieldDeclaration; }
        }

        public BoundEventFieldDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundVariableDeclaration declaration, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Declaration != declaration || Span != span) {
                return new BoundEventFieldDeclaration(attributeLists, modifiers, declaration, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEventFieldDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitEventFieldDeclaration(this);
        }
    }

    public abstract class BoundExpression : BoundNode {
        protected BoundExpression(Span span)
            : base(span) {

        }
    }

    public class BoundExpressionStatement : BoundStatement {
        public BoundExpressionStatement(BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ExpressionStatement; }
        }

        public BoundExpressionStatement Update(BoundExpression expression, Span span) {
            if (Expression != expression || Span != span) {
                return new BoundExpressionStatement(expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitExpressionStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitExpressionStatement(this);
        }
    }

    public class BoundFieldDeclaration : BoundBaseFieldDeclaration {
        public BoundFieldDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundVariableDeclaration declaration, Span span)
            : base(attributeLists, modifiers, declaration, span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.FieldDeclaration; }
        }

        public BoundFieldDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundVariableDeclaration declaration, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Declaration != declaration || Span != span) {
                return new BoundFieldDeclaration(attributeLists, modifiers, declaration, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitFieldDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitFieldDeclaration(this);
        }
    }

    public class BoundFinallyClause : BoundNode {
        public BoundFinallyClause(BoundBlock block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Block = block;
        }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.FinallyClause; }
        }

        public BoundFinallyClause Update(BoundBlock block, Span span) {
            if (Block != block || Span != span) {
                return new BoundFinallyClause(block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitFinallyClause(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitFinallyClause(this);
        }
    }

    public class BoundForEachStatement : BoundStatement {
        public BoundForEachStatement(BoundType elementType, BoundIdentifierName identifier, BoundExpression expression, BoundBlock block, Span span)
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

        public BoundType ElementType { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public BoundExpression Expression { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ForEachStatement; }
        }

        public BoundForEachStatement Update(BoundType elementType, BoundIdentifierName identifier, BoundExpression expression, BoundBlock block, Span span) {
            if (ElementType != elementType || Identifier != identifier || Expression != expression || Block != block || Span != span) {
                return new BoundForEachStatement(elementType, identifier, expression, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitForEachStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitForEachStatement(this);
        }
    }

    public class BoundForStatement : BoundStatement {
        public BoundForStatement(BoundVariableDeclaration declaration, ImmutableArray<BoundExpression> initializers, BoundExpression condition, ImmutableArray<BoundExpression> incrementors, BoundBlock block, Span span)
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

        public BoundVariableDeclaration Declaration { get; private set; }

        public ImmutableArray<BoundExpression> Initializers { get; private set; }

        public BoundExpression Condition { get; private set; }

        public ImmutableArray<BoundExpression> Incrementors { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ForStatement; }
        }

        public BoundForStatement Update(BoundVariableDeclaration declaration, ImmutableArray<BoundExpression> initializers, BoundExpression condition, ImmutableArray<BoundExpression> incrementors, BoundBlock block, Span span) {
            if (Declaration != declaration || Initializers != initializers || Condition != condition || Incrementors != incrementors || Block != block || Span != span) {
                return new BoundForStatement(declaration, initializers, condition, incrementors, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitForStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitForStatement(this);
        }
    }

    public class BoundGenericName : BoundSimpleName {
        public BoundGenericName(string identifier, ImmutableArray<BoundType> typeArguments, Span span)
            : base(identifier, span) {
            if (typeArguments == null) {
                throw new ArgumentNullException("typeArguments");
            }

            TypeArguments = typeArguments;
        }

        public ImmutableArray<BoundType> TypeArguments { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.GenericName; }
        }

        public BoundGenericName Update(string identifier, ImmutableArray<BoundType> typeArguments, Span span) {
            if (Identifier != identifier || TypeArguments != typeArguments || Span != span) {
                return new BoundGenericName(identifier, typeArguments, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitGenericName(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitGenericName(this);
        }

            public bool IsUnboundGenericName {
                get {
                    return TypeArguments.Any(p => p is BoundOmittedTypeArgument);
                }
            }
        
    }

    public class BoundIdentifierName : BoundSimpleName {
        public BoundIdentifierName(string identifier, Span span)
            : base(identifier, span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.IdentifierName; }
        }

        public BoundIdentifierName Update(string identifier, Span span) {
            if (Identifier != identifier || Span != span) {
                return new BoundIdentifierName(identifier, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitIdentifierName(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitIdentifierName(this);
        }
    }

    public class BoundIfStatement : BoundStatement {
        public BoundIfStatement(BoundExpression condition, BoundBlock block, ImmutableArray<BoundElseClause> elses, Span span)
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

        public BoundExpression Condition { get; private set; }

        public BoundBlock Block { get; private set; }

        public ImmutableArray<BoundElseClause> Elses { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.IfStatement; }
        }

        public BoundIfStatement Update(BoundExpression condition, BoundBlock block, ImmutableArray<BoundElseClause> elses, Span span) {
            if (Condition != condition || Block != block || Elses != elses || Span != span) {
                return new BoundIfStatement(condition, block, elses, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitIfStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitIfStatement(this);
        }
    }

    public class BoundImplicitArrayCreationExpression : BoundExpression {
        public BoundImplicitArrayCreationExpression(ImmutableArray<BoundArrayRankSpecifier> rankSpecifiers, BoundInitializerExpression initializer, Span span)
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

        public ImmutableArray<BoundArrayRankSpecifier> RankSpecifiers { get; private set; }

        public BoundInitializerExpression Initializer { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ImplicitArrayCreationExpression; }
        }

        public BoundImplicitArrayCreationExpression Update(ImmutableArray<BoundArrayRankSpecifier> rankSpecifiers, BoundInitializerExpression initializer, Span span) {
            if (RankSpecifiers != rankSpecifiers || Initializer != initializer || Span != span) {
                return new BoundImplicitArrayCreationExpression(rankSpecifiers, initializer, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitImplicitArrayCreationExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitImplicitArrayCreationExpression(this);
        }
    }

    public class BoundImportDirective : BoundNode {
        public BoundImportDirective(bool isStatic, BoundIdentifierName alias, BoundName name, Span span)
            : base(span) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            IsStatic = isStatic;
            Alias = alias;
            Name = name;
        }

        public bool IsStatic { get; private set; }

        public BoundIdentifierName Alias { get; private set; }

        public BoundName Name { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ImportDirective; }
        }

        public BoundImportDirective Update(bool isStatic, BoundIdentifierName alias, BoundName name, Span span) {
            if (IsStatic != isStatic || Alias != alias || Name != name || Span != span) {
                return new BoundImportDirective(isStatic, alias, name, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitImportDirective(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitImportDirective(this);
        }
    }

    public class BoundIndexerDeclaration : BoundBasePropertyDeclaration {
        public BoundIndexerDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType type, BoundName explicitInterfaceSpecifier, ImmutableArray<BoundParameter> parameters, ImmutableArray<BoundAccessorDeclaration> accessors, Span span)
            : base(attributeLists, modifiers, type, explicitInterfaceSpecifier, accessors, span) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }

            Parameters = parameters;
        }

        public ImmutableArray<BoundParameter> Parameters { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.IndexerDeclaration; }
        }

        public BoundIndexerDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType type, BoundName explicitInterfaceSpecifier, ImmutableArray<BoundParameter> parameters, ImmutableArray<BoundAccessorDeclaration> accessors, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Type != type || ExplicitInterfaceSpecifier != explicitInterfaceSpecifier || Parameters != parameters || Accessors != accessors || Span != span) {
                return new BoundIndexerDeclaration(attributeLists, modifiers, type, explicitInterfaceSpecifier, parameters, accessors, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitIndexerDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitIndexerDeclaration(this);
        }
    }

    public class BoundInitializerExpression : BoundExpression {
        public BoundInitializerExpression(ImmutableArray<BoundExpression> expressions, Span span)
            : base(span) {
            if (expressions == null) {
                throw new ArgumentNullException("expressions");
            }

            Expressions = expressions;
        }

        public ImmutableArray<BoundExpression> Expressions { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.InitializerExpression; }
        }

        public BoundInitializerExpression Update(ImmutableArray<BoundExpression> expressions, Span span) {
            if (Expressions != expressions || Span != span) {
                return new BoundInitializerExpression(expressions, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitInitializerExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitInitializerExpression(this);
        }
    }

    public class BoundInstanceExpression : BoundExpression {
        public BoundInstanceExpression(ThisOrBase type, Span span)
            : base(span) {

            Type = type;
        }

        public ThisOrBase Type { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.InstanceExpression; }
        }

        public BoundInstanceExpression Update(ThisOrBase type, Span span) {
            if (Type != type || Span != span) {
                return new BoundInstanceExpression(type, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitInstanceExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitInstanceExpression(this);
        }
    }

    public class BoundInvocationExpression : BoundExpression {
        public BoundInvocationExpression(BoundExpression expression, ImmutableArray<BoundArgument> arguments, Span span)
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

        public BoundExpression Expression { get; private set; }

        public ImmutableArray<BoundArgument> Arguments { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.InvocationExpression; }
        }

        public BoundInvocationExpression Update(BoundExpression expression, ImmutableArray<BoundArgument> arguments, Span span) {
            if (Expression != expression || Arguments != arguments || Span != span) {
                return new BoundInvocationExpression(expression, arguments, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitInvocationExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitInvocationExpression(this);
        }
    }

    public class BoundLambdaExpression : BoundExpression {
        public BoundLambdaExpression(ImmutableArray<Modifier> modifiers, ImmutableArray<BoundParameter> parameters, BoundNode body, Span span)
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

        public ImmutableArray<BoundParameter> Parameters { get; private set; }

        public BoundNode Body { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.LambdaExpression; }
        }

        public BoundLambdaExpression Update(ImmutableArray<Modifier> modifiers, ImmutableArray<BoundParameter> parameters, BoundNode body, Span span) {
            if (Modifiers != modifiers || Parameters != parameters || Body != body || Span != span) {
                return new BoundLambdaExpression(modifiers, parameters, body, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLambdaExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitLambdaExpression(this);
        }
    }

    public class BoundLiteralExpression : BoundExpression {
        public BoundLiteralExpression(LiteralType literalType, string value, Span span)
            : base(span) {

            LiteralType = literalType;
            Value = value;
        }

        public LiteralType LiteralType { get; private set; }

        public string Value { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.LiteralExpression; }
        }

        public BoundLiteralExpression Update(LiteralType literalType, string value, Span span) {
            if (LiteralType != literalType || Value != value || Span != span) {
                return new BoundLiteralExpression(literalType, value, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLiteralExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitLiteralExpression(this);
        }
    }

    public class BoundLocalDeclarationStatement : BoundStatement {
        public BoundLocalDeclarationStatement(ImmutableArray<Modifier> modifiers, BoundVariableDeclaration declaration, Span span)
            : base(span) {
            if (declaration == null) {
                throw new ArgumentNullException("declaration");
            }

            Modifiers = modifiers;
            Declaration = declaration;
        }

        public ImmutableArray<Modifier> Modifiers { get; private set; }

        public BoundVariableDeclaration Declaration { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.LocalDeclarationStatement; }
        }

        public BoundLocalDeclarationStatement Update(ImmutableArray<Modifier> modifiers, BoundVariableDeclaration declaration, Span span) {
            if (Modifiers != modifiers || Declaration != declaration || Span != span) {
                return new BoundLocalDeclarationStatement(modifiers, declaration, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLocalDeclarationStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitLocalDeclarationStatement(this);
        }
    }

    public class BoundLoopStatement : BoundStatement {
        public BoundLoopStatement(BoundBlock block, Span span)
            : base(span) {
            if (block == null) {
                throw new ArgumentNullException("block");
            }

            Block = block;
        }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.LoopStatement; }
        }

        public BoundLoopStatement Update(BoundBlock block, Span span) {
            if (Block != block || Span != span) {
                return new BoundLoopStatement(block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitLoopStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitLoopStatement(this);
        }
    }

    public class BoundMemberAccessExpression : BoundExpression {
        public BoundMemberAccessExpression(BoundExpression expression, BoundSimpleName name, Span span)
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

        public BoundExpression Expression { get; private set; }

        public BoundSimpleName Name { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.MemberAccessExpression; }
        }

        public BoundMemberAccessExpression Update(BoundExpression expression, BoundSimpleName name, Span span) {
            if (Expression != expression || Name != name || Span != span) {
                return new BoundMemberAccessExpression(expression, name, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitMemberAccessExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitMemberAccessExpression(this);
        }
    }

    public abstract class BoundMemberDeclaration : BoundNode {
        protected BoundMemberDeclaration(Span span)
            : base(span) {

        }
    }

    public class BoundMethodDeclaration : BoundBaseMethodDeclaration {
        public BoundMethodDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType returnType, BoundName explicitInterfaceSpecifier, BoundIdentifierName identifier, ImmutableArray<BoundTypeParameter> typeParameters, ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span)
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

        public BoundType ReturnType { get; private set; }

        public BoundName ExplicitInterfaceSpecifier { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public ImmutableArray<BoundTypeParameter> TypeParameters { get; private set; }

        public ImmutableArray<BoundTypeParameterConstraintClause> ConstraintClauses { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.MethodDeclaration; }
        }

        public BoundMethodDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType returnType, BoundName explicitInterfaceSpecifier, BoundIdentifierName identifier, ImmutableArray<BoundTypeParameter> typeParameters, ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || ReturnType != returnType || ExplicitInterfaceSpecifier != explicitInterfaceSpecifier || Identifier != identifier || TypeParameters != typeParameters || ConstraintClauses != constraintClauses || Parameters != parameters || Body != body || Span != span) {
                return new BoundMethodDeclaration(attributeLists, modifiers, returnType, explicitInterfaceSpecifier, identifier, typeParameters, constraintClauses, parameters, body, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitMethodDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitMethodDeclaration(this);
        }
    }

    public class BoundNakedNullableType : BoundType {
        public BoundNakedNullableType(Nullability type, Span span)
            : base(span) {

            Type = type;
        }

        public Nullability Type { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.NakedNullableType; }
        }

        public BoundNakedNullableType Update(Nullability type, Span span) {
            if (Type != type || Span != span) {
                return new BoundNakedNullableType(type, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNakedNullableType(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitNakedNullableType(this);
        }
    }

    public abstract class BoundName : BoundType {
        protected BoundName(Span span)
            : base(span) {

        }
    }

    public class BoundNamespaceDeclaration : BoundMemberDeclaration {
        public BoundNamespaceDeclaration(BoundName name, ImmutableArray<BoundImportDirective> imports, ImmutableArray<BoundMemberDeclaration> members, Span span)
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

        public BoundName Name { get; private set; }

        public ImmutableArray<BoundImportDirective> Imports { get; private set; }

        public ImmutableArray<BoundMemberDeclaration> Members { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.NamespaceDeclaration; }
        }

        public BoundNamespaceDeclaration Update(BoundName name, ImmutableArray<BoundImportDirective> imports, ImmutableArray<BoundMemberDeclaration> members, Span span) {
            if (Name != name || Imports != imports || Members != members || Span != span) {
                return new BoundNamespaceDeclaration(name, imports, members, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNamespaceDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitNamespaceDeclaration(this);
        }
    }

    public class BoundNullableType : BoundType {
        public BoundNullableType(BoundType elementType, Span span)
            : base(span) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }

            ElementType = elementType;
        }

        public BoundType ElementType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.NullableType; }
        }

        public BoundNullableType Update(BoundType elementType, Span span) {
            if (ElementType != elementType || Span != span) {
                return new BoundNullableType(elementType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNullableType(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitNullableType(this);
        }
    }

    public class BoundObjectCreationExpression : BoundExpression {
        public BoundObjectCreationExpression(BoundType targetType, ImmutableArray<BoundArgument> arguments, BoundInitializerExpression initializer, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
            Arguments = arguments;
            Initializer = initializer;
        }

        public BoundType TargetType { get; private set; }

        public ImmutableArray<BoundArgument> Arguments { get; private set; }

        public BoundInitializerExpression Initializer { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ObjectCreationExpression; }
        }

        public BoundObjectCreationExpression Update(BoundType targetType, ImmutableArray<BoundArgument> arguments, BoundInitializerExpression initializer, Span span) {
            if (TargetType != targetType || Arguments != arguments || Initializer != initializer || Span != span) {
                return new BoundObjectCreationExpression(targetType, arguments, initializer, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitObjectCreationExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitObjectCreationExpression(this);
        }
    }

    public class BoundOmittedArraySizeExpression : BoundExpression {
        public BoundOmittedArraySizeExpression(Span span)
            : base(span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.OmittedArraySizeExpression; }
        }

        public BoundOmittedArraySizeExpression Update(Span span) {
            if (Span != span) {
                return new BoundOmittedArraySizeExpression(span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOmittedArraySizeExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitOmittedArraySizeExpression(this);
        }
    }

    public class BoundOmittedTypeArgument : BoundType {
        public BoundOmittedTypeArgument(Span span)
            : base(span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.OmittedTypeArgument; }
        }

        public BoundOmittedTypeArgument Update(Span span) {
            if (Span != span) {
                return new BoundOmittedTypeArgument(span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOmittedTypeArgument(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitOmittedTypeArgument(this);
        }
    }

    public class BoundOperatorDeclaration : BoundBaseMethodDeclaration {
        public BoundOperatorDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType returnType, Operator operator_, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span)
            : base(attributeLists, modifiers, parameters, body, span) {
            if (returnType == null) {
                throw new ArgumentNullException("returnType");
            }

            ReturnType = returnType;
            Operator = operator_;
        }

        public BoundType ReturnType { get; private set; }

        public Operator Operator { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.OperatorDeclaration; }
        }

        public BoundOperatorDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType returnType, Operator operator_, ImmutableArray<BoundParameter> parameters, BoundBlock body, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || ReturnType != returnType || Operator != operator_ || Parameters != parameters || Body != body || Span != span) {
                return new BoundOperatorDeclaration(attributeLists, modifiers, returnType, operator_, parameters, body, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOperatorDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitOperatorDeclaration(this);
        }
    }

    public class BoundParameter : BoundNode {
        public BoundParameter(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<ParameterModifier> modifiers, BoundType parameterType, BoundIdentifierName identifier, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public ImmutableArray<ParameterModifier> Modifiers { get; private set; }

        public BoundType ParameterType { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.Parameter; }
        }

        public BoundParameter Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<ParameterModifier> modifiers, BoundType parameterType, BoundIdentifierName identifier, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || ParameterType != parameterType || Identifier != identifier || Span != span) {
                return new BoundParameter(attributeLists, modifiers, parameterType, identifier, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitParameter(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitParameter(this);
        }
    }

    public class BoundParenthesizedExpression : BoundExpression {
        public BoundParenthesizedExpression(BoundExpression expression, Span span)
            : base(span) {
            if (expression == null) {
                throw new ArgumentNullException("expression");
            }

            Expression = expression;
        }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ParenthesizedExpression; }
        }

        public BoundParenthesizedExpression Update(BoundExpression expression, Span span) {
            if (Expression != expression || Span != span) {
                return new BoundParenthesizedExpression(expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitParenthesizedExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitParenthesizedExpression(this);
        }
    }

    public class BoundPostfixUnaryExpression : BoundExpression {
        public BoundPostfixUnaryExpression(PostfixUnaryOperator operator_, BoundExpression operand, Span span)
            : base(span) {
            if (operand == null) {
                throw new ArgumentNullException("operand");
            }

            Operator = operator_;
            Operand = operand;
        }

        public PostfixUnaryOperator Operator { get; private set; }

        public BoundExpression Operand { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.PostfixUnaryExpression; }
        }

        public BoundPostfixUnaryExpression Update(PostfixUnaryOperator operator_, BoundExpression operand, Span span) {
            if (Operator != operator_ || Operand != operand || Span != span) {
                return new BoundPostfixUnaryExpression(operator_, operand, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPostfixUnaryExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitPostfixUnaryExpression(this);
        }
    }

    public class BoundPredefinedType : BoundType {
        public BoundPredefinedType(PredefinedType predefinedType, Span span)
            : base(span) {

            PredefinedType = predefinedType;
        }

        public PredefinedType PredefinedType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.PredefinedType; }
        }

        public BoundPredefinedType Update(PredefinedType predefinedType, Span span) {
            if (PredefinedType != predefinedType || Span != span) {
                return new BoundPredefinedType(predefinedType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPredefinedType(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitPredefinedType(this);
        }
    }

    public class BoundPrefixUnaryExpression : BoundExpression {
        public BoundPrefixUnaryExpression(PrefixUnaryOperator operator_, BoundExpression operand, Span span)
            : base(span) {
            if (operand == null) {
                throw new ArgumentNullException("operand");
            }

            Operator = operator_;
            Operand = operand;
        }

        public PrefixUnaryOperator Operator { get; private set; }

        public BoundExpression Operand { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.PrefixUnaryExpression; }
        }

        public BoundPrefixUnaryExpression Update(PrefixUnaryOperator operator_, BoundExpression operand, Span span) {
            if (Operator != operator_ || Operand != operand || Span != span) {
                return new BoundPrefixUnaryExpression(operator_, operand, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPrefixUnaryExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitPrefixUnaryExpression(this);
        }
    }

    public class BoundPropertyDeclaration : BoundBasePropertyDeclaration {
        public BoundPropertyDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType type, BoundName explicitInterfaceSpecifier, BoundIdentifierName identifier, ImmutableArray<BoundAccessorDeclaration> accessors, Span span)
            : base(attributeLists, modifiers, type, explicitInterfaceSpecifier, accessors, span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public BoundIdentifierName Identifier { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.PropertyDeclaration; }
        }

        public BoundPropertyDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundType type, BoundName explicitInterfaceSpecifier, BoundIdentifierName identifier, ImmutableArray<BoundAccessorDeclaration> accessors, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Type != type || ExplicitInterfaceSpecifier != explicitInterfaceSpecifier || Identifier != identifier || Accessors != accessors || Span != span) {
                return new BoundPropertyDeclaration(attributeLists, modifiers, type, explicitInterfaceSpecifier, identifier, accessors, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPropertyDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitPropertyDeclaration(this);
        }
    }

    public class BoundQualifiedName : BoundName {
        public BoundQualifiedName(BoundName left, BoundSimpleName right, Span span)
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

        public BoundName Left { get; private set; }

        public BoundSimpleName Right { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.QualifiedName; }
        }

        public BoundQualifiedName Update(BoundName left, BoundSimpleName right, Span span) {
            if (Left != left || Right != right || Span != span) {
                return new BoundQualifiedName(left, right, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitQualifiedName(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitQualifiedName(this);
        }
    }

    public class BoundReturnStatement : BoundStatement {
        public BoundReturnStatement(BoundExpression expression, Span span)
            : base(span) {

            Expression = expression;
        }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ReturnStatement; }
        }

        public BoundReturnStatement Update(BoundExpression expression, Span span) {
            if (Expression != expression || Span != span) {
                return new BoundReturnStatement(expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitReturnStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitReturnStatement(this);
        }
    }

    public abstract class BoundSimpleName : BoundName {
        protected BoundSimpleName(string identifier, Span span)
            : base(span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
        }

        public string Identifier { get; private set; }
    }

    public class BoundSizeOfExpression : BoundExpression {
        public BoundSizeOfExpression(BoundType targetType, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
        }

        public BoundType TargetType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.SizeOfExpression; }
        }

        public BoundSizeOfExpression Update(BoundType targetType, Span span) {
            if (TargetType != targetType || Span != span) {
                return new BoundSizeOfExpression(targetType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitSizeOfExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitSizeOfExpression(this);
        }
    }

    public abstract class BoundStatement : BoundNode {
        protected BoundStatement(Span span)
            : base(span) {

        }
    }

    public class BoundSwitchSection : BoundNode {
        public BoundSwitchSection(CaseOrDefault type, ImmutableArray<BoundExpression> values, BoundBlock block, Span span)
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

        public ImmutableArray<BoundExpression> Values { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.SwitchSection; }
        }

        public BoundSwitchSection Update(CaseOrDefault type, ImmutableArray<BoundExpression> values, BoundBlock block, Span span) {
            if (Type != type || Values != values || Block != block || Span != span) {
                return new BoundSwitchSection(type, values, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitSwitchSection(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitSwitchSection(this);
        }
    }

    public class BoundSwitchStatement : BoundStatement {
        public BoundSwitchStatement(BoundExpression expression, ImmutableArray<BoundSwitchSection> sections, Span span)
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

        public BoundExpression Expression { get; private set; }

        public ImmutableArray<BoundSwitchSection> Sections { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.SwitchStatement; }
        }

        public BoundSwitchStatement Update(BoundExpression expression, ImmutableArray<BoundSwitchSection> sections, Span span) {
            if (Expression != expression || Sections != sections || Span != span) {
                return new BoundSwitchStatement(expression, sections, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitSwitchStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitSwitchStatement(this);
        }
    }

    public class BoundThrowStatement : BoundStatement {
        public BoundThrowStatement(BoundExpression expression, Span span)
            : base(span) {

            Expression = expression;
        }

        public BoundExpression Expression { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.ThrowStatement; }
        }

        public BoundThrowStatement Update(BoundExpression expression, Span span) {
            if (Expression != expression || Span != span) {
                return new BoundThrowStatement(expression, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitThrowStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitThrowStatement(this);
        }
    }

    public class BoundTrackedType : BoundType {
        public BoundTrackedType(BoundType elementType, Span span)
            : base(span) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }

            ElementType = elementType;
        }

        public BoundType ElementType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TrackedType; }
        }

        public BoundTrackedType Update(BoundType elementType, Span span) {
            if (ElementType != elementType || Span != span) {
                return new BoundTrackedType(elementType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTrackedType(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTrackedType(this);
        }
    }

    public class BoundTryStatement : BoundStatement {
        public BoundTryStatement(BoundBlock block, ImmutableArray<BoundCatchClause> catches, BoundFinallyClause finally_, Span span)
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

        public BoundBlock Block { get; private set; }

        public ImmutableArray<BoundCatchClause> Catches { get; private set; }

        public BoundFinallyClause Finally { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TryStatement; }
        }

        public BoundTryStatement Update(BoundBlock block, ImmutableArray<BoundCatchClause> catches, BoundFinallyClause finally_, Span span) {
            if (Block != block || Catches != catches || Finally != finally_ || Span != span) {
                return new BoundTryStatement(block, catches, finally_, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTryStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTryStatement(this);
        }
    }

    public abstract class BoundType : BoundExpression {
        protected BoundType(Span span)
            : base(span) {

        }
    }

    public class BoundTypeConstraint : BoundTypeParameterConstraint {
        public BoundTypeConstraint(BoundType constrainedType, Span span)
            : base(span) {
            if (constrainedType == null) {
                throw new ArgumentNullException("constrainedType");
            }

            ConstrainedType = constrainedType;
        }

        public BoundType ConstrainedType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TypeConstraint; }
        }

        public BoundTypeConstraint Update(BoundType constrainedType, Span span) {
            if (ConstrainedType != constrainedType || Span != span) {
                return new BoundTypeConstraint(constrainedType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeConstraint(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTypeConstraint(this);
        }
    }

    public class BoundTypeDeclaration : BoundBaseTypeDeclaration {
        public BoundTypeDeclaration(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, TypeDeclarationType type, ImmutableArray<BoundTypeParameter> typeParameters, ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses, ImmutableArray<BoundMemberDeclaration> members, ImmutableArray<BoundType> baseTypes, Span span)
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

        public ImmutableArray<BoundTypeParameter> TypeParameters { get; private set; }

        public ImmutableArray<BoundTypeParameterConstraintClause> ConstraintClauses { get; private set; }

        public ImmutableArray<BoundMemberDeclaration> Members { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TypeDeclaration; }
        }

        public BoundTypeDeclaration Update(ImmutableArray<BoundAttributeList> attributeLists, ImmutableArray<Modifier> modifiers, BoundIdentifierName identifier, TypeDeclarationType type, ImmutableArray<BoundTypeParameter> typeParameters, ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses, ImmutableArray<BoundMemberDeclaration> members, ImmutableArray<BoundType> baseTypes, Span span) {
            if (AttributeLists != attributeLists || Modifiers != modifiers || Identifier != identifier || Type != type || TypeParameters != typeParameters || ConstraintClauses != constraintClauses || Members != members || BaseTypes != baseTypes || Span != span) {
                return new BoundTypeDeclaration(attributeLists, modifiers, identifier, type, typeParameters, constraintClauses, members, baseTypes, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTypeDeclaration(this);
        }
    }

    public class BoundTypeFamilyConstraint : BoundTypeParameterConstraint {
        public BoundTypeFamilyConstraint(TypeFamily family, Nullability? nullability, Span span)
            : base(span) {

            if (family != TypeFamily.Tracked && nullability == null) {
                throw new ArgumentException("Nullability is mandatory when family is not tracked");
            }
        

            Family = family;
            Nullability = nullability;
        }

        public TypeFamily Family { get; private set; }

        public Nullability? Nullability { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TypeFamilyConstraint; }
        }

        public BoundTypeFamilyConstraint Update(TypeFamily family, Nullability? nullability, Span span) {
            if (Family != family || Nullability != nullability || Span != span) {
                return new BoundTypeFamilyConstraint(family, nullability, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeFamilyConstraint(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTypeFamilyConstraint(this);
        }
    }

    public class BoundTypeOfExpression : BoundExpression {
        public BoundTypeOfExpression(BoundType targetType, Span span)
            : base(span) {
            if (targetType == null) {
                throw new ArgumentNullException("targetType");
            }

            TargetType = targetType;
        }

        public BoundType TargetType { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TypeOfExpression; }
        }

        public BoundTypeOfExpression Update(BoundType targetType, Span span) {
            if (TargetType != targetType || Span != span) {
                return new BoundTypeOfExpression(targetType, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeOfExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTypeOfExpression(this);
        }
    }

    public class BoundTypeParameter : BoundNode {
        public BoundTypeParameter(ImmutableArray<BoundAttributeList> attributeLists, Variance variance, BoundIdentifierName identifier, Span span)
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

        public ImmutableArray<BoundAttributeList> AttributeLists { get; private set; }

        public Variance Variance { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TypeParameter; }
        }

        public BoundTypeParameter Update(ImmutableArray<BoundAttributeList> attributeLists, Variance variance, BoundIdentifierName identifier, Span span) {
            if (AttributeLists != attributeLists || Variance != variance || Identifier != identifier || Span != span) {
                return new BoundTypeParameter(attributeLists, variance, identifier, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeParameter(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTypeParameter(this);
        }
    }

    public abstract class BoundTypeParameterConstraint : BoundNode {
        protected BoundTypeParameterConstraint(Span span)
            : base(span) {

        }
    }

    public class BoundTypeParameterConstraintClause : BoundNode {
        public BoundTypeParameterConstraintClause(BoundIdentifierName name, ImmutableArray<BoundTypeParameterConstraint> constraints, Span span)
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

        public BoundIdentifierName Name { get; private set; }

        public ImmutableArray<BoundTypeParameterConstraint> Constraints { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.TypeParameterConstraintClause; }
        }

        public BoundTypeParameterConstraintClause Update(BoundIdentifierName name, ImmutableArray<BoundTypeParameterConstraint> constraints, Span span) {
            if (Name != name || Constraints != constraints || Span != span) {
                return new BoundTypeParameterConstraintClause(name, constraints, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeParameterConstraintClause(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitTypeParameterConstraintClause(this);
        }
    }

    public class BoundUsingStatement : BoundStatement {
        public BoundUsingStatement(BoundVariableDeclaration declaration, BoundExpression expression, BoundBlock block, Span span)
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

        public BoundVariableDeclaration Declaration { get; private set; }

        public BoundExpression Expression { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.UsingStatement; }
        }

        public BoundUsingStatement Update(BoundVariableDeclaration declaration, BoundExpression expression, BoundBlock block, Span span) {
            if (Declaration != declaration || Expression != expression || Block != block || Span != span) {
                return new BoundUsingStatement(declaration, expression, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitUsingStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitUsingStatement(this);
        }
    }

    public class BoundVariableDeclaration : BoundNode {
        public BoundVariableDeclaration(BoundType variableType, ImmutableArray<BoundVariableDeclarator> variables, Span span)
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

        public BoundType VariableType { get; private set; }

        public ImmutableArray<BoundVariableDeclarator> Variables { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.VariableDeclaration; }
        }

        public BoundVariableDeclaration Update(BoundType variableType, ImmutableArray<BoundVariableDeclarator> variables, Span span) {
            if (VariableType != variableType || Variables != variables || Span != span) {
                return new BoundVariableDeclaration(variableType, variables, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVariableDeclaration(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitVariableDeclaration(this);
        }
    }

    public class BoundVariableDeclarationExpression : BoundExpression {
        public BoundVariableDeclarationExpression(BoundType variableType, BoundIdentifierName identifier, Span span)
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

        public BoundType VariableType { get; private set; }

        public BoundIdentifierName Identifier { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.VariableDeclarationExpression; }
        }

        public BoundVariableDeclarationExpression Update(BoundType variableType, BoundIdentifierName identifier, Span span) {
            if (VariableType != variableType || Identifier != identifier || Span != span) {
                return new BoundVariableDeclarationExpression(variableType, identifier, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVariableDeclarationExpression(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitVariableDeclarationExpression(this);
        }
    }

    public class BoundVariableDeclarator : BoundNode {
        public BoundVariableDeclarator(BoundIdentifierName identifier, BoundExpression value, Span span)
            : base(span) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
            Value = value;
        }

        public BoundIdentifierName Identifier { get; private set; }

        public BoundExpression Value { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.VariableDeclarator; }
        }

        public BoundVariableDeclarator Update(BoundIdentifierName identifier, BoundExpression value, Span span) {
            if (Identifier != identifier || Value != value || Span != span) {
                return new BoundVariableDeclarator(identifier, value, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVariableDeclarator(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitVariableDeclarator(this);
        }
    }

    public class BoundVarType : BoundType {
        public BoundVarType(Span span)
            : base(span) {

        }

        public override BoundKind Kind {
            get { return BoundKind.VarType; }
        }

        public BoundVarType Update(Span span) {
            if (Span != span) {
                return new BoundVarType(span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVarType(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
            return visitor.VisitVarType(this);
        }
    }

    public class BoundWhileStatement : BoundStatement {
        public BoundWhileStatement(BoundExpression condition, BoundBlock block, Span span)
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

        public BoundExpression Condition { get; private set; }

        public BoundBlock Block { get; private set; }

        public override BoundKind Kind {
            get { return BoundKind.WhileStatement; }
        }

        public BoundWhileStatement Update(BoundExpression condition, BoundBlock block, Span span) {
            if (Condition != condition || Block != block || Span != span) {
                return new BoundWhileStatement(condition, block, span);
            }

            return this;
        }

        public override void Accept(IBoundVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitWhileStatement(this);
            }
        }

        public override T Accept<T>(IBoundVisitor<T> visitor) {
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

    public interface IBoundVisitor {
        bool Done { get; }

        void VisitAccessorDeclaration(BoundAccessorDeclaration node);

        void VisitAliasQualifiedName(BoundAliasQualifiedName node);

        void VisitAnonymousObjectCreationExpression(BoundAnonymousObjectCreationExpression node);

        void VisitAnonymousObjectMemberDeclarator(BoundAnonymousObjectMemberDeclarator node);

        void VisitArgument(BoundArgument node);

        void VisitArrayCreationExpression(BoundArrayCreationExpression node);

        void VisitArrayRankSpecifier(BoundArrayRankSpecifier node);

        void VisitArrayType(BoundArrayType node);

        void VisitAssertStatement(BoundAssertStatement node);

        void VisitAttribute(BoundAttribute node);

        void VisitAttributeArgument(BoundAttributeArgument node);

        void VisitAttributeList(BoundAttributeList node);

        void VisitAwaitExpression(BoundAwaitExpression node);

        void VisitBinaryExpression(BoundBinaryExpression node);

        void VisitBlock(BoundBlock node);

        void VisitBreakStatement(BoundBreakStatement node);

        void VisitCastExpression(BoundCastExpression node);

        void VisitCatchClause(BoundCatchClause node);

        void VisitCompilationUnit(BoundCompilationUnit node);

        void VisitConditionalExpression(BoundConditionalExpression node);

        void VisitConstructorConstraint(BoundConstructorConstraint node);

        void VisitConstructorDeclaration(BoundConstructorDeclaration node);

        void VisitConstructorInitializer(BoundConstructorInitializer node);

        void VisitContinueStatement(BoundContinueStatement node);

        void VisitConversionOperatorDeclaration(BoundConversionOperatorDeclaration node);

        void VisitDefaultExpression(BoundDefaultExpression node);

        void VisitDelegateDeclaration(BoundDelegateDeclaration node);

        void VisitDeleteStatement(BoundDeleteStatement node);

        void VisitDestructorDeclaration(BoundDestructorDeclaration node);

        void VisitDoStatement(BoundDoStatement node);

        void VisitElementAccessExpression(BoundElementAccessExpression node);

        void VisitElseClause(BoundElseClause node);

        void VisitEmptyStatement(BoundEmptyStatement node);

        void VisitEnumDeclaration(BoundEnumDeclaration node);

        void VisitEnumMemberDeclaration(BoundEnumMemberDeclaration node);

        void VisitEventDeclaration(BoundEventDeclaration node);

        void VisitEventFieldDeclaration(BoundEventFieldDeclaration node);

        void VisitExpressionStatement(BoundExpressionStatement node);

        void VisitFieldDeclaration(BoundFieldDeclaration node);

        void VisitFinallyClause(BoundFinallyClause node);

        void VisitForEachStatement(BoundForEachStatement node);

        void VisitForStatement(BoundForStatement node);

        void VisitGenericName(BoundGenericName node);

        void VisitIdentifierName(BoundIdentifierName node);

        void VisitIfStatement(BoundIfStatement node);

        void VisitImplicitArrayCreationExpression(BoundImplicitArrayCreationExpression node);

        void VisitImportDirective(BoundImportDirective node);

        void VisitIndexerDeclaration(BoundIndexerDeclaration node);

        void VisitInitializerExpression(BoundInitializerExpression node);

        void VisitInstanceExpression(BoundInstanceExpression node);

        void VisitInvocationExpression(BoundInvocationExpression node);

        void VisitLambdaExpression(BoundLambdaExpression node);

        void VisitLiteralExpression(BoundLiteralExpression node);

        void VisitLocalDeclarationStatement(BoundLocalDeclarationStatement node);

        void VisitLoopStatement(BoundLoopStatement node);

        void VisitMemberAccessExpression(BoundMemberAccessExpression node);

        void VisitMethodDeclaration(BoundMethodDeclaration node);

        void VisitNakedNullableType(BoundNakedNullableType node);

        void VisitNamespaceDeclaration(BoundNamespaceDeclaration node);

        void VisitNullableType(BoundNullableType node);

        void VisitObjectCreationExpression(BoundObjectCreationExpression node);

        void VisitOmittedArraySizeExpression(BoundOmittedArraySizeExpression node);

        void VisitOmittedTypeArgument(BoundOmittedTypeArgument node);

        void VisitOperatorDeclaration(BoundOperatorDeclaration node);

        void VisitParameter(BoundParameter node);

        void VisitParenthesizedExpression(BoundParenthesizedExpression node);

        void VisitPostfixUnaryExpression(BoundPostfixUnaryExpression node);

        void VisitPredefinedType(BoundPredefinedType node);

        void VisitPrefixUnaryExpression(BoundPrefixUnaryExpression node);

        void VisitPropertyDeclaration(BoundPropertyDeclaration node);

        void VisitQualifiedName(BoundQualifiedName node);

        void VisitReturnStatement(BoundReturnStatement node);

        void VisitSizeOfExpression(BoundSizeOfExpression node);

        void VisitSwitchSection(BoundSwitchSection node);

        void VisitSwitchStatement(BoundSwitchStatement node);

        void VisitThrowStatement(BoundThrowStatement node);

        void VisitTrackedType(BoundTrackedType node);

        void VisitTryStatement(BoundTryStatement node);

        void VisitTypeConstraint(BoundTypeConstraint node);

        void VisitTypeDeclaration(BoundTypeDeclaration node);

        void VisitTypeFamilyConstraint(BoundTypeFamilyConstraint node);

        void VisitTypeOfExpression(BoundTypeOfExpression node);

        void VisitTypeParameter(BoundTypeParameter node);

        void VisitTypeParameterConstraintClause(BoundTypeParameterConstraintClause node);

        void VisitUsingStatement(BoundUsingStatement node);

        void VisitVariableDeclaration(BoundVariableDeclaration node);

        void VisitVariableDeclarationExpression(BoundVariableDeclarationExpression node);

        void VisitVariableDeclarator(BoundVariableDeclarator node);

        void VisitVarType(BoundVarType node);

        void VisitWhileStatement(BoundWhileStatement node);
    }

    public interface IBoundVisitor<T> {
        T VisitAccessorDeclaration(BoundAccessorDeclaration node);

        T VisitAliasQualifiedName(BoundAliasQualifiedName node);

        T VisitAnonymousObjectCreationExpression(BoundAnonymousObjectCreationExpression node);

        T VisitAnonymousObjectMemberDeclarator(BoundAnonymousObjectMemberDeclarator node);

        T VisitArgument(BoundArgument node);

        T VisitArrayCreationExpression(BoundArrayCreationExpression node);

        T VisitArrayRankSpecifier(BoundArrayRankSpecifier node);

        T VisitArrayType(BoundArrayType node);

        T VisitAssertStatement(BoundAssertStatement node);

        T VisitAttribute(BoundAttribute node);

        T VisitAttributeArgument(BoundAttributeArgument node);

        T VisitAttributeList(BoundAttributeList node);

        T VisitAwaitExpression(BoundAwaitExpression node);

        T VisitBinaryExpression(BoundBinaryExpression node);

        T VisitBlock(BoundBlock node);

        T VisitBreakStatement(BoundBreakStatement node);

        T VisitCastExpression(BoundCastExpression node);

        T VisitCatchClause(BoundCatchClause node);

        T VisitCompilationUnit(BoundCompilationUnit node);

        T VisitConditionalExpression(BoundConditionalExpression node);

        T VisitConstructorConstraint(BoundConstructorConstraint node);

        T VisitConstructorDeclaration(BoundConstructorDeclaration node);

        T VisitConstructorInitializer(BoundConstructorInitializer node);

        T VisitContinueStatement(BoundContinueStatement node);

        T VisitConversionOperatorDeclaration(BoundConversionOperatorDeclaration node);

        T VisitDefaultExpression(BoundDefaultExpression node);

        T VisitDelegateDeclaration(BoundDelegateDeclaration node);

        T VisitDeleteStatement(BoundDeleteStatement node);

        T VisitDestructorDeclaration(BoundDestructorDeclaration node);

        T VisitDoStatement(BoundDoStatement node);

        T VisitElementAccessExpression(BoundElementAccessExpression node);

        T VisitElseClause(BoundElseClause node);

        T VisitEmptyStatement(BoundEmptyStatement node);

        T VisitEnumDeclaration(BoundEnumDeclaration node);

        T VisitEnumMemberDeclaration(BoundEnumMemberDeclaration node);

        T VisitEventDeclaration(BoundEventDeclaration node);

        T VisitEventFieldDeclaration(BoundEventFieldDeclaration node);

        T VisitExpressionStatement(BoundExpressionStatement node);

        T VisitFieldDeclaration(BoundFieldDeclaration node);

        T VisitFinallyClause(BoundFinallyClause node);

        T VisitForEachStatement(BoundForEachStatement node);

        T VisitForStatement(BoundForStatement node);

        T VisitGenericName(BoundGenericName node);

        T VisitIdentifierName(BoundIdentifierName node);

        T VisitIfStatement(BoundIfStatement node);

        T VisitImplicitArrayCreationExpression(BoundImplicitArrayCreationExpression node);

        T VisitImportDirective(BoundImportDirective node);

        T VisitIndexerDeclaration(BoundIndexerDeclaration node);

        T VisitInitializerExpression(BoundInitializerExpression node);

        T VisitInstanceExpression(BoundInstanceExpression node);

        T VisitInvocationExpression(BoundInvocationExpression node);

        T VisitLambdaExpression(BoundLambdaExpression node);

        T VisitLiteralExpression(BoundLiteralExpression node);

        T VisitLocalDeclarationStatement(BoundLocalDeclarationStatement node);

        T VisitLoopStatement(BoundLoopStatement node);

        T VisitMemberAccessExpression(BoundMemberAccessExpression node);

        T VisitMethodDeclaration(BoundMethodDeclaration node);

        T VisitNakedNullableType(BoundNakedNullableType node);

        T VisitNamespaceDeclaration(BoundNamespaceDeclaration node);

        T VisitNullableType(BoundNullableType node);

        T VisitObjectCreationExpression(BoundObjectCreationExpression node);

        T VisitOmittedArraySizeExpression(BoundOmittedArraySizeExpression node);

        T VisitOmittedTypeArgument(BoundOmittedTypeArgument node);

        T VisitOperatorDeclaration(BoundOperatorDeclaration node);

        T VisitParameter(BoundParameter node);

        T VisitParenthesizedExpression(BoundParenthesizedExpression node);

        T VisitPostfixUnaryExpression(BoundPostfixUnaryExpression node);

        T VisitPredefinedType(BoundPredefinedType node);

        T VisitPrefixUnaryExpression(BoundPrefixUnaryExpression node);

        T VisitPropertyDeclaration(BoundPropertyDeclaration node);

        T VisitQualifiedName(BoundQualifiedName node);

        T VisitReturnStatement(BoundReturnStatement node);

        T VisitSizeOfExpression(BoundSizeOfExpression node);

        T VisitSwitchSection(BoundSwitchSection node);

        T VisitSwitchStatement(BoundSwitchStatement node);

        T VisitThrowStatement(BoundThrowStatement node);

        T VisitTrackedType(BoundTrackedType node);

        T VisitTryStatement(BoundTryStatement node);

        T VisitTypeConstraint(BoundTypeConstraint node);

        T VisitTypeDeclaration(BoundTypeDeclaration node);

        T VisitTypeFamilyConstraint(BoundTypeFamilyConstraint node);

        T VisitTypeOfExpression(BoundTypeOfExpression node);

        T VisitTypeParameter(BoundTypeParameter node);

        T VisitTypeParameterConstraintClause(BoundTypeParameterConstraintClause node);

        T VisitUsingStatement(BoundUsingStatement node);

        T VisitVariableDeclaration(BoundVariableDeclaration node);

        T VisitVariableDeclarationExpression(BoundVariableDeclarationExpression node);

        T VisitVariableDeclarator(BoundVariableDeclarator node);

        T VisitVarType(BoundVarType node);

        T VisitWhileStatement(BoundWhileStatement node);
    }

    public class AbstractBoundVisitor : IBoundVisitor {
        public bool Done { get; set; }

        public void DefaultVisit(BoundNode node) {
        }

        public void VisitAccessorDeclaration(BoundAccessorDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitAliasQualifiedName(BoundAliasQualifiedName node) {
            DefaultVisit(node);
        }

        public void VisitAnonymousObjectCreationExpression(BoundAnonymousObjectCreationExpression node) {
            DefaultVisit(node);
        }

        public void VisitAnonymousObjectMemberDeclarator(BoundAnonymousObjectMemberDeclarator node) {
            DefaultVisit(node);
        }

        public void VisitArgument(BoundArgument node) {
            DefaultVisit(node);
        }

        public void VisitArrayCreationExpression(BoundArrayCreationExpression node) {
            DefaultVisit(node);
        }

        public void VisitArrayRankSpecifier(BoundArrayRankSpecifier node) {
            DefaultVisit(node);
        }

        public void VisitArrayType(BoundArrayType node) {
            DefaultVisit(node);
        }

        public void VisitAssertStatement(BoundAssertStatement node) {
            DefaultVisit(node);
        }

        public void VisitAttribute(BoundAttribute node) {
            DefaultVisit(node);
        }

        public void VisitAttributeArgument(BoundAttributeArgument node) {
            DefaultVisit(node);
        }

        public void VisitAttributeList(BoundAttributeList node) {
            DefaultVisit(node);
        }

        public void VisitAwaitExpression(BoundAwaitExpression node) {
            DefaultVisit(node);
        }

        public void VisitBinaryExpression(BoundBinaryExpression node) {
            DefaultVisit(node);
        }

        public void VisitBlock(BoundBlock node) {
            DefaultVisit(node);
        }

        public void VisitBreakStatement(BoundBreakStatement node) {
            DefaultVisit(node);
        }

        public void VisitCastExpression(BoundCastExpression node) {
            DefaultVisit(node);
        }

        public void VisitCatchClause(BoundCatchClause node) {
            DefaultVisit(node);
        }

        public void VisitCompilationUnit(BoundCompilationUnit node) {
            DefaultVisit(node);
        }

        public void VisitConditionalExpression(BoundConditionalExpression node) {
            DefaultVisit(node);
        }

        public void VisitConstructorConstraint(BoundConstructorConstraint node) {
            DefaultVisit(node);
        }

        public void VisitConstructorDeclaration(BoundConstructorDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitConstructorInitializer(BoundConstructorInitializer node) {
            DefaultVisit(node);
        }

        public void VisitContinueStatement(BoundContinueStatement node) {
            DefaultVisit(node);
        }

        public void VisitConversionOperatorDeclaration(BoundConversionOperatorDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitDefaultExpression(BoundDefaultExpression node) {
            DefaultVisit(node);
        }

        public void VisitDelegateDeclaration(BoundDelegateDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitDeleteStatement(BoundDeleteStatement node) {
            DefaultVisit(node);
        }

        public void VisitDestructorDeclaration(BoundDestructorDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitDoStatement(BoundDoStatement node) {
            DefaultVisit(node);
        }

        public void VisitElementAccessExpression(BoundElementAccessExpression node) {
            DefaultVisit(node);
        }

        public void VisitElseClause(BoundElseClause node) {
            DefaultVisit(node);
        }

        public void VisitEmptyStatement(BoundEmptyStatement node) {
            DefaultVisit(node);
        }

        public void VisitEnumDeclaration(BoundEnumDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitEnumMemberDeclaration(BoundEnumMemberDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitEventDeclaration(BoundEventDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitEventFieldDeclaration(BoundEventFieldDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitExpressionStatement(BoundExpressionStatement node) {
            DefaultVisit(node);
        }

        public void VisitFieldDeclaration(BoundFieldDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitFinallyClause(BoundFinallyClause node) {
            DefaultVisit(node);
        }

        public void VisitForEachStatement(BoundForEachStatement node) {
            DefaultVisit(node);
        }

        public void VisitForStatement(BoundForStatement node) {
            DefaultVisit(node);
        }

        public void VisitGenericName(BoundGenericName node) {
            DefaultVisit(node);
        }

        public void VisitIdentifierName(BoundIdentifierName node) {
            DefaultVisit(node);
        }

        public void VisitIfStatement(BoundIfStatement node) {
            DefaultVisit(node);
        }

        public void VisitImplicitArrayCreationExpression(BoundImplicitArrayCreationExpression node) {
            DefaultVisit(node);
        }

        public void VisitImportDirective(BoundImportDirective node) {
            DefaultVisit(node);
        }

        public void VisitIndexerDeclaration(BoundIndexerDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitInitializerExpression(BoundInitializerExpression node) {
            DefaultVisit(node);
        }

        public void VisitInstanceExpression(BoundInstanceExpression node) {
            DefaultVisit(node);
        }

        public void VisitInvocationExpression(BoundInvocationExpression node) {
            DefaultVisit(node);
        }

        public void VisitLambdaExpression(BoundLambdaExpression node) {
            DefaultVisit(node);
        }

        public void VisitLiteralExpression(BoundLiteralExpression node) {
            DefaultVisit(node);
        }

        public void VisitLocalDeclarationStatement(BoundLocalDeclarationStatement node) {
            DefaultVisit(node);
        }

        public void VisitLoopStatement(BoundLoopStatement node) {
            DefaultVisit(node);
        }

        public void VisitMemberAccessExpression(BoundMemberAccessExpression node) {
            DefaultVisit(node);
        }

        public void VisitMethodDeclaration(BoundMethodDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitNakedNullableType(BoundNakedNullableType node) {
            DefaultVisit(node);
        }

        public void VisitNamespaceDeclaration(BoundNamespaceDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitNullableType(BoundNullableType node) {
            DefaultVisit(node);
        }

        public void VisitObjectCreationExpression(BoundObjectCreationExpression node) {
            DefaultVisit(node);
        }

        public void VisitOmittedArraySizeExpression(BoundOmittedArraySizeExpression node) {
            DefaultVisit(node);
        }

        public void VisitOmittedTypeArgument(BoundOmittedTypeArgument node) {
            DefaultVisit(node);
        }

        public void VisitOperatorDeclaration(BoundOperatorDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitParameter(BoundParameter node) {
            DefaultVisit(node);
        }

        public void VisitParenthesizedExpression(BoundParenthesizedExpression node) {
            DefaultVisit(node);
        }

        public void VisitPostfixUnaryExpression(BoundPostfixUnaryExpression node) {
            DefaultVisit(node);
        }

        public void VisitPredefinedType(BoundPredefinedType node) {
            DefaultVisit(node);
        }

        public void VisitPrefixUnaryExpression(BoundPrefixUnaryExpression node) {
            DefaultVisit(node);
        }

        public void VisitPropertyDeclaration(BoundPropertyDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitQualifiedName(BoundQualifiedName node) {
            DefaultVisit(node);
        }

        public void VisitReturnStatement(BoundReturnStatement node) {
            DefaultVisit(node);
        }

        public void VisitSizeOfExpression(BoundSizeOfExpression node) {
            DefaultVisit(node);
        }

        public void VisitSwitchSection(BoundSwitchSection node) {
            DefaultVisit(node);
        }

        public void VisitSwitchStatement(BoundSwitchStatement node) {
            DefaultVisit(node);
        }

        public void VisitThrowStatement(BoundThrowStatement node) {
            DefaultVisit(node);
        }

        public void VisitTrackedType(BoundTrackedType node) {
            DefaultVisit(node);
        }

        public void VisitTryStatement(BoundTryStatement node) {
            DefaultVisit(node);
        }

        public void VisitTypeConstraint(BoundTypeConstraint node) {
            DefaultVisit(node);
        }

        public void VisitTypeDeclaration(BoundTypeDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitTypeFamilyConstraint(BoundTypeFamilyConstraint node) {
            DefaultVisit(node);
        }

        public void VisitTypeOfExpression(BoundTypeOfExpression node) {
            DefaultVisit(node);
        }

        public void VisitTypeParameter(BoundTypeParameter node) {
            DefaultVisit(node);
        }

        public void VisitTypeParameterConstraintClause(BoundTypeParameterConstraintClause node) {
            DefaultVisit(node);
        }

        public void VisitUsingStatement(BoundUsingStatement node) {
            DefaultVisit(node);
        }

        public void VisitVariableDeclaration(BoundVariableDeclaration node) {
            DefaultVisit(node);
        }

        public void VisitVariableDeclarationExpression(BoundVariableDeclarationExpression node) {
            DefaultVisit(node);
        }

        public void VisitVariableDeclarator(BoundVariableDeclarator node) {
            DefaultVisit(node);
        }

        public void VisitVarType(BoundVarType node) {
            DefaultVisit(node);
        }

        public void VisitWhileStatement(BoundWhileStatement node) {
            DefaultVisit(node);
        }
    }

    public class AbstractBoundVisitor<T> : IBoundVisitor<T> {
        public T DefaultVisit(BoundNode node) {
            return default(T);
        }
        public T VisitAccessorDeclaration(BoundAccessorDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitAliasQualifiedName(BoundAliasQualifiedName node) {
            return DefaultVisit(node);
        }

        public T VisitAnonymousObjectCreationExpression(BoundAnonymousObjectCreationExpression node) {
            return DefaultVisit(node);
        }

        public T VisitAnonymousObjectMemberDeclarator(BoundAnonymousObjectMemberDeclarator node) {
            return DefaultVisit(node);
        }

        public T VisitArgument(BoundArgument node) {
            return DefaultVisit(node);
        }

        public T VisitArrayCreationExpression(BoundArrayCreationExpression node) {
            return DefaultVisit(node);
        }

        public T VisitArrayRankSpecifier(BoundArrayRankSpecifier node) {
            return DefaultVisit(node);
        }

        public T VisitArrayType(BoundArrayType node) {
            return DefaultVisit(node);
        }

        public T VisitAssertStatement(BoundAssertStatement node) {
            return DefaultVisit(node);
        }

        public T VisitAttribute(BoundAttribute node) {
            return DefaultVisit(node);
        }

        public T VisitAttributeArgument(BoundAttributeArgument node) {
            return DefaultVisit(node);
        }

        public T VisitAttributeList(BoundAttributeList node) {
            return DefaultVisit(node);
        }

        public T VisitAwaitExpression(BoundAwaitExpression node) {
            return DefaultVisit(node);
        }

        public T VisitBinaryExpression(BoundBinaryExpression node) {
            return DefaultVisit(node);
        }

        public T VisitBlock(BoundBlock node) {
            return DefaultVisit(node);
        }

        public T VisitBreakStatement(BoundBreakStatement node) {
            return DefaultVisit(node);
        }

        public T VisitCastExpression(BoundCastExpression node) {
            return DefaultVisit(node);
        }

        public T VisitCatchClause(BoundCatchClause node) {
            return DefaultVisit(node);
        }

        public T VisitCompilationUnit(BoundCompilationUnit node) {
            return DefaultVisit(node);
        }

        public T VisitConditionalExpression(BoundConditionalExpression node) {
            return DefaultVisit(node);
        }

        public T VisitConstructorConstraint(BoundConstructorConstraint node) {
            return DefaultVisit(node);
        }

        public T VisitConstructorDeclaration(BoundConstructorDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitConstructorInitializer(BoundConstructorInitializer node) {
            return DefaultVisit(node);
        }

        public T VisitContinueStatement(BoundContinueStatement node) {
            return DefaultVisit(node);
        }

        public T VisitConversionOperatorDeclaration(BoundConversionOperatorDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitDefaultExpression(BoundDefaultExpression node) {
            return DefaultVisit(node);
        }

        public T VisitDelegateDeclaration(BoundDelegateDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitDeleteStatement(BoundDeleteStatement node) {
            return DefaultVisit(node);
        }

        public T VisitDestructorDeclaration(BoundDestructorDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitDoStatement(BoundDoStatement node) {
            return DefaultVisit(node);
        }

        public T VisitElementAccessExpression(BoundElementAccessExpression node) {
            return DefaultVisit(node);
        }

        public T VisitElseClause(BoundElseClause node) {
            return DefaultVisit(node);
        }

        public T VisitEmptyStatement(BoundEmptyStatement node) {
            return DefaultVisit(node);
        }

        public T VisitEnumDeclaration(BoundEnumDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitEnumMemberDeclaration(BoundEnumMemberDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitEventDeclaration(BoundEventDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitEventFieldDeclaration(BoundEventFieldDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitExpressionStatement(BoundExpressionStatement node) {
            return DefaultVisit(node);
        }

        public T VisitFieldDeclaration(BoundFieldDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitFinallyClause(BoundFinallyClause node) {
            return DefaultVisit(node);
        }

        public T VisitForEachStatement(BoundForEachStatement node) {
            return DefaultVisit(node);
        }

        public T VisitForStatement(BoundForStatement node) {
            return DefaultVisit(node);
        }

        public T VisitGenericName(BoundGenericName node) {
            return DefaultVisit(node);
        }

        public T VisitIdentifierName(BoundIdentifierName node) {
            return DefaultVisit(node);
        }

        public T VisitIfStatement(BoundIfStatement node) {
            return DefaultVisit(node);
        }

        public T VisitImplicitArrayCreationExpression(BoundImplicitArrayCreationExpression node) {
            return DefaultVisit(node);
        }

        public T VisitImportDirective(BoundImportDirective node) {
            return DefaultVisit(node);
        }

        public T VisitIndexerDeclaration(BoundIndexerDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitInitializerExpression(BoundInitializerExpression node) {
            return DefaultVisit(node);
        }

        public T VisitInstanceExpression(BoundInstanceExpression node) {
            return DefaultVisit(node);
        }

        public T VisitInvocationExpression(BoundInvocationExpression node) {
            return DefaultVisit(node);
        }

        public T VisitLambdaExpression(BoundLambdaExpression node) {
            return DefaultVisit(node);
        }

        public T VisitLiteralExpression(BoundLiteralExpression node) {
            return DefaultVisit(node);
        }

        public T VisitLocalDeclarationStatement(BoundLocalDeclarationStatement node) {
            return DefaultVisit(node);
        }

        public T VisitLoopStatement(BoundLoopStatement node) {
            return DefaultVisit(node);
        }

        public T VisitMemberAccessExpression(BoundMemberAccessExpression node) {
            return DefaultVisit(node);
        }

        public T VisitMethodDeclaration(BoundMethodDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitNakedNullableType(BoundNakedNullableType node) {
            return DefaultVisit(node);
        }

        public T VisitNamespaceDeclaration(BoundNamespaceDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitNullableType(BoundNullableType node) {
            return DefaultVisit(node);
        }

        public T VisitObjectCreationExpression(BoundObjectCreationExpression node) {
            return DefaultVisit(node);
        }

        public T VisitOmittedArraySizeExpression(BoundOmittedArraySizeExpression node) {
            return DefaultVisit(node);
        }

        public T VisitOmittedTypeArgument(BoundOmittedTypeArgument node) {
            return DefaultVisit(node);
        }

        public T VisitOperatorDeclaration(BoundOperatorDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitParameter(BoundParameter node) {
            return DefaultVisit(node);
        }

        public T VisitParenthesizedExpression(BoundParenthesizedExpression node) {
            return DefaultVisit(node);
        }

        public T VisitPostfixUnaryExpression(BoundPostfixUnaryExpression node) {
            return DefaultVisit(node);
        }

        public T VisitPredefinedType(BoundPredefinedType node) {
            return DefaultVisit(node);
        }

        public T VisitPrefixUnaryExpression(BoundPrefixUnaryExpression node) {
            return DefaultVisit(node);
        }

        public T VisitPropertyDeclaration(BoundPropertyDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitQualifiedName(BoundQualifiedName node) {
            return DefaultVisit(node);
        }

        public T VisitReturnStatement(BoundReturnStatement node) {
            return DefaultVisit(node);
        }

        public T VisitSizeOfExpression(BoundSizeOfExpression node) {
            return DefaultVisit(node);
        }

        public T VisitSwitchSection(BoundSwitchSection node) {
            return DefaultVisit(node);
        }

        public T VisitSwitchStatement(BoundSwitchStatement node) {
            return DefaultVisit(node);
        }

        public T VisitThrowStatement(BoundThrowStatement node) {
            return DefaultVisit(node);
        }

        public T VisitTrackedType(BoundTrackedType node) {
            return DefaultVisit(node);
        }

        public T VisitTryStatement(BoundTryStatement node) {
            return DefaultVisit(node);
        }

        public T VisitTypeConstraint(BoundTypeConstraint node) {
            return DefaultVisit(node);
        }

        public T VisitTypeDeclaration(BoundTypeDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitTypeFamilyConstraint(BoundTypeFamilyConstraint node) {
            return DefaultVisit(node);
        }

        public T VisitTypeOfExpression(BoundTypeOfExpression node) {
            return DefaultVisit(node);
        }

        public T VisitTypeParameter(BoundTypeParameter node) {
            return DefaultVisit(node);
        }

        public T VisitTypeParameterConstraintClause(BoundTypeParameterConstraintClause node) {
            return DefaultVisit(node);
        }

        public T VisitUsingStatement(BoundUsingStatement node) {
            return DefaultVisit(node);
        }

        public T VisitVariableDeclaration(BoundVariableDeclaration node) {
            return DefaultVisit(node);
        }

        public T VisitVariableDeclarationExpression(BoundVariableDeclarationExpression node) {
            return DefaultVisit(node);
        }

        public T VisitVariableDeclarator(BoundVariableDeclarator node) {
            return DefaultVisit(node);
        }

        public T VisitVarType(BoundVarType node) {
            return DefaultVisit(node);
        }

        public T VisitWhileStatement(BoundWhileStatement node) {
            return DefaultVisit(node);
        }
    }

    public class BoundTreeWalker : IBoundVisitor {
        public bool Done { get; set; }

        public virtual void VisitList<T>(ImmutableArray<T> list)
            where T : BoundNode {
            foreach (var node in list) {
                Visit(node);
            }
        }

        public virtual void Visit(BoundNode node) {
            if (node != null) {
                node.Accept(this);
            }
        }

        public virtual void VisitAccessorDeclaration(BoundAccessorDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Body);
        }

        public virtual void VisitAliasQualifiedName(BoundAliasQualifiedName node) {
            Visit(node.Alias);
            Visit(node.Name);
        }

        public virtual void VisitAnonymousObjectCreationExpression(BoundAnonymousObjectCreationExpression node) {
            VisitList(node.Initializers);
        }

        public virtual void VisitAnonymousObjectMemberDeclarator(BoundAnonymousObjectMemberDeclarator node) {
            Visit(node.Name);
            Visit(node.Expression);
        }

        public virtual void VisitArgument(BoundArgument node) {
            Visit(node.Expression);
        }

        public virtual void VisitArrayCreationExpression(BoundArrayCreationExpression node) {
            Visit(node.Type);
            Visit(node.Initializer);
        }

        public virtual void VisitArrayRankSpecifier(BoundArrayRankSpecifier node) {
            Visit(node.Size);
        }

        public virtual void VisitArrayType(BoundArrayType node) {
            Visit(node.ElementType);
            VisitList(node.RankSpecifiers);
        }

        public virtual void VisitAssertStatement(BoundAssertStatement node) {
            Visit(node.Expression);
        }

        public virtual void VisitAttribute(BoundAttribute node) {
            Visit(node.Name);
            VisitList(node.Arguments);
        }

        public virtual void VisitAttributeArgument(BoundAttributeArgument node) {
            Visit(node.Name);
            Visit(node.Expression);
        }

        public virtual void VisitAttributeList(BoundAttributeList node) {
            VisitList(node.Attributes);
        }

        public virtual void VisitAwaitExpression(BoundAwaitExpression node) {
            Visit(node.Expression);
        }

        public virtual void VisitBinaryExpression(BoundBinaryExpression node) {
            Visit(node.Left);
            Visit(node.Right);
        }

        public virtual void VisitBlock(BoundBlock node) {
            VisitList(node.Statements);
        }

        public virtual void VisitBreakStatement(BoundBreakStatement node) {
        }

        public virtual void VisitCastExpression(BoundCastExpression node) {
            Visit(node.Expression);
            Visit(node.TargetType);
        }

        public virtual void VisitCatchClause(BoundCatchClause node) {
            Visit(node.ExceptionType);
            Visit(node.Identifier);
            Visit(node.Block);
        }

        public virtual void VisitCompilationUnit(BoundCompilationUnit node) {
            VisitList(node.AttributeLists);
            VisitList(node.Imports);
            VisitList(node.Members);
        }

        public virtual void VisitConditionalExpression(BoundConditionalExpression node) {
            Visit(node.Condition);
            Visit(node.WhenTrue);
            Visit(node.WhenFalse);
        }

        public virtual void VisitConstructorConstraint(BoundConstructorConstraint node) {
        }

        public virtual void VisitConstructorDeclaration(BoundConstructorDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Identifier);
            Visit(node.Initializer);
            VisitList(node.Parameters);
            Visit(node.Body);
        }

        public virtual void VisitConstructorInitializer(BoundConstructorInitializer node) {
            VisitList(node.Arguments);
        }

        public virtual void VisitContinueStatement(BoundContinueStatement node) {
        }

        public virtual void VisitConversionOperatorDeclaration(BoundConversionOperatorDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.TargetType);
            VisitList(node.Parameters);
            Visit(node.Body);
        }

        public virtual void VisitDefaultExpression(BoundDefaultExpression node) {
            Visit(node.TargetType);
        }

        public virtual void VisitDelegateDeclaration(BoundDelegateDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.ReturnType);
            Visit(node.Identifier);
            VisitList(node.TypeParameters);
            VisitList(node.Parameters);
            VisitList(node.ConstraintClauses);
        }

        public virtual void VisitDeleteStatement(BoundDeleteStatement node) {
            Visit(node.Expression);
        }

        public virtual void VisitDestructorDeclaration(BoundDestructorDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Identifier);
            VisitList(node.Parameters);
            Visit(node.Body);
        }

        public virtual void VisitDoStatement(BoundDoStatement node) {
            Visit(node.Condition);
            Visit(node.Block);
        }

        public virtual void VisitElementAccessExpression(BoundElementAccessExpression node) {
            Visit(node.Expression);
            VisitList(node.IndexExpressions);
        }

        public virtual void VisitElseClause(BoundElseClause node) {
            Visit(node.Condition);
            Visit(node.Block);
        }

        public virtual void VisitEmptyStatement(BoundEmptyStatement node) {
        }

        public virtual void VisitEnumDeclaration(BoundEnumDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Identifier);
            VisitList(node.Members);
            VisitList(node.BaseTypes);
        }

        public virtual void VisitEnumMemberDeclaration(BoundEnumMemberDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Identifier);
            Visit(node.Value);
        }

        public virtual void VisitEventDeclaration(BoundEventDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Type);
            Visit(node.ExplicitInterfaceSpecifier);
            Visit(node.Identifier);
            VisitList(node.Accessors);
        }

        public virtual void VisitEventFieldDeclaration(BoundEventFieldDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Declaration);
        }

        public virtual void VisitExpressionStatement(BoundExpressionStatement node) {
            Visit(node.Expression);
        }

        public virtual void VisitFieldDeclaration(BoundFieldDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Declaration);
        }

        public virtual void VisitFinallyClause(BoundFinallyClause node) {
            Visit(node.Block);
        }

        public virtual void VisitForEachStatement(BoundForEachStatement node) {
            Visit(node.ElementType);
            Visit(node.Identifier);
            Visit(node.Expression);
            Visit(node.Block);
        }

        public virtual void VisitForStatement(BoundForStatement node) {
            Visit(node.Declaration);
            VisitList(node.Initializers);
            Visit(node.Condition);
            VisitList(node.Incrementors);
            Visit(node.Block);
        }

        public virtual void VisitGenericName(BoundGenericName node) {
            VisitList(node.TypeArguments);
        }

        public virtual void VisitIdentifierName(BoundIdentifierName node) {
        }

        public virtual void VisitIfStatement(BoundIfStatement node) {
            Visit(node.Condition);
            Visit(node.Block);
            VisitList(node.Elses);
        }

        public virtual void VisitImplicitArrayCreationExpression(BoundImplicitArrayCreationExpression node) {
            VisitList(node.RankSpecifiers);
            Visit(node.Initializer);
        }

        public virtual void VisitImportDirective(BoundImportDirective node) {
            Visit(node.Alias);
            Visit(node.Name);
        }

        public virtual void VisitIndexerDeclaration(BoundIndexerDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Type);
            Visit(node.ExplicitInterfaceSpecifier);
            VisitList(node.Parameters);
            VisitList(node.Accessors);
        }

        public virtual void VisitInitializerExpression(BoundInitializerExpression node) {
            VisitList(node.Expressions);
        }

        public virtual void VisitInstanceExpression(BoundInstanceExpression node) {
        }

        public virtual void VisitInvocationExpression(BoundInvocationExpression node) {
            Visit(node.Expression);
            VisitList(node.Arguments);
        }

        public virtual void VisitLambdaExpression(BoundLambdaExpression node) {
            VisitList(node.Parameters);
            Visit(node.Body);
        }

        public virtual void VisitLiteralExpression(BoundLiteralExpression node) {
        }

        public virtual void VisitLocalDeclarationStatement(BoundLocalDeclarationStatement node) {
            Visit(node.Declaration);
        }

        public virtual void VisitLoopStatement(BoundLoopStatement node) {
            Visit(node.Block);
        }

        public virtual void VisitMemberAccessExpression(BoundMemberAccessExpression node) {
            Visit(node.Expression);
            Visit(node.Name);
        }

        public virtual void VisitMethodDeclaration(BoundMethodDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.ReturnType);
            Visit(node.ExplicitInterfaceSpecifier);
            Visit(node.Identifier);
            VisitList(node.TypeParameters);
            VisitList(node.ConstraintClauses);
            VisitList(node.Parameters);
            Visit(node.Body);
        }

        public virtual void VisitNakedNullableType(BoundNakedNullableType node) {
        }

        public virtual void VisitNamespaceDeclaration(BoundNamespaceDeclaration node) {
            Visit(node.Name);
            VisitList(node.Imports);
            VisitList(node.Members);
        }

        public virtual void VisitNullableType(BoundNullableType node) {
            Visit(node.ElementType);
        }

        public virtual void VisitObjectCreationExpression(BoundObjectCreationExpression node) {
            Visit(node.TargetType);
            VisitList(node.Arguments);
            Visit(node.Initializer);
        }

        public virtual void VisitOmittedArraySizeExpression(BoundOmittedArraySizeExpression node) {
        }

        public virtual void VisitOmittedTypeArgument(BoundOmittedTypeArgument node) {
        }

        public virtual void VisitOperatorDeclaration(BoundOperatorDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.ReturnType);
            VisitList(node.Parameters);
            Visit(node.Body);
        }

        public virtual void VisitParameter(BoundParameter node) {
            VisitList(node.AttributeLists);
            Visit(node.ParameterType);
            Visit(node.Identifier);
        }

        public virtual void VisitParenthesizedExpression(BoundParenthesizedExpression node) {
            Visit(node.Expression);
        }

        public virtual void VisitPostfixUnaryExpression(BoundPostfixUnaryExpression node) {
            Visit(node.Operand);
        }

        public virtual void VisitPredefinedType(BoundPredefinedType node) {
        }

        public virtual void VisitPrefixUnaryExpression(BoundPrefixUnaryExpression node) {
            Visit(node.Operand);
        }

        public virtual void VisitPropertyDeclaration(BoundPropertyDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Type);
            Visit(node.ExplicitInterfaceSpecifier);
            Visit(node.Identifier);
            VisitList(node.Accessors);
        }

        public virtual void VisitQualifiedName(BoundQualifiedName node) {
            Visit(node.Left);
            Visit(node.Right);
        }

        public virtual void VisitReturnStatement(BoundReturnStatement node) {
            Visit(node.Expression);
        }

        public virtual void VisitSizeOfExpression(BoundSizeOfExpression node) {
            Visit(node.TargetType);
        }

        public virtual void VisitSwitchSection(BoundSwitchSection node) {
            VisitList(node.Values);
            Visit(node.Block);
        }

        public virtual void VisitSwitchStatement(BoundSwitchStatement node) {
            Visit(node.Expression);
            VisitList(node.Sections);
        }

        public virtual void VisitThrowStatement(BoundThrowStatement node) {
            Visit(node.Expression);
        }

        public virtual void VisitTrackedType(BoundTrackedType node) {
            Visit(node.ElementType);
        }

        public virtual void VisitTryStatement(BoundTryStatement node) {
            Visit(node.Block);
            VisitList(node.Catches);
            Visit(node.Finally);
        }

        public virtual void VisitTypeConstraint(BoundTypeConstraint node) {
            Visit(node.ConstrainedType);
        }

        public virtual void VisitTypeDeclaration(BoundTypeDeclaration node) {
            VisitList(node.AttributeLists);
            Visit(node.Identifier);
            VisitList(node.TypeParameters);
            VisitList(node.ConstraintClauses);
            VisitList(node.Members);
            VisitList(node.BaseTypes);
        }

        public virtual void VisitTypeFamilyConstraint(BoundTypeFamilyConstraint node) {
        }

        public virtual void VisitTypeOfExpression(BoundTypeOfExpression node) {
            Visit(node.TargetType);
        }

        public virtual void VisitTypeParameter(BoundTypeParameter node) {
            VisitList(node.AttributeLists);
            Visit(node.Identifier);
        }

        public virtual void VisitTypeParameterConstraintClause(BoundTypeParameterConstraintClause node) {
            Visit(node.Name);
            VisitList(node.Constraints);
        }

        public virtual void VisitUsingStatement(BoundUsingStatement node) {
            Visit(node.Declaration);
            Visit(node.Expression);
            Visit(node.Block);
        }

        public virtual void VisitVariableDeclaration(BoundVariableDeclaration node) {
            Visit(node.VariableType);
            VisitList(node.Variables);
        }

        public virtual void VisitVariableDeclarationExpression(BoundVariableDeclarationExpression node) {
            Visit(node.VariableType);
            Visit(node.Identifier);
        }

        public virtual void VisitVariableDeclarator(BoundVariableDeclarator node) {
            Visit(node.Identifier);
            Visit(node.Value);
        }

        public virtual void VisitVarType(BoundVarType node) {
        }

        public virtual void VisitWhileStatement(BoundWhileStatement node) {
            Visit(node.Condition);
            Visit(node.Block);
        }
    }

    public class BoundTreeRewriter : IBoundVisitor<BoundNode> {
        public ImmutableArray<T> VisitList<T>(ImmutableArray<T> nodes)
            where T : BoundNode {
            if (nodes == null) {
                throw new ArgumentNullException("nodes");
            }

            if (nodes.Count == 0) {
                return nodes;
            }

            ImmutableArray<T>.Builder result = null;

            for (int i = 0; i < nodes.Count; i++) {
                var item = nodes[i];
                var visited = (T)item.Accept(this);

                if (item != visited && result == null) {
                    result = new ImmutableArray<T>.Builder();
                    for (int j = 0; j < i; j++) {
                        result.Add(nodes[j]);
                    }
                }

                if (result != null && visited != null) {
                    result.Add(visited);
                }
            }

            if (result != null) {
                return result.Build();
            }

            return nodes;
        }

        public T Visit<T>(T node)
            where T : BoundNode {
            if (node != null) {
                return (T)node.Accept(this);
            }

            return null;
        }

        public BoundNode VisitAccessorDeclaration(BoundAccessorDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundBlock body = Visit(node.Body);
            return node.Update(attributeLists, node.Modifiers, node.Type, body, node.Span);
        }

        public BoundNode VisitAliasQualifiedName(BoundAliasQualifiedName node) {
            BoundIdentifierName alias = Visit(node.Alias);
            BoundSimpleName name = Visit(node.Name);
            return node.Update(alias, name, node.Span);
        }

        public BoundNode VisitAnonymousObjectCreationExpression(BoundAnonymousObjectCreationExpression node) {
            ImmutableArray<BoundAnonymousObjectMemberDeclarator> initializers = VisitList(node.Initializers);
            return node.Update(initializers, node.Span);
        }

        public BoundNode VisitAnonymousObjectMemberDeclarator(BoundAnonymousObjectMemberDeclarator node) {
            BoundIdentifierName name = Visit(node.Name);
            BoundExpression expression = Visit(node.Expression);
            return node.Update(name, expression, node.Span);
        }

        public BoundNode VisitArgument(BoundArgument node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(node.Modifiers, expression, node.Span);
        }

        public BoundNode VisitArrayCreationExpression(BoundArrayCreationExpression node) {
            BoundArrayType type = Visit(node.Type);
            BoundInitializerExpression initializer = Visit(node.Initializer);
            return node.Update(type, initializer, node.Span);
        }

        public BoundNode VisitArrayRankSpecifier(BoundArrayRankSpecifier node) {
            BoundExpression size = Visit(node.Size);
            return node.Update(size, node.IsTracked, node.Span);
        }

        public BoundNode VisitArrayType(BoundArrayType node) {
            BoundType elementType = Visit(node.ElementType);
            ImmutableArray<BoundArrayRankSpecifier> rankSpecifiers = VisitList(node.RankSpecifiers);
            return node.Update(elementType, rankSpecifiers, node.Span);
        }

        public BoundNode VisitAssertStatement(BoundAssertStatement node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(expression, node.Span);
        }

        public BoundNode VisitAttribute(BoundAttribute node) {
            BoundName name = Visit(node.Name);
            ImmutableArray<BoundAttributeArgument> arguments = VisitList(node.Arguments);
            return node.Update(name, arguments, node.Span);
        }

        public BoundNode VisitAttributeArgument(BoundAttributeArgument node) {
            BoundIdentifierName name = Visit(node.Name);
            BoundExpression expression = Visit(node.Expression);
            return node.Update(name, expression, node.Span);
        }

        public BoundNode VisitAttributeList(BoundAttributeList node) {
            ImmutableArray<BoundAttribute> attributes = VisitList(node.Attributes);
            return node.Update(node.Target, attributes, node.Span);
        }

        public BoundNode VisitAwaitExpression(BoundAwaitExpression node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(expression, node.Span);
        }

        public BoundNode VisitBinaryExpression(BoundBinaryExpression node) {
            BoundExpression left = Visit(node.Left);
            BoundExpression right = Visit(node.Right);
            return node.Update(node.Operator, left, right, node.Span);
        }

        public BoundNode VisitBlock(BoundBlock node) {
            ImmutableArray<BoundStatement> statements = VisitList(node.Statements);
            return node.Update(statements, node.Span);
        }

        public BoundNode VisitBreakStatement(BoundBreakStatement node) {
            return node.Update(node.Span);
        }

        public BoundNode VisitCastExpression(BoundCastExpression node) {
            BoundExpression expression = Visit(node.Expression);
            BoundType targetType = Visit(node.TargetType);
            return node.Update(expression, targetType, node.Span);
        }

        public BoundNode VisitCatchClause(BoundCatchClause node) {
            BoundType exceptionType = Visit(node.ExceptionType);
            BoundIdentifierName identifier = Visit(node.Identifier);
            BoundBlock block = Visit(node.Block);
            return node.Update(exceptionType, identifier, block, node.Span);
        }

        public BoundNode VisitCompilationUnit(BoundCompilationUnit node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            ImmutableArray<BoundImportDirective> imports = VisitList(node.Imports);
            ImmutableArray<BoundMemberDeclaration> members = VisitList(node.Members);
            return node.Update(attributeLists, imports, members, node.Span);
        }

        public BoundNode VisitConditionalExpression(BoundConditionalExpression node) {
            BoundExpression condition = Visit(node.Condition);
            BoundExpression whenTrue = Visit(node.WhenTrue);
            BoundExpression whenFalse = Visit(node.WhenFalse);
            return node.Update(condition, whenTrue, whenFalse, node.Span);
        }

        public BoundNode VisitConstructorConstraint(BoundConstructorConstraint node) {
            return node.Update(node.Span);
        }

        public BoundNode VisitConstructorDeclaration(BoundConstructorDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundIdentifierName identifier = Visit(node.Identifier);
            BoundConstructorInitializer initializer = Visit(node.Initializer);
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            BoundBlock body = Visit(node.Body);
            return node.Update(attributeLists, node.Modifiers, identifier, initializer, parameters, body, node.Span);
        }

        public BoundNode VisitConstructorInitializer(BoundConstructorInitializer node) {
            ImmutableArray<BoundArgument> arguments = VisitList(node.Arguments);
            return node.Update(node.Type, arguments, node.Span);
        }

        public BoundNode VisitContinueStatement(BoundContinueStatement node) {
            return node.Update(node.Span);
        }

        public BoundNode VisitConversionOperatorDeclaration(BoundConversionOperatorDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType targetType = Visit(node.TargetType);
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            BoundBlock body = Visit(node.Body);
            return node.Update(attributeLists, node.Modifiers, node.Type, targetType, parameters, body, node.Span);
        }

        public BoundNode VisitDefaultExpression(BoundDefaultExpression node) {
            BoundType targetType = Visit(node.TargetType);
            return node.Update(targetType, node.Span);
        }

        public BoundNode VisitDelegateDeclaration(BoundDelegateDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType returnType = Visit(node.ReturnType);
            BoundIdentifierName identifier = Visit(node.Identifier);
            ImmutableArray<BoundTypeParameter> typeParameters = VisitList(node.TypeParameters);
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses = VisitList(node.ConstraintClauses);
            return node.Update(attributeLists, node.Modifiers, returnType, identifier, typeParameters, parameters, constraintClauses, node.Span);
        }

        public BoundNode VisitDeleteStatement(BoundDeleteStatement node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(expression, node.Span);
        }

        public BoundNode VisitDestructorDeclaration(BoundDestructorDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundIdentifierName identifier = Visit(node.Identifier);
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            BoundBlock body = Visit(node.Body);
            return node.Update(attributeLists, node.Modifiers, identifier, parameters, body, node.Span);
        }

        public BoundNode VisitDoStatement(BoundDoStatement node) {
            BoundExpression condition = Visit(node.Condition);
            BoundBlock block = Visit(node.Block);
            return node.Update(condition, block, node.Span);
        }

        public BoundNode VisitElementAccessExpression(BoundElementAccessExpression node) {
            BoundExpression expression = Visit(node.Expression);
            ImmutableArray<BoundExpression> indexExpressions = VisitList(node.IndexExpressions);
            return node.Update(expression, indexExpressions, node.Span);
        }

        public BoundNode VisitElseClause(BoundElseClause node) {
            BoundExpression condition = Visit(node.Condition);
            BoundBlock block = Visit(node.Block);
            return node.Update(node.Type, condition, block, node.Span);
        }

        public BoundNode VisitEmptyStatement(BoundEmptyStatement node) {
            return node.Update(node.Span);
        }

        public BoundNode VisitEnumDeclaration(BoundEnumDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundIdentifierName identifier = Visit(node.Identifier);
            ImmutableArray<BoundEnumMemberDeclaration> members = VisitList(node.Members);
            ImmutableArray<BoundType> baseTypes = VisitList(node.BaseTypes);
            return node.Update(attributeLists, node.Modifiers, identifier, members, baseTypes, node.Span);
        }

        public BoundNode VisitEnumMemberDeclaration(BoundEnumMemberDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundIdentifierName identifier = Visit(node.Identifier);
            BoundExpression value = Visit(node.Value);
            return node.Update(attributeLists, identifier, value, node.Span);
        }

        public BoundNode VisitEventDeclaration(BoundEventDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType type = Visit(node.Type);
            BoundName explicitInterfaceSpecifier = Visit(node.ExplicitInterfaceSpecifier);
            BoundIdentifierName identifier = Visit(node.Identifier);
            ImmutableArray<BoundAccessorDeclaration> accessors = VisitList(node.Accessors);
            return node.Update(attributeLists, node.Modifiers, type, explicitInterfaceSpecifier, identifier, accessors, node.Span);
        }

        public BoundNode VisitEventFieldDeclaration(BoundEventFieldDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundVariableDeclaration declaration = Visit(node.Declaration);
            return node.Update(attributeLists, node.Modifiers, declaration, node.Span);
        }

        public BoundNode VisitExpressionStatement(BoundExpressionStatement node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(expression, node.Span);
        }

        public BoundNode VisitFieldDeclaration(BoundFieldDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundVariableDeclaration declaration = Visit(node.Declaration);
            return node.Update(attributeLists, node.Modifiers, declaration, node.Span);
        }

        public BoundNode VisitFinallyClause(BoundFinallyClause node) {
            BoundBlock block = Visit(node.Block);
            return node.Update(block, node.Span);
        }

        public BoundNode VisitForEachStatement(BoundForEachStatement node) {
            BoundType elementType = Visit(node.ElementType);
            BoundIdentifierName identifier = Visit(node.Identifier);
            BoundExpression expression = Visit(node.Expression);
            BoundBlock block = Visit(node.Block);
            return node.Update(elementType, identifier, expression, block, node.Span);
        }

        public BoundNode VisitForStatement(BoundForStatement node) {
            BoundVariableDeclaration declaration = Visit(node.Declaration);
            ImmutableArray<BoundExpression> initializers = VisitList(node.Initializers);
            BoundExpression condition = Visit(node.Condition);
            ImmutableArray<BoundExpression> incrementors = VisitList(node.Incrementors);
            BoundBlock block = Visit(node.Block);
            return node.Update(declaration, initializers, condition, incrementors, block, node.Span);
        }

        public BoundNode VisitGenericName(BoundGenericName node) {
            ImmutableArray<BoundType> typeArguments = VisitList(node.TypeArguments);
            return node.Update(node.Identifier, typeArguments, node.Span);
        }

        public BoundNode VisitIdentifierName(BoundIdentifierName node) {
            return node.Update(node.Identifier, node.Span);
        }

        public BoundNode VisitIfStatement(BoundIfStatement node) {
            BoundExpression condition = Visit(node.Condition);
            BoundBlock block = Visit(node.Block);
            ImmutableArray<BoundElseClause> elses = VisitList(node.Elses);
            return node.Update(condition, block, elses, node.Span);
        }

        public BoundNode VisitImplicitArrayCreationExpression(BoundImplicitArrayCreationExpression node) {
            ImmutableArray<BoundArrayRankSpecifier> rankSpecifiers = VisitList(node.RankSpecifiers);
            BoundInitializerExpression initializer = Visit(node.Initializer);
            return node.Update(rankSpecifiers, initializer, node.Span);
        }

        public BoundNode VisitImportDirective(BoundImportDirective node) {
            BoundIdentifierName alias = Visit(node.Alias);
            BoundName name = Visit(node.Name);
            return node.Update(node.IsStatic, alias, name, node.Span);
        }

        public BoundNode VisitIndexerDeclaration(BoundIndexerDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType type = Visit(node.Type);
            BoundName explicitInterfaceSpecifier = Visit(node.ExplicitInterfaceSpecifier);
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            ImmutableArray<BoundAccessorDeclaration> accessors = VisitList(node.Accessors);
            return node.Update(attributeLists, node.Modifiers, type, explicitInterfaceSpecifier, parameters, accessors, node.Span);
        }

        public BoundNode VisitInitializerExpression(BoundInitializerExpression node) {
            ImmutableArray<BoundExpression> expressions = VisitList(node.Expressions);
            return node.Update(expressions, node.Span);
        }

        public BoundNode VisitInstanceExpression(BoundInstanceExpression node) {
            return node.Update(node.Type, node.Span);
        }

        public BoundNode VisitInvocationExpression(BoundInvocationExpression node) {
            BoundExpression expression = Visit(node.Expression);
            ImmutableArray<BoundArgument> arguments = VisitList(node.Arguments);
            return node.Update(expression, arguments, node.Span);
        }

        public BoundNode VisitLambdaExpression(BoundLambdaExpression node) {
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            BoundNode body = Visit(node.Body);
            return node.Update(node.Modifiers, parameters, body, node.Span);
        }

        public BoundNode VisitLiteralExpression(BoundLiteralExpression node) {
            return node.Update(node.LiteralType, node.Value, node.Span);
        }

        public BoundNode VisitLocalDeclarationStatement(BoundLocalDeclarationStatement node) {
            BoundVariableDeclaration declaration = Visit(node.Declaration);
            return node.Update(node.Modifiers, declaration, node.Span);
        }

        public BoundNode VisitLoopStatement(BoundLoopStatement node) {
            BoundBlock block = Visit(node.Block);
            return node.Update(block, node.Span);
        }

        public BoundNode VisitMemberAccessExpression(BoundMemberAccessExpression node) {
            BoundExpression expression = Visit(node.Expression);
            BoundSimpleName name = Visit(node.Name);
            return node.Update(expression, name, node.Span);
        }

        public BoundNode VisitMethodDeclaration(BoundMethodDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType returnType = Visit(node.ReturnType);
            BoundName explicitInterfaceSpecifier = Visit(node.ExplicitInterfaceSpecifier);
            BoundIdentifierName identifier = Visit(node.Identifier);
            ImmutableArray<BoundTypeParameter> typeParameters = VisitList(node.TypeParameters);
            ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses = VisitList(node.ConstraintClauses);
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            BoundBlock body = Visit(node.Body);
            return node.Update(attributeLists, node.Modifiers, returnType, explicitInterfaceSpecifier, identifier, typeParameters, constraintClauses, parameters, body, node.Span);
        }

        public BoundNode VisitNakedNullableType(BoundNakedNullableType node) {
            return node.Update(node.Type, node.Span);
        }

        public BoundNode VisitNamespaceDeclaration(BoundNamespaceDeclaration node) {
            BoundName name = Visit(node.Name);
            ImmutableArray<BoundImportDirective> imports = VisitList(node.Imports);
            ImmutableArray<BoundMemberDeclaration> members = VisitList(node.Members);
            return node.Update(name, imports, members, node.Span);
        }

        public BoundNode VisitNullableType(BoundNullableType node) {
            BoundType elementType = Visit(node.ElementType);
            return node.Update(elementType, node.Span);
        }

        public BoundNode VisitObjectCreationExpression(BoundObjectCreationExpression node) {
            BoundType targetType = Visit(node.TargetType);
            ImmutableArray<BoundArgument> arguments = VisitList(node.Arguments);
            BoundInitializerExpression initializer = Visit(node.Initializer);
            return node.Update(targetType, arguments, initializer, node.Span);
        }

        public BoundNode VisitOmittedArraySizeExpression(BoundOmittedArraySizeExpression node) {
            return node.Update(node.Span);
        }

        public BoundNode VisitOmittedTypeArgument(BoundOmittedTypeArgument node) {
            return node.Update(node.Span);
        }

        public BoundNode VisitOperatorDeclaration(BoundOperatorDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType returnType = Visit(node.ReturnType);
            ImmutableArray<BoundParameter> parameters = VisitList(node.Parameters);
            BoundBlock body = Visit(node.Body);
            return node.Update(attributeLists, node.Modifiers, returnType, node.Operator, parameters, body, node.Span);
        }

        public BoundNode VisitParameter(BoundParameter node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType parameterType = Visit(node.ParameterType);
            BoundIdentifierName identifier = Visit(node.Identifier);
            return node.Update(attributeLists, node.Modifiers, parameterType, identifier, node.Span);
        }

        public BoundNode VisitParenthesizedExpression(BoundParenthesizedExpression node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(expression, node.Span);
        }

        public BoundNode VisitPostfixUnaryExpression(BoundPostfixUnaryExpression node) {
            BoundExpression operand = Visit(node.Operand);
            return node.Update(node.Operator, operand, node.Span);
        }

        public BoundNode VisitPredefinedType(BoundPredefinedType node) {
            return node.Update(node.PredefinedType, node.Span);
        }

        public BoundNode VisitPrefixUnaryExpression(BoundPrefixUnaryExpression node) {
            BoundExpression operand = Visit(node.Operand);
            return node.Update(node.Operator, operand, node.Span);
        }

        public BoundNode VisitPropertyDeclaration(BoundPropertyDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundType type = Visit(node.Type);
            BoundName explicitInterfaceSpecifier = Visit(node.ExplicitInterfaceSpecifier);
            BoundIdentifierName identifier = Visit(node.Identifier);
            ImmutableArray<BoundAccessorDeclaration> accessors = VisitList(node.Accessors);
            return node.Update(attributeLists, node.Modifiers, type, explicitInterfaceSpecifier, identifier, accessors, node.Span);
        }

        public BoundNode VisitQualifiedName(BoundQualifiedName node) {
            BoundName left = Visit(node.Left);
            BoundSimpleName right = Visit(node.Right);
            return node.Update(left, right, node.Span);
        }

        public BoundNode VisitReturnStatement(BoundReturnStatement node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(expression, node.Span);
        }

        public BoundNode VisitSizeOfExpression(BoundSizeOfExpression node) {
            BoundType targetType = Visit(node.TargetType);
            return node.Update(targetType, node.Span);
        }

        public BoundNode VisitSwitchSection(BoundSwitchSection node) {
            ImmutableArray<BoundExpression> values = VisitList(node.Values);
            BoundBlock block = Visit(node.Block);
            return node.Update(node.Type, values, block, node.Span);
        }

        public BoundNode VisitSwitchStatement(BoundSwitchStatement node) {
            BoundExpression expression = Visit(node.Expression);
            ImmutableArray<BoundSwitchSection> sections = VisitList(node.Sections);
            return node.Update(expression, sections, node.Span);
        }

        public BoundNode VisitThrowStatement(BoundThrowStatement node) {
            BoundExpression expression = Visit(node.Expression);
            return node.Update(expression, node.Span);
        }

        public BoundNode VisitTrackedType(BoundTrackedType node) {
            BoundType elementType = Visit(node.ElementType);
            return node.Update(elementType, node.Span);
        }

        public BoundNode VisitTryStatement(BoundTryStatement node) {
            BoundBlock block = Visit(node.Block);
            ImmutableArray<BoundCatchClause> catches = VisitList(node.Catches);
            BoundFinallyClause finally_ = Visit(node.Finally);
            return node.Update(block, catches, finally_, node.Span);
        }

        public BoundNode VisitTypeConstraint(BoundTypeConstraint node) {
            BoundType constrainedType = Visit(node.ConstrainedType);
            return node.Update(constrainedType, node.Span);
        }

        public BoundNode VisitTypeDeclaration(BoundTypeDeclaration node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundIdentifierName identifier = Visit(node.Identifier);
            ImmutableArray<BoundTypeParameter> typeParameters = VisitList(node.TypeParameters);
            ImmutableArray<BoundTypeParameterConstraintClause> constraintClauses = VisitList(node.ConstraintClauses);
            ImmutableArray<BoundMemberDeclaration> members = VisitList(node.Members);
            ImmutableArray<BoundType> baseTypes = VisitList(node.BaseTypes);
            return node.Update(attributeLists, node.Modifiers, identifier, node.Type, typeParameters, constraintClauses, members, baseTypes, node.Span);
        }

        public BoundNode VisitTypeFamilyConstraint(BoundTypeFamilyConstraint node) {
            return node.Update(node.Family, node.Nullability, node.Span);
        }

        public BoundNode VisitTypeOfExpression(BoundTypeOfExpression node) {
            BoundType targetType = Visit(node.TargetType);
            return node.Update(targetType, node.Span);
        }

        public BoundNode VisitTypeParameter(BoundTypeParameter node) {
            ImmutableArray<BoundAttributeList> attributeLists = VisitList(node.AttributeLists);
            BoundIdentifierName identifier = Visit(node.Identifier);
            return node.Update(attributeLists, node.Variance, identifier, node.Span);
        }

        public BoundNode VisitTypeParameterConstraintClause(BoundTypeParameterConstraintClause node) {
            BoundIdentifierName name = Visit(node.Name);
            ImmutableArray<BoundTypeParameterConstraint> constraints = VisitList(node.Constraints);
            return node.Update(name, constraints, node.Span);
        }

        public BoundNode VisitUsingStatement(BoundUsingStatement node) {
            BoundVariableDeclaration declaration = Visit(node.Declaration);
            BoundExpression expression = Visit(node.Expression);
            BoundBlock block = Visit(node.Block);
            return node.Update(declaration, expression, block, node.Span);
        }

        public BoundNode VisitVariableDeclaration(BoundVariableDeclaration node) {
            BoundType variableType = Visit(node.VariableType);
            ImmutableArray<BoundVariableDeclarator> variables = VisitList(node.Variables);
            return node.Update(variableType, variables, node.Span);
        }

        public BoundNode VisitVariableDeclarationExpression(BoundVariableDeclarationExpression node) {
            BoundType variableType = Visit(node.VariableType);
            BoundIdentifierName identifier = Visit(node.Identifier);
            return node.Update(variableType, identifier, node.Span);
        }

        public BoundNode VisitVariableDeclarator(BoundVariableDeclarator node) {
            BoundIdentifierName identifier = Visit(node.Identifier);
            BoundExpression value = Visit(node.Value);
            return node.Update(identifier, value, node.Span);
        }

        public BoundNode VisitVarType(BoundVarType node) {
            return node.Update(node.Span);
        }

        public BoundNode VisitWhileStatement(BoundWhileStatement node) {
            BoundExpression condition = Visit(node.Condition);
            BoundBlock block = Visit(node.Block);
            return node.Update(condition, block, node.Span);
        }
    }

    public enum BoundKind {
        AccessorDeclaration,
        AliasQualifiedName,
        AnonymousObjectCreationExpression,
        AnonymousObjectMemberDeclarator,
        Argument,
        ArrayCreationExpression,
        ArrayRankSpecifier,
        ArrayType,
        AssertStatement,
        Attribute,
        AttributeArgument,
        AttributeList,
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
        TypeParameter,
        TypeParameterConstraintClause,
        UsingStatement,
        VariableDeclaration,
        VariableDeclarationExpression,
        VariableDeclarator,
        VarType,
        WhileStatement,
    }
}
