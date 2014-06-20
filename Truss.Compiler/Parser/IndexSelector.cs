using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Parser {
    internal class IndexSelector : ISelector {
        private readonly ImmutableArray<ExpressionSyntax> _expressions;
        private readonly Span _span;

        public IndexSelector(ImmutableArray<ExpressionSyntax> expressions, Span span) {
            _expressions = expressions;
            _span = span;
        }

        public ExpressionSyntax Build(ExpressionSyntax value) {
            return new ElementAccessExpressionSyntax(value, _expressions, _span);
        }
    }
}
