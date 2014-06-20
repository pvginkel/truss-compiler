using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class EventAccessorSymbol : MethodSymbol {
        public override SymbolKind Kind {
            get { return SymbolKind.EventAccessor; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEventAccessor(this);
            }
        }
    }
}
