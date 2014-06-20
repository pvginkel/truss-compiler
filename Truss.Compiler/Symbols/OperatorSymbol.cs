using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public class OperatorSymbol : OperatorSymbolBase {
        public Operator Operator { get; private set; }

        public OperatorSymbol(Operator @operator, NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol returnType)
            : base(declaringType, modifiers, returnType) {
            Operator = @operator;
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitOperator(this);
            }
        }

        public override MethodKind MethodKind {
            get { return MethodKind.Operator; }
        }
    }
}
