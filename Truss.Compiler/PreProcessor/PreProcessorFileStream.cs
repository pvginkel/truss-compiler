using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.PreProcessor {
    public class PreProcessorFileStream : ANTLRFileStream {
        public PreProcessorFileStream(string fileName, IEnumerable<string> defines)
            : base(fileName) {
            Init(defines);
        }

        public PreProcessorFileStream(string fileName, Encoding encoding, IEnumerable<string> defines)
            : base(fileName, encoding) {
            Init(defines);
        }

        private void Init(IEnumerable<string> defines) {
            data = PreProcessor.Process(data, n, defines);
            n = data.Length;
        }
    }
}
