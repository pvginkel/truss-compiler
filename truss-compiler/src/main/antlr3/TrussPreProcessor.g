grammar TrussPreProcessor;

options {
    backtrack = true;
}

@header {
    package truss.compiler.preprocessor;
}

@lexer::header {
    package truss.compiler.preprocessor;
}

////////////////////////////////////////////////////////////////////////////////
// PARSER                                                                     //
////////////////////////////////////////////////////////////////////////////////

directive returns [Directive value]
    :
        WS*
        ( e1=ifDirective { value = e1; }
        | e2=elIfDirective { value = e2; }
        | e3=elseDirective { value = e3; }
        | e4=endIfDirective { value = e4; }
        | e5=defineDirective { value = e5; }
        | e6=undefDirective { value = e6; }
        )
        EOF
    ;

ifDirective returns [IfDirective value]
    :
        KW_IF
        e=expression
        { value = new IfDirective(e); }
    ;

elIfDirective returns [ElIfDirective value]
    :
        KW_ELIF
        e=expression
        { value = new ElIfDirective(e); }
    ;

elseDirective returns [ElseDirective value]
    :
        KW_ELSE
        { value = new ElseDirective(); }
    ;

endIfDirective returns [EndIfDirective value]
    :
        KW_ENDIF
        { value = new EndIfDirective(); }
    ;

defineDirective returns [DefineDirective value]
    :
        KW_DEFINE
        i=IDENTIFIER
        { value = new DefineDirective(i.getText(), true); }
    ;

undefDirective returns [DefineDirective value]
    :
        KW_UNDEF
        i=IDENTIFIER
        { value = new DefineDirective(i.getText(), false); }
    ;

expression returns [boolean value]
    :
        oe=orExpression { value = oe; }
    ;

orExpression returns [boolean value]
    :
        ae=andExpression { value = ae; }
        (
            OP_BAR_BAR
            ae=andExpression { value = value || ae; }
        )*
    ;

andExpression returns [boolean value]
    :
        ee=equalityExpression { value = ee; }
        (
            OP_AMPERSAND_AMPERSAND
            ee=equalityExpression { value = value && ee; }
        )*
    ;

equalityExpression returns [boolean value]
@init {
    boolean isNotEquals = false;
}
    :
        ue=unaryExpression { value = ue; }
        (
            (
                OP_EQUALS_EQUALS { isNotEquals = false; }
            |
                OP_EXCLAMATION_EQUALS { isNotEquals = true; }
            )
            ue=unaryExpression
            {
                if (isNotEquals) {
                    value = value != ue;
                } else {
                    value = value == ue;
                }
            }
        )*
    ;

unaryExpression returns [boolean value]
    :
        pe=primaryExpression { value = pe; }
    |
        OP_EXCLAMATION
        ue=unaryExpression { value = !ue; }
    ;

primaryExpression returns [boolean value]
    :
        KW_TRUE { value = true; }
    |
        KW_FALSE { value = false; }
    |
        i=IDENTIFIER { value = isDefined(i.getText()); }
    |
        OP_PAREN_OPEN e=expression OP_PAREN_CLOSE { value = e; }
    ;

////////////////////////////////////////////////////////////////////////////////
// LEXER                                                                      //
////////////////////////////////////////////////////////////////////////////////

KW_DEFINE : 'define' ;
KW_UNDEF : 'undef' ;
KW_TRUE : 'true' ;
KW_FALSE : 'false' ;
KW_IF : 'if' ;
KW_ELIF : 'elif' ;
KW_ELSE : 'else' ;
KW_ENDIF : 'endif' ;

OP_AMPERSAND_AMPERSAND : '&&' ;
OP_BAR_BAR : '||' ;
OP_EXCLAMATION : '!';
OP_PAREN_OPEN : '(' ;
OP_PAREN_CLOSE : ')' ;
OP_EXCLAMATION_EQUALS : '!=' ;
OP_EQUALS_EQUALS : '==' ;

WS
    :
        ( ' ' | '\t' | '\u000C' )
        { $channel = HIDDEN; }
    ;

// TODO: Identifiers should support full Unicode set.

fragment DIGIT : '0'..'9' ;
fragment LETTER : 'a'..'z' | 'A'..'Z' ;
fragment FIRST_LETTER : LETTER | '_' ;
fragment NEXT_LETTER : FIRST_LETTER | DIGIT ;

IDENTIFIER : FIRST_LETTER NEXT_LETTER* ;
