using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class EventSymbol : Symbol {
        public override SymbolKind Kind {
            get { return SymbolKind.Event; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEvent(this);
            }
        }
    }
}
