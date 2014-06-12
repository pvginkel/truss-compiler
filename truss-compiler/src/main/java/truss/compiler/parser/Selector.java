package truss.compiler.parser;

import truss.compiler.syntax.ExpressionSyntax;

interface Selector {
    ExpressionSyntax build(ExpressionSyntax value);
}
