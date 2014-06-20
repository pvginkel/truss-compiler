using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public enum TypeKind {
        ArrayType,
        Class,
        Delegate,
        Enum,
        Interface,
        Invalid,
        NakedNullableType,
        NullableType,
        Struct,
        Terminator,
        TrackedType,
        TypeParameter,
        VarType
    }
}
