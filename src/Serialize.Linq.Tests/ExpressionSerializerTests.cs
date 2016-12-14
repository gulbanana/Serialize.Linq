#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Tests.Internals;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Serialize.Linq.Tests
{

    public class ExpressionSerializerTests
    {
        [Theory, MemberData(nameof(SerializerTestData.TestExpressions), MemberType = typeof(SerializerTestData))]
        public void SerializeDeserializeTest(Expression expected)
        {
            var text = Json.Serialize(expected);

            // this.TestContext.WriteLine("{0} serializes to text with length {1}: {2}", expected, text.Length, text);

            var actual = Json.Deserialize(text);

            //if (expected == null)
            //{
            //    Assert.Null(actual, $"Input expression was null, but output is {actual} for '{textSerializer.GetType()}'");
            //    continue;
            //}
            //Assert.NotNull(actual, $"Input expression was {expected}, but output is null for '{textSerializer.GetType()}'"));
            ExpressionAssert.AreEqual(expected, actual);
        }

        [Fact]
        public void SerializeDeserializeComplexExpressionWithCompileTest()
        {
            var expected = (Expression<Func<Bar, bool>>)(p => p.LastName == "Miller" && p.FirstName.StartsWith("M"));
            expected.Compile();

            var text = Json.Serialize(expected);
            // this.TestContext.WriteLine("{0} serializes to bytes with length {1}", expected, bytes.Length);

            var actual = (Expression<Func<Bar, bool>>)(Json.Deserialize(text));
            //Assert.NotNull(actual, "Input expression was {0}, but output is null for '{1}'", expected, binSerializer.GetType());
            ExpressionAssert.AreEqual(expected, actual);

            actual.Compile();
        }
        
        [Fact]
        public void NullableDecimalTest()
        {
            var expected = Expression.Constant(0m, typeof(decimal?));

            var text = Json.Serialize(expected);

            // this.TestContext.WriteLine("{0} serializes to text with length {1}: {2}", expected, text.Length, text);

            var actual = Json.Deserialize(text);
            //Assert.NotNull(actual, "Input expression was {0}, but output is null for '{1}'", expected, textSerializer.GetType());
            ExpressionAssert.AreEqual(expected, actual);
        }
        
        [Fact]
        public void SerializeNewObjWithoutParameters()
        {
            Expression<Func<List<int>, List<int>>> exp = l => new List<int>();

            var result = Json.Serialize(exp);
            Assert.NotNull(result);
        }

        [Fact]
        public void SerializeFuncExpressionsWithoutParameters()
        {
            Expression<Func<bool>> exp = () => false;

            var result = Json.Serialize(exp);
            Assert.NotNull(result);
        }
        
        [Fact]
        public void SerializeDeserializeGuidValueAsJson()
        {
            SerializeDeserializeExpressionAsText(CreateGuidExpression());
        }

        [Fact]
        public void ExpressionWithConstantDateTimeAsJson()
        {
            SerializeDeserializeExpressionAsText(CreateConstantDateTimeExpression());
        }

        [Fact]
        public void ExpressionWithConstantTypeAsJson()
        {
            SerializeDeserializeExpressionAsText(CreateConstantTypeExpression());
        }

        private static ConstantExpression CreateConstantDateTimeExpression()
        {
            return Expression.Constant(DateTime.Today);
        }

        private static Expression<Func<Guid>> CreateGuidExpression()
        {
            var guidValue = Guid.NewGuid();
            return () => guidValue;
        }

        private static ConstantExpression CreateConstantTypeExpression()
        {
            return Expression.Constant(typeof(string));
        }

        private static Expression SerializeDeserializeExpressionAsText(Expression expression)
        {
            var serialized = Json.Serialize(expression);

            return Json.Deserialize(serialized);
        }
    }
}
