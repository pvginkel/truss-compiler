using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.PreProcessor {
    internal class DefineDirective : IDirective {
        public DefineDirective(string identifier, bool define) {
            if (identifier == null) {
                throw new ArgumentNullException("identifier");
            }

            Identifier = identifier;
            IsDefine = define;
        }

        public string Identifier { get; private set; }

        public bool IsDefine { get; private set; }

        public DirectiveKind Kind {
            get { return DirectiveKind.Define; }
        }
    }
}
