using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class DestructorSymbol : MethodSymbol {
        public DestructorSymbol(NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol returnType)
            : base(declaringType, modifiers, returnType) {
        }

        public override MethodKind MethodKind {
            get { return MethodKind.Destructor; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitDestructor(this);
            }
        }
    }
}
