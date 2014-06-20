using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class PropertyAccessorSymbol : MethodSymbol {
        public override SymbolKind Kind {
            get { return SymbolKind.PropertyAccessor; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPropertyAccessor(this);
            }
        }
    }
}
