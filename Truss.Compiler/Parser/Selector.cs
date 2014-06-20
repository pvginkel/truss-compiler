using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Parser {
    internal interface ISelector {
        ExpressionSyntax Build(ExpressionSyntax value);
    }
}
