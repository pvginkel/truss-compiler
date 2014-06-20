using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    [Flags]
    public enum SymbolModifier {
        None = 0,

        Internal = 1 << 0,
        Private = 1 << 1,
        Protected = 1 << 2,
        Public = 1 << 3,

        Abstract = 1 << 4,
        Async = 1 << 5,
        Extern = 1 << 6,
        New = 1 << 7,
        Override = 1 << 8,
        Partial = 1 << 9,
        Readonly = 1 << 10,
        Sealed = 1 << 11,
        Static = 1 << 12,
        Tracked = 1 << 13,
        Virtual = 1 << 14,
        Volatile = 1 << 15,

        AccessModifiers = Internal | Private | Protected | Public,
    }
}
