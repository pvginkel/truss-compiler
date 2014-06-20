using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Support {
    public abstract class Argument {
        protected Argument(string option, string description, bool mandatory) {
            if (option == null) {
                throw new ArgumentNullException("option");
            }
            if (description == null) {
                throw new ArgumentNullException("description");
            }

            Option = option;
            Description = description;
            IsMandatory = mandatory;
        }

        public string Option { get; private set; }

        public string Description { get; private set; }

        public bool IsProvided { get; set; }

        public bool IsMandatory { get; private set; }

        public abstract bool AllowMultiple { get; }

        public abstract bool HasParameter { get; }

        public abstract void SetValue(string value);
    }
}
