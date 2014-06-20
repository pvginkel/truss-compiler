using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Support {
    internal static class EnumExtensions {
        public static bool HasSingleFlag(this Enum self, params Enum[] flags) {
            return CountFlags(self, flags) == 1;
        }

        public static bool HasMultipleFlags(this Enum self, params Enum[] flags) {
            return CountFlags(self, flags) > 1;
        }

        public static int CountFlags(this Enum self, params Enum[] flags) {
            if (flags == null) {
                throw new ArgumentNullException("flags");
            }

            int count = 0;

            foreach (var flag in flags) {
                if (Equals(flag, self)) {
                    count++;
                }
            }

            return count;
        }
    }
}
