using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.PreProcessor {
    internal enum DirectiveKind {
        Define,
        ElIf,
        Else,
        Endif,
        If
    }
}
