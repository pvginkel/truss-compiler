using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.PreProcessor {
    public class PreProcessorStringStream : ANTLRStringStream {
        public PreProcessorStringStream(ErrorList errors, string input, IEnumerable<string> defines)
            : base(input) {
            Init(errors, defines);
        }

        public PreProcessorStringStream(ErrorList errors, char[] data, int numberOfActualCharsInArray, IEnumerable<string> defines)
            : base(data, numberOfActualCharsInArray) {
            Init(errors, defines);
        }

        private void Init(ErrorList errors, IEnumerable<string> defines) {
            data = PreProcessor.Process(errors, data, n, defines);
            n = data.Length;
        }
    }
}
