using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class ConstructorSymbol : MethodSymbol {
        public override SymbolKind Kind {
            get { return SymbolKind.Constructor; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructor(this);
            }
        }
    }
}
