grammar Truss;

options {
    backtrack = true;
}

@header {
    package truss.compiler.parser;
    
    import truss.compiler.*;
    import truss.compiler.syntax.*;
}

@lexer::header {
    package truss.compiler.parser;
    
    import truss.compiler.*;
    import truss.compiler.syntax.*;
}

////////////////////////////////////////////////////////////////////////////////
// PARSER                                                                     //
////////////////////////////////////////////////////////////////////////////////

compilationUnit returns [CompilationUnitSyntax value]
    :
        EOF
    ;

////////////////////////////////////////////////////////////////////////////////
// LEXER                                                                      //
////////////////////////////////////////////////////////////////////////////////

