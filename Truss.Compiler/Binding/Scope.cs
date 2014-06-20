using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal abstract class Scope {
        protected Scope(Symbol symbol, Scope parent) {
            Symbol = symbol;
            Parent = parent;
        }

        public virtual ErrorList Errors {
            get { return Parent.Errors; }
        }

        public Symbol Symbol { get; private set; }

        public Scope Parent { get; private set; }

        public virtual ContainerSymbol ResolveContainer(NameSyntax name, ResolveMode type) {
            return Parent.ResolveContainer(name, type);
        }

        public virtual GlobalSymbol GetGlobalSymbol() {
            return Parent.GetGlobalSymbol();
        }
    }
}
