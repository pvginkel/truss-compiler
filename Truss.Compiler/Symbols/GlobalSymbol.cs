using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class GlobalSymbol : ContainerSymbol {
        public GlobalSymbol()
            : base(null) {
        }

        public override SymbolKind Kind {
            get { return SymbolKind.Global; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitGlobal(this);
            }
        }
    }
}
