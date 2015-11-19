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
    }
}