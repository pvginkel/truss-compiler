using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Truss.Compiler.PreProcessor {
    public class PreProcessor {
        private readonly char[] _data;
        private readonly HashSet<string> _defines;
        private readonly Stack<State> _includes = new Stack<State>();
        private bool _include;

        public static char[] Process(char[] data, int size, IEnumerable<string> defines) {
            return new PreProcessor(data, size, defines)._data;
        }

        private PreProcessor(char[] data, int size, IEnumerable<string> defines) {
            if (data == null) {
                throw new ArgumentNullException("data");
            }

            _defines = new HashSet<string>(defines);
            _data = Parse(data, size);
        }

        private char[] Parse(char[] data, int size) {
            var result = new StringBuilder();
            var directive = new StringBuilder();

            bool hadNonSpace = false;
            bool hadStart = false;
            bool hadEnd = false;
            int line = 1;
            _include = true;

            for (int i = 0; i <= size; i++) {
                bool atEnd = i == size;
                char c = atEnd ? '\n' : data[i];

                // Always add the newline to out to preserve line count
                if ((c == '\r' || c == '\n') && !atEnd) {
                    result.Append(c);
                    hadNonSpace = false;
                }

                if (hadStart) {
                    if (c == '\n') {
                        ParseDirective(directive.ToString().TrimEnd('\r'), line);

                        hadStart = false;
                        hadEnd = false;

                        directive.Clear();
                    } else if (!hadEnd) {
                        if (c == '/' && i < data.Length - 1 && data[i + 1] == '/') {
                            hadEnd = true;
                        } else {
                            directive.Append(c);
                        }
                    }
                } else if (c != '\n' && c != '\r') {
                    if (c == '#' && !hadNonSpace) {
                        hadStart = true;
                    } else if (_include) {
                        result.Append(c);
                        if (!Char.IsWhiteSpace(c)) {
                            hadNonSpace = true;
                        }
                    }
                }

                if (c == '\n' && !atEnd) {
                    line++;
                }
            }

            if (_includes.Count > 0) {
                throw new PreProcessException("Missing #endif", line);
            }

            return result.ToString().ToCharArray();
        }

        private void ParseDirective(string input, int line) {
            var lexer = new TrussPreProcessorLexer(new ANTLRStringStream(input));
            var parser = new TrussPreProcessorParser(new CommonTokenStream(lexer));

            parser.Defines = _defines;

            try {
                AddDirective(parser.ParseDirective(), line);
            } catch (RecognitionException) {
                throw new PreProcessException("Cannot parse pre processor directive", line);
            }
        }

        private void AddDirective(IDirective directive, int line) {
            switch (directive.Kind) {
                case DirectiveKind.If:
                    _includes.Push(((IfDirective)directive).Expression ? State.Include : State.Exclude);

                    RecalculateIncludes();
                    break;

                case DirectiveKind.ElIf:
                    VerifyHadIf("#elif", line);

                    AdjustIncludes(((ElIfDirective)directive).Expression);

                    RecalculateIncludes();
                    break;

                case DirectiveKind.Else:
                    VerifyHadIf("#else", line);

                    AdjustIncludes(true);

                    RecalculateIncludes();
                    break;

                case DirectiveKind.Endif:
                    VerifyHadIf("#endif", line);

                    _includes.Pop();

                    RecalculateIncludes();
                    break;

                case DirectiveKind.Define:
                    if (_include) {
                        var define = (DefineDirective)directive;

                        if (define.IsDefine) {
                            _defines.Add(define.Identifier);
                        } else {
                            _defines.Remove(define.Identifier);
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void AdjustIncludes(bool expression) {
            if (_includes.Pop() == State.Exclude) {
                _includes.Push(expression ? State.Include : State.Exclude);
            } else {
                _includes.Push(State.HadInclude);
            }
        }

        private void RecalculateIncludes() {
            _include = true;

            foreach (var include in _includes) {
                if (!IsInclude(include)) {
                    _include = false;
                    return;
                }
            }
        }

        private void VerifyHadIf(string directive, int line) {
            if (_includes.Count == 0) {
                throw new PreProcessException(directive + " without a matching #if", line);
            }
        }

        private static bool IsInclude(State state) {
            return state == State.Include;
        }

        private enum State {
            Include,
            Exclude,
            HadInclude
        }
    }
}
