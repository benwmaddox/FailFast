using System;
using System.Collections.Generic;

namespace FailFast
{
    public abstract class FailFastClass
    {
        public abstract void Define();
        public Queue<FailFastTest> Tests { get; set; }

        protected FailFastClass()
        {
            Tests = new Queue<FailFastTest>();
        }

        protected void Test(string testName, Action testAction)
        {
            Tests.Enqueue(new FailFastTest(testName, testAction));
        }

        protected void IgnoreUntil(string testName, Action testAction, DateTime ignoreExpiration)
        {
            if (DateTime.Now > ignoreExpiration)
            {
                Test(testName, testAction);
            }
            else if (DateTime.Now.AddYears(1) < ignoreExpiration)
            {
                Test(testName,
                     () => {
                             throw new ArgumentException(
                                 string.Format("{0} was ignored until {1}. You can't delay forever." +
                                               Environment.NewLine + "Try a date within a year.",
                                               testName, ignoreExpiration));
                         });
            }
        }

        protected void AssertEquals(int expected, int actual)
        {
            if (expected != actual)
                throw new ArgumentException(string.Format("Failed Assertion. Expected {0}. Got {1}.", expected.ToString(), actual.ToString()));
        }
    }
}