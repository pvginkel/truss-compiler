using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class ErrorType {
        internal ErrorType(int number, Severity severity, string message) {
            Number = number;
            Severity = severity;
            Message = message;
        }

        public int Number { get; private set; }

        public Severity Severity { get; private set; }

        public string Message { get; private set; }
    }
}
