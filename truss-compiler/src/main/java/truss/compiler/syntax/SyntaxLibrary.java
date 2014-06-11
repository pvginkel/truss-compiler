package truss.compiler.syntax;

import truss.compiler.CollectionUtils;

import java.util.List;

public class SyntaxLibrary {
    private final List<CompilationUnitSyntax> compilationUnits;

    public SyntaxLibrary(List<CompilationUnitSyntax> compilationUnits) {
        this.compilationUnits = CollectionUtils.copyList(compilationUnits);
    }

    public List<CompilationUnitSyntax> getCompilationUnits() {
        return compilationUnits;
    }
}
