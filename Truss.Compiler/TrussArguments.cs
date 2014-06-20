using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truss.Compiler.Support;

namespace Truss.Compiler {
    public class TrussArguments : Arguments {
        private readonly ArgumentFlag _noCorLib = new ArgumentFlag("--no-corlib", "Do not link corlib.dar");
        private readonly ArgumentOption _output = new ArgumentOption("-o", "Output file name", true);
        private readonly ArgumentMultiple _link = new ArgumentMultiple("-l", "DTL libraries to link against", false);

        public TrussArguments(string[] args) {
            AddArgument(_noCorLib);
            AddArgument(_output);
            AddArgument(_link);

            Parse(args);
        }

        public bool NoCorLib {
            get { return _noCorLib.IsProvided; }
        }

        public string Output {
            get { return _output.Value; }
        }

        public List<string> Link {
            get { return _link.Values; }
        }

        public List<string> Files {
            get { return Remaining; }
        }
    }
}
