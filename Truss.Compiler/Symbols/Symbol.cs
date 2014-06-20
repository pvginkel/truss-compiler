using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Symbols {
    public abstract class Symbol {
        private readonly List<Span> _spans = new List<Span>();

        protected Symbol() {
            Spans = new ReadOnlyCollection<Span>(_spans);
        }

        protected void AddSpan(Span span) {
            if (span == null) {
                throw new ArgumentNullException("span");
            }

            _spans.Add(span);
        }

        public IList<Span> Spans { get; private set; }

        public virtual string Name {
            get { return null; }
        }

        public virtual string MetadataName {
            get { return Name; }
        }

        public abstract SymbolKind Kind { get; }

        public abstract void Accept(ISymbolVisitor visitor);
    }
}
