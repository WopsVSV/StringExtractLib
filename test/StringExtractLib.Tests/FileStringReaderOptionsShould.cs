using NUnit.Framework;
using System;

namespace StringExtractLib.Tests
{
    public class FileStringReaderOptionsShould
    {
        [Test]
        public void GetCreatedWithDefaults()
        {
            var defaultOptions = new FileStringReaderOptions();

            Assert.AreEqual(defaultOptions.MinimumLength, 1);
            Assert.AreEqual(defaultOptions.MaximumLength, null);
            Assert.AreEqual(defaultOptions.SearchedStringType, StringType.Both);
            Assert.AreEqual(defaultOptions.ChunkSize, 4096);
        }

        [Test]
        public void GetConstructedFromStringReaderOptions()
        {
            var stringReaderOptions = new StringReaderOptions()
            {
                MinimumLength = 3,
                MaximumLength = 12,
                SearchedStringType = StringType.Utf8
            };
            var fileStringReaderOptions = new FileStringReaderOptions(stringReaderOptions);

            Assert.AreEqual(fileStringReaderOptions.MinimumLength, 3);
            Assert.AreEqual(fileStringReaderOptions.MaximumLength, 12);
            Assert.AreEqual(fileStringReaderOptions.SearchedStringType, StringType.Utf8);
            Assert.AreEqual(fileStringReaderOptions.ChunkSize, new FileStringReaderOptions().ChunkSize);
        }

        [Test]
        public void ThrowWhenConstructedWithInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var options = new FileStringReaderOptions(1, 6, StringType.Both, 0);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var options = new FileStringReaderOptions(8, 12, StringType.Both, 8);
            });
        }
    }
}
