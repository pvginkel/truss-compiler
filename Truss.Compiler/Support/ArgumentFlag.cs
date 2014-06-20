using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Support {
    public class ArgumentFlag : Argument {
        public ArgumentFlag(string option, string description)
            : base(option, description, false) {
        }

        public override bool AllowMultiple {
            get { return false; }
        }

        public override bool HasParameter {
            get { return false; }
        }

        public override void SetValue(string value) {
            throw new InvalidOperationException();
        }
    }
}
