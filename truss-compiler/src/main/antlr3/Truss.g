grammar Truss;

options {
    backtrack = true;
}

scope declarationPrefix {
    ImmutableArray<AttributeListSyntax> attributes;
    ImmutableArray<Modifier> modifiers;
    TypeSyntax type;
}

@header {
    package truss.compiler.parser;
    
    import truss.compiler.*;
    import truss.compiler.syntax.*;
    import truss.compiler.support.*;
}

@lexer::header {
    package truss.compiler.parser;
    
    import truss.compiler.*;
    import truss.compiler.syntax.*;
    import truss.compiler.support.*;
}

////////////////////////////////////////////////////////////////////////////////
// PARSER                                                                     //
////////////////////////////////////////////////////////////////////////////////

compilationUnit returns [CompilationUnitSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<AttributeListSyntax> attributeLists = new ImmutableArray.Builder<>();
    ImmutableArray.Builder<ImportDirectiveSyntax> imports = new ImmutableArray.Builder<>();
    ImmutableArray.Builder<MemberDeclarationSyntax> members = new ImmutableArray.Builder<>();
}
    :
        (
            // Only assembly attributes belong to this scope; the rest is
            // attached to the member it belongs to.
            (
                OP_BRACKET_OPEN
                in=identifierName
                { "assembly".equals(in.getIdentifier()) }?=>
                OP_COLON
            )=>
            al=attributeList { attributeLists.add(al); }
        |
            id=importDirective { imports.add(id); }
        |
            nsmd=namespaceScopeMemberDeclaration { members.add(nsmd); }
        )*
        { value = new CompilationUnitSyntax(attributeLists.build(), imports.build(), members.build(), span(start)); }
        EOF
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
    Token start = input.LT(1);
    boolean isStatic = false;
}
    :
        KW_IMPORT
        ( KW_STATIC { isStatic = true; } )?
        (
            in=identifierName
            OP_EQUALS
        )
        n=name
        OP_SEMICOLON
        { value = new ImportDirectiveSyntax(isStatic, in, n, span(start)); }
    ;

namespaceDeclaration returns [NamespaceDeclarationSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<ImportDirectiveSyntax> imports = new ImmutableArray.Builder<>();
    ImmutableArray.Builder<MemberDeclarationSyntax> members = new ImmutableArray.Builder<>();
}
    :
        KW_NAMESPACE
        n=name
        OP_BRACE_OPEN
        ( id=importDirective { imports.add(id); }
        | nsmd=namespaceScopeMemberDeclaration { members.add(nsmd); }
        )*
        { value = new NamespaceDeclarationSyntax(n, imports.build(), members.build(), span(start)); }
        OP_BRACE_CLOSE
    ;

// Attributes

attributeListList returns [ImmutableArray<AttributeListSyntax> value]
@init {
    ImmutableArray.Builder<AttributeListSyntax> builder = new ImmutableArray.Builder<>();
}
    :
        (
            al=attributeList { builder.add(al); }
        )*
        { value = builder.build(); }
    ;

attributeList returns [AttributeListSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<AttributeSyntax> attributes = new ImmutableArray.Builder<>();
    AttributeTarget target = AttributeTarget.NONE;
}
    :
        OP_BRACKET_OPEN
        (
            OP_COLON
            at=attributeTarget
            { target = at; }
        )?
        (
            a=attribute
            { attributes.add(a); }
        )+
        OP_BRACKET_CLOSE
        { value = new AttributeListSyntax(target, attributes.build(), span(start)); }
    ;

attribute returns [AttributeSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<AttributeArgumentSyntax> arguments = new ImmutableArray.Builder<>();
}
    :
        n=name
        (
            OP_PAREN_OPEN
            (
                a=attributeArgument
                { arguments.add(a); }
                (
                    OP_COMMA
                    a=attributeArgument
                    { arguments.add(a); }
                )*
            )?
            OP_PAREN_CLOSE
        )?
        { value = new AttributeSyntax(n, arguments.build(), span(start)); }
    ;

attributeArgument returns [AttributeArgumentSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        (
            in=identifierName
            OP_EQUALS
        )?
        e=nonAssignmentExpression
        { value = new AttributeArgumentSyntax(in, e, span(start)); }
    ;

attributeTarget returns [AttributeTarget value]
@init {
    Token start = input.LT(1);
}
    :
        in=identifierName
        { value = parseAttributeTarget(in.getIdentifier(), span(start)); }
    ;

parameterList returns [ImmutableArray<ParameterSyntax> value]
@init {
    ImmutableArray.Builder<ParameterSyntax> parameters = new ImmutableArray.Builder<>();
}
    :
        OP_PAREN_OPEN
        (
            p=parameter
            { parameters.add(p); }
            (
                OP_COMMA
                p=parameter
                { parameters.add(p); }
            )*
        )?
        OP_PAREN_CLOSE
        { value = parameters.build(); }
    ;

parameter returns [ParameterSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<ParameterModifier> modifiers = new ImmutableArray.Builder<>();
}
    :
        all=attributeListList
        (
            pm=parameterModifier
            { modifiers.add(pm); }
        )*
        t=type
        in=identifierName
        { value = new ParameterSyntax(all, modifiers.build(), t, in, span(start)); }
    ;

parameterModifier returns [ParameterModifier value]
    : KW_THIS { value = ParameterModifier.THIS; }
    | KW_REF { value = ParameterModifier.REF; }
    | KW_OUT { value = ParameterModifier.OUT; }
    | KW_PARAMS { value = ParameterModifier.PARAMS; }
    ;

// Delegate declaration

delegateDeclaration returns [DelegateDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_DELEGATE
        rt=type
        in=identifierName
        tpl=typeParameterList
        pl=parameterList
        tpccl=typeParameterConstraintClauseList
        OP_SEMICOLON
        { value = new DelegateDeclarationSyntax($declarationPrefix::attributes, $declarationPrefix::modifiers, rt, in, tpl, pl, tpccl, span(start)); }
    ;

typeParameterList returns [ImmutableArray<TypeParameterSyntax> value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<TypeParameterSyntax> typeParameters = new ImmutableArray.Builder<>();
}
    :
        (
            OP_LESS_THAN
            tp=typeParameter
            { typeParameters.add(tp); }
            (
                OP_COMMA
                tp=typeParameter
                { typeParameters.add(tp); }
            )*
            OP_GREATER_THAN
        )?
        { value = typeParameters.build(); }
    ;

typeParameter returns [TypeParameterSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        all=attributeListList
        tpv=typeParameterVariance
        in=identifierName
        { value = new TypeParameterSyntax(all, tpv, in, span(start)); }
    ;

typeParameterVariance returns [Variance value]
    : KW_IN { value = Variance.IN; }
    | KW_OUT { value = Variance.OUT; }
    | { value = Variance.NONE; }
    ;

// Type declarations

typeDeclaration returns [TypeDeclarationSyntax value]
@init {
    Token start = input.LT(1);
    TypeDeclarationType type = null;
    ImmutableArray.Builder<TypeSyntax> baseTypes = new ImmutableArray.Builder<>();
    ImmutableArray.Builder<MemberDeclarationSyntax> memberDeclarations = new ImmutableArray.Builder<>();
}
@after {
    value = new TypeDeclarationSyntax(
        $declarationPrefix::attributes,
        $declarationPrefix::modifiers,
        in,
        baseTypes.build(),
        type,
        tpl,
        tpccl,
        memberDeclarations.build(),
        span(start)
    );
}
    :
        ( KW_CLASS { type = TypeDeclarationType.CLASS; }
        | KW_INTERFACE { type = TypeDeclarationType.INTERFACE; }
        | KW_STRUCT { type = TypeDeclarationType.STRUCT; }
        )
        in=identifierName
        tpl=typeParameterList
        (
            OP_COLON
            (
                t=type
                { baseTypes.add(t); }
                (
                    OP_COMMA
                    t=type
                    { baseTypes.add(t); }
                )*
            )
        )?
        tpccl=typeParameterConstraintClauseList
        OP_BRACE_OPEN
        (
            md=memberDeclaration
            { memberDeclarations.add(md); }
        )*
        OP_BRACE_CLOSE
    ;

typeParameterConstraintClauseList returns [ImmutableArray<TypeParameterConstraintClauseSyntax> value]
@init {
    ImmutableArray.Builder<TypeParameterConstraintClauseSyntax> builder = new ImmutableArray.Builder<>();
}
    :
        (
            tpc=typeParameterConstraintClause
            { builder.add(tpc); }
            
        )*
        { value = builder.build(); }
    ;

typeParameterConstraintClause returns [TypeParameterConstraintClauseSyntax value]
@init {
    ImmutableArray.Builder<TypeParameterConstraintSyntax> constraints = new ImmutableArray.Builder<>();
    Token start = input.LT(1);
}
    :
        KW_WHERE
        in=identifierName
        OP_COLON
        (
            tpc=typeParameterConstraint
            { constraints.add(tpc); }
        )+
        { value = new TypeParameterConstraintClauseSyntax(in, constraints.build(), span(start)); }
    ;

typeParameterConstraint returns [TypeParameterConstraintSyntax value]
@init {
    Token start = input.LT(1);
    ClassOrStruct classOrStruct = null;
}
    :
        KW_NEW OP_PAREN_OPEN OP_PAREN_CLOSE
        { value = new ConstructorConstraintSyntax(span(start)); }
    |
        (
            KW_CLASS { classOrStruct = ClassOrStruct.CLASS; }
        |
            KW_STRUCT { classOrStruct = ClassOrStruct.STRUCT; }
        )
        { value = new ClassOrStructConstraintSyntax(classOrStruct, span(start)); }
    |
        t=type
        { value = new TypeConstraintSyntax(t, span(start)); }
    ;

modifiers returns [ImmutableArray<Modifier> value]
@init {
    ImmutableArray.Builder<Modifier> builder = new ImmutableArray.Builder<>();
}
    :
    (
        m=modifier
        { builder.add(m); }
    )*
    { value = builder.build(); }
    ;

modifier returns [Modifier value]
    : KW_ABSTRACT { value = Modifier.ABSTRACT; }
    | KW_ASYNC { value = Modifier.ASYNC; }
    | KW_EXTERN { value = Modifier.EXTERN; }
    | KW_INTERNAL { value = Modifier.INTERNAL; }
    | KW_PARTIAL { value = Modifier.PARTIAL; }
    | KW_PRIVATE { value = Modifier.PRIVATE; }
    | KW_PROTECTED { value = Modifier.PROTECTED; }
    | KW_PUBLIC { value = Modifier.PUBLIC; }
    | KW_READONLY { value = Modifier.READ_ONLY; }
    | KW_SEALED { value = Modifier.SEALED; }
    | KW_STATIC { value = Modifier.STATIC; }
    | KW_VIRTUAL { value = Modifier.VIRTUAL; }
    | KW_VOLATILE { value = Modifier.VOLATILE; }
    ;

// Enum declaration

enumDeclaration returns [EnumDeclarationSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<EnumMemberDeclarationSyntax> members = new ImmutableArray.Builder<>();
}
    :
        KW_ENUM
        in=identifierName
        (
            OP_COLON
            pts=predefinedTypeSyntax
        )?
        OP_BRACE_OPEN
        (
            emd=enumMemberDeclaration
            { members.add(emd); }
            (
                OP_COMMA
                emd=enumMemberDeclaration
                { members.add(emd); }
            )*
            OP_COMMA?
        )?
        OP_BRACE_CLOSE
        {
            value = new EnumDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                in,
                ImmutableArray.<TypeSyntax>asList(pts),
                members.build(),
                span(start)
            );
        }
    ;

enumMemberDeclaration returns [EnumMemberDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        all=attributeListList
        in=identifierName
        (
            OP_EQUALS
            e=expression
        )?
        { value = new EnumMemberDeclarationSyntax(all, in, e, span(start)); }
    ;

// Member declarations

memberDeclaration returns [MemberDeclarationSyntax value]
scope declarationPrefix;
    :
    all=attributeListList { $declarationPrefix::attributes = all; }
    m=modifiers { $declarationPrefix::modifiers = m; }
    (
        ( identifierName OP_PAREN_OPEN )=> e2=constructorDeclaration { value = e2; }
    |
        ( explicitInterfaceName KW_THIS )=> e6=indexerDeclaration { value = e6; }
    |
        (
            t=type { $declarationPrefix::type = t; }
            e1=typedMemberDeclaration
        )
    |
        e3=conversionOperatorDeclaration { value = e3; }
    |
        { $declarationPrefix::modifiers.size() == 0 }?=> e4=destructorDeclaration { value = e4; }
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
        ( KW_OPERATOR )=>
        e1=operatorDeclaration
        { value = e1; }
    |
        ( explicitInterfaceName identifierName )=>
        (
            ( OP_BRACE_OPEN )=>
            e2=propertyDeclaration
            { value = e2; }
        |
            ( OP_LESS_THAN | OP_PAREN_OPEN )=>
            e3=methodDeclaration
            { value = e3; }
        |
            e4=fieldDeclaration
            { value = e4; }
        )
    ;

// Property/field declaration

propertyOrFieldDeclaration returns [MemberDeclarationSyntax value]
    :
    ;

propertyDeclaration returns [PropertyDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ein=explicitInterfaceName
        in=identifierName
        al=accessorList
        { value = new PropertyDeclarationSyntax($declarationPrefix::attributes, $declarationPrefix::modifiers, $declarationPrefix::type, ein, in, al, span(start)); }
    ;

fieldDeclaration returns [FieldDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        vdwt=variableDeclarationWithoutType
        { value = new FieldDeclarationSyntax($declarationPrefix::attributes, $declarationPrefix::modifiers, vdwt, span(start)); }
        OP_SEMICOLON
    ;

// Event declaration

eventDeclaration returns [MemberDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_EVENT
        (
            ( variableDeclaration OP_SEMICOLON )=>
            vd=variableDeclaration
            { value = new EventFieldDeclarationSyntax($declarationPrefix::attributes, $declarationPrefix::modifiers, vd, span(start)); }
            OP_SEMICOLON
        |
            t=type
            ein=explicitInterfaceName
            in=identifierName
            al=accessorList
            { value = new EventDeclarationSyntax($declarationPrefix::attributes, $declarationPrefix::modifiers, t, ein, in, al, span(start)); }
        )
    ;

explicitInterfaceName returns [NameSyntax value]
    :
        (
            n=name { value = n; }
            OP_DOT
        )?
    ;

accessorList returns [ImmutableArray<AccessorDeclarationSyntax> value]
@init {
    Token start = null;
    ImmutableArray.Builder<AccessorDeclarationSyntax> accessors = new ImmutableArray.Builder<>();
}
    :
        OP_BRACE_OPEN
        (
            { start = input.LT(1); }
            all=attributeListList
            m=modifiers
            adt=accessorDeclarationType
            b=block
            { accessors.add(new AccessorDeclarationSyntax(all, m, adt, b, span(start))); }
        )+
        OP_BRACE_CLOSE
        { value = accessors.build(); }
    ;

accessorDeclarationType returns [AccessorDeclarationType value]
    :
        in=identifierName
        { value = parseAccessorDeclarationType(in.getIdentifier(), in.getSpan()); }
    ;

// Constructor declaration

constructorDeclaration returns [ConstructorDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        in=identifierName
        pl=parameterList
        ( ci=constructorInitializer )?
        b=block
        {
            value = new ConstructorDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                in,
                ci,
                pl,
                b,
                span(start)
            );
        }
    ;

constructorInitializer returns [ConstructorInitializerSyntax value]
@init {
    Token start = input.LT(1);
    ThisOrBase type = null;
}
    :
        OP_COLON
        ( KW_THIS { type = ThisOrBase.THIS; }
        | KW_BASE { type = ThisOrBase.BASE; }
        )
        al=argumentList
        { value = new ConstructorInitializerSyntax(type, al, span(start)); }
    ;

// Destructor declaration

destructorDeclaration returns [DestructorDeclarationSyntax value]
@init {
    Token start = input.LT(1);
    assert $declarationPrefix::modifiers.size() == 0;
}
    :
        OP_TILDE
        in=identifierName
        OP_PAREN_OPEN
        OP_PAREN_CLOSE
        b=block
        {
            value = new DestructorDeclarationSyntax(
                $declarationPrefix::attributes,
                ImmutableArray.<Modifier>empty(),
                in,
                ImmutableArray.<ParameterSyntax>empty(),
                b,
                span(start)
            );
        }
    ;

// Method declaration

methodDeclaration returns [MethodDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ein=explicitInterfaceName
        in=identifierName
        tpl=typeParameterList
        pl=parameterList
        tpccl=typeParameterConstraintClauseList
        b=block
        {
            value = new MethodDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                $declarationPrefix::type,
                ein,
                in,
                tpl,
                tpccl,
                pl,
                b,
                span(start)
            );
        }
    ;

// Index declaration

indexerDeclaration returns [IndexerDeclarationSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ein=explicitInterfaceName
        KW_THIS
        OP_BRACKET_OPEN
        p=parameter
        OP_BRACKET_CLOSE
        al=accessorList
        {
            value = new IndexerDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                $declarationPrefix::type,
                ein,
                p,
                al,
                span(start)
            );
        }
    ;

// Conversion operator declaration

conversionOperatorDeclaration returns [ConversionOperatorDeclarationSyntax value]
@init {
    Token start = input.LT(1);
    ImplicitOrExplicit type = null;
}
    :
        ( KW_EXPLICIT { type = ImplicitOrExplicit.EXPLICIT; }
        | KW_IMPLICIT { type = ImplicitOrExplicit.IMPLICIT; }
        )
        KW_OPERATOR
        t=type
        pl=parameterList
        b=block
        {
            value = new ConversionOperatorDeclarationSyntax(
                $declarationPrefix::attributes,
                $declarationPrefix::modifiers,
                type,
                t,
                pl,
                b,
                span(start)
            );
        }
    ;

// Operator declaration

operatorDeclaration returns [OperatorDeclarationSyntax value]
@init {
    Token start = input.LT(1);
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
                span(start)
            );
        }
    ;

operator returns [Operator value]
    : OP_AMPERSAND { value = Operator.AMPERSAND; }
    | OP_ASTERISK { value = Operator.ASTERISK; }
    | OP_BAR { value = Operator.BAR; }
    | OP_CARET { value = Operator.CARET; }
    | OP_EQUALS_EQUALS { value = Operator.EQUALS_EQUALS; }
    | OP_EXCLAMATION { value = Operator.EXCLAMATION; }
    | OP_EXCLAMATION_EQUALS { value = Operator.EXCLAMATION_EQUALS; }
    | KW_FALSE { value = Operator.FALSE; }
    | OP_GREATER_THAN { value = Operator.GREATER_THAN; }
    | OP_GREATER_THAN_EQUALS { value = Operator.GREATER_THAN_EQUALS; }
    | OP_GREATER_THAN_GREATER_THAN { value = Operator.GREATER_THAN_GREATER_THAN; }
    | OP_LESS_THAN { value = Operator.LESS_THAN; }
    | OP_LESS_THAN_EQUALS { value = Operator.LESS_THAN_EQUALS; }
    | OP_LESS_THAN_LESS_THAN { value = Operator.LESS_THAN_LESS_THAN; }
    | OP_MINUS { value = Operator.MINUS; }
    | OP_MINUS_MINUS { value = Operator.MINUS_MINUS; }
    | OP_PERCENT { value = Operator.PERCENT; }
    | OP_PLUS { value = Operator.PLUS; }
    | OP_PLUS_PLUS { value = Operator.PLUS_PLUS; }
    | OP_SLASH { value = Operator.SLASH; }
    | OP_TILDE { value = Operator.TILDE; }
    | KW_TRUE { value = Operator.TRUE; }
    ;

// Type name parsing

castType returns [TypeSyntax value]
@init {
    Token start = input.LT(1);
    NakedNullableType type = null;
}
    :
        (
            OP_QUESTION
            { type = NakedNullableType.NULLABLE; }
        |
            OP_EXCLAMATION
            { type = NakedNullableType.NOT_NULLABLE; }
        )
        { value = new NakedNullableTypeSyntax(type, span(start)); }
    |
        t=type
        { value = t; }
    ;

type returns [TypeSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        at=notArrayType
        { value = at; }
        (
            OP_BRACKET_OPEN
            OP_BRACKET_CLOSE
            { value = new ArrayTypeSyntax(value, span(start)); }
        )?
    ;

name returns [NameSyntax value]
    : qn=qualifiedName { value = qn; }
    ;

predefinedTypeSyntax returns [PredefinedTypeSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        pt=predefinedType
        { value = new PredefinedTypeSyntax(pt, span(start)); }
    ;

notArrayType returns [TypeSyntax value]
@init {
    Token start = input.LT(1);
}
    :
    (
        pds=predefinedTypeSyntax
        { value = pds; }
    |
        qn=qualifiedName
        { value = qn; }
    )
    (
        OP_QUESTION
        { value = new NullableTypeSyntax(value, span(start)); }
    )?
    ;

qualifiedName returns [NameSyntax value]
@init {
    Token start = input.LT(1);
    IdentifierNameSyntax alias = null;
}
    :
        (
            in=identifierName OP_COLON_COLON
            { alias = in; }
        )?
        i=simpleName
        {
            if (alias != null) {
                value = new AliasQualifiedNameSyntax(alias, i, span(start));
            } else {
                value = i;
            }
        }
        (
            OP_DOT
            i=simpleName
            { value = new QualifiedNameSyntax(value, i, span(start)); }
        )*
    ;

simpleName returns [SimpleNameSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        i=identifierName
        { value = i; }
        (
            gta=genericTypeArguments
            { value = new GenericNameSyntax(i.getIdentifier(), gta, span(start)); }
        )?
    ;

genericTypeArguments returns [ImmutableArray<TypeSyntax> value]
@init {
    ImmutableArray.Builder<TypeSyntax> builder = new ImmutableArray.Builder<>();
    
    beginGeneric();
}
    :
        OP_LESS_THAN
        { openGeneric(); }
        gta=genericTypeArgument
        { builder.add(gta); }
        (
            OP_COMMA
            gta=genericTypeArgument
            { builder.add(gta); }
        )*
        // This is a bit narly. What this is trying to accomplish is to
        // disambiguate between >> as right shift and >> as two generic type
        // close signs. We count the number of open braces we had, and allow
        // that number of more to be closed here. Then, if we're closing one here
        // that belongs to a recursive call, the getPendingGenericCloses() will
        // be too low, and we'll match the empty branch.
        { getPendingGenericCloses() > 0 }?=> { }
        (
            { getPendingGenericCloses() >= 2 }?=>
            OP_GREATER_THAN_GREATER_THAN
            { closeGeneric(); closeGeneric(); }
        |
            { getPendingGenericCloses() >= 1 }?=>
            OP_GREATER_THAN
            { closeGeneric(); }
        )
        { value = builder.build(); }
    ;

genericTypeArgument returns [TypeSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        (
            t=type
            { value = t; }
        )?
        {
            if (value == null) {
                new OmittedTypeArgumentSyntax(span(start));
            }
        }
    ;

identifierName returns [IdentifierNameSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        i=IDENTIFIER
        { value = new IdentifierNameSyntax(i.getText(), span(start)); }
    ;

predefinedType returns [PredefinedType value]
    : KW_BOOL { value = PredefinedType.BOOL; }
    | KW_BYTE { value = PredefinedType.BYTE; }
    | KW_CHAR { value = PredefinedType.CHAR; }
    | KW_DECIMAL { value = PredefinedType.DECIMAL; }
    | KW_DOUBLE { value = PredefinedType.DOUBLE; }
    | KW_FLOAT { value = PredefinedType.FLOAT; }
    | KW_INT { value = PredefinedType.INT; }
    | KW_LONG { value = PredefinedType.LONG; }
    | KW_OBJECT { value = PredefinedType.OBJECT; }
    | KW_SBYTE { value = PredefinedType.SBYTE; }
    | KW_SHORT { value = PredefinedType.SHORT; }
    | KW_STRING { value = PredefinedType.STRING; }
    | KW_UINT { value = PredefinedType.UINT; }
    | KW_ULONG { value = PredefinedType.ULONG; }
    | KW_USHORT { value = PredefinedType.USHORT; }
    | KW_VOID { value = PredefinedType.VOID; }
    ;

// Statements

block returns [BlockSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<StatementSyntax> statements = new ImmutableArray.Builder<>();
}
    :
        OP_BRACE_OPEN
        (
            s=statement
            { statements.add(s); }
        )*
        OP_BRACE_CLOSE
        { return new BlockSyntax(statements.build(), span(start)); }
    ;

statement returns [StatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ( type? identifierName )=>
        (
            ( identifierName OP_SEMICOLON )=>
            e1=identifierName
            OP_SEMICOLON
            { value = new ExpressionStatementSyntax(e1, span(start)); }
        |
            ( type identifierName OP_SEMICOLON )=>
            e2=variableDeclarationExpression
            OP_SEMICOLON
            { value = new ExpressionStatementSyntax(e2, span(start)); }
        |
            e3=localDeclarationStatement { value = e3; }
        )
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
    ;

assertStatement returns [AssertStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_ASSERT
        e=expression
        OP_SEMICOLON
        { value = new AssertStatementSyntax(e, span(start)); }
    ;

breakContinueStatement returns [StatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
    ( KW_BREAK { value = new BreakStatementSyntax(span(start)); }
    | KW_CONTINUE { value = new ContinueStatementSyntax(span(start)); }
    )
    OP_SEMICOLON
    ;

doStatement returns [DoStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_DO
        e=expression
        b=block
        { value = new DoStatementSyntax(e, b, span(start)); }
    ;

emptyStatement returns [EmptyStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        OP_SEMICOLON
        { value = new EmptyStatementSyntax(span(start)); }
    ;

expressionStatement returns [ExpressionStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        e=expression
        OP_SEMICOLON
        { value = new ExpressionStatementSyntax(e, span(start)); }
    ;

forEachStatement returns [ForEachStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_FOREACH
        t=type
        in=identifierName
        KW_IN
        e=expression
        b=block
        { value = new ForEachStatementSyntax(t, in, e, b, span(start)); }
    ;

forStatement returns [ForStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_FOR
        (
            ( type identifierName )=>
            vd=variableDeclaration
        |
            el1=expressionList
        )?
        OP_SEMICOLON
        ( e=expression )?
        OP_SEMICOLON
        ( el2=expressionList )?
        b=block
        { value = new ForStatementSyntax(vd, el1, e, el2, b, span(start)); }
    ;

expressionList returns [ImmutableArray<ExpressionSyntax> value]
@init {
    ImmutableArray.Builder<ExpressionSyntax> expressions = new ImmutableArray.Builder<>();
}
    :
        e=expression
        { expressions.add(e); }
        (
            OP_COMMA
            e=expression
            { expressions.add(e); }
        )*
        { value = expressions.build(); }
    ;

ifStatement returns [IfStatementSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<ElseClauseSyntax> elses = new ImmutableArray.Builder<>();
}
    :
        KW_IF
        e=expression
        b=block
        ( eic=elIfClause { elses.add(eic); } )*
        ( ec=elseClause { elses.add(ec); } )?
        { value = new IfStatementSyntax(e, b, elses.build(), span(start)); }
    ;

elIfClause returns [ElseClauseSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_ELIF
        e=expression
        b=block
        { value = new ElseClauseSyntax(e, b, span(start)); }
    ;

elseClause returns [ElseClauseSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_ELSE
        b=block
        { value = new ElseClauseSyntax(null, b, span(start)); }
    ;

localDeclarationStatement returns [LocalDeclarationStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        m=modifiers
        vd=variableDeclaration
        OP_SEMICOLON
        { value = new LocalDeclarationStatementSyntax(m, vd, span(start)); }
    ;

variableDeclaration returns [VariableDeclarationSyntax value]
scope declarationPrefix;
    :
        t=type
        { $declarationPrefix::type = t; }
        vdwt=variableDeclarationWithoutType
        { value = vdwt; }
    ;

variableDeclarationWithoutType returns [VariableDeclarationSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<VariableDeclaratorSyntax> declarators = new ImmutableArray.Builder<>();
}
    :
        vd=variableDeclarator
        { declarators.add(vd); }
        (
            OP_COMMA
            vd=variableDeclarator
            { declarators.add(vd); }
        )*
        { value = new VariableDeclarationSyntax($declarationPrefix::type, declarators.build(), span(start)); }
    ;

variableDeclarator returns [VariableDeclaratorSyntax value]
@init {
    Token start = input.LT(1);
    ExpressionSyntax arraySize = null;
    ExpressionSyntax assigned = null;
}
    :
        in=identifierName
        (
            OP_BRACKET_OPEN
            (
                e=expression
                { arraySize = e; }
            |
                { arraySize = new OmittedArraySizeExpressionSyntax(span(input.LT(-1))); }
            )
            OP_BRACKET_CLOSE
        )?
        (
            OP_EQUALS
            e=expression
            { assigned = e; }
        )?
        { value = new VariableDeclaratorSyntax(in, arraySize, assigned, span(start)); }
    ;

returnStatement returns [ReturnStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_RETURN
        ( e=expression )?
        OP_SEMICOLON
        { value = new ReturnStatementSyntax(e, span(start)); }
    ;

switchStatement returns [SwitchStatementSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<SwitchSectionSyntax> sections = new ImmutableArray.Builder<>();
}
    :
        KW_SWITCH
        e=expression
        OP_BRACE_OPEN
        (
            ss=switchSection
            { sections.add(ss); }
        )*
        OP_BRACE_CLOSE
        { value = new SwitchStatementSyntax(e, sections.build(), span(start)); }
    ;

switchSection returns [SwitchSectionSyntax value]
@init {
    Token start = input.LT(1);
    CaseOrDefault type = null;
}
    :
        (
            KW_DEFAULT
            { type = CaseOrDefault.DEFAULT; }
        |
            KW_CASE
            { type = CaseOrDefault.CASE; }
            el=expressionList
        )
        b=block
        { value = new SwitchSectionSyntax(type, el == null ? ImmutableArray.<ExpressionSyntax>empty() : el, b, span(start)); }
    ;

throwStatement returns [ThrowStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_THROW
        e=expression
        OP_SEMICOLON
        { value = new ThrowStatementSyntax(e, span(start)); }
    ;

tryStatement returns [TryStatementSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<CatchClauseSyntax> catches = new ImmutableArray.Builder<>();
}
    :
        KW_TRY
        b=block
        (
            cc=catchClause
            { catches.add(cc); }
        )*
        ( fc=finallyClause )?
        { value = new TryStatementSyntax(b, catches.build(), fc, span(start)); }
    ;

catchClause returns [CatchClauseSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_CATCH
        (
            t=type
            ( in=identifierName )?
        )?
        b=block
        { value = new CatchClauseSyntax(t, in, b, span(start)); }
    ;

finallyClause returns [FinallyClauseSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_FINALLY
        b=block
        { value = new FinallyClauseSyntax(b, span(start)); }
    ;

usingStatement returns [UsingStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_USING
        (
            ( type identifierName )=>
            vd=variableDeclaration
        |
            e=expression
        )
        b=block
        { value = new UsingStatementSyntax(vd, e, b, span(start)); }
    ;

loopStatement returns [LoopStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_LOOP
        b=block
        { value = new LoopStatementSyntax(b, span(start)); }
    ;

whileStatement returns [WhileStatementSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_WHILE
        e=expression
        b=block
        { value = new WhileStatementSyntax(e, b, span(start)); }
    ;

// Expressions

expression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ce=conditionalExpression
        { value = ce; }
        (
            ao=assignmentOperator
            e=expression
            { value = new BinaryExpressionSyntax(ao, value, e, span(start)); }
        )?
    ;

nonAssignmentExpression returns [ExpressionSyntax value]
    :
        ce=conditionalExpression
        { value = ce; }
    ;

assignmentOperator returns [BinaryOperator value]
    : OP_EQUALS { value = BinaryOperator.EQUALS; }
    | OP_PLUS_EQUALS { value = BinaryOperator.PLUS_EQUALS; }
    | OP_MINUS_EQUALS { value = BinaryOperator.MINUS_EQUALS; }
    | OP_ASTERISK_EQUALS { value = BinaryOperator.ASTERISK_EQUALS; }
    | OP_SLASH_EQUALS { value = BinaryOperator.SLASH_EQUALS; }
    | OP_PERCENT_EQUALS { value = BinaryOperator.PERCENT_EQUALS; }
    | OP_GREATER_THAN_GREATER_THAN_EQUALS { value = BinaryOperator.GREATER_THAN_GREATER_THAN_EQUALS; }
    | OP_LESS_THAN_LESS_THAN_EQUALS { value = BinaryOperator.LESS_THAN_LESS_THAN_EQUALS; }
    | OP_AMPERSAND_EQUALS { value = BinaryOperator.AMPERSAND_EQUALS; }
    | OP_BAR_EQUALS { value = BinaryOperator.BAR_EQUALS; }
    | OP_CARET_EQUALS { value = BinaryOperator.CARET_EQUALS; }
    ;

conditionalExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ce=coalescingExpression
        { value = ce; }
        (
            OP_QUESTION e=expression
            OP_COLON ce=conditionalExpression
            { value = new ConditionalExpressionSyntax(value, e, ce, span(start)); }
        )?
    ;

coalescingExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        loe=logicalOrExpression
        { value = loe; }
        (
            OP_QUESTION_QUESTION
            loe=logicalOrExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.QUESTION_QUESTION, value, loe, span(start)); }
        )?
    ;

logicalOrExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        lae=logicalAndExpression
        { value = lae; }
        (
            OP_BAR_BAR lae=logicalAndExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.BAR_BAR, value, lae, span(start)); }
        )*
    ;

logicalAndExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        boe=bitwiseOrExpression
        { value = boe; }
        (
            OP_AMPERSAND_AMPERSAND boe=bitwiseOrExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.AMPERSAND_AMPERSAND, value, boe, span(start)); }
        )*
    ;

bitwiseOrExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        bxe=bitwiseXorExpression
        { value = bxe; }
        (
            OP_BAR bxe=bitwiseXorExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.BAR, value, bxe, span(start)); }
        )*
    ;

bitwiseXorExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        bae=bitwiseAndExpression
        { value = bae; }
        (
            OP_CARET bae=bitwiseAndExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.CARET, value, bae, span(start)); }
        )*
    ;

bitwiseAndExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ee=equalityExpression
        { value = ee; }
        (
            OP_AMPERSAND ee=equalityExpression
            { value = new BinaryExpressionSyntax(BinaryOperator.AMPERSAND, value, ee, span(start)); }
        )*
    ;

equalityExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        rex=relationalExpression
        { value = rex; }
        (
            eo=equalityOperator rex=relationalExpression
            { value = new BinaryExpressionSyntax(eo, value, rex, span(start)); }
        )*
    ;

equalityOperator returns [BinaryOperator value]
    : OP_EQUALS_EQUALS { value = BinaryOperator.EQUALS_EQUALS; }
    | OP_EXCLAMATION_EQUALS { value = BinaryOperator.EXCLAMATION_EQUALS; }
    ;

relationalExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        se=shiftExpression
        { value = se; }
        (
            (
                ro=relationalOperator se=shiftExpression
                { value = new BinaryExpressionSyntax(ro, value, se, span(start)); }
            |
                iao=isAsOperator qn=qualifiedName
                { value = new BinaryExpressionSyntax(iao, value, qn, span(start)); }
            )
        )*
    ;

relationalOperator returns [BinaryOperator value]
    : OP_GREATER_THAN { value = BinaryOperator.GREATER_THAN; }
    | OP_GREATER_THAN_EQUALS { value = BinaryOperator.GREATER_THAN_EQUALS; }
    | OP_LESS_THAN { value = BinaryOperator.LESS_THAN; }
    | OP_LESS_THAN_EQUALS { value = BinaryOperator.LESS_THAN_EQUALS; }
    ;

isAsOperator returns [BinaryOperator value]
    : KW_IS { value = BinaryOperator.IS; }
    | KW_AS { value = BinaryOperator.AS; }
    ;

shiftExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        ae=additiveExpression
        { value = ae; }
        (
            so=shiftOperator ae=additiveExpression
            { value = new BinaryExpressionSyntax(so, value, ae, span(start)); }
        )*
    ;

shiftOperator returns [BinaryOperator value]
    : OP_GREATER_THAN_GREATER_THAN { value = BinaryOperator.GREATER_THAN_GREATER_THAN; }
    | OP_LESS_THAN_LESS_THAN { value = BinaryOperator.LESS_THAN_LESS_THAN; }
    ;

additiveExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        me=multiplicativeExpression
        { value = me; }
        (
            ao=additiveOperator me=multiplicativeExpression
            { value = new BinaryExpressionSyntax(ao, value, me, span(start)); }
        )*
    ;

additiveOperator returns [BinaryOperator value]
    : OP_PLUS { value = BinaryOperator.PLUS; }
    | OP_MINUS { value = BinaryOperator.MINUS; }
    ;

multiplicativeExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        eu=unaryExpression
        { value = eu; }
        (
            mo=multiplicativeOperator ue=unaryExpression
            { value = new BinaryExpressionSyntax(mo, value, ue, span(start)); }
        )*
    ;

multiplicativeOperator returns [BinaryOperator value]
    : OP_ASTERISK { value = BinaryOperator.ASTERISK; }
    | OP_SLASH { value = BinaryOperator.SLASH; }
    | OP_PERCENT { value = BinaryOperator.PERCENT; }
    ;

unaryExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        puo1=prefixUnaryOperator ue=unaryExpression
        { value = new PrefixUnaryExpressionSyntax(puo1, ue, span(start)); }
    |

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
            { value = s.build(value); }
        )*
        (
            puo2=postfixUnaryOperator
            { value = new PostfixUnaryExpressionSyntax(puo2, value, span(start)); }
        )?
    ;

awaitExpression returns [AwaitExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_AWAIT
        ue=unaryExpression
        { value = new AwaitExpressionSyntax(ue, span(start)); }
    ;

prefixUnaryOperator returns [PrefixUnaryOperator value]
    : OP_AMPERSAND { value = PrefixUnaryOperator.AMPERSAND; }
    | OP_EXCLAMATION { value = PrefixUnaryOperator.EXCLAMATION; }
    | OP_MINUS { value = PrefixUnaryOperator.MINUS; }
    | OP_MINUS_MINUS { value = PrefixUnaryOperator.MINUS_MINUS; }
    | OP_PLUS { value = PrefixUnaryOperator.PLUS; }
    | OP_PLUS_PLUS { value = PrefixUnaryOperator.PLUS_PLUS; }
    | OP_TILDE { value = PrefixUnaryOperator.TILDE; }
    ;

postfixUnaryOperator returns [PostfixUnaryOperator value]
    : OP_MINUS_MINUS { value = PostfixUnaryOperator.MINUS_MINUS; }
    | OP_PLUS_PLUS { value = PostfixUnaryOperator.PLUS_PLUS; }
    ;

castExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        OP_PAREN_OPEN ct=castType OP_PAREN_CLOSE
        ue=unaryExpression
        { value = new CastExpressionSyntax(ue, ct, span(start)); }
    ;

argumentList returns [ImmutableArray<ArgumentSyntax> value]
@init {
    ImmutableArray.Builder<ArgumentSyntax> arguments = new ImmutableArray.Builder<>();
}
    :
        OP_PAREN_OPEN
        (
            a=argument
            { arguments.add(a); }
            (
                OP_COMMA
                a=argument
                { arguments.add(a); }
            )*
        )?
        OP_PAREN_CLOSE
        { value = arguments.build(); }
    ;

argument returns [ArgumentSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<ParameterModifier> modifiers = new ImmutableArray.Builder<>();
}
    :
        (
            pm=parameterModifier
            { modifiers.add(pm); }
        )*
        e=expression
        { value = new ArgumentSyntax(modifiers.build(), e, span(start)); }
    ;

primaryExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    : KW_THIS { value = new InstanceExpressionSyntax(ThisOrBase.THIS, span(start)); }
    | KW_BASE { value = new InstanceExpressionSyntax(ThisOrBase.BASE, span(start)); }
    | e1=variableDeclarationExpression { value = e1; }
    | e2=literal { value = e2; }
    | e3=arrayCreation { value = e3; }
    | OP_PAREN_OPEN e4=expression OP_PAREN_CLOSE { value = new ParenthesizedExpressionSyntax(e4, span(start)); }
    | KW_TYPEOF OP_PAREN_OPEN e5=type OP_PAREN_CLOSE { value = new TypeOfExpressionSyntax(e5, span(start)); }
    | KW_SIZEOF OP_PAREN_OPEN e6=type OP_PAREN_CLOSE { value = new SizeOfExpressionSyntax(e6, span(start)); }
    | KW_DEFAULT OP_PAREN_OPEN e7=type OP_PAREN_CLOSE { value = new DefaultExpressionSyntax(e7, span(start)); }
    | e8=newExpression { value = e8; }
    | e9=lambdaExpression { value = e9; }
    | e10=anonymousObject { value = e10; }
    | e11=implicitArrayCreation { value = e11; }
    | e12=identifierName { value = e12; }
    ;

variableDeclarationExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        t=type
        in=identifierName
        { value = new VariableDeclarationExpressionSyntax(t, in, span(start)); }
    ;

selector returns [Selector value]
@init {
    Token start = input.LT(1);
}
    : OP_DOT sn=simpleName { return new MemberAccessSelector(sn, span(start)); }
    | al=argumentList { return new ArgumentListSelector(al, span(start)); }
    | OP_BRACKET_OPEN e=expression OP_BRACKET_CLOSE { return new IndexSelector(e, span(start)); }
    ;

arrayCreation returns [ArrayCreationExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_NEW
        t=notArrayType
        OP_BRACKET_OPEN
        e=expression
        OP_BRACKET_CLOSE
        { value = new ArrayCreationExpressionSyntax(new ArrayTypeSyntax(t, t.getSpan()), e, span(start)); }
    ;

implicitArrayCreation returns [ImplicitArrayCreationExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_NEW
        OP_BRACE_OPEN
        ( el=expressionList )?
        OP_BRACE_CLOSE
        { value = new ImplicitArrayCreationExpressionSyntax(el, span(start)); }
    ;

anonymousObject returns [AnonymousObjectCreationExpressionSyntax value]
@init {
    Token start = input.LT(1);
    ImmutableArray.Builder<AnonymousObjectMemberDeclaratorSyntax> members = new ImmutableArray.Builder<>();
}
    :
        KW_NEW
        OP_BRACE_OPEN
        aom=anonymousObjectMember
        { members.add(aom); }
        (
            OP_COMMA
            aom=anonymousObjectMember
            { members.add(aom); }
        )*
        OP_COMMA?
        OP_BRACE_CLOSE
        { value = new AnonymousObjectCreationExpressionSyntax(members.build(), span(start)); }
    ;

anonymousObjectMember returns [AnonymousObjectMemberDeclaratorSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        in=identifierName
        OP_EQUALS
        e=expression
        { value = new AnonymousObjectMemberDeclaratorSyntax(in, e, span(start)); }
    ;

lambdaExpression returns [ExpressionSyntax value]
@init {
    ImmutableArray<ParameterSyntax> parameters = null;
    Token start = input.LT(1);
}
    :
        m=modifiers
        (
            in=identifierName
            {
                parameters = ImmutableArray.asList(new ParameterSyntax(
                    ImmutableArray.<AttributeListSyntax>empty(),
                    ImmutableArray.<ParameterModifier>empty(),
                    null,
                    in,
                    in.getSpan()
                ));
            }
        |
            pl=parameterList
            { parameters = pl; }
        )
        OP_EQUALS_GREATER_THAN
        (
            ex=expression
            { value = new LambdaExpressionSyntax(m, parameters, ex, span(start)); }
        |
            b=block
            { value = new LambdaExpressionSyntax(m, parameters, b, span(start)); }
        )
    ;

newExpression returns [ObjectCreationExpressionSyntax value]
@init {
    Token start = input.LT(1);
}
    :
        KW_NEW
        t=type
        (
            al=argumentList
            nmel=newMemberExpressionList?
        |
            nmel=newMemberExpressionList
        )
        {
            value = new ObjectCreationExpressionSyntax(
                t,
                al == null ? ImmutableArray.<ArgumentSyntax>empty() : al,
                nmel == null ? ImmutableArray.<ExpressionSyntax>empty() : nmel,
                span(start)
            );
        }
    ;

newMemberExpressionList returns [ImmutableArray<ExpressionSyntax> value]
@init {
    ImmutableArray.Builder<ExpressionSyntax> members = new ImmutableArray.Builder<>();
}
    :
        OP_BRACE_OPEN
        (
            nme=newMemberExpression
            { members.add(nme); }
            (
                OP_COMMA
                nme=newMemberExpression
                { members.add(nme); }
            )*
            OP_COMMA?
        )?
        OP_BRACE_CLOSE
    ;

newMemberExpression returns [ExpressionSyntax value]
@init {
    Token start = input.LT(1);
    BinaryOperator operator = null;
}
    :
        in=identifierName
        (
            OP_EQUALS
            { operator = BinaryOperator.EQUALS; }
        |
            OP_PLUS_EQUALS
            { operator = BinaryOperator.PLUS_EQUALS; }
        )
        e=expression
        { value = new BinaryExpressionSyntax(operator, in, e, span(start)); }
    ;

literal returns [LiteralExpressionSyntax value]
@init {
    Token start = input.LT(0);
    LiteralType type = null;
    String string = null;
}
@after {
    value = new LiteralExpressionSyntax(type, string, span(start));
}
    : KW_TRUE { type = LiteralType.TRUE; }
    | KW_FALSE { type = LiteralType.FALSE; }
    | KW_NIL { type = LiteralType.NIL; }
    | hi=HEX_INTEGER { string = hi.getText(); type = LiteralType.HEX; }
    | i=INTEGER { string = i.getText(); type = LiteralType.INTEGER; }
    | f=FLOAT { string = f.getText(); type = LiteralType.FLOAT; }
    | c=CHAR { string = c.getText(); type = LiteralType.CHAR; }
    | s=STRING { string = s.getText(); type = LiteralType.STRING; }
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
KW_CONTINUE : 'continue' ;
KW_DECIMAL : 'decimal' ;
KW_DEFAULT : 'default' ;
KW_DELEGATE : 'delegate' ;
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
OP_GREATER_THAN_GREATER_THAN : '>>' ;
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

fragment NEW_LINE
    :
    ( '\r' // CR
    | '\n' // NL
    | '\r\n' // CR NL
    | '\u0085' // New line
    | '\u2028' // Line separator
    | '\u2029' // Paragraph separator
    )
    { skip(); }
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
        { skip(); }
    ;

BLOCK_COMMENT
@init {
    boolean docComment = false;
}
    :
        '/*'
        { docComment = (char)input.LT(1) == '*'; }
        ( options { greedy=false; } : . )*
        '*/'
        {
            if (docComment) {
                $channel = HIDDEN;
            } else {
                skip();
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
    { skip(); }
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
