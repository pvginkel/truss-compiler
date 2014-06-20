using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal class SymbolManager {
        //private static readonly SyntaxNode[] EmptyNodes = new SyntaxNode[0];

        private readonly Dictionary<SyntaxNode, Symbol> _bySyntaxNode = new Dictionary<SyntaxNode, Symbol>();
        //private readonly Dictionary<Symbol, ImmutableArray<SyntaxNode>> _bySymbol = new Dictionary<Symbol, ImmutableArray<SyntaxNode>>();

        public SymbolManager() {
            GlobalSymbol = new GlobalSymbol();
        }

        public GlobalSymbol GlobalSymbol { get; private set; }

        public Symbol Find(SyntaxNode node) {
            if (node == null) {
                throw new ArgumentNullException("node");
            }

            Symbol result;
            _bySyntaxNode.TryGetValue(node, out result);
            return result;
        }

        //public ImmutableArray<SyntaxNode> Find(Symbol symbol) {
        //    if (symbol == null) {
        //        throw new ArgumentNullException("symbol");
        //    }

        //    ImmutableArray<SyntaxNode> result;
        //    if (_bySymbol.TryGetValue(symbol, out result)) {
        //        return result;
        //    }

        //    return ImmutableArray<SyntaxNode>.Empty;
        //}

        public void Add(SyntaxNode node, Symbol symbol) {
            if (node == null) {
                throw new ArgumentNullException("node");
            }
            if (symbol == null) {
                throw new ArgumentNullException("symbol");
            }

            _bySyntaxNode.Add(node, symbol);

            symbol.AddSpan(node.Span);

            //// We optimize for the case where there is only one node for a symbol. The only case where there are
            //// multiple is for partial classes.

            //ImmutableArray<SyntaxNode> nodes;
            //if (_bySymbol.TryGetValue(symbol, out nodes)) {
            //    _bySymbol[symbol] = new ImmutableArray<SyntaxNode>.Builder()
            //        .AddRange(nodes)
            //        .Add(node)
            //        .Build();
            //} else {
            //    _bySymbol.Add(symbol, ImmutableArray<SyntaxNode>.Create(node));
            //}
        }
    }
}
