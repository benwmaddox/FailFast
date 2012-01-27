using System;

namespace FailFast
{
    public class FailFastTest 
    {
        public string TestName { get; set; }
        public Action TestAction { get; set; }

        public FailFastTest(string testName, Action testAction)
        {
            TestName = testName;
            TestAction = testAction;
        }
    }
}