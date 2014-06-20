using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public abstract class MemberSymbol : Symbol {
        public abstract SymbolModifier Access { get; }

        public NamedTypeSymbol DeclaringType { get; private set; }

        protected MemberSymbol(NamedTypeSymbol declaringType) {
            if (declaringType == null) {
                throw new ArgumentNullException("declaringType");
            }

            DeclaringType = declaringType;
        }
    }
}
