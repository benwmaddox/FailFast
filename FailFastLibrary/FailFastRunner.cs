using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FailFastLibrary;

namespace FailFast
{
    public class FailFastRunner
    {
        /// <summary>
        /// Slowest option
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> FindTestClassesFromPath(string path)
        {
            List<Assembly> assemblies = new List<Assembly>();
         
            if (Path.GetExtension(path) == ".dll" && !path.Contains("FailFastLibrary.dll"))
            {
                //shadow copy of assembly
                var newpath = Path.GetTempFileName();
                File.Copy(path, newpath,true);
                assemblies.Add(Assembly.LoadFile(newpath));
            }

            return assemblies.SelectMany(assembly => assembly.GetTypes().Where(type => type.BaseType == typeof(FailFastClass)));
        }

        public static IEnumerable<Type> FindTestClassesFromLoadedDirectory()
        {
            List<Assembly> assemblies = new List<Assembly>();
            var directory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            foreach (var filePath in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(filePath) == ".dll" && !filePath.Contains("FailFastLibrary.dll"))
                {
                    assemblies.Add(Assembly.LoadFile(filePath));
                }
            }

            return assemblies.SelectMany(assembly => assembly.GetTypes().Where(type => type.BaseType == typeof(FailFastClass)));
        }

        /// <summary>
        /// Fastest option, but requires declaring types in calling code
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Type> FindTestClassesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly => assembly.GetTypes().Where(type => type.BaseType == typeof(FailFastClass)));
        }

        public static IEnumerable<Type> FindTestClassesFromLoadedModules()
        {
            //Note: may only work if a reference is used in current assembly.
            var currentAssembly = Assembly.GetCallingAssembly();
            var assemblies = currentAssembly.GetReferencedAssemblies();
            IEnumerable<Assembly> loadAssemblies = assemblies.Select(Assembly.Load);
            return loadAssemblies.SelectMany(assembly => assembly.GetTypes().Where(type => type.BaseType == typeof (FailFastClass)));
        }

        public static RunResult RunTests(IEnumerable<FailFastClass> testClasses)
        {
            bool testFailed = false;
            string failedTestName = "";
            string stackTrace = "";
            string errorMessage = "";
            Exception exception = null;
            int testsRun = 0;
            foreach (var failFastClass in testClasses)
            {
                if (testFailed)
                    break;

                failFastClass.Define();

                foreach (var test in failFastClass.Tests)
                {
                    if (testFailed)
                        break;

                    try
                    {
                        test.TestAction.Invoke();
                        testsRun++;
                    }
                    catch (Exception ex)
                    {
                        testFailed = true;
                        failedTestName = test.TestName;
                        errorMessage = ex.Message;
                        exception = ex;
                        stackTrace = ex.StackTrace;
                    }
                }
                failFastClass.Tests.Clear();
            }


            string runMessage;
            
            if (testFailed)
            {
                runMessage = string.Format("{0} tests passed. {3}Test \"{1}\" failed.{3}{4}{3}{3}Trace: {2}",
                                       testsRun, failedTestName, stackTrace, Environment.NewLine, errorMessage);
            }
            else
            {
                runMessage = string.Format("All {0} tests passed.", testsRun);
            }

            var runResult = new RunResult()
                                {
                                    AllTestsPass = !testFailed,
                                    FailedTestException = exception,
                                    RunMessage = runMessage
                                };
            return runResult;
        }
    }

    public class RunResult
    {
        public bool AllTestsPass { get; set; }
        public Exception FailedTestException { get; set; }
        public string RunMessage { get; set; }
    }
}