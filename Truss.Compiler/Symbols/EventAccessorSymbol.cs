using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class EventAccessorSymbol : MethodSymbol {
        public EventAccessorKind AccessorKind { get; private set; }

        public EventSymbol Event { get; private set; }

        public EventAccessorSymbol(EventAccessorKind kind, EventSymbol @event, SymbolModifier modifiers, TypeSymbol returnType)
            : base(@event.DeclaringType, modifiers, returnType) {
            if (@event == null) {
                throw new ArgumentNullException("event");
            }

            AccessorKind = kind;
            Event = @event;
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitEventAccessor(this);
            }
        }

        public override MethodKind MethodKind {
            get { return MethodKind.EventAccessor; }
        }
    }
}
