using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Parser {
    internal class ArgumentListSelector : ISelector {
        private readonly ImmutableArray<ArgumentSyntax> _argumentList;
        private readonly Span _span;

        public ArgumentListSelector(ImmutableArray<ArgumentSyntax> argumentList, Span span) {
            _argumentList = argumentList;
            _span = span;
        }

        public ExpressionSyntax Build(ExpressionSyntax value) {
            return new InvocationExpressionSyntax(value, _argumentList, _span);
        }
    }
}
