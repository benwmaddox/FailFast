using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FailFast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleTests;

namespace TestWrapper
{
    [TestClass]
    public class FailFastWrapper
    {
        [TestMethod]
        public void FailFast()
        {
            //Change to include your tests assemblies.
            var assemblies = new List<Assembly>() {Assembly.GetAssembly(typeof (BasicTests))};


            var testClasses = new List<FailFastClass>();
            var types = FailFastRunner.FindTestClassesFromAssemblies(assemblies);
            testClasses.AddRange(types.Select(t => Activator.CreateInstance(t) as FailFastClass));

            var runResult = FailFastRunner.RunTests(testClasses);
            if (!runResult.AllTestsPass)
            {
                throw new Exception(runResult.RunMessage, runResult.FailedTestException);
            }
        }
    }
}
