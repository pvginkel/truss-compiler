using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;

namespace Truss.Compiler.Binding {
    // Validate the symbol tree. This verifies that the symbol tree is in a valid state so that we don't have to
    // take too much crap into account in the body binder.
    //
    // Here we validate the following:
    // * Modifiers on classes. This verifies whether access modifiers are valid (e.g. no public inheriting from
    //   internal) and whether the abstract/sealed/static/readonly modifiers are valid;
    // * Method names. This verifies whether there are duplicate method names with the same signature. This resolves
    //   generated method names (e.g. accessors and operators) and validates these too;
    // * Modifiers on methods. This verifies whether method modifiers are correct. This verifies e.g. whether
    //   the class is abstract if the method is, whether all methods are static when the class is, etc.
    //
    // Things we don't check (because they've already been checked):
    // * Static on operators;
    // * Extension method must be in a static class;
    // * Duplicate types in a namespace. However, we do check whether nested types conflict with member names;
    // * Duplicate type parameter names.
    internal partial class SymbolTreeValidator : AbstractSymbolVisitor {
        private readonly ErrorList _errors;

        public SymbolTreeValidator(ErrorList errors) {
            if (errors == null) {
                throw new ArgumentNullException("errors");
            }

            _errors = errors;
        }

        public override void DefaultVisit(Symbol symbol) {
            var containerSymbol = symbol as ContainerSymbol;
            if (containerSymbol != null) {
                foreach (var member in containerSymbol.Members) {
                    member.Accept(this);
                }
            }
        }

        public override void VisitNamedType(NamedTypeSymbol symbol) {
            // TODO: Accessibility && member modifiers.

            ValidateMemberSignatures(symbol);
        }
    }
}
