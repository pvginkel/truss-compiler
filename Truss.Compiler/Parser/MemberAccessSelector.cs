using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Parser {
    internal class MemberAccessSelector : ISelector {
        private readonly SimpleNameSyntax _memberName;
        private readonly Span _span;

        public MemberAccessSelector(SimpleNameSyntax memberName, Span span) {
            _memberName = memberName;
            _span = span;
        }

        public ExpressionSyntax Build(ExpressionSyntax value) {
            return new MemberAccessExpressionSyntax(value, _memberName, _span);
        }
    }
}
