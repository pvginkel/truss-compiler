using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Support {
    public class Arguments {
        private readonly Dictionary<string, Argument> _arguments = new Dictionary<string, Argument>();

        public Arguments() {
            Remaining = new List<string>();
        }

        public void AddArgument(Argument argument) {
            if (argument == null) {
                throw new ArgumentNullException("argument");
            }
            if (_arguments.ContainsKey(argument.Option)) {
                throw new ArgumentException("Duplicate argument");
            }

            _arguments.Add(argument.Option, argument);
        }

        public void Parse(string[] args) {
            Argument parsing = null;

            foreach (string arg in args) {
                if (parsing != null) {
                    parsing.IsProvided = true;
                    parsing.SetValue(arg);
                    parsing = null;
                } else {
                    parsing = _arguments[arg];

                    if (parsing == null) {
                        Remaining.Add(arg);
                    } else {
                        if (!parsing.HasParameter) {
                            parsing.IsProvided = true;
                            parsing = null;
                        } else if (parsing.IsProvided && !parsing.AllowMultiple) {
                            throw new ArgumentException(string.Format("Option '{0}' can only appear once", arg));
                        }
                    }
                }
            }

            foreach (Argument argument in _arguments.Values) {
                if (argument.IsMandatory && !argument.IsProvided) {
                    throw new ArgumentException(string.Format("Option '{0}' is mandatory", argument.Option));
                }
            }
        }

        public List<string> Remaining { get; private set; }
    }
}
