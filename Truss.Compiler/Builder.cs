using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class Builder {
        private readonly TrussArguments _arguments;

        public Builder(TrussArguments arguments) {
            if (arguments == null) {
                throw new ArgumentNullException("arguments");
            }

            _arguments = arguments;
        }

        public int Build() {
            return 0;
        }
    }
}
