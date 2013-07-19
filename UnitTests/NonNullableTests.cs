using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongerTypes.NonNullable;

namespace UnitTests
{
    [TestClass]
    public class NonNullableTests
    {
        private class SampleClass
        {
            int SampleField
            {
                get { return 7; }
            }
        }

        [TestMethod]
        public void InstantiationTest()
        {
            var sample = new SampleClass();
            var test = new NonNullable<SampleClass>(sample);

            Assert.AreEqual(sample, test.Value);
        }

        [TestMethod]
        public void NullInstantiationTest()
        {
            bool exceptionThrown = false;

            try
            {
                var test = new NonNullable<SampleClass>(null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "ArgumentNullException is expected when instantiating with a null instance");
        }

        [TestMethod]
        public void ParameterlessConstructorTest()
        {
            bool exceptionThrown = false;
            var test = new NonNullable<SampleClass>();

            try
            {
                var temp = test.Value;
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "InvalidOperationException is expected when creating a NonNullable via the parameterless contrustor");
        }
    }
}
