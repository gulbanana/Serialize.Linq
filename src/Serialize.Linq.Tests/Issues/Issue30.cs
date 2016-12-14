using Serialize.Linq.Tests.Internals;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Serialize.Linq.Tests.Issues
{
    // https://github.com/esskar/Serialize.Linq/issues/30
    public class Issue30
    {
        /*
        [Fact]
        public void SerializeDeserializeLambdaWithNullableTest()
        {
            foreach (var textSerializer in new ITextSerializer[] { new JsonSerializer(), new XmlSerializer() })
            {
                var serializer = new ExpressionSerializer(textSerializer);
                string actual;
                string expected;

                {
                    int seven = 7;
                    Expression<Func<Fish, bool>> expectedExpression = f => f.Count == seven;
                    expected = serializer.SerializeText(expectedExpression);
                }

                {
                    int? seven = 7;
                    Expression<Func<Fish, bool>> actualExpression = f => f.Count == seven;
                    actual = serializer.SerializeText(actualExpression);
                }

                Assert.Equal(expected, actual);
            }
        }*/

        [Fact]
        public void SerializeLambdaWithNullableTest()
        {
            var fish = new[]
            {
                new Fish {Count = 0},
                new Fish {Count = 1},
                new Fish(),
                new Fish {Count = 1}
            };
            int? count = 1;
            Expression<Func<Fish, bool>> expectedExpression = f => f.Count == count;
            var expected = fish.Where(expectedExpression.Compile()).Count();

            var serialized = Json.Serialize(expectedExpression);
            var actualExpression = (Expression<Func<Fish, bool>>)(Json.Deserialize(serialized));
            var actual = fish.Where(actualExpression.Compile()).Count();

            Assert.Equal(expected, actual);
        }
    }
}
