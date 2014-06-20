using System;
using System.Collections.Generic;
using System.Text;
using Truss.Compiler.Symbols;

namespace Truss.Compiler.Binding {
    internal class Import {
        public Import(string alias, ContainerSymbol symbol, ImportType type) {
            Alias = alias;
            Symbol = symbol;
            Type = type;
        }

        public string Alias { get; private set; }

        public ContainerSymbol Symbol { get; private set; }

        public ImportType Type { get; private set; }
    }
}