using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal class SymbolManager {
        private readonly Dictionary<SyntaxNode, Symbol> _map = new Dictionary<SyntaxNode, Symbol>();

        public SymbolManager() {
            GlobalSymbol = new GlobalSymbol();
        }

        public GlobalSymbol GlobalSymbol { get; private set; }

        public Symbol Find(SyntaxNode node) {
            if (node == null) {
                throw new ArgumentNullException("node");
            }

            return _map[node];
        }

        public void Add(SyntaxNode node, Symbol symbol) {
            if (node == null) {
                throw new ArgumentNullException("node");
            }
            if (symbol == null) {
                throw new ArgumentNullException("symbol");
            }

            _map.Add(node, symbol);
        }
    }
}
