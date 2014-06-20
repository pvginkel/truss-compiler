using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    public class Binder {
        public GlobalSymbol Bind(SyntaxLibrary library) {
            if (library == null) {
                throw new ArgumentNullException("library");
            }

            var manager = new SymbolManager();

            // Phase I: create symbols for all top level elements. This finds everything that does not
            // have a dependency on other symbols.

            foreach (var compilationUnit in library.CompilationUnits) {
                compilationUnit.Accept(new TopLevelSymbolBinder(manager));
            }

            // Process all types. This parses the modifiers and caches the information on private fields.
            // We can't do this while binding because partial types may specify modifiers on different
            // files.

            manager.GlobalSymbol.Accept(new CompleteTypesWalker());

            // Phase II: resolve all metadata symbols. This resolves all symbols that do not belong to
            // source code (statements/expressions). By now we should have a complete set of types,
            // so we should be good to go.

            foreach (var compilationUnit in library.CompilationUnits) {
                compilationUnit.Accept(new MetadataSymbolBinder(manager));
            }

            return manager.GlobalSymbol;
        }

        private class CompleteTypesWalker : SymbolTreeWalker {
            public override void VisitType(TypeSymbol symbol) {
                symbol.ParseModifiers();

                base.VisitType(symbol);
            }
        }
    }
}
