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

        [TestMethod]
        public void NewExceptionTest()
        {
            string expected = "test message";
            var ex = new Exceptional<string>(new DivideByZeroException(expected));

            Assert.IsTrue(ex.HasException);
            Assert.AreEqual(expected, ex.Exception.Message);
        }

        [TestMethod]
        public void WrapConversionTest()
        {
            var sample = "test";
            Exceptional<string> ex = sample;

            Assert.AreEqual(sample, ex.Value);
        }

        private string SampleMethod()
        {
            throw new ArgumentNullException();
        }
    }
}
