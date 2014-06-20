using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class DestructorSymbol : MethodSymbol {
        public override SymbolKind Kind {
            get { return SymbolKind.Destructor; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDestructor(this);
            }
        }
    }
}
