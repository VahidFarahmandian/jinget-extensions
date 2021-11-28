﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jinget.Core.CodeDom.Tests
{
    [TestClass()]
    public class JingetDynamicCodeTests
    {
        [TestMethod()]
        public void should_compile_and_execute_dynamic_code_at_runtime_void_parameterless()
        {
            string expectedSource = @"
using System;
namespace JingetDynamic {
    internal sealed class DynamicInvoker {
        public void DynamicInvoke() {
            int x = 2*2;
        }
    }
}
";
            string source = @"int x = 2*2";

            var result = new JingetDynamicCode().Execute(source, out List<string> errors, out string compiledSourceCode);

            Assert.IsNull(result);
            Assert.IsFalse(errors.Any());
            Assert.AreEqual(expectedSource.Trim(), compiledSourceCode);
        }

        [TestMethod]
        public void should_compile_and_execute_dynamic_code_at_runtime_int_parameterless()
        {
            int expectedResult = 4;
            string expectedSource = @"
using System;
namespace JingetDynamic {
    internal sealed class DynamicInvoker {
        public int DynamicInvoke() {
            return 2*2;
        }
    }
}
";
            string source = @"return 2*2";

            var result = new JingetDynamicCode().Execute(source, out List<string> errors, out string compiledSourceCode,
                new JingetDynamicCode.MethodOptions { ReturnType = typeof(int) });

            Assert.IsFalse(errors.Any());
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedSource.Trim(), compiledSourceCode);
        }

        [TestMethod]
        public void should_compile_and_execute_dynamic_code_at_runtime_void_parametric()
        {
            string expectedSource = @"
using System;
using System.Linq;
namespace JingetDynamic {
    internal sealed class DynamicInvoker {
        public void DynamicInvoke(int a, double b) {
            double c = a*b;
        }
    }
}
";
            string source = @"using System.Linq; double c = a*b;";

            var result = new JingetDynamicCode().Execute(source, out List<string> errors, out string compiledSourceCode,
                new JingetDynamicCode.MethodOptions
                {
                    Parameters = new List<JingetDynamicCode.MethodOptions.ParameterOptions>
                {
                    new JingetDynamicCode.MethodOptions.ParameterOptions {Name = "a",Type = typeof(int)},
                    new JingetDynamicCode.MethodOptions.ParameterOptions {Name = "b",Type = typeof(double)}
                }
                });

            Assert.IsFalse(errors.Any());
            Assert.IsNull(result);
            Assert.AreEqual(expectedSource.Trim(), compiledSourceCode);
        }

        [TestMethod]
        public void should_compile_and_return_error()
        {
            string source = @"c = a*b;";

            var result = new JingetDynamicCode().Execute(source, out List<string> errors, out string compiledSourceCode,
                new JingetDynamicCode.MethodOptions
                {
                    Parameters = new List<JingetDynamicCode.MethodOptions.ParameterOptions>
                {
                    new JingetDynamicCode.MethodOptions.ParameterOptions {Name = "a",Type = typeof(int)},
                    new JingetDynamicCode.MethodOptions.ParameterOptions {Name = "b",Type = typeof(double)}
                }
                });

            Assert.IsTrue(errors.Any());
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void should_throw_exception()
        {
            string source = "";
            for (int i = 0; i < 10001; i++) source += "c = a*b;";

            new JingetDynamicCode().Execute(source, out List<string> _, out string _);
        }
    }
}