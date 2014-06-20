using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal static class SymbolModifierExtensions {
        public static SymbolModifier ToSymbolModifier(this Modifier self) {
            switch (self) {
                case Modifier.Abstract:
                    return SymbolModifier.Abstract;
                case Modifier.Async:
                    return SymbolModifier.Async;
                case Modifier.Extern:
                    return SymbolModifier.Extern;
                case Modifier.Internal:
                    return SymbolModifier.Internal;
                case Modifier.New:
                    return SymbolModifier.New;
                case Modifier.Override:
                    return SymbolModifier.Override;
                case Modifier.Partial:
                    return SymbolModifier.Partial;
                case Modifier.Private:
                    return SymbolModifier.Private;
                case Modifier.Protected:
                    return SymbolModifier.Protected;
                case Modifier.Public:
                    return SymbolModifier.Public;
                case Modifier.Readonly:
                    return SymbolModifier.Readonly;
                case Modifier.Sealed:
                    return SymbolModifier.Sealed;
                case Modifier.Static:
                    return SymbolModifier.Static;
                case Modifier.Tracked:
                    return SymbolModifier.Tracked;
                case Modifier.Virtual:
                    return SymbolModifier.Virtual;
                case Modifier.Volatile:
                    return SymbolModifier.Volatile;
                default:
                    throw new ArgumentOutOfRangeException("self");
            }
        }
    }
}
