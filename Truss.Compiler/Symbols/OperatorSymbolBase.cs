using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public abstract class OperatorSymbolBase : MethodSymbol {
        protected OperatorSymbolBase(NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol returnType)
            : base(declaringType, modifiers, returnType) {
        }
    }
}
