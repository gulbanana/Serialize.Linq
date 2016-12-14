using Serialize.Linq.Extensions;
using Serialize.Linq.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Serialize.Linq.Tests.Issues
{
    /// <summary>
    /// https://github.com/esskar/Serialize.Linq/issues/50
    /// </summary>
    
    public class Issue50
    {
        [Fact]
        public void SerializeArrayAsJson()
        {
            var list = new [] { "one", "two" };
            Expression<Func<Test, bool>> expression = test => list.Contains(test.Code);

            var value = Json.Serialize(expression);

            Assert.NotNull(value);
        }

        [Fact]
        public void SerializeListAsJson()
        {
            var list = new List<string> { "one", "two" };
            Expression<Func<Test, bool>> expression = test => list.Contains(test.Code);

            var value = Json.Serialize(expression);

            Assert.NotNull(value);
        }

        [Fact]
        public void SerializeDeserializeArrayAsJson()
        {
            var list = new[] { "one", "two" };
            Expression<Func<Test, bool>> expression = test => list.Contains(test.Code);

            var text = Json.Serialize(expression);

            var actualExpression = (Expression<Func<Test, bool>>)Json.Deserialize(text);
            var func = actualExpression.Compile();


            Assert.True(func(new Test { Code = "one" }), "one failed.");
            Assert.True(func(new Test { Code = "two" }), "two failed.");
            Assert.False(func(new Test { Code = "three" }), "three failed.");
        }

        public class Test
        {
            public string Code { get; set; }
        }
    }
}
