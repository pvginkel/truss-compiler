using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.PreProcessor {
    public class PreProcessorStringStream : ANTLRStringStream {
        public PreProcessorStringStream(string input, IEnumerable<string> defines)
            : base(input) {
            Init(defines);
        }

        public PreProcessorStringStream(char[] data, int numberOfActualCharsInArray, IEnumerable<string> defines)
            : base(data, numberOfActualCharsInArray) {
            Init(defines);
        }

        private void Init(IEnumerable<string> defines) {
            data = PreProcessor.Process(data, n, defines);
            n = data.Length;
        }
    }
}
