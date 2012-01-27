using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FailFast
{
    public class FailFastRunner
    {
        public static IEnumerable<Type> FindTestClassesFromAssembly()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.BaseType == typeof(FailFastClass));
        }

        public static string RunTests(IEnumerable<FailFastClass> testClasses)
        {
            bool testFailed = false;
            string failedTestName = "";
            string stackTrace = "";
            string errorMessage = "";
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
                        stackTrace = ex.StackTrace;
                    }
                }
                failFastClass.Tests.Clear();
            }


            string result;
            
            if (testFailed)
            {
                result = string.Format("{0} tests passed. {3}Test \"{1}\" failed.{3}{4}{3}{3}Trace: {2}",
                                       testsRun, failedTestName, stackTrace, Environment.NewLine, errorMessage);
            }
            else
            {
                result = string.Format("All {0} tests passed.", testsRun);
            }
            return result;
        }
    }
}