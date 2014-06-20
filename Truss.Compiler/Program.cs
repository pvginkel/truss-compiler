using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class Program {
        public static int Main(string[] args) {
            try {
                var arguments = new TrussArguments(args);

                // TODO: Not yet implements.

                Debug.Assert(arguments.NoCorLib);
                Debug.Assert(arguments.Link.Count == 0);

                return new Builder(arguments).Build();
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);

                return -1;
            }
        }
    }
}
