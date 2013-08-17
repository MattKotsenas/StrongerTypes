using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongerTypes.Weak;

namespace UnitTests
{
    /// <summary>
    /// Summary description for WeakTests
    /// </summary>
    [TestClass]
    public class WeakTests
    {
        private class TestClass
        {
            public string Field { get { return "expected"; } }
        }

        [TestMethod]
        public void StrongRefTest()
        {
            TestClass expected = new TestClass();
            Weak<TestClass> weakRef = new Weak<TestClass>(expected);

            TestClass actual;
            Assert.IsTrue(weakRef.TryGetTarget(out actual));
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void WeakRefTest()
        {
            // This may not be a good test, as there is no fool-proof way to prevent GCs.
            // If this fails a lot we may have to remove it.
            Weak<TestClass> weakRef = new Weak<TestClass>(new TestClass());

            TestClass actual;
            Assert.IsTrue(weakRef.TryGetTarget(out actual));
            Assert.AreEqual((new TestClass()).Field, actual.Field);
        }

        [TestMethod]
        public void DeadTest()
        {
            Weak<TestClass> weakRef = new Weak<TestClass>(new TestClass());
            GC.Collect();

            TestClass actual;
            Assert.IsFalse(weakRef.TryGetTarget(out actual));
            Assert.IsNull(actual);
        }
    }
}
