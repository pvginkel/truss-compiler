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

        public static NamespaceSymbol FromDeclaration(ErrorList errors, NamespaceDeclarationSyntax syntax, ContainerSymbol container) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }
            if (syntax == null) {
                throw new ArgumentNullException("syntax");
            }
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            return CreateChain(errors, syntax.Name, container);
        }

        private static NamespaceSymbol CreateChain(ErrorList errors, NameSyntax name, ContainerSymbol container) {
            if (name is QualifiedNameSyntax) {
                var qualifiedName = (QualifiedNameSyntax)name;
                container = CreateChain(errors, qualifiedName.Left, container);
                name = qualifiedName.Right;
            }

            if (!(name is IdentifierNameSyntax)) {
                errors.Add(Error.InvalidNamespaceIdentifier, name.Span);

                return null;
            }

            string identifier = ((IdentifierNameSyntax)name).Identifier;

            NamespaceSymbol symbol;
            if (!container.TryGetMemberByMetadataName(identifier, out symbol)) {
                symbol = new NamespaceSymbol(identifier, container);

                container.AddMember(symbol);
            }

            return symbol;
        }
    }
}
