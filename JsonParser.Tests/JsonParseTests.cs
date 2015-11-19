using NUnit.Framework;

namespace JsonParser.Tests
{
    public class JsonParseTests
    {
        [Test]
        public void TrueReturnsTrue()
        {
            var json = "true";

            var result = Json.Parse(json);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FalseReturnsFalse()
        {
            var json = "false";

            var result = Json.Parse(json);

            Assert.That(result, Is.False);
        }

        [Test]
        public void NullReturnsNull()
        {
            var json = "null";

            var result = Json.Parse(json);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void StringWithoutEscapeSequencesReturnsString()
        {
            var json = @"""Hello, world!""";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo("Hello, world!"));
        }

        [Test]
        public void StringWithEscapedQuotesReturnsString()
        {
            var json = @"""\""Ow.\"" - my pancreas""";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo(@"""Ow."" - my pancreas"));
        }

        [Test]
        public void StringWithEScapedBackslashesReturnsString()
        {
            var json = @"""c:\\dev\\code\\foo.cs""";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo(@"c:\dev\code\foo.cs"));
        }

        [Test]
        public void NumberWithNoDecimalPlaceReturnsInt()
        {
            var json = @"123";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo(123));
            Assert.That(result, Is.InstanceOf<int>());
        }

        [Test]
        public void NegativeNumberWithNoDecimalPlaceReturnsInt()
        {
            var json = @"-123";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo(-123));
            Assert.That(result, Is.InstanceOf<int>());
        }

        [Test, Ignore]
        public void NumberWithDecimalPlaceReturnsDouble()
        {
            var json = @"123.45";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo(123.45));
            Assert.That(result, Is.InstanceOf<double>());
        }
    }
}