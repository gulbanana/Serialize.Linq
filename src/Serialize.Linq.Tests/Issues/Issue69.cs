using Serialize.Linq.Tests.Internals;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Serialize.Linq.Tests.Issues
{
    /// <summary>
    /// https://github.com/esskar/Serialize.Linq/issues/69
    /// </summary>
    public class Issue69
    {
        [Fact]
        public void JsonSerializeAndDeserialize1969Utc()
        {
            SerializeAndDeserializeDateTimeJson(new DateTime(1969, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }

        [Fact]
        public void JsonSerializeAndDeserialize1969Local()
        {
            SerializeAndDeserializeDateTimeJson(new DateTime(1969, 1, 1, 0, 0, 0, DateTimeKind.Local));
        }

        [Fact]
        public void JsonSerializeAndDeserialize1970Utc()
        {
            SerializeAndDeserializeDateTimeJson(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }

        [Fact]
        public void JsonSerializeAndDeserialize1970Local()
        {
            SerializeAndDeserializeDateTimeJson(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local));
        }

        [Fact]
        public void JsonSerializeAndDeserialize1971Utc()
        {
            SerializeAndDeserializeDateTimeJson(new DateTime(1971, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }

        [Fact]
        public void JsonSerializeAndDeserialize1971Local()
        {
            SerializeAndDeserializeDateTimeJson(new DateTime(1971, 1, 1, 0, 0, 0, DateTimeKind.Local));
        }

        private void SerializeAndDeserializeDateTimeJson(DateTime dt)
        {
            Expression<Func<DateTime>> actual = () => dt;
            actual = actual.Update(Expression.Constant(dt), new List<ParameterExpression>());

            var serialized = Json.Serialize(actual);
            var expected = Json.Deserialize(serialized);
            ExpressionAssert.AreEqual(expected, actual);
        }
    }
}