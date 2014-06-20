grammar Truss;

options {
	language = CSharp3;
}

scope declarationPrefix {
    ImmutableArray<AttributeListSyntax> attributes;
    ImmutableArray<Modifier> modifiers;
    TypeSyntax type;
}

@lexer::namespace { Truss.Compiler.Parser }
@parser::namespace { Truss.Compiler.Parser }
@lexer::modifier { internal }
@parser::modifier { internal }

@header {
    using Truss.Compiler;
    using Truss.Compiler.Syntax;
    using Truss.Compiler.Support;
    using System.Diagnostics;
}

@lexer::header {
    using Truss.Compiler;
    using Truss.Compiler.Syntax;
    using Truss.Compiler.Support;
    using System.Diagnostics;
}

////////////////////////////////////////////////////////////////////////////////
// PARSER                                                                     //
////////////////////////////////////////////////////////////////////////////////

compilationUnit returns [CompilationUnitSyntax value]
@init {
    var start = input.LT(1);
    var attributeLists = new ImmutableArray<AttributeListSyntax>.Builder();
    var imports = new ImmutableArray<ImportDirectiveSyntax>.Builder();
    var members = new ImmutableArray<MemberDeclarationSyntax>.Builder();
}
    :
        (
            // Only assembly attributes belong to this scope; the rest is
            // attached to the member it belongs to.
            ( assemblyAttributeHeadScan )=> al=attributeList { attributeLists.Add(al); }
        |
            id=importDirective { imports.Add(id); }
        |
            nsmd=namespaceScopeMemberDeclaration { members.Add(nsmd); }
        )*
        { value = new CompilationUnitSyntax(attributeLists.Build(), imports.Build(), members.Build(), Span(start)); }
        EOF
    ;

assemblyAttributeHeadScan
    :
        OP_BRACKET_OPEN
        i=IDENTIFIER
        { i.Text == "assembly" }?=> { }
        OP_COLON
    ;

namespaceScopeMemberDeclaration returns [MemberDeclarationSyntax value]
scope declarationPrefix;
    :
        nd=namespaceDeclaration { value = nd; }
    |
        all=attributeListList { $declarationPrefix::attributes = all; }
        m=modifiers { $declarationPrefix::modifiers = m; }
        ( dd=delegateDeclaration { value = dd; }
        | td=typeDeclaration { value = td; }
        | ed=enumDeclaration { value = ed; }
        )
    ;

importDirective returns [ImportDirectiveSyntax value]
@init {
    var start = input.LT(1);
    bool isStatic = false;
}
    :
        KW_IMPORT
        ( KW_STATIC { isStatic = true; } )?
        (
            idn=identifierName
            OP_EQUALS
        )?
        n=name
        OP_SEMICOLON
        { value = new ImportDirectiveSyntax(isStatic, idn, n, Span(start)); }
    ;

namespaceDeclaration returns [NamespaceDeclarationSyntax value]
@init {
    var start = input.LT(1);
    var imports = new ImmutableArray<ImportDirectiveSyntax>.Builder();
    var members = new ImmutableArray<MemberDeclarationSyntax>.Builder();
}
    :
        KW_NAMESPACE
        n=name
        OP_BRACE_OPEN
        ( id=importDirective { imports.Add(id); }
        | nsmd=namespaceScopeMemberDeclaration { members.Add(nsmd); }
        )*
        { value = new NamespaceDeclarationSyntax(n, imports.Build(), members.Build(), Span(start)); }
        OP_BRACE_CLOSE
    ;

// Attributes

attributeListList returns [ImmutableArray<AttributeListSyntax> value]
@init {
    var builder = new ImmutableArray<AttributeListSyntax>.Builder();
}
    :
        (
            al=attributeList { builder.Add(al); }
        )*
        { value = builder.Build(); }
    ;

attributeList returns [AttributeListSyntax value]
@init {
    var start = input.LT(1);
    var attributes = new ImmutableArray<AttributeSyntax>.Builder();
    var target = AttributeTarget.None;
}
    :
        OP_BRACKET_OPEN
        (
            at=attributeTarget
            { target = at; }
            OP_COLON
        )?
        a=attribute
        { attributes.Add(a); }
        (
            OP_COMMA
            a=attribute
            { attributes.Add(a); }
        )*
        OP_BRACKET_CLOSE
        { value = new AttributeListSyntax(target, attributes.Build(), Span(start)); }
    ;

attribute returns [AttributeSyntax value]
@init {
    var start = input.LT(1);
    var arguments = new ImmutableArray<AttributeArgumentSyntax>.Builder();
}
    :
        n=name
        (
            OP_PAREN_OPEN
            (
                a=attributeArgument
                { arguments.Add(a); }
                (
                    OP_COMMA
                    a=attributeArgument
                    { arguments.Add(a); }
                )*
            )?
            OP_PAREN_CLOSE
        )?
        { value = new AttributeSyntax(n, arguments.Build(), Span(start)); }
    ;

attributeArgument returns [AttributeArgumentSyntax value]
@init {
    var start = input.LT(1);
}
    :
        (
            ( identifierName OP_EQUALS )=>
            idn=identifierName
            OP_EQUALS
        )?
        e=expression
        { value = new AttributeArgumentSyntax(idn, e, Span(start)); }
    ;

attributeTarget returns [AttributeTarget value]
@init {
    var start = input.LT(1);
}
    :
        idn=identifierName
        { value = ParseAttributeTarget(idn.Identifier, Span(start)); }
    |
        KW_EVENT
        { value = ParseAttributeTarget("event", Span(start)); }
    |
        KW_RETURN
        { value = ParseAttributeTarget("return", Span(start)); }
    ;

parameterList returns [ImmutableArray<ParameterSyntax> value]
    :
        OP_PAREN_OPEN
        (
            bpl=bareParameterList
            { value = bpl; }
        |
            { value = ImmutableArray<ParameterSyntax>.Empty; }
        )
        OP_PAREN_CLOSE
    ;

bareParameterList returns [ImmutableArray<ParameterSyntax> value]
@init {
    var parameters = new ImmutableArray<ParameterSyntax>.Builder();
}
    :
        p=parameter
        { parameters.Add(p); }
        (
            OP_COMMA
            p=parameter
            { parameters.Add(p); }
        )*
        { value = parameters.Build(); }
    ;

parameter returns [ParameterSyntax value]
@init {
    var start = input.LT(1);
    var modifiers = new ImmutableArray<ParameterModifier>.Builder();
}
    :
        all=attributeListList
        (
            pm=parameterModifier
            { modifiers.Add(pm); }
        )*
        t=typeSyntax
        idn=identifierName
        { value = new ParameterSyntax(all, modifiers.Build(), t, idn, Span(start)); }
    ;

parameterModifier returns [ParameterModifier value]
    : KW_THIS { value = ParameterModifier.This; }
    | KW_PARAMS { value = ParameterModifier.Params; }
    | KW_CONSUMES { value = ParameterModifier.Consumes; }
    | am=argumentModifier { value = am; }
    ;

argumentModifier returns [ParameterModifier value]
    : KW_REF { value = ParameterModifier.Ref; }
    | KW_OUT { value = ParameterModifier.Out; }
    ;

// Delegate declaration

delegateDeclaration returns [DelegateDeclarationSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_DELEGATE
        rt=typeSyntax
        idn=identifierName
        tpl=typeParameterList
        pl=parameterList
        tpccl=typeParameterConstraintClauseList
        OP_SEMICOLON
        { value = new DelegateDeclarationSyntax($declarationPrefix::attributes, $declarationPrefix::modifiers, rt, idn, tpl, pl, tpccl, Span(start)); }
    ;

typeParameterList returns [ImmutableArray<TypeParameterSyntax> value]
@init {
    var start = input.LT(1);
    var typeParameters = new ImmutableArray<TypeParameterSyntax>.Builder();
}
    :
        (
            OP_LESS_THAN
            tp=typeParameter
            { typeParameters.Add(tp); }
            (
                OP_COMMA
                tp=typeParameter
                { typeParameters.Add(tp); }
            )*
            OP_GREATER_THAN
        )?
        { value = typeParameters.Build(); }
    ;

typeParameter returns [TypeParameterSyntax value]
@init {
    var start = input.LT(1);
}
    :
        all=attributeListList
        tpv=typeParameterVariance
        idn=identifierName
        { value = new TypeParameterSyntax(all, tpv, idn, Span(start)); }
    ;

typeParameterVariance returns [Variance value]
    : KW_IN { value = Variance.In; }
    | KW_OUT { value = Variance.Out; }
    | { value = Variance.None; }
    ;

// Type declarations

typeDeclaration returns [TypeDeclarationSyntax value]
@init {
    var start = input.LT(1);
    TypeDeclarationType? type = null;
    var baseTypes = new ImmutableArray<TypeSyntax>.Builder();
    var memberDeclarations = new ImmutableArray<MemberDeclarationSyntax>.Builder();
    bool tracked = false;
}
@after {
    ImmutableArray<Modifier> modifiers = $declarationPrefix::modifiers;
    
    if (tracked) {
        var builder = new ImmutableArray<Modifier>.Builder();
        builder.AddRange(modifiers);
        builder.Add(Modifier.Tracked);
        modifiers = builder.Build();
    }

    value = new TypeDeclarationSyntax(
        $declarationPrefix::attributes,
        modifiers,
        idn,
        type.Value,
        tpl,
        tpccl,
        memberDeclarations.Build(),
        baseTypes.Build(),
        Span(start)
    );
}
    :
        ( KW_CLASS { type = TypeDeclarationType.Class; }
        | KW_INTERFACE { type = TypeDeclarationType.Interface; }
        | KW_STRUCT ( OP_CARET { tracked = true; } )? { type = TypeDeclarationType.Struct; }
        )
        idn=identifierName
        tpl=typeParameterList
        (
            OP_COLON
            (
                t=typeSyntax
                { baseTypes.Add(t); }
                (
                    OP_COMMA
                    t=typeSyntax
                    { baseTypes.Add(t); }
                )*
            )
        )?
        tpccl=typeParameterConstraintClauseList
        OP_BRACE_OPEN
        (
            md=memberDeclaration
            { memberDeclarations.Add(md); }
        )*
        OP_BRACE_CLOSE
    ;

typeParameterConstraintClauseList returns [ImmutableArray<TypeParameterConstraintClauseSyntax> value]
@init {
    var builder = new ImmutableArray<TypeParameterConstraintClauseSyntax>.Builder();
}
    :
        (
            tpc=typeParameterConstraintClause
            { builder.Add(tpc); }
            
        )*
        { value = builder.Build(); }
    ;

typeParameterConstraintClause returns [TypeParameterConstraintClauseSyntax value]
@init {
    var constraints = new ImmutableArray<TypeParameterConstraintSyntax>.Builder();
    var start = input.LT(1);
}
    :
        KW_WHERE
        idn=identifierName
        OP_COLON
        tpc=typeParameterConstraint
        { constraints.Add(tpc); }
        (
            OP_COMMA
            tpc=typeParameterConstraint
            { constraints.Add(tpc); }
        )*
        { value = new TypeParameterConstraintClauseSyntax(idn, constraints.Build(), Span(start)); }
    ;

typeParameterConstraint returns [TypeParameterConstraintSyntax value]
@init {
    var start = input.LT(1);
    TypeFamily? family = null;
}
    :
        KW_NEW OP_PAREN_OPEN OP_PAREN_CLOSE
        { value = new ConstructorConstraintSyntax(Span(start)); }
    |
        (
            n=nullable { family = TypeFamily.Any; }
        |
            KW_CLASS n=nullable { family = TypeFamily.Class; }
        |
            KW_STRUCT
            ( n=nullable { family = TypeFamily.Struct; }
            | OP_CARET { family = TypeFamily.Tracked; }
            )
        )
        { value = new TypeFamilyConstraintSyntax(family.Value, family == TypeFamily.Tracked ? (Nullable?)null : n, Span(start)); }
    |
        t=typeSyntax
        { value = new TypeConstraintSyntax(t, Span(start)); }
    ;

nullable returns [Nullable value]
    : OP_QUESTION { value = Nullable.Nullable; }
    | OP_EXCLAMATION { value = Nullable.NotNullable; }
    ;

modifiers returns [ImmutableArray<Modifier> value]
@init {
    var builder = new ImmutableArray<Modifier>.Builder();
}
    :
    (
        m=modifier
        { builder.Add(m); }
    )*
    { value = builder.Build(); }
    ;

modifier returns [Modifier value]
    : KW_ABSTRACT { value = Modifier.Abstract; }
    | KW_ASYNC { value = Modifier.Async; }
    | KW_EXTERN { value = Modifier.Extern; }
    | KW_INTERNAL { value = Modifier.Internal; }
    | KW_NEW { value = Modifier.New; }
    | KW_OVERRIDE { value = Modifier.Override; }
    | KW_PARTIAL { value = Modifier.Partial; }
    | KW_PRIVATE { value = Modifier.Private; }
    | KW_PROTECTED { value = Modifier.Protected; }
    | KW_PUBLIC { value = Modifier.Public; }
    | KW_READONLY { value = Modifier.Readonly; }
    | KW_SEALED { value = Modifier.Sealed; }
    | KW_STATIC { value = Modifier.Static; }
    | KW_VIRTUAL { value = Modifier.Virtual; }
    | KW_VOLATILE { value = Modifier.Volatile; }
    ;

// Enum declaration

enumDeclaration returns [EnumDeclarationSyntax value]
@init {
    var start = input.LT(1);
    var members = new ImmutableArray<EnumMemberDeclarationSyntax>.Builder();
}
    :
        KW_ENUM
        idn=identifierName
        (
            OP_COLON
            t=typeSyntax
        )?
        OP_BRACE_OPEN
        (
            emd=enumMemberDeclaration
            { members.Add(emd); }
            (
                OP_COMMA
                emd=enumMemberDeclaration
                { members.Add(emd); }
            )*
            OP_COMMA?
        )?
        OP_BRACE_CLOSE
        {
            value = new EnumDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                idn,
                members.Build(),
                t == null ? ImmutableArray<TypeSyntax>.Empty : ImmutableArray<TypeSyntax>.Create(t),
                Span(start)
            );
        }
    ;

enumMemberDeclaration returns [EnumMemberDeclarationSyntax value]
@init {
    var start = input.LT(1);
}
    :
        all=attributeListList
        idn=identifierName
        (
            OP_EQUALS
            e=expression
        )?
        { value = new EnumMemberDeclarationSyntax(all, idn, e, Span(start)); }
    ;

// Member declarations

memberDeclaration returns [MemberDeclarationSyntax value]
scope declarationPrefix;
    :
    all=attributeListList { $declarationPrefix::attributes = all; }
    m=modifiers { $declarationPrefix::modifiers = m; }
    (
        ( constructorDeclarationHeadScan )=> e2=constructorDeclaration
        { value = e2; }
    |
        t=typeSyntax { $declarationPrefix::type = t; }
        e1=typedMemberDeclaration
        { value = e1; }
    |
        e3=conversionOperatorDeclaration { value = e3; }
    |
        { $declarationPrefix::modifiers.Count == 0 }?=> e4=destructorDeclaration { value = e4; }
    |
        e5=eventDeclaration { value = e5; }
    |
        e7=delegateDeclaration { value = e7; }
    |
        e8=typeDeclaration { value = e8; }
    |
        e9=enumDeclaration { value = e9; }
    )
    ;

// Typed member declaration

typedMemberDeclaration returns [MemberDeclarationSyntax value]
    :
        e1=operatorDeclaration
        { value = e1; }
    |
        e2=fieldDeclaration
        { value = e2; }
    ;

// Property/field/indexer/method declaration


fieldDeclaration returns [MemberDeclarationSyntax value]
scope {
    MemberName memberName;
}
@init {
    var start = input.LT(1);
}
    :
        ( variableDeclarationWithoutType OP_SEMICOLON )=>
        vdwt=variableDeclarationWithoutType
        {
            value = new FieldDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                vdwt,
                Span(start)
            );
        }
        OP_SEMICOLON
    |
        ( ( name OP_DOT )? KW_THIS )=>
        ( n=name OP_DOT )?
        KW_THIS
        OP_BRACKET_OPEN
        bpl=bareParameterList
        OP_BRACKET_CLOSE
        al=accessorList
        {
            value = new IndexerDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                $declarationPrefix::type,
                n,
                bpl,
                al,
                Span(start)
            );
        }
    |
        mn=memberName
        { $fieldDeclaration::memberName = mn; }
        (
            pdt=propertyDeclarationTail
            { value = pdt; }
        |
            mdt=methodDeclarationTail
            { value = mdt; }
        )
    ;

propertyDeclarationTail returns [PropertyDeclarationSyntax value]
@init {
    var start = input.LT(1);
    Debug.Assert($fieldDeclaration::memberName.TypeParameters.Count == 0);
}
    :
        al=accessorList
        {
            value = new PropertyDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                $declarationPrefix::type,
                $fieldDeclaration::memberName.InterfaceName,
                $fieldDeclaration::memberName.Identifier,
                al,
                Span(start)
            );
        }
    ;

methodDeclarationTail returns [MethodDeclarationSyntax value]
@init {
    var start = input.LT(1);
}
    :
        pl=parameterList
        tpccl=typeParameterConstraintClauseList
        b=block
        {
            value = new MethodDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                $declarationPrefix::type,
                $fieldDeclaration::memberName.InterfaceName,
                $fieldDeclaration::memberName.Identifier,
                $fieldDeclaration::memberName.TypeParameters,
                tpccl,
                pl,
                b,
                Span(start)
            );
        }
    ;

// Event declaration

eventDeclaration returns [MemberDeclarationSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_EVENT
        (
            ( variableDeclaration OP_SEMICOLON )=>
            vd=variableDeclaration
            {
                value = new EventFieldDeclarationSyntax(
                    $declarationPrefix::attributes,
                    $declarationPrefix::modifiers,
                    vd,
                    Span(start)
                );
            }
            OP_SEMICOLON
        |
            t=typeSyntax
            mn=memberName
            al=accessorList
            {
                value = new EventDeclarationSyntax(
                    $declarationPrefix::attributes,
                    $declarationPrefix::modifiers,
                    t,
                    mn.InterfaceName,
                    mn.Identifier,
                    al,
                    Span(start)
                );
            }
        )
    ;

accessorList returns [ImmutableArray<AccessorDeclarationSyntax> value]
@init {
    IToken start = null;
    var accessors = new ImmutableArray<AccessorDeclarationSyntax>.Builder();
}
    :
        OP_BRACE_OPEN
        (
            { start = input.LT(1); }
            all=attributeListList
            m=modifiers
            adt=accessorDeclarationType
            b=block
            { accessors.Add(new AccessorDeclarationSyntax(all, m, adt, b, Span(start))); }
        )*
        OP_BRACE_CLOSE
        { value = accessors.Build(); }
    ;

accessorDeclarationType returns [AccessorDeclarationType value]
    :
        idn=identifierName
        { value = ParseAccessorDeclarationType(idn.Identifier, idn.Span); }
    ;

// Constructor declaration

constructorDeclarationHeadScan : identifierName OP_PAREN_OPEN ;

constructorDeclaration returns [ConstructorDeclarationSyntax value]
@init {
    var start = input.LT(1);
}
    :
        idn=identifierName
        pl=parameterList
        ( ci=constructorInitializer )?
        b=block
        {
            value = new ConstructorDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                idn,
                ci,
                pl,
                b,
                Span(start)
            );
        }
    ;

constructorInitializer returns [ConstructorInitializerSyntax value]
@init {
    var start = input.LT(1);
    ThisOrBase? type = null;
}
    :
        OP_COLON
        ( KW_THIS { type = ThisOrBase.This; }
        | KW_BASE { type = ThisOrBase.Base; }
        )
        al=argumentList
        { value = new ConstructorInitializerSyntax(type.Value, al, Span(start)); }
    ;

// Destructor declaration

destructorDeclaration returns [DestructorDeclarationSyntax value]
@init {
    var start = input.LT(1);
    Debug.Assert($declarationPrefix::modifiers.Count == 0);
}
    :
        OP_TILDE
        idn=identifierName
        OP_PAREN_OPEN
        OP_PAREN_CLOSE
        b=block
        {
            value = new DestructorDeclarationSyntax(
                $declarationPrefix::attributes,
                ImmutableArray<Modifier>.Empty,
                idn,
                ImmutableArray<ParameterSyntax>.Empty,
                b,
                Span(start)
            );
        }
    ;

// Conversion operator declaration

conversionOperatorDeclaration returns [ConversionOperatorDeclarationSyntax value]
@init {
    var start = input.LT(1);
    ImplicitOrExplicit? type = null;
}
    :
        ( KW_EXPLICIT { type = ImplicitOrExplicit.Explicit; }
        | KW_IMPLICIT { type = ImplicitOrExplicit.Implicit; }
        )
        KW_OPERATOR
        t=typeSyntax
        pl=parameterList
        b=block
        {
            value = new ConversionOperatorDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                type.Value,
                t,
                pl,
                b,
                Span(start)
            );
        }
    ;

// Operator declaration

operatorDeclaration returns [OperatorDeclarationSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_OPERATOR
        o=operator
        pl=parameterList
        b=block
        {
            value = new OperatorDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                $declarationPrefix::type,
                o,
                pl,
                b,
                Span(start)
            );
        }
    ;

operator returns [Operator value]
    : OP_AMPERSAND { value = Operator.Ampersand; }
    | OP_ASTERISK { value = Operator.Asterisk; }
    | OP_BAR { value = Operator.Bar; }
    | OP_CARET { value = Operator.Caret; }
    | OP_EQUALS_EQUALS { value = Operator.EqualsEquals; }
    | OP_EXCLAMATION { value = Operator.Exclamation; }
    | OP_EXCLAMATION_EQUALS { value = Operator.ExclamationEquals; }
    | KW_FALSE { value = Operator.False; }
    | OP_GREATER_THAN { value = Operator.GreaterThan; }
    | OP_GREATER_THAN_EQUALS { value = Operator.GreaterThanEquals; }
    | op_GREATER_THAN_GREATER_THAN { value = Operator.GreaterThanGreaterThan; }
    | OP_LESS_THAN { value = Operator.LessThan; }
    | OP_LESS_THAN_EQUALS { value = Operator.LessThanEquals; }
    | OP_LESS_THAN_LESS_THAN { value = Operator.LessThanLessThan; }
    | OP_MINUS { value = Operator.Minus; }
    | OP_MINUS_MINUS { value = Operator.MinusMinus; }
    | OP_PERCENT { value = Operator.Percent; }
    | OP_PLUS { value = Operator.Plus; }
    | OP_PLUS_PLUS { value = Operator.PlusPlus; }
    | OP_SLASH { value = Operator.Slash; }
    | OP_TILDE { value = Operator.Tilde; }
    | KW_TRUE { value = Operator.True; }
    ;

// Statements

block returns [BlockSyntax value]
@init {
    var start = input.LT(1);
    var statements = new ImmutableArray<StatementSyntax>.Builder();
}
    :
        OP_BRACE_OPEN
        (
            s=statement
            { statements.Add(s); }
        )*
        OP_BRACE_CLOSE
        { return new BlockSyntax(statements.Build(), Span(start)); }
    ;

statement returns [StatementSyntax value]
@init {
    var start = input.LT(1);
}
    : ( KW_READONLY? variableDeclarationHeadScan )=> e1=localDeclarationStatement { value = e1; }
    | e4=assertStatement { value = e4; }
    | e5=block { value = e5; }
    | e6=breakContinueStatement { value = e6; }
    | e7=doStatement { value = e7; }
    | e8=emptyStatement { value = e8; }
    | e9=expressionStatement { value = e9; }
    | e10=forEachStatement { value = e10; }
    | e11=forStatement { value = e11; }
    | e12=ifStatement { value = e12; }
    | e13=returnStatement { value = e13; }
    | e14=switchStatement { value = e14; }
    | e15=throwStatement { value = e15; }
    | e16=tryStatement { value = e16; }
    | e17=usingStatement { value = e17; }
    | e18=loopStatement { value = e18; }
    | e19=whileStatement { value = e19; }
    | e20=deleteStatement { value = e20; }
    ;

assertStatement returns [AssertStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_ASSERT
        eod=expressionOrDeclaration
        OP_SEMICOLON
        { value = new AssertStatementSyntax(eod, Span(start)); }
    ;

breakContinueStatement returns [StatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
    ( KW_BREAK { value = new BreakStatementSyntax(Span(start)); }
    | KW_CONTINUE { value = new ContinueStatementSyntax(Span(start)); }
    )
    OP_SEMICOLON
    ;

doStatement returns [DoStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_DO
        b=block
        KW_WHILE
        eod=expressionOrDeclaration
        OP_SEMICOLON
        { value = new DoStatementSyntax(eod, b, Span(start)); }
    ;

emptyStatement returns [EmptyStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        OP_SEMICOLON
        { value = new EmptyStatementSyntax(Span(start)); }
    ;

expressionStatement returns [ExpressionStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        e=expression
        OP_SEMICOLON
        { value = new ExpressionStatementSyntax(e, Span(start)); }
    ;

forEachStatement returns [ForEachStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_FOREACH
        t=typeSyntax
        idn=identifierName
        KW_IN
        e=expression
        b=block
        { value = new ForEachStatementSyntax(t, idn, e, b, Span(start)); }
    ;

forStatement returns [ForStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_FOR
        (
            ( variableDeclarationHeadScan )=>
            vd=variableDeclaration
        |
            el1=expressionList
        )?
        OP_SEMICOLON
        ( e=expression )?
        OP_SEMICOLON
        ( el2=expressionList )?
        b=block
        { value = new ForStatementSyntax(vd, el1, e, el2, b, Span(start)); }
    ;

expressionList returns [ImmutableArray<ExpressionSyntax> value]
@init {
    var expressions = new ImmutableArray<ExpressionSyntax>.Builder();
}
    :
        e=expression
        { expressions.Add(e); }
        (
            OP_COMMA
            e=expression
            { expressions.Add(e); }
        )*
        { value = expressions.Build(); }
    ;

expressionOrDeclarationList returns [ImmutableArray<ExpressionSyntax> value]
@init {
    var expressions = new ImmutableArray<ExpressionSyntax>.Builder();
}
    :
        eod=expressionOrDeclaration
        { expressions.Add(eod); }
        (
            OP_COMMA
            eod=expressionOrDeclaration
            { expressions.Add(eod); }
        )*
        { value = expressions.Build(); }
    ;

ifStatement returns [IfStatementSyntax value]
@init {
    var start = input.LT(1);
    var elses = new ImmutableArray<ElseClauseSyntax>.Builder();
}
    :
        KW_IF
        eod=expressionOrDeclaration
        b=block
        ( eic=elIfClause { elses.Add(eic); } )*
        ( ec=elseClause { elses.Add(ec); } )?
        { value = new IfStatementSyntax(eod, b, elses.Build(), Span(start)); }
    ;

elIfClause returns [ElseClauseSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_ELIF
        eod=expressionOrDeclaration
        b=block
        { value = new ElseClauseSyntax(ElIfOrElse.ElIf, eod, b, Span(start)); }
    ;

elseClause returns [ElseClauseSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_ELSE
        b=block
        { value = new ElseClauseSyntax(ElIfOrElse.Else, null, b, Span(start)); }
    ;

localDeclarationStatement returns [LocalDeclarationStatementSyntax value]
@init {
    var modifiers = new ImmutableArray<Modifier>.Builder();
    var start = input.LT(1);
}
    :
        (
            KW_READONLY
            { modifiers.Add(Modifier.Readonly); }
        )?
        vd=variableDeclaration
        OP_SEMICOLON
        { value = new LocalDeclarationStatementSyntax(modifiers.Build(), vd, Span(start)); }
    ;

variableDeclarationHeadScan : typeSyntax identifierName ;

variableDeclaration returns [VariableDeclarationSyntax value]
scope declarationPrefix;
    :
        t=typeSyntax
        { $declarationPrefix::type = t; }
        vdwt=variableDeclarationWithoutType
        { value = vdwt; }
    ;

variableDeclarationWithoutType returns [VariableDeclarationSyntax value]
@init {
    var start = input.LT(1);
    var declarators = new ImmutableArray<VariableDeclaratorSyntax>.Builder();
}
    :
        vd=variableDeclarator
        { declarators.Add(vd); }
        (
            OP_COMMA
            vd=variableDeclarator
            { declarators.Add(vd); }
        )*
        { value = new VariableDeclarationSyntax($declarationPrefix::type, declarators.Build(), Span(start)); }
    ;

variableDeclarator returns [VariableDeclaratorSyntax value]
@init {
    var start = input.LT(1);
}
    :
        idn=identifierName
        (
            OP_EQUALS
            e=expression
        )?
        { value = new VariableDeclaratorSyntax(idn, e, Span(start)); }
    ;

returnStatement returns [ReturnStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_RETURN
        // This doesn't make sense but there is no reason not to allow this.
        ( eod=expressionOrDeclaration )?
        OP_SEMICOLON
        { value = new ReturnStatementSyntax(eod, Span(start)); }
    ;

switchStatement returns [SwitchStatementSyntax value]
@init {
    var start = input.LT(1);
    var sections = new ImmutableArray<SwitchSectionSyntax>.Builder();
}
    :
        KW_SWITCH
        eod=expressionOrDeclaration
        OP_BRACE_OPEN
        (
            ss=switchSection
            { sections.Add(ss); }
        )*
        OP_BRACE_CLOSE
        { value = new SwitchStatementSyntax(eod, sections.Build(), Span(start)); }
    ;

switchSection returns [SwitchSectionSyntax value]
@init {
    var start = input.LT(1);
    CaseOrDefault? type = null;
}
    :
        (
            KW_DEFAULT
            { type = CaseOrDefault.Default; }
        |
            KW_CASE
            { type = CaseOrDefault.Case; }
            el=expressionList
        )
        b=block
        {
            value = new SwitchSectionSyntax(
                type.Value,
                el == null ? ImmutableArray<ExpressionSyntax>.Empty : el,
                b,
                Span(start)
            );
        }
    ;

throwStatement returns [ThrowStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_THROW
        // This doesn't make sense but there is no reason not to allow this.
        eod=expressionOrDeclaration?
        OP_SEMICOLON
        { value = new ThrowStatementSyntax(eod, Span(start)); }
    ;

deleteStatement returns [DeleteStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_DELETE
        // This doesn't make sense but there is no reason not to allow this.
        eod=expressionOrDeclaration
        OP_SEMICOLON
        { value = new DeleteStatementSyntax(eod, Span(start)); }
    ;

tryStatement returns [TryStatementSyntax value]
@init {
    var start = input.LT(1);
    var catches = new ImmutableArray<CatchClauseSyntax>.Builder();
}
    :
        KW_TRY
        b=block
        (
            cc=catchClause
            { catches.Add(cc); }
        )*
        ( fc=finallyClause )?
        { value = new TryStatementSyntax(b, catches.Build(), fc, Span(start)); }
    ;

catchClause returns [CatchClauseSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_CATCH
        (
            t=typeSyntax
            ( idn=identifierName )?
        )?
        b=block
        { value = new CatchClauseSyntax(t, idn, b, Span(start)); }
    ;

finallyClause returns [FinallyClauseSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_FINALLY
        b=block
        { value = new FinallyClauseSyntax(b, Span(start)); }
    ;

usingStatement returns [UsingStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_USING
        (
            ( variableDeclarationHeadScan )=>
            vd=variableDeclaration
        |
            e=expression
        )
        b=block
        { value = new UsingStatementSyntax(vd, e, b, Span(start)); }
    ;

loopStatement returns [LoopStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_LOOP
        b=block
        { value = new LoopStatementSyntax(b, Span(start)); }
    ;

whileStatement returns [WhileStatementSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_WHILE
        eod=expressionOrDeclaration
        b=block
        { value = new WhileStatementSyntax(eod, b, Span(start)); }
    ;

// Expressions

expressionOrDeclaration returns [ExpressionSyntax value]
    : ( variableDeclarationHeadScan )=> vde=variableDeclarationExpression { value = vde; }
    | e=expression { value = e; }
    ;

expression returns [ExpressionSyntax value]
    :
        ( lambdaExpressionHeadScan )=> e2=lambdaExpression
        { value = e2; }
    |
        ae=assignmentExpression
        { value = ae; }
    ;

assignmentExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        nae=nonAssignmentExpression
        { value = nae; }
        (
            ao=assignmentOperator
            eod=expressionOrDeclaration
            { value = new BinaryExpressionSyntax(ao, value, eod, Span(start)); }
        )?
    ;

nonAssignmentExpression returns [ExpressionSyntax value]
    :
        ce=conditionalExpression
        { value = ce; }
    ;

assignmentOperator returns [BinaryOperator value]
    : OP_EQUALS { value = BinaryOperator.Equals; }
    | OP_PLUS_EQUALS { value = BinaryOperator.PlusEquals; }
    | OP_MINUS_EQUALS { value = BinaryOperator.MinusEquals; }
    | OP_ASTERISK_EQUALS { value = BinaryOperator.AsteriskEquals; }
    | OP_SLASH_EQUALS { value = BinaryOperator.SlashEquals; }
    | OP_PERCENT_EQUALS { value = BinaryOperator.PercentEquals; }
    | OP_GREATER_THAN_GREATER_THAN_EQUALS { value = BinaryOperator.GreaterThanGreaterThanEquals; }
    | OP_LESS_THAN_LESS_THAN_EQUALS { value = BinaryOperator.LessThanLessThanEquals; }
    | OP_AMPERSAND_EQUALS { value = BinaryOperator.AmpersandEquals; }
    | OP_BAR_EQUALS { value = BinaryOperator.BarEquals; }
    | OP_CARET_EQUALS { value = BinaryOperator.CaretEquals; }
    ;

lambdaExpressionHeadScan : KW_ASYNC? lambdaParameterList OP_EQUALS_GREATER_THAN ; 

lambdaExpression returns [ExpressionSyntax value]
@init {
    var modifiers = new ImmutableArray<Modifier>.Builder();
    var start = input.LT(1);
}
    :
        (
            KW_ASYNC
            { modifiers.Add(Modifier.Async); }
        )?
        lpl=lambdaParameterList
        OP_EQUALS_GREATER_THAN
        (
            e=expression
            { value = new LambdaExpressionSyntax(modifiers.Build(), lpl, e, Span(start)); }
        |
            b=block
            { value = new LambdaExpressionSyntax(modifiers.Build(), lpl, b, Span(start)); }
        )
    ;

lambdaParameterList returns [ImmutableArray<ParameterSyntax> value]
    :
        OP_PAREN_OPEN
        (
            elpl=explicitLambdaParameterList
            { value = elpl; }
        |
            ilpl=implicitLambdaParameterList
            { value = ilpl; }
        |
            { value = ImmutableArray<ParameterSyntax>.Empty; }
        )
        OP_PAREN_CLOSE
    |
        ilp=implicitLambdaParameter
        { value = ImmutableArray<ParameterSyntax>.Create(ilp); }
    ;

explicitLambdaParameterList returns [ImmutableArray<ParameterSyntax> value]
@init {
    var builder = new ImmutableArray<ParameterSyntax>.Builder();
}
    :
        elp=explicitLambdaParameter
        { builder.Add(elp); }
        (
            OP_COMMA
            elp=explicitLambdaParameter
            { builder.Add(elp); }
        )*
        { value = builder.Build(); }
    ;

explicitLambdaParameter returns [ParameterSyntax value]
@init {
    var start = input.LT(1);
    ParameterModifier? modifier = null;
}
    :
        ( am=argumentModifier { modifier = am; } )? t=typeSyntax idn=identifierName
        {
            value = new ParameterSyntax(
                ImmutableArray<AttributeListSyntax>.Empty,
                modifier != null ? ImmutableArray<ParameterModifier>.Create(modifier.Value) : ImmutableArray<ParameterModifier>.Empty,
                t,
                idn,
                Span(start)
            );
        }
    ;

implicitLambdaParameterList returns [ImmutableArray<ParameterSyntax> value]
@init {
    var builder = new ImmutableArray<ParameterSyntax>.Builder();
}
    :
        ilp=implicitLambdaParameter
        { builder.Add(ilp); }
        (
            OP_COMMA
            ilp=implicitLambdaParameter
            { builder.Add(ilp); }
        )*
        { value = builder.Build(); }
    ;

implicitLambdaParameter returns [ParameterSyntax value]
@init {
    var start = input.LT(1);
}
    :
        idn=identifierName
        {
            value = new ParameterSyntax(
                ImmutableArray<AttributeListSyntax>.Empty,
                ImmutableArray<ParameterModifier>.Empty,
                null,
                idn,
                Span(start)
            );
        }
    ;

conditionalExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        ce=coalescingExpression
        { value = ce; }
        (
            OP_QUESTION e=expression
            OP_COLON ce=conditionalExpression
            { value = new ConditionalExpressionSyntax(value, e, ce, Span(start)); }
        )?
    ;

coalescingExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        loe=logicalOrExpression
        { value = loe; }
        (
            OP_QUESTION_QUESTION
            loe=logicalOrExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.QuestionQuestion, value, loe, Span(start)); }
        )?
    ;

logicalOrExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        lae=logicalAndExpression
        { value = lae; }
        (
            OP_BAR_BAR lae=logicalAndExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.BarBar, value, lae, Span(start)); }
        )*
    ;

logicalAndExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        boe=bitwiseOrExpression
        { value = boe; }
        (
            OP_AMPERSAND_AMPERSAND boe=bitwiseOrExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.AmpersandAmpersand, value, boe, Span(start)); }
        )*
    ;

bitwiseOrExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        bxe=bitwiseXorExpression
        { value = bxe; }
        (
            OP_BAR bxe=bitwiseXorExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.Bar, value, bxe, Span(start)); }
        )*
    ;

bitwiseXorExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        bae=bitwiseAndExpression
        { value = bae; }
        (
            OP_CARET bae=bitwiseAndExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.Caret, value, bae, Span(start)); }
        )*
    ;

bitwiseAndExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        ee=equalityExpression
        { value = ee; }
        (
            OP_AMPERSAND ee=equalityExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.Ampersand, value, ee, Span(start)); }
        )*
    ;

equalityExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        rex=relationalExpression
        { value = rex; }
        (
            eo=equalityOperator rex=relationalExpression
            { value = new BinaryExpressionSyntax(eo, value, rex, Span(start)); }
        )*
    ;

equalityOperator returns [BinaryOperator value]
    : OP_EQUALS_EQUALS { value = BinaryOperator.EqualsEquals; }
    | OP_EXCLAMATION_EQUALS { value = BinaryOperator.ExclamationEquals; }
    ;

relationalExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        se=shiftExpression
        { value = se; }
        (
            (
                ro=relationalOperator se=shiftExpression
                { value = new BinaryExpressionSyntax(ro, value, se, Span(start)); }
            |
                iao=isAsOperator n=name
                { value = new BinaryExpressionSyntax(iao, value, n, Span(start)); }
            )
        )*
    ;

relationalOperator returns [BinaryOperator value]
    : OP_GREATER_THAN { value = BinaryOperator.GreaterThan; }
    | OP_GREATER_THAN_EQUALS { value = BinaryOperator.GreaterThanEquals; }
    | OP_LESS_THAN { value = BinaryOperator.LessThan; }
    | OP_LESS_THAN_EQUALS { value = BinaryOperator.LessThanEquals; }
    ;

isAsOperator returns [BinaryOperator value]
    : KW_IS { value = BinaryOperator.Is; }
    | KW_AS { value = BinaryOperator.As; }
    ;

shiftExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        ae=additiveExpression
        { value = ae; }
        (
            so=shiftOperator ae=additiveExpression
            { value = new BinaryExpressionSyntax(so, value, ae, Span(start)); }
        )*
    ;

shiftOperator returns [BinaryOperator value]
    : op_GREATER_THAN_GREATER_THAN { value = BinaryOperator.GreaterThanGreaterThan; }
    | OP_LESS_THAN_LESS_THAN { value = BinaryOperator.LessThanLessThan; }
    ;

additiveExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        me=multiplicativeExpression
        { value = me; }
        (
            ao=additiveOperator me=multiplicativeExpression
            { value = new BinaryExpressionSyntax(ao, value, me, Span(start)); }
        )*
    ;

additiveOperator returns [BinaryOperator value]
    : OP_PLUS { value = BinaryOperator.Plus; }
    | OP_MINUS { value = BinaryOperator.Minus; }
    ;

multiplicativeExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        eu=unaryExpression
        { value = eu; }
        (
            mo=multiplicativeOperator ue=unaryExpression
            { value = new BinaryExpressionSyntax(mo, value, ue, Span(start)); }
        )*
    ;

multiplicativeOperator returns [BinaryOperator value]
    : OP_ASTERISK { value = BinaryOperator.Asterisk; }
    | OP_SLASH { value = BinaryOperator.Slash; }
    | OP_PERCENT { value = BinaryOperator.Percent; }
    ;

unaryExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        puo1=prefixUnaryOperator ue=unaryExpression
        { value = new PrefixUnaryExpressionSyntax(puo1, ue, Span(start)); }
    |
        ( OP_PAREN_OPEN castType OP_PAREN_CLOSE expression )=>
        ce=castExpression
        { value = ce; }
    |
        ae=awaitExpression
        { value = ae; }
    |
        p=primaryExpression
        { value = p; }
        (
            s=selector
            { value = s.Build(value); }
        )*
        (
            puo2=postfixUnaryOperator
            { value = new PostfixUnaryExpressionSyntax(puo2, value, Span(start)); }
        )?
    ;

awaitExpression returns [AwaitExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        KW_AWAIT
        ue=unaryExpression
        { value = new AwaitExpressionSyntax(ue, Span(start)); }
    ;

prefixUnaryOperator returns [PrefixUnaryOperator value]
    : OP_AMPERSAND { value = PrefixUnaryOperator.Ampersand; }
    | OP_EXCLAMATION { value = PrefixUnaryOperator.Exclamation; }
    | OP_MINUS { value = PrefixUnaryOperator.Minus; }
    | OP_MINUS_MINUS { value = PrefixUnaryOperator.MinusMinus; }
    | OP_PLUS { value = PrefixUnaryOperator.Plus; }
    | OP_PLUS_PLUS { value = PrefixUnaryOperator.PlusPlus; }
    | OP_TILDE { value = PrefixUnaryOperator.Tilde; }
    ;

postfixUnaryOperator returns [PostfixUnaryOperator value]
    : OP_MINUS_MINUS { value = PostfixUnaryOperator.MinusMinus; }
    | OP_PLUS_PLUS { value = PostfixUnaryOperator.PlusPlus; }
    ;

castExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        OP_PAREN_OPEN ct=castType OP_PAREN_CLOSE
        ue=unaryExpression
        { value = new CastExpressionSyntax(ue, ct, Span(start)); }
    ;

argument returns [ArgumentSyntax value]
@init {
    var start = input.LT(1);
    var modifiers = new ImmutableArray<ParameterModifier>.Builder();
}
    :
        (
            pm=argumentModifier
            { modifiers.Add(pm); }
        )*
        eod=expressionOrDeclaration
        { value = new ArgumentSyntax(modifiers.Build(), eod, Span(start)); }
    ;

primaryExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    : KW_THIS { value = new InstanceExpressionSyntax(ThisOrBase.This, Span(start)); }
    | KW_BASE { value = new InstanceExpressionSyntax(ThisOrBase.Base, Span(start)); }
    | e2=literal { value = e2; }
    | OP_PAREN_OPEN e4=expressionOrDeclaration OP_PAREN_CLOSE { value = new ParenthesizedExpressionSyntax(e4, Span(start)); }
    | KW_TYPEOF OP_PAREN_OPEN e5=typeSyntax OP_PAREN_CLOSE { value = new TypeOfExpressionSyntax(e5, Span(start)); }
    | KW_SIZEOF OP_PAREN_OPEN e6=typeSyntax OP_PAREN_CLOSE { value = new SizeOfExpressionSyntax(e6, Span(start)); }
    | KW_DEFAULT OP_PAREN_OPEN e7=typeSyntax OP_PAREN_CLOSE { value = new DefaultExpressionSyntax(e7, Span(start)); }
    | e12=identifierName { value = e12; }
    | e13=primaryNewExpression { value = e13; }
    ;

primaryNewExpression returns [ExpressionSyntax value]
scope {
    IToken start;
    TypeSyntax type;
}
@init {
    var start = input.LT(1);
    $primaryNewExpression::start = start;
}
    :
        KW_NEW
        (
            t=typeSyntax
            { $primaryNewExpression::type = t; }
            (
                oce=objectCreationExpression
                { value = oce; }
            |
                ooci=objectOrCollectionInitializer
                {
                    if (t is ArrayTypeSyntax) {
                        value = new ArrayCreationExpressionSyntax(
                            (ArrayTypeSyntax)t,
                            ooci,
                            Span(start)
                        );
                    } else {
                        value = new ObjectCreationExpressionSyntax(
                            t,
                            null,
                            ooci,
                            Span(start)
                        );
                    }
                }
            |
                ace=arrayCreationExpression
                { value = ace; }
            )
        |
            aoi=anonymousObjectInitializer
            { value = aoi; }
        |
            rs=rankSpecifier
            ai=arrayInitializer
            {
                value = new ImplicitArrayCreationExpressionSyntax(
                    ImmutableArray<ArrayRankSpecifierSyntax>.Create(rs),
                    ai,
                    Span(start)
                );
            }
        )
    ;

arrayCreationExpression returns [ExpressionSyntax value]
@init {
    var builder = new ImmutableArray<ArrayRankSpecifierSyntax>.Builder();
}
    :
        obo=OP_BRACKET_OPEN eod=expressionOrDeclaration OP_BRACKET_CLOSE
        { builder.Add(new ArrayRankSpecifierSyntax(eod, Span(obo))); }
        (
            rsl=rankSpecifierList
            { builder.AddRange(rsl); }
        )?
        (
            ( OP_BRACE_OPEN )=>
            ai=arrayInitializer
        )?
        {
            value = new ArrayCreationExpressionSyntax(
                new ArrayTypeSyntax(
                    $primaryNewExpression::type,
                    builder.Build(),
                    Span($primaryNewExpression::start)
                ),
                ai,
                Span($primaryNewExpression::start)
            );
        }
    ;

objectCreationExpression returns [ExpressionSyntax value]
    :
        al=argumentList
        (
            ( OP_BRACE_OPEN )=>
            ooci=objectOrCollectionInitializer
        )?
        {
            value = new ObjectCreationExpressionSyntax(
                $primaryNewExpression::type,
                al,
                ooci,
                Span($primaryNewExpression::start)
            );
        }
    ;

arrayInitializer returns [InitializerExpressionSyntax value]
@init {
    var start = input.LT(1);
    var builder = new ImmutableArray<ExpressionSyntax>.Builder();
}
    :
        OP_BRACE_OPEN
        (
            vi=variableInitializer
            { builder.Add(vi); }
            (
                OP_COMMA
                vi=variableInitializer
                { builder.Add(vi); }
            )*
            OP_COMMA?
        )?
        OP_BRACE_CLOSE
        {
            value = new InitializerExpressionSyntax(
                builder.Build(),
                Span(start)
            );
        }
    ;

variableInitializer returns [ExpressionSyntax value]
    : e1=expressionOrDeclaration { value = e1; }
    | e2=arrayInitializer { value = e2; }
    ;

anonymousObjectInitializer returns [AnonymousObjectCreationExpressionSyntax value]
@init {
    var builder = new ImmutableArray<AnonymousObjectMemberDeclaratorSyntax>.Builder();
}
    :
        OP_BRACE_OPEN
        (
            md=memberDeclarator
            { builder.Add(md); }
            (
                OP_COMMA
                md=memberDeclarator
                { builder.Add(md); }
            )*
            OP_COMMA?
        )?
        OP_BRACE_CLOSE
        {
            value = new AnonymousObjectCreationExpressionSyntax(
                builder.Build(),
                Span($primaryNewExpression::start)
            );
        }
    ;

memberDeclarator returns [AnonymousObjectMemberDeclaratorSyntax value]
@init {
    var start = input.LT(1);
}
    :
        pe=primaryExpression
        {
            value = new AnonymousObjectMemberDeclaratorSyntax(
                null,
                pe,
                Span(start)
            );
        }
    |
        idn=identifierName OP_EQUALS eod=expressionOrDeclaration
        {
            value = new AnonymousObjectMemberDeclaratorSyntax(
                idn,
                eod,
                Span(start)
            );
        }
    ;

objectOrCollectionInitializer returns [InitializerExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        OP_BRACE_OPEN OP_BRACE_CLOSE
        {
            value = new InitializerExpressionSyntax(
                ImmutableArray<ExpressionSyntax>.Empty,
                Span(start)
            );
        }
    |
        oi=objectInitializer
        { value = oi; }
    |
        ci=collectionInitializer
        { value = ci; }
    ;

objectInitializer returns [InitializerExpressionSyntax value]
@init {
    var start = input.LT(1);
    var builder = new ImmutableArray<ExpressionSyntax>.Builder();
}
    :
        OP_BRACE_OPEN
        mi=memberInitializer
        { builder.Add(mi); }
        (
            OP_COMMA
            mi=memberInitializer
            { builder.Add(mi); }
        )*
        OP_COMMA?
        OP_BRACE_CLOSE
        {
            value = new InitializerExpressionSyntax(
                builder.Build(),
                Span(start)
            );
        }
    ;

memberInitializer returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
    BinaryOperator? operator_ = null;
}
    :
        idn=identifierName
        (
            OP_EQUALS
            { operator_ = BinaryOperator.Equals; }
        |
            OP_PLUS_EQUALS
            { operator_ = BinaryOperator.PlusEquals; }
        )
        miv=memberInitializerValue
        { value = new BinaryExpressionSyntax(operator_.Value, idn, miv, Span(start)); }
    ;

memberInitializerValue returns [ExpressionSyntax value]
    : e1=expressionOrDeclaration { value = e1; }
    | e2=objectOrCollectionInitializer { value = e2; }
    ;

collectionInitializer returns [InitializerExpressionSyntax value]
@init {
    var start = input.LT(1);
    var builder = new ImmutableArray<ExpressionSyntax>.Builder();
}
    :
        OP_BRACE_OPEN
        ei=elementInitializer
        { builder.Add(ei); }
        (
            OP_COMMA
            ei=elementInitializer
            { builder.Add(ei); }
        )*
        OP_COMMA?
        OP_BRACE_CLOSE
        {
            value = new InitializerExpressionSyntax(
                builder.Build(),
                Span(start)
            );
        }
    ;

elementInitializer returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        nae=nonAssignmentExpression
        { value = nae; }
    |
        OP_BRACE_OPEN eodl=expressionOrDeclarationList OP_BRACE_CLOSE
        {
            value = new InitializerExpressionSyntax(
                eodl,
                Span(start)
            );
        }
    ;

variableDeclarationExpression returns [ExpressionSyntax value]
@init {
    var start = input.LT(1);
}
    :
        t=typeSyntax
        idn=identifierName
        { value = new VariableDeclarationExpressionSyntax(t, idn, Span(start)); }
    ;

selector returns [ISelector value]
@init {
    var start = input.LT(1);
}
    : OP_DOT sn=simpleName { return new MemberAccessSelector(sn, Span(start)); }
    | al=argumentList { return new ArgumentListSelector(al, Span(start)); }
    | OP_BRACKET_OPEN eodl=expressionOrDeclarationList OP_BRACKET_CLOSE { return new IndexSelector(eodl, Span(start)); }
    ;

argumentList returns [ImmutableArray<ArgumentSyntax> value]
@init {
    var arguments = new ImmutableArray<ArgumentSyntax>.Builder();
}
    :
        OP_PAREN_OPEN
        (
            a=argument
            { arguments.Add(a); }
            (
                OP_COMMA
                a=argument
                { arguments.Add(a); }
            )*
        )?
        OP_PAREN_CLOSE
        { value = arguments.Build(); }
    ;

literal returns [LiteralExpressionSyntax value]
@init {
    var start = input.LT(1);
    LiteralType? type = null;
    string text = null;
}
@after {
    value = new LiteralExpressionSyntax(type.Value, text, Span(start));
}
    : KW_TRUE { type = LiteralType.True; }
    | KW_FALSE { type = LiteralType.False; }
    | KW_NIL { type = LiteralType.Nil; }
    | hi=HEX_INTEGER { text = hi.Text; type = LiteralType.Hex; }
    | i=INTEGER { text = i.Text; type = LiteralType.Integer; }
    | f=FLOAT { text = f.Text; type = LiteralType.Float; }
    | c=CHAR { text = c.Text; type = LiteralType.Char; }
    | s=STRING { text = s.Text; type = LiteralType.String; }
    ;

// Type name parsing

// Type name parsing is narly. The primary problem is that member names can
// have an explicit interface declaration, which confuses the hell out of the
// grammar. This would be fixable, if the type parameters wouldn't allow
// attributes. Because they do, we can't use normal name parsing and fix it
// afterwards. Instead, we parse into a temporary tree here and convert it
// appropriately.

// These are the entry points. These are called by the rest of the grammar and
// expect syntax nodes. Sometings we do here directly; other things are
// delegated to the name parsing system.

// Identifier name is duplicated. The reason for this is that identifierName
// is used a lot, and there is no use in going through the name parsing system
// for the usual case.

identifierName returns [IdentifierNameSyntax value]
@init {
    var start = input.LT(1);
}
    :
        i=IDENTIFIER
        { value = new IdentifierNameSyntax(i.Text, Span(start)); }
    ;

// castType is not used in the name parsing system and just delegates to type.

castType returns [TypeSyntax value]
@init {
    var start = input.LT(1);
}
    :
        n=nullable
        { value = new NakedNullableTypeSyntax(n, Span(start)); }
    |
        t=typeSyntax
        { value = t; }
    ;

// These are the rest of the entry points. These delegate to the np__ rules.

name returns [NameSyntax value]
    :
        qn=np__qualifiedName
        { value = qn.ToName(); }
    ;

typeSyntax returns [TypeSyntax value]
@init {
    var start = input.LT(1);
}
    :
        // We don't have to parse 'var' in np__type because it's only valid when
        // it stands on itself.
        KW_VAR
        { value = new VarTypeSyntax(Span(start)); }
    |
        t=np__type
        { value = t.ToType(); }
    ;

memberName returns [MemberName value]
    :
        n=np__qualifiedName
        { value = n.ToMemberName(); }
    ;

simpleName returns [SimpleNameSyntax value]
    :
        sn=np__simpleName
        { value = sn.ToSimpleName(); }
    ;

// Type parsing

// This is the type parsing system. This parses into the TypeParser classes.
// Through the entry point rules, the correct type is returned.

np__type returns [TypeParser value]
@init {
    var start = input.LT(1);
    var builder = new ImmutableArray<ArrayRankSpecifierSyntax>.Builder();
}
    :
        bt=np__baseType
        { value = bt; }
        (
            ( OP_QUESTION )=>
            OP_QUESTION
            { value = new NullableTypeParser(value, Span(start)); }
        )?
        (
            ( rankSpecifier )=>
            rsl=rankSpecifierList
            { value = new ArrayTypeParser(value, rsl, Span(start)); }
        )?
        (
            OP_CARET
            { value = new TrackedTypeParser(value, Span(start)); }
        )?
    ;

rankSpecifierList returns [ImmutableArray<ArrayRankSpecifierSyntax> value]
@init {
    var builder = new ImmutableArray<ArrayRankSpecifierSyntax>.Builder();
}
    :
        (
            ( rankSpecifier )=>
            rs=rankSpecifier
            { builder.Add(rs); }
        )+
        { value = builder.Build(); }
    ;

rankSpecifier returns [ArrayRankSpecifierSyntax value]
@init {
    var start = input.LT(1);
}
    :
        OP_BRACKET_OPEN
        OP_BRACKET_CLOSE
        { value = new ArrayRankSpecifierSyntax(new OmittedArraySizeExpressionSyntax(Span(start)), Span(start)); }
    ;

np__baseType returns [TypeParser value]
@init {
    var start = input.LT(1);
}
    :
        pt=np__predefinedType
        { value = new PredefinedTypeParser(pt, Span(start)); }
    |
        qn=np__qualifiedName
        { value = qn; }
    ;

np__qualifiedName returns [NameParser value]
@init {
    var start = input.LT(1);
    IdentifierNameParser alias = null;
}
    :
        (
            idn=np__identifierName OP_COLON_COLON
            { alias = idn; }
        )?
        sn=np__simpleName
        {
            if (alias != null) {
                value = new AliasQualifiedNameParser(alias, sn, Span(start));
            } else {
                value = sn;
            }
        }
        (
            OP_DOT
            sn=np__simpleName
            { value = new QualifiedNameParser(value, sn, Span(start)); }
        )*
    ;

np__simpleName returns [SimpleNameParser value]
@init {
    var start = input.LT(1);
}
    :
        idn=np__identifierName
        { value = idn; }
        (
            ( OP_LESS_THAN )=>
            gta=np__genericTypeArguments
            { value = new GenericNameParser(Errors, idn.Identifier, gta, Span(start)); }
        )?
    ;

np__genericTypeArguments returns [ImmutableArray<TypeParser> value]
@init {
    var builder = new ImmutableArray<TypeParser>.Builder();
}
    :
        OP_LESS_THAN
        gta=np__genericTypeArgument
        { builder.Add(gta); }
        (
            OP_COMMA
            gta=np__genericTypeArgument
            { builder.Add(gta); }
        )*
        // This is a bit narly. What this is trying to accomplish is to
        // disambiguate between >> as right shift and >> as two generic type
        // close signs. We count the number of open braces we had, and allow
        // that number of more to be closed here. Then, if we're closing one here
        // that belongs to a recursive call, the getPendingGenericCloses() will
        // be too low, and we'll match the empty branch.
        op_GREATER_THAN_ANY
        { value = builder.Build(); }
    ;

np__genericTypeArgument returns [TypeParser value]
@init {
    var start = input.LT(1);
}
    :
        (
            all=attributeListList
            tpv=typeParameterVariance
            t=np__type
            { value = new TypeParameterParser(Errors, all, tpv, t, Span(start)); }
        |
            { value = new OmittedTypeArgumentParser(Span(start)); }
        )
    ;

np__identifierName returns [IdentifierNameParser value]
@init {
    var start = input.LT(1);
}
    :
        i=IDENTIFIER
        { value = new IdentifierNameParser(i.Text, Span(start)); }
    ;

np__predefinedType returns [PredefinedType value]
    : KW_BOOL { value = PredefinedType.Bool; }
    | KW_BYTE { value = PredefinedType.Byte; }
    | KW_CHAR { value = PredefinedType.Char; }
    | KW_DECIMAL { value = PredefinedType.Decimal; }
    | KW_DOUBLE { value = PredefinedType.Double; }
    | KW_FLOAT { value = PredefinedType.Float; }
    | KW_INT { value = PredefinedType.Int; }
    | KW_LONG { value = PredefinedType.Long; }
    | KW_OBJECT { value = PredefinedType.Object; }
    | KW_SBYTE { value = PredefinedType.SByte; }
    | KW_SHORT { value = PredefinedType.Short; }
    | KW_STRING { value = PredefinedType.String; }
    | KW_UINT { value = PredefinedType.UInt; }
    | KW_ULONG { value = PredefinedType.ULong; }
    | KW_USHORT { value = PredefinedType.UShort; }
    | KW_VOID { value = PredefinedType.Void; }
    ;

////////////////////////////////////////////////////////////////////////////////
// LEXER                                                                      //
////////////////////////////////////////////////////////////////////////////////

// Attribute target specifiers do not appear in this list because this would
// introduce too many keywords. Instead they are an identifier token, which is
// verified.

KW_ABSTRACT : 'abstract' ;
KW_AS : 'as' ;
KW_ASSERT : 'assert' ;
KW_ASYNC : 'async' ;
KW_AWAIT : 'await' ;
KW_BASE : 'base' ;
KW_BOOL : 'bool' ;
KW_BREAK : 'break' ;
KW_BYTE : 'byte' ;
KW_CASE : 'case' ;
KW_CATCH : 'catch' ;
KW_CHAR : 'char' ;
KW_CLASS : 'class' ;
KW_CONSUMES : 'consumes' ;
KW_CONTINUE : 'continue' ;
KW_DECIMAL : 'decimal' ;
KW_DEFAULT : 'default' ;
KW_DELEGATE : 'delegate' ;
KW_DELETE : 'delete' ;
KW_DO : 'do' ;
KW_DOUBLE : 'double' ;
KW_ELIF : 'elif' ;
KW_ELSE : 'else' ;
KW_ENUM : 'enum' ;
KW_EVENT : 'event' ;
KW_EXPLICIT : 'explicit' ;
KW_EXTERN : 'extern' ;
KW_FALSE : 'false' ;
KW_FINALLY : 'finally' ;
KW_FLOAT : 'float' ;
KW_FOR : 'for' ;
KW_FOREACH : 'foreach' ;
KW_IF : 'if' ;
KW_IMPLICIT : 'implicit' ;
KW_IMPORT : 'import' ;
KW_IN : 'in' ;
KW_INT : 'int' ;
KW_INTERFACE : 'interface' ;
KW_INTERNAL : 'internal' ;
KW_IS : 'is' ;
KW_LONG : 'long' ;
KW_LOOP : 'loop' ;
KW_NAMESPACE : 'namespace' ;
KW_NEW : 'new' ;
KW_NIL : 'nil' ;
KW_OBJECT : 'object' ;
KW_OPERATOR : 'operator' ;
KW_OUT : 'out' ;
KW_OVERRIDE : 'override' ;
KW_PARAMS : 'params' ;
KW_PARTIAL : 'partial' ;
KW_PRIVATE : 'private' ;
KW_PROTECTED : 'protected' ;
KW_PUBLIC : 'public' ;
KW_READONLY : 'readonly' ;
KW_REF : 'ref' ;
KW_RETURN : 'return' ;
KW_SBYTE : 'sbyte' ;
KW_SEALED : 'sealed' ;
KW_SHORT : 'short' ;
KW_SIZEOF : 'sizeof' ;
KW_STATIC : 'static' ;
KW_STRING : 'string' ;
KW_STRUCT : 'struct' ;
KW_SWITCH : 'switch' ;
KW_THIS : 'this' ;
KW_THROW : 'throw' ;
KW_TRUE : 'true' ;
KW_TRY : 'try' ;
KW_TYPEOF : 'typeof' ;
KW_UINT : 'uint' ;
KW_ULONG : 'ulong' ;
KW_USHORT : 'ushort' ;
KW_USING : 'using' ;
KW_VAR : 'var' ;
KW_VIRTUAL : 'virtual' ;
KW_VOID : 'void' ;
KW_VOLATILE : 'volatile' ;
KW_WHERE : 'where' ;
KW_WHILE : 'while' ;

OP_AMPERSAND : '&' ;
OP_AMPERSAND_AMPERSAND : '&&' ;
OP_AMPERSAND_EQUALS : '&=' ;
OP_ASTERISK : '*' ;
OP_ASTERISK_EQUALS : '*=' ;
OP_BAR : '|' ;
OP_BAR_BAR : '||' ;
OP_BAR_EQUALS : '|=' ;
OP_BRACE_CLOSE : '}' ;
OP_BRACE_OPEN : '{' ;
OP_BRACKET_CLOSE : ']' ;
OP_BRACKET_OPEN : '[' ;
OP_CARET : '^' ;
OP_CARET_EQUALS : '^=' ;
OP_COLON : ':' ;
OP_COLON_COLON : '::' ;
OP_COMMA : ',' ;
OP_DOT : '.' ;
OP_EQUALS : '=' ;
OP_EQUALS_EQUALS : '==' ;
OP_EQUALS_GREATER_THAN : '=>' ;
OP_EXCLAMATION : '!' ;
OP_EXCLAMATION_EQUALS : '!=' ;
OP_GREATER_THAN : '>' ;
OP_GREATER_THAN_EQUALS : '>=' ;
OP_GREATER_THAN_GREATER_THAN : t='>>' { EmitGreaterThanGreaterThan(t); } ;
OP_GREATER_THAN_GREATER_THAN_EQUALS : '>>=' ;
OP_LESS_THAN : '<' ;
OP_LESS_THAN_EQUALS : '<=' ;
OP_LESS_THAN_LESS_THAN : '<<' ;
OP_LESS_THAN_LESS_THAN_EQUALS : '<<=' ;
OP_MINUS : '-' ;
OP_MINUS_EQUALS : '-=' ;
OP_MINUS_MINUS : '--' ;
OP_PAREN_CLOSE : ')' ;
OP_PAREN_OPEN : '(' ;
OP_PERCENT : '%' ;
OP_PERCENT_EQUALS : '%=' ;
OP_PLUS : '+' ;
OP_PLUS_EQUALS : '+=' ;
OP_PLUS_PLUS : '++' ;
OP_QUESTION : '?' ;
OP_QUESTION_QUESTION : '??' ;
OP_SEMICOLON : ';' ;
OP_SLASH : '/' ;
OP_SLASH_EQUALS : '/=' ;
OP_TILDE : '~' ;

// The OP_GREATER_THAN_GREATER_THAN is split into two tokens. The reason for
// doing this is that this very easily allows us to correctly disambiguate
// >> for generic parameters. When the OP_GREATER_THAN_GREATER_THAN token is
// needed, op_GREATER_THAN_GREATER_THAN (lower case o; parser rule) is used,
// and when either > or >> is allowed, op_GREATER_THAN_ANY is used (again,
// parser rule).

fragment OP_GREATER_THAN_GREATER_THAN_FIRST : ;
fragment OP_GREATER_THAN_GREATER_THAN_SECOND : ;
op_GREATER_THAN_GREATER_THAN : OP_GREATER_THAN_GREATER_THAN_FIRST OP_GREATER_THAN_GREATER_THAN_SECOND ;
op_GREATER_THAN_ANY : OP_GREATER_THAN | OP_GREATER_THAN_GREATER_THAN_FIRST | OP_GREATER_THAN_GREATER_THAN_SECOND ;

NEW_LINE
    :
    ( '\r' // CR
    | '\n' // NL
    | '\r\n' // CR NL
    | '\u0085' // New line
    | '\u2028' // Line separator
    | '\u2029' // Paragraph separator
    )
    { Skip(); }
    ;

fragment NEW_LINE_CHARACTER
    : '\r' // CR
    | '\n' // NL
    | '\u0085' // New line
    | '\u2028' // Line separator
    | '\u2029' // Paragraph separator
    ;

fragment INPUT_CHARACTER : ~NEW_LINE_CHARACTER ;

LINE_COMMENT
    :
        '//' INPUT_CHARACTER*
        { Skip(); }
    ;

BLOCK_COMMENT
@init {
    bool docComment = false;
}
    :
        '/*'
        { docComment = (char)input.LT(1) == '*'; }
        ( options { greedy=false; } : . )*
        '*/'
        {
            if (docComment) {
                $channel = Hidden;
            } else {
                Skip();
            }
        }
    ;

WHITESPACE
    :
    ( ' '
    | '\t'
    | '\v'
    | '\f'
    )
    { Skip(); }
    ;

fragment DIGIT : '0'..'9' ;
fragment HEX_DIGIT : DIGIT | 'a'..'f' | 'A' .. 'F' ;
fragment IDENTIFIER_FIRST_CHARACTER : 'a'..'z' | 'A'..'Z' | '_' ;
fragment IDENTIFIER_CHARACTER : IDENTIFIER_FIRST_CHARACTER | DIGIT ;

IDENTIFIER : '@'? IDENTIFIER_FIRST_CHARACTER IDENTIFIER_CHARACTER* ;

fragment INTEGER_SUFFIX
    : ( 'u' | 'U' ) ( 'l' | 'L' )?
    | 'l' | 'L'
    ;

fragment INTEGER : /* Handled by FLOAT */ ;
HEX_INTEGER : '0' ( 'x' | 'X' ) HEX_DIGIT+ INTEGER_SUFFIX? ;

fragment EXPONENT : ( 'e' | 'E' ) ( '+' | '-' )? DIGIT+ ;
fragment FLOAT_SUFFIX
    : 'f' | 'F'
    | 'd' | 'D'
    | 'm' | 'M'
    ; 

FLOAT
    :
        DIGIT+
        (
            ( '.' DIGIT ) => ( '.' DIGIT+ )? EXPONENT? FLOAT_SUFFIX?
        |
            FLOAT_SUFFIX
        |
            { $type = INTEGER; }
        )
    |
        '.' DIGIT+ EXPONENT? FLOAT_SUFFIX?
    ;

// Here we specifically don't parse escapes. We want the lexer to succeed even when
// the strings are of an invalid format. Later on, we'll show nice error messages
// when the strings couldn't be parsed.

fragment LITERAL_STRING
    : '"' ( '\\' . | ~( '"' | '\\' | NEW_LINE_CHARACTER ) )* '"'
    ;

fragment VERBATIM_STRING
    : '@"' ( '""' | ~'"' ) '"'
    ;

STRING : LITERAL_STRING | VERBATIM_STRING ;

CHAR
    : '\'' ( '\\' . | ~( '\'' | '\\' ) )* '\''
    ;
