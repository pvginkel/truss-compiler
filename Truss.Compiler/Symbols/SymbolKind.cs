using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public enum SymbolKind {
        Constructor,
        ConversionOperator,
        Destructor,
        Event,
        EventAccessor,
        Field,
        Global,
        InvalidType,
        MemberMethod,
        Namespace,
        Operator,
        Parameter,
        Property,
        PropertyAccessor,
        Type,
        TypeParameter
    }
}
