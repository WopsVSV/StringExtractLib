using NUnit.Framework;
using StringExtractLib.Options;
using System.Linq;

namespace StringExtractLib.Tests
{
    public class FileStringReaderShould
    {
        private const string Utf8String = "DUMMYUTF8";
        private const string Utf16String = "DUMMYUTF16";
        private const string DummyFile = "DummyFile.dll";

        [Test]
        public void BeCreateable()
        {
            Assert.DoesNotThrow(() =>
            {
                _ = new FileStringReader(DummyFile, new FileStringReaderOptions());
            });
        }

        [Test]
        public void ReadAllStrings()
        {
            var reader = new FileStringReader(DummyFile);
            
            var strings = reader.ReadAll();

            Assert.IsNotEmpty(strings);
            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsTrue(strings.Contains(Utf16String));
        }

        [Test]
        public void ReadOnlyUtf8Strings()
        {
            var reader = new FileStringReader(
                DummyFile,
                new FileStringReaderOptions(stringType: StringType.Utf8));

            var strings = reader.ReadAll();

            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsFalse(strings.Contains(Utf16String));
        }

        [Test]
        public void ReadStringsWithFileStringReaderOptions()
        {
            var options = new FileStringReaderOptions(stringType: StringType.Utf8);
            var reader = new FileStringReader(DummyFile);

            var strings = reader.ReadAll(options);

            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsFalse(strings.Contains(Utf16String));
        }

        [Test]
        public void ReadStringsWithStringReaderOptions()
        {
            var options = new StringReaderOptions(minimumLength: 6, stringType: StringType.Utf8);
            var reader = new FileStringReader(DummyFile);

            var strings = reader.ReadAll(options);

            Assert.IsTrue(strings.Contains(Utf8String));
            Assert.IsFalse(strings.Contains(Utf16String));
            Assert.That(strings.All(s => s.Length >= 6));
        }

        [Test]
        public void ReadOnlyUtf16Strings()
        {
            var reader = new FileStringReader(
                DummyFile,
                new FileStringReaderOptions(stringType: StringType.Utf16));

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
            var reader = new FileStringReader(
              DummyFile,
              new Options.FileStringReaderOptions(minimumSize: minimumSize));

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
            var reader = new FileStringReader(
              DummyFile,
              new FileStringReaderOptions(maximumSize: maximumSize));

            var strings = reader.ReadAll();

            Assert.That(strings.All(s => s.Length <= maximumSize));
        }

        [Test]
        [TestCase(3, 8, StringType.Both, 4096)]
        [TestCase(4, 12, StringType.Both, null)]
        [TestCase(1, 100, StringType.Utf16, 500)]
        [TestCase(10, 10, StringType.Utf8, 8192)]
        [TestCase(15, null, StringType.Both, 1000)]
        [TestCase(4, 5, StringType.Utf8, null)]
        [TestCase(1, 7, StringType.Utf8, 16)]
        [TestCase(8, 8, StringType.Both, 9)]
        public void ReadStringsWithMixedSettings(int minimumSize, int? maximumSize, StringType stringType, int? chunkSize)
        {
            var reader = new FileStringReader(
              DummyFile,
              new FileStringReaderOptions(minimumSize, maximumSize, stringType, chunkSize));

            var strings = reader.ReadAll();

            Assert.IsNotEmpty(strings);
            Assert.That(strings.All(s => s.Length >= minimumSize));

            if(maximumSize.HasValue)
                Assert.That(strings.All(s => s.Length <= maximumSize));
        }
    }
}