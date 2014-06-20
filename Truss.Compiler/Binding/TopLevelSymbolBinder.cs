using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal class TopLevelSymbolBinder : SyntaxTreeWalker {
        private readonly ErrorList _errors;
        private readonly SymbolManager _manager;
        private readonly Stack<ContainerSymbol> _stack = new Stack<ContainerSymbol>();

        public TopLevelSymbolBinder(ErrorList errors, SymbolManager manager) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
            if (manager == null) {
                throw new ArgumentNullException("manager");
            }

            _errors = errors;
            _manager = manager;
            _stack.Push(manager.GlobalSymbol);
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax syntax) {
            _manager.Add(
                syntax,
                TypeSymbol.FromDelegate(_errors, syntax, _stack.Peek())
            );
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax syntax) {
            _manager.Add(
                syntax,
                TypeSymbol.FromEnum(_errors, syntax, _stack.Peek())
            );
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax syntax) {
            var symbol = NamespaceSymbol.FromDeclaration(_errors, syntax, _stack.Peek());

            _manager.Add(syntax, symbol);
            _stack.Push(symbol);

            base.VisitNamespaceDeclaration(syntax);

            _stack.Pop();
        }

        public override void VisitTypeDeclaration(TypeDeclarationSyntax syntax) {
            var symbol = TypeSymbol.FromType(_errors, syntax, _stack.Peek());

            _manager.Add(syntax, symbol);
            _stack.Push(symbol);

            base.VisitTypeDeclaration(syntax);

            _stack.Pop();
        }
    }
}
