package truss.compiler.binding;

import org.apache.commons.lang.Validate;
import truss.compiler.symbols.GlobalSymbol;
import truss.compiler.symbols.SymbolTreeWalker;
import truss.compiler.symbols.TypeSymbol;
import truss.compiler.syntax.CompilationUnitSyntax;
import truss.compiler.syntax.SyntaxLibrary;

public class Binder {
    public GlobalSymbol bind(SyntaxLibrary library) throws Exception {
        Validate.notNull(library, "library");

        SymbolManager manager = new SymbolManager();

        // Phase I: create symbols for all top level elements. This finds everything that does not
        // have a dependency on other symbols.

        for (CompilationUnitSyntax compilationUnit : library.getCompilationUnits()) {
            compilationUnit.accept(new TopLevelSymbolBinder(manager));
        }

        // Process all types. This parses the modifiers and caches the information on private fields.
        // We can't do this while binding because partial types may specify modifiers on different
        // files.

        manager.getGlobalSymbol().accept(new SymbolTreeWalker() {
            @Override
            public void visitType(TypeSymbol symbol) {
                symbol.parseModifiers();

                super.visitType(symbol);
            }
        });

        // Phase II: resolve all metadata symbols. This resolves all symbols that do not belong to
        // source code (statements/expressions). By now we should have a complete set of types,
        // so we should be good to go.

        for (CompilationUnitSyntax compilationUnit : library.getCompilationUnits()) {
            compilationUnit.accept(new MetadataSymbolBinder(manager));
        }

        return manager.getGlobalSymbol();
    }
}
