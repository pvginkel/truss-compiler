using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public enum MethodKind {
        Constructor,
        ConversionOperator,
        Destructor,
        EventAccessor,
        MemberMethod,
        Operator,
        PropertyAccessor
    }
}
