using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Truss.Compiler.Symbols;
using Truss.Compiler.Syntax;

namespace Truss.Compiler.Binding {
    internal abstract class ContainerScope : Scope {
        private readonly List<Import> _imports;

        public new ContainerSymbol Symbol {
            get { return (ContainerSymbol)base.Symbol; }
        }

        protected ContainerScope(ContainerSymbol symbol, List<Import> imports, Scope parent)
            : base(symbol, parent) {
            _imports = imports;
        }

        public override ContainerSymbol ResolveContainer(NameSyntax name, ResolveMode mode) {
            var results = ResolveContainersForSymbol(Symbol, name, mode, _imports);

            if (results != null) {
                if (results.Count > 1) {
                    Errors.Add(Error.AmbiguousContainerSymbolMatch, name.Span, NameUtils.PrettyPrint(name));
                    return null;
                }

                return results[0];
            }

            return base.ResolveContainer(name, mode);
        }

        protected List<ContainerSymbol> ResolveContainersForSymbol(ContainerSymbol symbol, NameSyntax name, ResolveMode mode, List<Import> imports) {
            return ResolveContainersForSymbol(symbol, name, mode, imports, true);
        }

        private List<ContainerSymbol> ResolveContainersForSymbol(ContainerSymbol symbol, NameSyntax name, ResolveMode mode, List<Import> imports, bool isTail) {
            List<ContainerSymbol> parents;

            if (name is QualifiedNameSyntax) {
                // If this level is a qualified name, first resolve the left part.

                var qualifiedName = (QualifiedNameSyntax)name;
                parents = ResolveContainersForSymbol(symbol, qualifiedName.Left, mode, imports, false);
                if (parents.Count == 0) {
                    return null;
                }

                // Extract the tail out of the qualified name and continue with that.

                name = qualifiedName.Right;

                // Import matching is only for the top level.

                imports = null;
            } else if (name is AliasQualifiedNameSyntax) {
                var aliasQualifiedName = (AliasQualifiedNameSyntax)name;

                // Currently we only support the global alias. Later we could add support for linking libraries
                // under an alias.

                if (WellKnownNames.AliasGlobal != aliasQualifiedName.Alias.Identifier) {
                    Errors.Add(Error.InvalidAlias, aliasQualifiedName.Alias.Span, aliasQualifiedName.Alias.Identifier);
                    return null;
                }

                name = aliasQualifiedName.Name;

                parents = new List<ContainerSymbol> {
                    GetGlobalSymbol()
                };
            } else {
                parents = new List<ContainerSymbol> {
                    symbol
                };
            }

            // By now we should have ended up with a simple name.

            Debug.Assert(name is SimpleNameSyntax);

            // Find all matching container symbols in all matching children.

            var simpleName = (SimpleNameSyntax)name;
            string metadataName = NameUtils.GetMetadataName(simpleName);

            var result = new List<ContainerSymbol>();

            foreach (var parent in parents) {
                foreach (var member in parent.GetMemberByMetadataName(metadataName)) {
                    if (IsSymbolValidForMode(member, mode, isTail)) {
                        result.Add((ContainerSymbol)member);
                    }
                }
            }

            // If we have imports, match those too.

            if (imports != null) {
                string identifier = simpleName.Identifier;

                foreach (var import in imports) {
                    ContainerSymbol member = null;

                    if (import.Alias != null) {
                        // If we're matching an alias, the alias will be against the full name. So, when the
                        // alias is against a generic type, the symbol of the alias will be the generic
                        // type. This means that the matching name may not be a generic type and we can
                        // only match identifier names here. Identifier names in this context will be
                        // either a type name without generic arguments, or the first part of a namespace.
                        // Both are fine (isSymbolValidForMode will make sure that we only add valid symbols).

                        if (
                            simpleName is IdentifierNameSyntax &&
                            identifier == import.Alias
                            ) {
                            member = import.Symbol;
                        }
                    } else {
                        if (metadataName == import.Symbol.MetadataName) {
                            member = import.Symbol;
                        }
                    }

                    if (member != null && IsSymbolValidForMode(member, mode, isTail)) {
                        result.Add(member);
                    }
                }
            }

            if (result.Count > 0) {
                return result;
            }

            return null;
        }

        private static bool IsSymbolValidForMode(Symbol member, ResolveMode mode, bool isTail) {
            // If we're at the tail, we need to return something that is valid for the mode.
            // Otherwise, we can just add all matching containers.

            if (!isTail) {
                return member is ContainerSymbol;
            }

            return
                (member is NamedTypeSymbol && mode.AllowType()) ||
                (member.Kind == SymbolKind.Namespace && mode.AllowNamespace());
        }
    }
}