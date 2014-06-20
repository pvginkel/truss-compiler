using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Binding;

namespace Truss.Compiler.Symbols {
    public class ParameterSymbol : Symbol {
        private readonly string _name;

        public TypeSymbol ParameterType { get; private set; }

        public override string Name {
            get { return _name; }
        }

        public bool IsThis {
            get { return Modifiers.HasFlag(ParameterModifier.This); }
        }

        public bool IsRef {
            get { return Modifiers.HasFlag(ParameterModifier.Ref); }
        }

        public bool IsOut {
            get { return Modifiers.HasFlag(ParameterModifier.Out); }
        }

        public bool IsParams {
            get { return Modifiers.HasFlag(ParameterModifier.Params); }
        }

        public bool IsConsumes {
            get { return Modifiers.HasFlag(ParameterModifier.Consumes); }
        }

        public ParameterModifier Modifiers { get; private set; }

        public ParameterSymbol(ParameterModifier modifiers, string name, TypeSymbol parameterType) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (parameterType == null) {
                throw new ArgumentNullException("parameterType");
            }

            _name = name;
            Modifiers = modifiers;
            ParameterType = parameterType;
        }

        public override SymbolKind Kind {
            get { return SymbolKind.Parameter; }
        }

        public override void Accept(ISymbolVisitor visitor) {
            if (!visitor.Done) {
                visitor.VisitParameter(this);
            }
        }
    }
}
