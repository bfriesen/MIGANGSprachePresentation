using System.Collections.Generic;
using System.Dynamic;
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

        [Test]
        public void NumberWithDecimalPlaceReturnsDouble()
        {
            var json = @"123.45";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo(123.45));
            Assert.That(result, Is.InstanceOf<double>());
        }

        [Test]
        public void NegativeNumberWithDecimalPlaceReturnsDouble()
        {
            var json = @"-123.45";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo(-123.45));
            Assert.That(result, Is.InstanceOf<double>());
        }

        [Test]
        public void EmptyObjectReturnsExpandoObject()
        {
            var json = @"{}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());

            var d = (IDictionary<string, object>)result;

            Assert.That(d, Is.Empty);
        }

        [Test]
        public void ObjectWithOnePropertyReturnsExpandoObject()
        {
            var json = @"{""foo"":true}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());

            var d = (IDictionary<string, object>)result;

            Assert.That(d.Count, Is.EqualTo(1));
            Assert.That(result.foo, Is.True);
        }

        [Test]
        public void ObjectWithOneObjectPropertyReturnsExpandoObject()
        {
            var json = @"{""foo"":{""bar"":true}}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());

            Assert.That(result.foo.bar, Is.True);
        }

        [Test]
        public void ObjectWithMultiplePropertiesReturnsExpandoObject()
        {
            var json = @"{""foo"":true,""bar"":false,""baz"":null}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());

            var d = (IDictionary<string, object>)result;

            Assert.That(d.Count, Is.EqualTo(3));
            Assert.That(result.foo, Is.True);
            Assert.That(result.bar, Is.False);
            Assert.That(result.baz, Is.Null);
        }

        [Test]
        public void EmptyArrayReturnsObjectArray()
        {
            var json = @"[]";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<object[]>());
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ArrayWithOneItemReturnsObjectArray()
        {
            var json = @"[true]";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<object[]>());

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.True);
        }

        [Test]
        public void ArrayWithMultipleItemReturnsObjectArray()
        {
            var json = @"[true,false,null]";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<object[]>());

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0], Is.True);
            Assert.That(result[1], Is.False);
            Assert.That(result[2], Is.Null);
        }
    }
}