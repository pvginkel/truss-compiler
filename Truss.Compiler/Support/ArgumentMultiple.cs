using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Support {
    public class ArgumentMultiple : Argument {
        public ArgumentMultiple(string option, string description, bool mandatory)
            : base(option, description, mandatory) {
            Values = new List<string>();
        }

        public override bool AllowMultiple {
            get { return true; }
        }

        public override bool HasParameter {
            get { return true; }
        }

        public override void SetValue(string value) {
            Values.Add(value);
        }

        public List<string> Values { get; private set; }
    }
}
