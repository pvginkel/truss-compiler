using System;
using System.Collections.Generic;
using System.Text;
using Truss.Compiler.Symbols;

namespace Truss.Compiler.Binding {
    internal class MethodScope : Scope {
        public new MethodSymbol Symbol {
            get { return (MethodSymbol)base.Symbol; }
        }

        public MethodScope(MethodSymbol symbol, Scope parent)
            : base(symbol, parent) {
        }
    }
}