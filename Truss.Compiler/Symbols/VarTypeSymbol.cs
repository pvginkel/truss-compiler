using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class VarTypeSymbol : TypeSymbol {
        public static readonly VarTypeSymbol Instance = new VarTypeSymbol();

        private VarTypeSymbol()
            : base(WellKnownNames.TypeVar, WellKnownNames.TypeVar, null) {
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitVarType(this);
            }
        }

        public override TypeKind TypeKind {
            get { return TypeKind.VarType; }
        }
    }
}
