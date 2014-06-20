using System;
using System.Collections.Generic;
using System.Text;

namespace Truss.Compiler.Binding {
    internal static class ResolveModeExtensions {
        public static bool AllowType(this ResolveMode self) {
            return self == ResolveMode.Type || self == ResolveMode.TypeOrNamespace;
        }

        public static bool AllowNamespace(this ResolveMode self) {
            return self == ResolveMode.Namespace || self == ResolveMode.TypeOrNamespace;
        }
    }
}