using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongerTypes.Exceptional;
using System.IO;
using System.Runtime.Serialization;

namespace UnitTests
{
    [TestClass]
    public class ExceptionalTests
    {
        [TestMethod]
        public void SimpleConstructorTest()
        {
            int expected = 7;
            var ex = new Exceptional<int, Exception>(expected);

            Assert.IsFalse(ex.HasException);
            Assert.IsNull(ex.Exception);
            Assert.AreEqual(expected, ex.Value);
        }

        [TestMethod]
        public void FuncConstructorTest()
        {
            int expected = 7;
            var ex = new Exceptional<int, Exception>(() => expected);

            Assert.IsFalse(ex.HasException);
            Assert.IsNull(ex.Exception);
            Assert.AreEqual(expected, ex.Value);
        }

        [TestMethod]
        public void ExceptionTest()
        {
            var expected = typeof(ArgumentNullException);
            var ex = new Exceptional<string, ArgumentNullException>(SampleMethod);

            Assert.IsTrue(ex.HasException);
            Assert.IsInstanceOfType(ex.Exception, expected);
            Assert.AreEqual(default(string), ex.Value);
        }

        [TestMethod]
        public void NewExceptionTest()
        {
            string expected = "test message";
            var ex = new Exceptional<string, DivideByZeroException>(new DivideByZeroException(expected));

            Assert.IsTrue(ex.HasException);
            Assert.AreEqual(expected, ex.Exception.Message);
        }

        [TestMethod]
        public void WrapConversionTest()
        {
            var sample = "test";
            Exceptional<string, Exception> ex = sample;

            Assert.AreEqual(sample, ex.Value);
        }

        [TestMethod]
        public void SerializationTest()
        {
            var expectedVal = "test123";
            var expected = new Exceptional<string, ArgumentNullException>(expectedVal);

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(Exceptional<string, ArgumentNullException>));
                serializer.WriteObject(stream, expected);

                // Reset stream to beginning
                stream.Flush();
                stream.Position = 0;

                var actual = (Exceptional<string, ArgumentNullException>)serializer.ReadObject(stream);

                Assert.AreEqual(expectedVal, actual.Value);
            }

            expected = new Exceptional<string, ArgumentNullException>(SampleMethod);

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(Exceptional<string, ArgumentNullException>));
                serializer.WriteObject(stream, expected);

                // Reset stream to beginning
                stream.Flush();
                stream.Position = 0;

                var actual = (Exceptional<string, ArgumentNullException>)serializer.ReadObject(stream);

                Assert.IsTrue(actual.HasException);
                Assert.IsInstanceOfType(actual.Exception, typeof(ArgumentNullException));
            }
        }

        private string SampleMethod()
        {
            throw new ArgumentNullException();
        }
    }
}
