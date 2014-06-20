using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class TrackedTypeSymbol : TypeSymbol {
        public TypeSymbol ElementType { get; private set; }

        public override bool IsTracked {
            get { return true; }
        }

        public TrackedTypeSymbol(TypeSymbol elementType, string name, string metadataName, ContainerSymbol parent)
            : base(name, metadataName, parent) {
            if (elementType == null) {
                throw new ArgumentNullException("elementType");
            }

            ElementType = elementType;
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitTrackedType(this);
            }
        }

        public override TypeKind TypeKind {
            get { return TypeKind.TrackedType; }
        }
    }
}
