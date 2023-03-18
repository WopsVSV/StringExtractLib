using NUnit.Framework;
using StringExtractLib.Options;
using System;

namespace StringExtractLib.Tests
{
    public class StringReaderOptionsShould
    {
        [Test]
        public void GetCreatedWithDefaults()
        {
            var defaultOptions = new StringReaderOptions();

            Assert.AreEqual(defaultOptions.MinimumLength, 1);
            Assert.AreEqual(defaultOptions.MaximumLength, null);
            Assert.AreEqual(defaultOptions.SearchedStringType, StringType.Both);
        }

        [Test]
        public void GetCreatedProperlyWithAnyConstructor()
        {
            var defaultOptions = new StringReaderOptions();

            var options1 = new StringReaderOptions(5);

            Assert.AreEqual(options1.MinimumLength, 5);
            Assert.AreEqual(options1.MaximumLength, defaultOptions.MaximumLength);
            Assert.AreEqual(options1.SearchedStringType, defaultOptions.SearchedStringType);

            var options2 = new StringReaderOptions(7, 12);

            Assert.AreEqual(options2.MinimumLength, 7);
            Assert.AreEqual(options2.MaximumLength, 12);
            Assert.AreEqual(options2.SearchedStringType, defaultOptions.SearchedStringType);

            var options3 = new StringReaderOptions(3, null, StringType.Utf8);

            Assert.AreEqual(options3.MinimumLength, 3);
            Assert.AreEqual(options3.MaximumLength, null);
            Assert.AreEqual(options3.SearchedStringType, StringType.Utf8);
        }


        [Test]
        [TestCase(2, 6, StringType.Utf8)]
        [TestCase(3, 12, StringType.Utf16)]
        [TestCase(12, null, StringType.Both)]
        public void ChangePropertiesWithCorrectValues(int minimumLength, int? maximumLength, StringType searchedStringType)
        {
            var options = new StringReaderOptions();

            options.MinimumLength = minimumLength;
            options.MaximumLength = maximumLength;
            options.SearchedStringType = searchedStringType;

            Assert.AreEqual(options.MinimumLength, minimumLength);
            Assert.AreEqual(options.MaximumLength, maximumLength);
            Assert.AreEqual(options.SearchedStringType, searchedStringType);
        }

        [Test]
        public void ThrowWhenConstructedWithInvalidValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var options = new StringReaderOptions(-2);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var options = new StringReaderOptions(4, 1);
            });
        }
    }
}
