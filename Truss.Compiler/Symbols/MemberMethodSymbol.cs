using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class MemberMethodSymbol : MethodSymbol {
        public override SymbolKind Kind {
            get { return SymbolKind.MemberMethod; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitMemberMethod(this);
            }
        }
    }
}
