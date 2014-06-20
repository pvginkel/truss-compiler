using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class ConversionOperatorSymbol : OperatorSymbolBase {
        public override SymbolKind Kind {
            get { return SymbolKind.ConversionOperator; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConversionOperator(this);
            }
        }
    }
}
