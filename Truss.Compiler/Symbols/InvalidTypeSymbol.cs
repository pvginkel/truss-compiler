using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class InvalidTypeSymbol : TypeSymbol {
        public const string InvalidName = "**INVALID TYPE**";

        public InvalidTypeSymbol(ContainerSymbol parent)
            : base(InvalidName, InvalidName, parent) {
        }

        public override TypeKind TypeKind {
            get { return TypeKind.Invalid; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitInvalidType(this);
            }
        }
    }
}
