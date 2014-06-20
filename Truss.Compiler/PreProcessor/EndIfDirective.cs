using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.PreProcessor {
    internal class EndIfDirective : IDirective {
        public DirectiveKind Kind {
            get { return DirectiveKind.Endif; }
        }
    }
}
