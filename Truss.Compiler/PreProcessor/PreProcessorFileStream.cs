using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.PreProcessor {
    public class PreProcessorFileStream : ANTLRFileStream {
        public PreProcessorFileStream(ErrorList errors, string fileName, IEnumerable<string> defines)
            : base(fileName) {
            Init(errors, defines);
        }

        public PreProcessorFileStream(ErrorList errors, string fileName, Encoding encoding, IEnumerable<string> defines)
            : base(fileName, encoding) {
            Init(errors, defines);
        }

        private void Init(ErrorList errors, IEnumerable<string> defines) {
            data = PreProcessor.Process(errors, data, n, defines);
            n = data.Length;
        }
    }
}
