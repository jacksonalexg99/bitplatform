﻿using BitCodeAnalyzer.SystemAnalyzers;
using BitCodeAnalyzer.SystemCodeFixes;
using BitCodeAnalyzer.Test.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace BitCodeAnalyzer.Test.SystemAnalyzers
{
    [TestClass]
    public class ClassWithoutModifierAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        [TestCategory("Analyzer")]
        public async Task FindClassesWithoutModifier()
        {
            const string sourceCodeWithClassWithoutModifier = @"
    namespace MyNamespace
    {
        public class MyClass
        {   

        }

        class MyClass
        {   

        }
    }";
            DiagnosticResult classWithoutModifier = new DiagnosticResult
            {
                Id = nameof(ClassWithoutModifierAnalyzer),
                Message = ClassWithoutModifierAnalyzer.Message,
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation(9, 9) }
            };

            await VerifyCSharpDiagnostic(sourceCodeWithClassWithoutModifier, classWithoutModifier);
        }

        [TestMethod]
        [TestCategory("CodeFixProvider")]
        public async Task AddPublicModifierTest()
        {
            const string sourceCodeWithClassWithoutModifier = @"
    namespace MyNamespace
    {
        public class MyClass
        {   

        }

        class MyClass
        {   

        }
    }";

            const string fixedSourceCode = @"
    namespace MyNamespace
    {
        public class MyClass
        {   

        }

      public class MyClass
        {   

        }
    }";

            await VerifyCSharpFix(sourceCodeWithClassWithoutModifier, fixedSourceCode);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new ClassWithoutModifierCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ClassWithoutModifierAnalyzer();
        }
    }
}
