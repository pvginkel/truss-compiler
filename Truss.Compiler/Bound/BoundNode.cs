using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Bound {
    public abstract class BoundNode {
        protected BoundNode(Span span) {
            if (span == null) {
                throw new ArgumentNullException("span");
            }

            Span = span;
        }

        public Span Span { get; private set; }

        public abstract BoundKind Kind { get; }

        public abstract void Accept(IBoundVisitor visitor);

        public abstract T Accept<T>(IBoundVisitor<T> visitor);
    }
}
