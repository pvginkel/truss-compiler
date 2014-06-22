﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetBrains.Annotations {
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class StringFormatMethodAttribute : Attribute {
        public StringFormatMethodAttribute(string formatParameterName) {
            FormatParameterName = formatParameterName;
        }

        public string FormatParameterName { get; private set; }
    }
}
