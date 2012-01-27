using System;

namespace FailFast
{
    public class FailingTests : FailFastClass
    {
        public override void Define()
        {
            //Test("Bad Math Test", () => AssertEquals(5, 41 + 1));
            //Test("I can fail", () => { throw new NotImplementedException(); });
            //Test("Failed so bad the test runner doesn't even know.", () => { throw new NotImplementedException(); });
        }
    }
}