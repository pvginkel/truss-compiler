using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Truss.Compiler.Test {
    public interface ITestCaseCallback {
        void Test(String name, String fileName, String code, String expected);
    }
}
