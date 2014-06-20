using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public enum SymbolKind {
        Event,
        Field,
        Global,
        IndexerProperty,
        Method,
        Namespace,
        Parameter,
        Property,
        Type,
    }
}
