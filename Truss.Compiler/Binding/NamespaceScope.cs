using System;
using System.Collections.Generic;
using System.Text;
using Truss.Compiler.Symbols;

namespace Truss.Compiler.Binding {
    internal class NamespaceScope : ContainerScope {
        private readonly NamespaceSymbol _symbol;

        public NamespaceScope(NamespaceSymbol symbol, List<Import> imports, Scope parent)
            : base(symbol, imports, parent) {
            _symbol = symbol;
        }
    }
}