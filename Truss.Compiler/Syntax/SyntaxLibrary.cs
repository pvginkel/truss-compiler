using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;

namespace Truss.Compiler.Syntax {
    public class SyntaxLibrary {
        public SyntaxLibrary(ImmutableArray<CompilationUnitSyntax> compilationUnits) {
            if (compilationUnits == null) {
                throw new ArgumentNullException("compilationUnits");
            }

            CompilationUnits = compilationUnits;
        }

        public ImmutableArray<CompilationUnitSyntax> CompilationUnits { get; private set; }
    }
}
