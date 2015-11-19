using System.Collections.Generic;
using System.Dynamic;
using Sprache;
using System;

namespace JsonParser
{
    internal static class Parser
    {
        public static Func<string, object> GetJsonParser()
        {
            var mainParser = new MainParser();

            var literalParser = GetLiteralParser();
            var stringParser = GetStringParser();
            var numberParser = GetNumberParser();
            var objectParser = GetObjectParser(stringParser, mainParser);
            var arrayParser = GetArrayParser(mainParser);

            mainParser.Value = literalParser.Or(stringParser).Or(numberParser).Or(objectParser).Or(arrayParser);

            return mainParser.Value.Parse;
        }

        private static Parser<object> GetArrayParser(MainParser mainParser)
        {
            return
                from openBracket in Parse.Char('[')
                from firstItem in Parse.Ref(() => mainParser.Value).Optional()
                from closeBracket in Parse.Char(']')
                select GetObjectArray(firstItem);
        }

        private static object[] GetObjectArray(IOption<object> firstItem)
        {
            var list = new List<object>();

            if (firstItem.IsDefined)
            {
                list.Add(firstItem.Get());
            }

            return list.ToArray();
        }

        private static Parser<object> GetObjectParser(Parser<string> stringParser, MainParser mainParser)
        {
            var memberParser =
                from name in stringParser
                from colon in Parse.Char(':')
                from value in Parse.Ref(() => mainParser.Value)
                select new Member { Name = name, Value = value };

            var objectParser =
                from openBrace in Parse.Char('{')
                from firstMember in memberParser.Optional()
                from otherMembers in
                    (from comma in Parse.Char(',')
                     from member in memberParser
                     select member).Many()
                from closeBrace in Parse.Char('}')
                select GetExpandoObject(firstMember, otherMembers);

            return objectParser;
        }

        private static ExpandoObject GetExpandoObject(IOption<Member> firstMember, IEnumerable<Member> otherMembers)
        {
            var expandoObject = new ExpandoObject();
            var d = (IDictionary<string, object>)expandoObject;

            if (firstMember.IsDefined)
            {
                var member = firstMember.Get();
                d.Add(member.Name, member.Value);
            }

            foreach (var member in otherMembers)
            {
                d.Add(member.Name, member.Value);
            }

            return expandoObject;
        }

        private class MainParser
        {
            public Parser<object> Value;
        }

        private class Member
        {
            public string Name;
            public object Value;
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