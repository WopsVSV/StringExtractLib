using NUnit.Framework;
using StringExtractLib.Options;
using System;
using System.Linq;

namespace StringExtractLib.Tests
{
    public class ByteStringReaderShould
    {
        private const string Utf8String = "DUMMYUTF8";
        private const string Utf16String = "DUMMYUTF16";
        private const string DummyFile = "DummyFile.dll";

        private byte[]? _source;

        [SetUp]
        public void Setup()
        {
            _source = System.IO.File.ReadAllBytes(DummyFile);
        }

        [Test]
        public void BeCreateable()
        {
            Assert.DoesNotThrow(() =>
            {
                _ = new ByteStringReader(_source!, new StringReaderOptions());
            });
        }

        [Test]
        public void ReadAllStrings()
        {
            var reader = new ByteStringReader(_source!);

            var strings = reader.ReadAll();

            Assert.IsNotEmpty(strings);
            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsTrue(strings.Contains(Utf16String));
        }

        [Test]
        public void ReadOnlyUtf8Strings()
        {
            var reader = new ByteStringReader(
                _source!,
                new StringReaderOptions(stringType: StringType.Utf8));

            var strings = reader.ReadAll();

            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsFalse(strings.Contains(Utf16String));
        }

        [Test]
        public void ReadStringsWithFileStringReaderOptions()
        {
            var options = new FileStringReaderOptions(stringType: StringType.Utf8);
            var reader = new ByteStringReader(_source!);

            var strings = reader.ReadAll(options);

            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsFalse(strings.Contains(Utf16String));
        }

        [Test]
        public void ReadStringsWithStringReaderOptions()
        {
            var options = new StringReaderOptions(minimumLength: 6, stringType: StringType.Utf8);
            var reader = new ByteStringReader(_source!);

            var strings = reader.ReadAll(options);

            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsFalse(strings.Contains(Utf16String));
            Assert.That(strings.All(s => s.Length >= 6));
        }

        [Test]
        public void ReadOnlyUtf16Strings()
        {
            var reader = new ByteStringReader(
                _source!,
                new StringReaderOptions(stringType: StringType.Utf16));

            var strings = reader.ReadAll();

            Assert.IsTrue(strings.Contains(Utf16String));
            Assert.IsFalse(strings.Contains(Utf8String));
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(8)]
        public void ReadStringsWithMinimumLength(int minimumSize)
        {
            var reader = new ByteStringReader(
              _source!,
              new StringReaderOptions(minimumLength: minimumSize));

            var strings = reader.ReadAll();

            Assert.That(strings.All(s => s.Length >= minimumSize));
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(8)]
        public void ReadStringsWithMaximumLength(int maximumSize)
        {
            var reader = new ByteStringReader(
              _source!,
              new StringReaderOptions(maximumLength: maximumSize));

            var strings = reader.ReadAll();

            Assert.That(strings.All(s => s.Length <= maximumSize));
        }

        [Test]
        [TestCase(3, 8, StringType.Both)]
        [TestCase(4, 12, StringType.Both)]
        [TestCase(1, 100, StringType.Utf16)]
        [TestCase(10, 10, StringType.Utf8)]
        [TestCase(15, null, StringType.Both)]
        [TestCase(4, 5, StringType.Utf8)]
        [TestCase(1, 7, StringType.Utf8)]
        [TestCase(8, 8, StringType.Both)]
        public void ReadStringsWithMixedSettings(int minimumSize, int? maximumSize, StringType stringType)
        {
            var reader = new ByteStringReader(
              _source!,
              new StringReaderOptions(minimumSize, maximumSize, stringType));

            var strings = reader.ReadAll();

            Assert.IsNotEmpty(strings);
            Assert.That(strings.All(s => s.Length >= minimumSize));

            if (maximumSize.HasValue)
                Assert.That(strings.All(s => s.Length <= maximumSize));
        }
    }
}
