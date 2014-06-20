using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.PreProcessor {
    internal class ElseDirective : IDirective {
        public DirectiveKind Kind {
            get { return DirectiveKind.Else; }
        }
    }
}
