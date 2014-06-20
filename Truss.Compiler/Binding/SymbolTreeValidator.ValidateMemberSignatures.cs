using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Symbols;

namespace Truss.Compiler.Binding {
    partial class SymbolTreeValidator {
        private void ValidateMemberSignatures(NamedTypeSymbol type) {
            // Validate member names. The following is checked:
            // * Member names may not be equal to the containing type name;
            // * Nested type, event, field, property and type parameter names must be unique including method names.
            //   Duplicate method names (i.e. overloads) are only allowed if there is no other type of member.

            var names = new Dictionary<string, List<Symbol>>();

            foreach (var member in type.Members) {
                // Don't add constructors, destructors, indexers and operators. These don't have names we need to check.

                if (ShouldValidateName(member)) {
                    AddMember(names, member.Name, member);
                }

                // Add generated names.

                switch (member.Kind) {
                    case SymbolKind.Event:
                        var eventMember = (EventSymbol)member;
                        if (eventMember.AddMethod != null) {
                            AddMember(names, NameUtils.GetEventAccessorName(eventMember.AddMethod), eventMember.AddMethod);
                        }
                        if (eventMember.RemoveMethod != null) {
                            AddMember(names, NameUtils.GetEventAccessorName(eventMember.RemoveMethod), eventMember.RemoveMethod);
                        }
                        break;

                    case SymbolKind.IndexerProperty:
                    case SymbolKind.Property:
                        var propertyMember = (PropertySymbol)member;
                        if (propertyMember.GetMethod != null) {
                            AddMember(names, NameUtils.GetPropertyAccessorName(propertyMember.GetMethod), propertyMember.GetMethod);
                        }
                        if (propertyMember.SetMethod != null) {
                            AddMember(names, NameUtils.GetPropertyAccessorName(propertyMember.SetMethod), propertyMember.SetMethod);
                        }
                        break;

                    case SymbolKind.Method:
                        var methodMember = (MethodSymbol)member;
                        switch (methodMember.MethodKind) {
                            case MethodKind.ConversionOperator:
                                AddMember(names, NameUtils.GetConversionOperatorName((ConversionOperatorSymbol)member), member);
                                break;

                            case MethodKind.Operator:
                                AddMember(names, NameUtils.GetOperatorName((OperatorSymbol)member), methodMember);
                                break;
                        }
                        break;
                }
            }

            foreach (var typeParameter in type.TypeParameters) {
                AddMember(names, typeParameter.Name, typeParameter);
            }

            foreach (var item in names) {
                var symbols = item.Value;

                bool anyConflictingName = false;
                int methodCount = 0;

                if (item.Key == type.Name) {
                    foreach (var member in symbols) {
                        if (!IsConstructorOrDestructor(member)) {
                            anyConflictingName = true;
                            break;
                        }
                    }
                } else {
                    if (symbols.Count == 1) {
                        continue;
                    }

                    // If there is any non method member, we have a problem.


                    foreach (var member in symbols) {
                        if (member.Kind != SymbolKind.Method) {
                            anyConflictingName = true;
                        } else {
                            methodCount++;
                        }
                    }
                }

                if (anyConflictingName) {
                    foreach (var member in symbols) {
                        _errors.Add(Error.DuplicateIdentifier, member.Spans);
                    }
                    continue;
                }

                if (methodCount <= 1) {
                    continue;
                }

                var reported = new HashSet<Symbol>();

                for (int i = 0; i < symbols.Count; i++) {
                    var a = symbols[i];

                    if (a.Kind != SymbolKind.Method) {
                        continue;
                    }

                    for (int j = 0; j < i; j++) {
                        var b = symbols[j];

                        if (
                            b.Kind == SymbolKind.Method &&
                            SignaturesEqual((MethodSymbol)a, (MethodSymbol)b)
                        ) {
                            if (!reported.Contains(a)) {
                                reported.Add(a);
                                _errors.Add(Error.DuplicateSignature, a.Spans);
                            }
                            if (!reported.Contains(b)) {
                                reported.Add(b);
                                _errors.Add(Error.DuplicateSignature, b.Spans);
                            }
                        }
                    }
                }
            }
        }

        private static bool SignaturesEqual(MethodSymbol a, MethodSymbol b) {
            if (a.TypeParameters.Count != b.TypeParameters.Count || a.Parameters.Count != b.Parameters.Count) {
                return false;
            }

            for (int i = 0; i < a.Parameters.Count; i++) {
                if (ParameterSignatureEqual(a.Parameters[i], b.Parameters[i])) {
                    return true;
                }
            }

            return false;
        }

        private static bool ParameterSignatureEqual(ParameterSymbol a, ParameterSymbol b) {
            if (a.ParameterType != b.ParameterType) {
                return false;
            }

            // Filter out the params and this modifier. This don't affect the real signature; just how the
            // methods can be used.

            var aModifiers = a.Modifiers & ~(ParameterModifier.Params | ParameterModifier.This);
            var bModifiers = b.Modifiers & ~(ParameterModifier.Params | ParameterModifier.This);

            return aModifiers == bModifiers;
        }

        private static bool IsConstructorOrDestructor(Symbol member) {
            var method = member as MethodSymbol;
            if (method == null) {
                return false;
            }
            return method.MethodKind == MethodKind.Constructor || method.MethodKind == MethodKind.Destructor;
        }

        private static bool ShouldValidateName(Symbol member) {
            switch (member.Kind) {
                case SymbolKind.Event:
                case SymbolKind.Field:
                case SymbolKind.Property:
                case SymbolKind.Type:
                    return true;

                case SymbolKind.Method:
                    switch (((MethodSymbol)member).MethodKind) {
                        case MethodKind.MemberMethod:
                        case MethodKind.Constructor:
                        case MethodKind.Destructor:
                            return true;
                    }
                    break;
            }

            return false;
        }

        private static void AddMember(Dictionary<string, List<Symbol>> names, string name, Symbol member) {
            List<Symbol> symbols;
            if (!names.TryGetValue(name, out symbols)) {
                symbols = new List<Symbol>();
                names.Add(name, symbols);
            }

            symbols.Add(member);
        }
    }
}
