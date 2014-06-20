using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Support {
    public class ArgumentOption : Argument {
        public ArgumentOption(string option, string description, bool mandatory)
            : base(option, description, mandatory) {
        }

        public override bool AllowMultiple {
            get { return false; }
        }

        public override bool HasParameter {
            get { return true; }
        }

        public string Value { get; private set; }

        public override void SetValue(string value) {
            Value = value;
        }
    }
}
