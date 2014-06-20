using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Syntax {
    public abstract class SyntaxNode {
        protected SyntaxNode(Span span) {
            if (span == null) {
                throw new ArgumentNullException("span");
            }

            Span = span;
        }

        public abstract SyntaxKind Kind { get; }

        public Span Span { get; private set; }

        public abstract void Accept(ISyntaxVisitor visitor);

        public abstract T Accept<T>(ISyntaxVisitor<T> visitor);
    }
}
