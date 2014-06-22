using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    public class Binder {
        public GlobalSymbol Bind(ErrorList errors, SyntaxLibrary library) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
            if (library == null) {
                throw new ArgumentNullException("library");
            }

            var manager = new SymbolManager();

            // Phase I: create symbols for all top level elements. This finds everything that does not
            // have a dependency on other symbols.

            foreach (var compilationUnit in library.CompilationUnits) {
                compilationUnit.Accept(new TopLevelSymbolBinder(errors, manager));
            }

            // Process all types. This parses the modifiers and caches the information on private fields.
            // We can't do this while binding because partial types may specify modifiers on different
            // files.

            manager.GlobalSymbol.Accept(new CompleteTypesWalker(errors));

            // Phase II: resolve all metadata symbols. This resolves all symbols that do not belong to
            // source code (statements/expressions). By now we should have a complete set of types,
            // so we should be good to go.

            foreach (var compilationUnit in library.CompilationUnits) {
                compilationUnit.Accept(new MetadataSymbolBinder(errors, manager));
            }

            // And sanity check the complete symbol tree. This validates whether the configuration of the
            // different symbols is valid. Some of this has already been done in the resolving phase. This e.g.
            // checks whether there are duplicate symbols, e.g. duplicate methods with the same signature.

            manager.GlobalSymbol.Accept(new SymbolTreeValidator(errors));

            // We're now ready to start binding bodies and attributes. If we have errors now, we bail, because it
            // would be very difficult to actually bind something if we're not in a valid state.

            if (errors.HasErrors) {
                return manager.GlobalSymbol;
            }

            // Phase III: resolve all bodies and attributes.

            foreach (var compilationUnit in library.CompilationUnits) {
                compilationUnit.Accept(new ContentBinder(errors, manager));
            }

            return manager.GlobalSymbol;
        }

        private class CompleteTypesWalker : SymbolTreeWalker {
            private readonly ErrorList _errors;

            public CompleteTypesWalker(ErrorList errors) {
                _errors = errors;
            }

            public override void VisitNamedType(NamedTypeSymbol symbol) {
                symbol.ParseModifiers(_errors);

                base.VisitNamedType(symbol);
            }
        }
    }
}
