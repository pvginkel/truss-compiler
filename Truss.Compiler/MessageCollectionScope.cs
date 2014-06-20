using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Truss.Compiler {
    public static class MessageCollectionScope {
        [ThreadStatic]
        private static MessageCollection _current;

        public static MessageCollection Current {
            get { return _current; }
        }

        public static void AddMessage(Message message) {
            if (message == null) {
                throw new ArgumentNullException("message");
            }

            var messages = Current;

            Debug.Assert(messages != null);

            messages.Add(message);
        }

        public static IDisposable Create(MessageCollection messages) {
            if (messages == null) {
                throw new ArgumentNullException("messages");
            }

            _current = messages;

            return new Handle();
        }

        private class Handle : IDisposable {
            private bool _disposed;

            public void Dispose() {
                if (!_disposed) {
                    _current = null;

                    _disposed = true;
                }
            }
        }
    }
}
