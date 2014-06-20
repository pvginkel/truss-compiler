using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class NullableTypeSymbol : TypeSymbol {
        public TypeSymbol ElementType { get; private set; }

        public NullableTypeSymbol(TypeSymbol elementType, string name, string metadataName, ContainerSymbol parent)
            : base(name, metadataName, parent) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }

            ElementType = elementType;
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNullableType(this);
            }
        }

        public override TypeKind TypeKind {
            get { return TypeKind.NullableType; }
        }
    }
}
