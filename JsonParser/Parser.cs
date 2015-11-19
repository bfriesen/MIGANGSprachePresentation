using System.Dynamic;
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
            var numberParser = GetNumberParser();
            var objectParser = GetObjectParser();

            var mainParser = literalParser.Or(stringParser).Or(numberParser).Or(objectParser);

            return mainParser.Parse;
        }

        private static Parser<object> GetObjectParser()
        {
            var objectParser =
                from openBrace in Parse.Char('{')
                from closeBrace in Parse.Char('}')
                select new ExpandoObject();

            return objectParser;
        }

        private static Parser<object> GetNumberParser()
        {
            var doubleParser =
                from negativeSign in Parse.Char('-').Optional()
                from wholePart in Parse.Numeric.AtLeastOnce().Text()
                from dot in Parse.Char('.')
                from decimalPart in Parse.Numeric.AtLeastOnce().Text()
                select (object)(double.Parse(wholePart + dot + decimalPart) * (negativeSign.IsDefined ? -1 : 1));

            var intParser =
                from negativeSign in Parse.Char('-').Optional()
                from stringValue in Parse.Numeric.AtLeastOnce().Text()
                select (object)(int.Parse(stringValue) * (negativeSign.IsDefined ? -1 : 1));

            return doubleParser.Or(intParser);
        }

        private static Parser<string> GetStringParser()
        {
            var escapedQuoteParser =
                from backslash in Parse.Char('\\')
                from quote in Parse.Char('"')
                select quote;

            var escapedBackslashParser =
                from backslash1 in Parse.Char('\\')
                from backslash2 in Parse.Char('\\')
                select backslash2;

            var nonescapedCharParser = Parse.CharExcept('"');

            var stringParser =
                from q1 in Parse.Char('"')
                from value in escapedBackslashParser.Or(escapedQuoteParser).Or(nonescapedCharParser)
                    .Many().Text()
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