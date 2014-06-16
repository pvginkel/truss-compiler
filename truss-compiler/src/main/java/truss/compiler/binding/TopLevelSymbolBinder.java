package truss.compiler.binding;

import org.apache.commons.lang.Validate;
import truss.compiler.symbols.ContainerSymbol;
import truss.compiler.symbols.NamespaceSymbol;
import truss.compiler.symbols.TypeSymbol;
import truss.compiler.syntax.*;

import java.util.Stack;

class TopLevelSymbolBinder extends SyntaxTreeWalker {
    private final SymbolManager manager;
    private final Stack<ContainerSymbol> stack = new Stack<>();

    public TopLevelSymbolBinder(SymbolManager manager) {
        Validate.notNull(manager, "manager");

        this.manager = manager;
        stack.push(manager.getGlobalSymbol());
    }

    @Override
    public void visitDelegateDeclaration(DelegateDeclarationSyntax syntax) throws Exception {
        manager.put(
            syntax,
            TypeSymbol.fromDelegate(syntax, stack.peek())
        );
    }

    @Override
    public void visitEnumDeclaration(EnumDeclarationSyntax syntax) throws Exception {
        manager.put(
            syntax,
            TypeSymbol.fromEnum(syntax, stack.peek())
        );
    }

    @Override
    public void visitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) throws Exception {
        ContainerSymbol symbol = NamespaceSymbol.fromDeclaration(
            syntax,
            stack.peek()
        );

        manager.put(syntax, symbol);
        stack.push(symbol);

        super.visitNamespaceDeclaration(syntax);

        stack.pop();
    }

    @Override
    public void visitTypeDeclaration(TypeDeclarationSyntax syntax) throws Exception {
        ContainerSymbol symbol = TypeSymbol.fromType(
            syntax,
            stack.peek()
        );

        manager.put(syntax, symbol);
        stack.push(symbol);

        super.visitTypeDeclaration(syntax);

        stack.pop();
    }
}
