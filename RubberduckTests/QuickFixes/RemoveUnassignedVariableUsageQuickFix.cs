﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rubberduck.Inspections.Concrete;
using Rubberduck.Inspections.QuickFixes;
using RubberduckTests.Mocks;

namespace RubberduckTests.QuickFixes
{
    [TestClass]
    public class RemoveUnassignedVariableUsageQuickFixTests
    {

        [TestMethod]
        [TestCategory("QuickFixes")]
        public void UnassignedVariableUsage_QuickFixWorks()
        {
            const string inputCode =
@"Sub Foo()
    Dim b As Boolean
    Dim bb As Boolean
    bb = b
End Sub";

            const string expectedCode =
@"Sub Foo()
    Dim b As Boolean
    Dim bb As Boolean
    
End Sub";

            var vbe = MockVbeBuilder.BuildFromSingleStandardModule(inputCode, out var component);
            var state = MockParser.CreateAndParse(vbe.Object);

            var inspection = new UnassignedVariableUsageInspection(state);
            var inspectionResults = inspection.GetInspectionResults();

            new RemoveUnassignedVariableUsageQuickFix(state).Fix(inspectionResults.First());
            Assert.AreEqual(expectedCode, state.GetRewriter(component).GetText());
        }
    }
}
