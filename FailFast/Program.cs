using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SampleTests;

namespace FailFast
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            var testClasses = new List<FailFastClass>();
            var types = FailFastRunner.FindTestClassesFromAssemblies(new List<Assembly>(){Assembly.GetAssembly(typeof(BasicTests))});
            //Testing many classes
            for (int i = 0; i < 41; i++)
            {
                testClasses.AddRange(types.Select(t => Activator.CreateInstance(t) as FailFastClass));
            }
            stopWatch.Stop();
            Console.WriteLine("Test Class Lookup: {0} seconds", stopWatch.ElapsedMilliseconds / 1000m);
            stopWatch.Reset();

            stopWatch.Start();
            var runResult = FailFastRunner.RunTests(testClasses);
            stopWatch.Stop();
            Console.WriteLine(string.Format("Test Run Time: {0} seconds", stopWatch.ElapsedMilliseconds / 1000m));
            Console.WriteLine(runResult.RunMessage);

            Console.ReadLine();
        }
    }
}
