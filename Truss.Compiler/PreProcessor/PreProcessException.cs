using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.PreProcessor {
    internal class PreProcessException : Exception {
        public PreProcessException(string message, int line)
            : base(message) {
            Line = line;
        }

        public int Line { get; private set; }
    }
}
