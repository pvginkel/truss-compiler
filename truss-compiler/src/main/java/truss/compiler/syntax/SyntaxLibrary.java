package truss.compiler.syntax;

import org.apache.commons.lang.Validate;
import truss.compiler.support.ImmutableArray;

public class SyntaxLibrary {
    private final ImmutableArray<CompilationUnitSyntax> compilationUnits;

    public SyntaxLibrary(ImmutableArray<CompilationUnitSyntax> compilationUnits) {
        Validate.notNull(compilationUnits, "compilationUnits");

        this.compilationUnits = compilationUnits;
    }

    public ImmutableArray<CompilationUnitSyntax> getCompilationUnits() {
        return compilationUnits;
    }
}
