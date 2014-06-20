using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class ArgumentException : Exception {
        public ArgumentException(string message)
            : base(message) {
        }
    }
}
