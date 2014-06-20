using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class ConstructorSymbol : MethodSymbol {
        public ConstructorSymbol(NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol returnType)
            : base(declaringType, modifiers, returnType) {
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConstructor(this);
            }
        }

        public override MethodKind MethodKind {
            get { return MethodKind.Constructor; }
        }
    }
}
