using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongerTypes.ReadOnly;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace UnitTests
{
    [TestClass]
    public class ReadOnlyTests
    {
        [TestMethod]
        public void ArrayIndexSetTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                rod["key1"] = "new value";
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via array index did not throw an exception when it should");
        }

        [TestMethod]
        public void AsReadOnlyTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = source.AsReadOnly();

            Assert.IsInstanceOfType(rod, typeof(ReadOnlyDictionary<string, string>));
        }

        [TestMethod]
        public void DictionaryAddTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                ((IDictionary<string, string>)rod).Add("key3", "value3");
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via array index did not throw an exception when it should");
        }

        [TestMethod]
        public void DictionaryRemoveTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                ((IDictionary<string, string>)rod).Remove("key1");
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via array index did not throw an exception when it should");
        }

        [TestMethod]
        public void DictionaryClearTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                ((IDictionary<string, string>)rod).Clear();
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via array index did not throw an exception when it should");
        }

        [TestMethod]
        public void KeyAddTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                rod.Keys.Add("key3");
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via dictionary.Keys.Add() did not throw an exception when it should");
        }

        [TestMethod]
        public void KeyRemoveTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                rod.Keys.Remove("key1");
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via dictionary.Keys.Add() did not throw an exception when it should");
        }

        [TestMethod]
        public void ValuesAddTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                rod.Values.Add("value3");
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via dictionary.Keys.Add() did not throw an exception when it should");
        }

        [TestMethod]
        public void ValuesRemoveTest()
        {
            var source = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
            var rod = new ReadOnlyDictionary<string, string>(source);

            bool exceptionThrown = false;
            try
            {
                rod.Values.Remove("value1");
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Setting via dictionary.Keys.Add() did not throw an exception when it should");
        }

        [TestMethod]
        public void SerializationTest()
        {
            var expected = (new Dictionary<int, int>() { { 1, 2 } }).AsReadOnly();

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(ReadOnlyDictionary<int, int>));
                serializer.WriteObject(stream, expected);

                // Reset stream to beginning
                stream.Flush();
                stream.Position = 0;

                var actual = (ReadOnlyDictionary<int, int>)serializer.ReadObject(stream);

                foreach (var key in expected.Keys)
                {
                    Assert.AreEqual(expected[key], actual[key]);
                }
            }
        }
    }
}
