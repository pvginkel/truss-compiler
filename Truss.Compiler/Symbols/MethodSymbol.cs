using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;

namespace Truss.Compiler.Symbols {
    public abstract class MethodSymbol : MemberSymbol {
        public abstract MethodKind MethodKind { get; }

        public override SymbolKind Kind {
            get { return SymbolKind.Method; }
        }

        public override SymbolModifier Access {
            get { return Modifiers & SymbolModifier.AccessModifiers; }
        }

        public bool IsAbstract {
            get { return Modifiers.HasFlag(SymbolModifier.Abstract); }
        }

        public bool IsAsync {
            get { return Modifiers.HasFlag(SymbolModifier.Async); }
        }

        public bool IsExtern {
            get { return Modifiers.HasFlag(SymbolModifier.Extern); }
        }

        public bool IsNew {
            get { return Modifiers.HasFlag(SymbolModifier.New); }
        }

        public bool IsOverride {
            get { return Modifiers.HasFlag(SymbolModifier.Override); }
        }

        public bool IsSealed {
            get { return Modifiers.HasFlag(SymbolModifier.Sealed); }
        }

        public bool IsStatic {
            get { return Modifiers.HasFlag(SymbolModifier.Static); }
        }

        public bool IsVirtual {
            get { return Modifiers.HasFlag(SymbolModifier.Virtual); }
        }

        public SymbolModifier Modifiers { get; private set; }

        public ImmutableArray<TypeParameterSymbol> TypeParameters { get; internal set; }

        public ImmutableArray<ParameterSymbol> Parameters { get; internal set; } 

        public TypeSymbol ReturnType { get; private set; }

        protected MethodSymbol(NamedTypeSymbol declaringType, SymbolModifier modifiers, TypeSymbol returnType)
            : base(declaringType) {
            if (declaringType == null) {
                throw new ArgumentNullException("declaringType");
            }
            if (returnType == null) {
                throw new ArgumentNullException("returnType");
            }
            Debug.Assert((modifiers & SymbolModifier.AccessModifiers) != 0);

            Modifiers = modifiers;
            ReturnType = returnType;
        }
    }
}
