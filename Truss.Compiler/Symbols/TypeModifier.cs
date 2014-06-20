using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    [Flags]
    public enum TypeModifier {
        None = 0,
        Abstract = 1 << 0,
        Internal = 1 << 1,
        Partial = 1 << 2,
        Public = 1 << 3,
        Readonly = 1 << 4,
        Sealed = 1 << 5,
        Static = 1 << 6
    }
}
