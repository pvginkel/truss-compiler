using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public enum TypeKind {
        Class,
        Struct,
        Interface,
        Enum,
        TypeParameter,
        Invalid,
        Delegate
    }
}
