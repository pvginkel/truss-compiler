using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public class MessageCollection {
        private readonly List<Message> _messages= new List<Message>();

        public MessageCollection() {
            Messages = new ReadOnlyCollection<Message>(_messages);
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

        public IList<Message> Messages { get; private set; }

        public void Add(Message message) {
            if (message == null) {
                throw new ArgumentNullException("message");
            }

            _messages.Add(message);

            switch (message.Type.Severity) {
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

        public void Add(MessageType type, params object[] args) {
            Add(new Message(type, args));
        }

        public void Add(MessageType type, string fileName, params object[] args) {
            Add(new Message(type, fileName, args));
        }

        public void Add(MessageType type, Span span, params object[] args) {
            Add(new Message(type, span, args));
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
