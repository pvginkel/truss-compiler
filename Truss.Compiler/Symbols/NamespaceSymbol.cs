using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Symbols {
    public class NamespaceSymbol : ContainerSymbol {
        private readonly string _name;

        protected NamespaceSymbol(string name, ContainerSymbol container)
            : base(container) {
            _name = name;
        }

        public override string Name {
            get { return _name; }
        }

        public override SymbolKind Kind {
            get { return SymbolKind.Namespace; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitNamespace(this);
            }
        }

        public static NamespaceSymbol FromDeclaration(NamespaceDeclarationSyntax syntax, ContainerSymbol container) {
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            var symbol = CreateChain(syntax.Name, container);

            symbol.AddSpan(syntax.Span);

            return symbol;
        }

        private static NamespaceSymbol CreateChain(NameSyntax name, ContainerSymbol container) {
            if (name is QualifiedNameSyntax) {
                var qualifiedName = (QualifiedNameSyntax)name;
                container = CreateChain(qualifiedName.Left, container);
                name = qualifiedName.Right;
            }

            if (!(name is IdentifierNameSyntax)) {
                MessageCollectionScope.AddMessage(new Message(
                    MessageType.InvalidNamespaceIdentifier,
                    name.Span
                ));

                return null;
            }

            string identifier = ((IdentifierNameSyntax)name).Identifier;

            foreach (var member in container.GetMemberByMetadataName(identifier)) {
                if (member is NamespaceSymbol) {
                    return (NamespaceSymbol)member;
                }
            }

            var symbol = new NamespaceSymbol(identifier, container);

            container.AddMember(symbol);

            return symbol;
        }
    }
}
