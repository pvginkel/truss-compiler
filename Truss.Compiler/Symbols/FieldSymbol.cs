using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class FieldSymbol : Symbol {
        public override SymbolKind Kind {
            get { return SymbolKind.Field; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitField(this);
            }
        }
    }
}
