using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class SymbolTreeWalker : AbstractSymbolVisitor {
        public override void DefaultVisit(Symbol symbol) {
            if (symbol is ContainerSymbol) {
                VisitList((ContainerSymbol)symbol);
            }
        }

        public virtual void VisitList(ContainerSymbol container) {
            foreach (var symbol in container.Members) {
                symbol.Accept(this);
            }
        }
    }
}
