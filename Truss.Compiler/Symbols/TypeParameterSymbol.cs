using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class TypeParameterSymbol : TypeSymbolBase {
        private TypeParameterSymbol(string name, string metadataName, ContainerSymbol parent)
            : base(name, metadataName, parent) {
        }

        public override TypeKind TypeKind {
            get { return TypeKind.TypeParameter; }
        }

        public override SymbolKind Kind {
            get { return SymbolKind.TypeParameter; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTypeParameter(this);
            }
        }
    }
}
