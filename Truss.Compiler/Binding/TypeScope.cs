using System;
using System.Collections.Generic;
using System.Text;
using Truss.Compiler.Symbols;

namespace Truss.Compiler.Binding {
    internal class TypeScope : ContainerScope {
        public new NamedTypeSymbol Symbol {
            get { return (NamedTypeSymbol)base.Symbol; }
        }

        public TypeScope(NamedTypeSymbol symbol, Scope parent)
            : base(symbol, null, parent) {
        }
    }
}