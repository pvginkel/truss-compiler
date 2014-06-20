using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Binding {
    [Flags]
    public enum ParameterModifier {
        None = 0,
        This = 1 << 0,
        Ref = 1 << 1,
        Out = 1 << 2,
        Params = 1 << 3,
        Consumes = 1 << 4
    }
}
