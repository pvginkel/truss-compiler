using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class PropertySymbol : Symbol {
        public override SymbolKind Kind {
            get { return SymbolKind.Property; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitProperty(this);
            }
        }
    }
}
