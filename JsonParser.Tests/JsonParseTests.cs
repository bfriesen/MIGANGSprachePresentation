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

        [Test, Ignore]
        public void StringWithoutEscapeSequencesReturnsString()
        {
            var json = @"""Hello, world!""";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo("Hello, world!"));
        }
    }
}