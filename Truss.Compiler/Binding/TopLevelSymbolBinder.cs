using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal class TopLevelSymbolBinder : SyntaxTreeWalker {
        private readonly SymbolManager _manager;
        private readonly Stack<ContainerSymbol> _stack = new Stack<ContainerSymbol>();

        public TopLevelSymbolBinder(SymbolManager manager) {
            if (manager == null) {
                throw new ArgumentNullException("manager");
            }

            _manager = manager;
            _stack.Push(manager.GlobalSymbol);
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            _manager.Add(
                syntax,
                TypeSymbol.FromDelegate(syntax, _stack.Peek())
            );
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            _manager.Add(
                syntax,
                TypeSymbol.FromEnum(syntax, _stack.Peek())
            );
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) {
            var symbol = NamespaceSymbol.FromDeclaration(
                syntax,
                _stack.Peek()
            );

            _manager.Add(syntax, symbol);
            _stack.Push(symbol);

            base.VisitNamespaceDeclaration(syntax);

            _stack.Pop();
        }

        public override void VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            var symbol = TypeSymbol.FromType(
                syntax,
                _stack.Peek()
            );

            _manager.Add(syntax, symbol);
            _stack.Push(symbol);

            base.VisitTypeDeclaration(syntax);

            _stack.Pop();
        }
    }
}
