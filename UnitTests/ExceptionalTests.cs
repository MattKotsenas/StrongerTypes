using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongerTypes.Exceptional;

namespace UnitTests
{
    [TestClass]
    public class ExceptionalTests
    {
        [TestMethod]
        public void SimpleConstructorTest()
        {
            int expected = 7;
            var ex = new Exceptional<int>(expected);

            Assert.IsFalse(ex.HasException);
            Assert.IsNull(ex.Exception);
            Assert.AreEqual(expected, ex.Value);
        }

        [TestMethod]
        public void FuncConstructorTest()
        {
            int expected = 7;
            var ex = new Exceptional<int>(() => expected);

            Assert.IsFalse(ex.HasException);
            Assert.IsNull(ex.Exception);
            Assert.AreEqual(expected, ex.Value);
        }

        [TestMethod]
        public void ExceptionTest()
        {
            var expected = typeof(ArgumentNullException);
            var ex = new Exceptional<string>(SampleMethod);

            Assert.IsTrue(ex.HasException);
            Assert.IsInstanceOfType(ex.Exception, expected);
            Assert.AreEqual(default(string), ex.Value);
        }

        private string SampleMethod()
        {
            throw new ArgumentNullException();
        }
    }
}
