using System;
using System.Collections.Generic;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal class GlobalScope : ContainerScope {
        private readonly GlobalSymbol _symbol;
        private readonly ErrorList _errors;

        public override ErrorList Errors {
            get { return _errors; }
        }
        
        public GlobalScope(ErrorList errors, GlobalSymbol symbol, List<Import> imports)
            : base(symbol, imports, null) {
            _errors = errors;
            _symbol = symbol;
        }

        public override GlobalSymbol GetGlobalSymbol() {
            return _symbol;
        }

        public override ContainerSymbol ResolveContainer(NameSyntax name, ResolveMode mode) {
            var result = base.ResolveContainer(name, mode);

            if (result == null) {
                Errors.Add(Error.CannotResolveName, name.Span, NameUtils.PrettyPrint(name));

                if (mode.AllowType()) {
                    return new InvalidTypeSymbol(_symbol);
                }
                if (mode.AllowNamespace()) {
                    return new InvalidNamespaceSymbol(_symbol);
                }

                throw new InvalidOperationException();
            }

            return result;
        }
    }
}