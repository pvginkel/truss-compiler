using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class ErrorList {
        private readonly List<Error> _messages= new List<Error>();

        public ErrorList() {
            Messages = new ReadOnlyCollection<Error>(_messages);
        }

        public bool HasErrors {
            get { return Errors > 0; }
        }

        public int Errors { get; private set; }

        public bool HasErrorsOrWarnings {
            get { return Errors > 0 || Warnings > 0; }
        }

        public int Warnings { get; private set; }

        public bool HasMessages {
            get { return _messages.Count > 0; }
        }

        public int Infos { get; private set; }

        public IList<Error> Messages { get; private set; }

        public void Add(Error error) {
            if (error == null) {
                throw new ArgumentNullException("error");
            }

            _messages.Add(error);

            switch (error.Type.Severity) {
                case Severity.Error:
                    Errors++;
                    break;
                case Severity.Warn:
                    Warnings++;
                    break;
                case Severity.Info:
                    Infos++;
                    break;
            }
        }

        public void Add(ErrorType type, params object[] args) {
            Add(new Error(type, args));
        }

        public void Add(ErrorType type, string fileName, params object[] args) {
            Add(new Error(type, fileName, args));
        }

        public void Add(ErrorType type, Span span, params object[] args) {
            Add(new Error(type, span, args));
        }

        public void Add(ErrorType type, IEnumerable<Span> spans, params object[] args) {
            if (spans == null) {
                throw new ArgumentNullException("spans");
            }

            foreach (var span in spans) {
                Add(new Error(type, span, args));
            }
        }

        public override string ToString() {
            var sb = new StringBuilder();

            foreach (var message in _messages) {
                sb.Append(message);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
