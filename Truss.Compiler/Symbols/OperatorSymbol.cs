using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class OperatorSymbol : OperatorSymbolBase {
        public override SymbolKind Kind {
            get { return SymbolKind.Operator; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOperator(this);
            }
        }
    }
}
