using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongerTypes.NonNullable;
using System.Runtime.Serialization;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class NonNullableTests
    {
        [Serializable]
        internal class SampleClass
        {
            public int SampleField { get; set; }
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

        [TestMethod]
        public void UnwrapConverstionTest()
        {
            NonNullable<SampleClass> nonNullable = new NonNullable<SampleClass>(new SampleClass());
            SampleClass implicitlyConverted = nonNullable;

            Assert.IsNotNull(implicitlyConverted);
        }

        [TestMethod]
        public void WrapConverstionTest()
        {
            SampleClass myClass = new SampleClass();
            NonNullable<SampleClass> nonNullable = (NonNullable<SampleClass>)myClass;

            Assert.AreSame(myClass, nonNullable.Value);
        }

        [TestMethod]
        public void WrapNullConverstionTest()
        {
            bool exceptionThrown = false;

            try
            {
                NonNullable<SampleClass> nonNullable = (NonNullable<SampleClass>)null;
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void SerializationTest()
        {
            int expectedVal = 15;

            var sample = new SampleClass();
            sample.SampleField = expectedVal;

            var expected = new NonNullable<SampleClass>(sample);

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(NonNullable<SampleClass>));
                serializer.WriteObject(stream, expected);

                // Reset stream to beginning
                stream.Flush();
                stream.Position = 0;

                var actual = (NonNullable<SampleClass>)serializer.ReadObject(stream);

                Assert.AreEqual(expectedVal, actual.Value.SampleField);
            }
        }
    }
}
