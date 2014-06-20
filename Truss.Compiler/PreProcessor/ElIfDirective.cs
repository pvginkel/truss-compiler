using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.PreProcessor {
    internal class ElIfDirective : IDirective {
        public ElIfDirective(bool expression) {
            Expression = expression;
        }

        public bool Expression { get; private set; }

        public DirectiveKind Kind {
            get { return DirectiveKind.ElIf; }
        }
    }
}
