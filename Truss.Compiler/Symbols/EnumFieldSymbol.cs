﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public class EnumFieldSymbol : FieldSymbol {
        public EnumFieldSymbol(NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol fieldType, string name)
            : base(declaringType, modifiers, fieldType, name) {
        }

        public override FieldKind FieldKind {
            get { return FieldKind.EnumField; }
        }
    }
}
