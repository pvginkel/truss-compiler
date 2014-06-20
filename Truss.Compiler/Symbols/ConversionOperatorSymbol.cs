using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public class ConversionOperatorSymbol : OperatorSymbolBase {
        public ImplicitOrExplicit Type { get; private set; }

        public ConversionOperatorSymbol(ImplicitOrExplicit type, NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol returnType)
            : base(declaringType, modifiers, returnType) {
            Type = type;
        }

        public override MethodKind MethodKind {
            get { return MethodKind.ConversionOperator; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitConversionOperator(this);
            }
        }
    }
}
