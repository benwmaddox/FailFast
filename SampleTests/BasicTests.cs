using FailFast;
using FailFastLibrary;

namespace SampleTests
{
    public class BasicTests : FailFastClass
    {
        public override void Define()
        {
            Test("I can run", () => { });

            Test("I can run an assertion", () => AssertEquals(2, 1 + 1));

            for (int i = 0; i < 100000; i++)
                Test("Extra tests", () => AssertEquals(6, 2*3));

        }
    }
}