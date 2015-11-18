using Sprache;
using System;

namespace JsonParser
{
    internal static class Parser
    {
        public static Func<string, object> GetJsonParser()
        {
            var literalParser = GetLiteralParser();
            var stringParser = GetStringParser();

            var mainParser = literalParser.Or(stringParser);

            return mainParser.Parse;
        }

        private static Parser<string> GetStringParser()
        {
            var nonescapedCharParser = Parse.CharExcept('"');

            var stringParser =
                from q1 in Parse.Char('"')
                from value in nonescapedCharParser.Many().Text()
                from q2 in Parse.Char('"')
                select value;

            return stringParser;
        }

        private static Parser<object> GetLiteralParser()
        {
            var trueParser =
                from t in Parse.String("true")
                select (object)true;

            var falseParser =
                from t in Parse.String("false")
                select (object)false;

            var nullParser =
                from t in Parse.String("null")
                select (object)null;

            return trueParser.Or(falseParser).Or(nullParser);
        }
    }
}