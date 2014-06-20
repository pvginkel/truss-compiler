using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class ParameterSymbol : Symbol {
        public override SymbolKind Kind {
            get { return SymbolKind.Parameter; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitParameter(this);
            }
        }
    }
}
