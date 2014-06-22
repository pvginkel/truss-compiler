using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Generate {
    internal class Program {
        private static void Main(string[] args) {
            new Generator(args[0], GeneratorMode.Bound).Generate();
            new Generator(args[0], GeneratorMode.Syntax).Generate();
        }
    }
}
