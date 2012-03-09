using System;
using FailFast;
using FailFastLibrary;
using Should;

namespace SampleTests
{
    public class FailingTests : FailFastClass
    {
        public override void Define()
        {
            //Test("Bad Math Test", () => (41 + 1).ShouldEqual(5));
            //Test("I can fail", () => { throw new NotImplementedException(); });
            //Test("Failed so bad the test runner doesn't even know.", () => { throw new NotImplementedException(); });
        }
    }
}