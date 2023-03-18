using NUnit.Framework;
using StringExtractLib.Options;
using System;

namespace StringExtractLib.Tests
{
    public class MemoryStringReaderShould
    {
        private IntPtr _dummyHandle;

        [SetUp]
        public void Setup()
        {
            _dummyHandle = new IntPtr(16);
        }

        [Test]
        public void BeCreateable()
        {
            var memorySource = new MemorySource(_dummyHandle, 0, 16);
            var options = new StringReaderOptions();

            Assert.DoesNotThrow(() =>
            {
                _ = new MemoryStringReader(memorySource);
                _ = new MemoryStringReader(memorySource, options);
            });
        }

        [Test]
        public void AssignPropertiesProperly()
        {
            var memorySource = new MemorySource(_dummyHandle, 0, 16);
            var options = new StringReaderOptions(3, 16, StringType.Utf16);
            var reader = new MemoryStringReader(memorySource, options);

            Assert.AreEqual(reader.Options, options);
            Assert.AreEqual(reader.Source, memorySource);
        }

        [Test]
        [TestCase(2, 0)]
        [TestCase(-1, 6)]
        [TestCase(-1, -5)]
        public void ThrowForInvalidMemorySource(int address, int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new MemorySource(_dummyHandle, address, length);
            });
        }

        [Test]
        public void CreateMemorySourceProperly()
        {
            var memorySource = new MemorySource(_dummyHandle, 2, 16);

            Assert.AreEqual(memorySource.Handle, _dummyHandle);
            Assert.AreEqual(memorySource.Address, 2);
            Assert.AreEqual(memorySource.Length, 16);
        }
    }
}
