using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public class NakedNullableTypeSymbol : TypeSymbol {
        public static readonly NakedNullableTypeSymbol Nullable = new NakedNullableTypeSymbol(Nullability.Nullable, "?");
        public static readonly NakedNullableTypeSymbol NotNullable = new NakedNullableTypeSymbol(Nullability.NotNullable, "!");

        public Nullability Nullability { get; private set; }

        private NakedNullableTypeSymbol(Nullability nullability, string name)
            : base(name, name, null) {
            Nullability = nullability;
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNakedNullableType(this);
            }
        }

        public override TypeKind TypeKind {
            get { return TypeKind.NakedNullableType; }
        }
    }
}
