using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class PropertyAccessorSymbol : MethodSymbol {
        public PropertyAccessorKind AccessorKind { get; private set; }

        public PropertySymbol Property { get; private set; }

        public PropertyAccessorSymbol(PropertyAccessorKind kind, PropertySymbol property, SymbolModifier modifiers, TypeSymbol returnType)
            : base(property.DeclaringType, modifiers, returnType) {
            if (property == null) {
                throw new ArgumentNullException("property");
            }

            AccessorKind = kind;
            Property = property;
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitPropertyAccessor(this);
            }
        }

        public override MethodKind MethodKind {
            get { return MethodKind.EventAccessor; }
        }
    }
}
