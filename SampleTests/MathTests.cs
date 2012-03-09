using FailFast;
using FailFastLibrary;

namespace SampleTests
{
    public class MathTests : FailFastClass
    {
        public override void Define()
        {
            Test("Mutiplication Works", () => AssertEquals(15, 3*5));
            Test("Division Works", () => AssertEquals(15, 60 / 4));
        }
    }
}