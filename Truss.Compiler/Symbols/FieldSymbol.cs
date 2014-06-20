using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public abstract class FieldSymbol : MemberSymbol {
        private readonly string _name;

        public override SymbolKind Kind {
            get { return SymbolKind.Field; }
        }

        public abstract FieldKind FieldKind { get; }

        public SymbolModifier Modifiers { get; private set; }

        public bool IsNew {
            get { return Modifiers.HasFlag(SymbolModifier.New); }
        }

        public bool IsReadonly {
            get { return Modifiers.HasFlag(SymbolModifier.Readonly); }
        }

        public bool IsStatic {
            get { return Modifiers.HasFlag(SymbolModifier.Static); }
        }

        public bool IsVirtual {
            get { return Modifiers.HasFlag(SymbolModifier.Virtual); }
        }

        public bool IsVolatile {
            get { return Modifiers.HasFlag(SymbolModifier.Volatile); }
        }

        public override SymbolModifier Access {
            get { return Modifiers & SymbolModifier.AccessModifiers; }
        }

        public override string Name {
            get { return _name; }
        }

        public TypeSymbol FieldType { get; private set; }

        protected FieldSymbol(NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol fieldType, string name)
            : base(declaringType) {
            if (fieldType == null) {
                throw new ArgumentNullException("fieldType");
            }
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            Debug.Assert((modifiers & SymbolModifier.AccessModifiers) != 0);

            Modifiers = modifiers;
            FieldType = fieldType;
            _name = name;
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitField(this);
            }
        }
    }
}
