using Serialize.Linq.Tests.Internals;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Serialize.Linq.Tests.Issues
{

    public class Issue37
    {
        [Fact]
        public void DynamicsTests()
        {
            var expressions = new List<Expression>();

            Expression<Func<Item, dynamic>> objectExp = item => new {item.Name, item.ProductId};
            Expression<Func<string, dynamic>> stringExp = str => new { Text = str };

            expressions.Add(objectExp);
            expressions.Add(stringExp);

            foreach (var expected in expressions)
            {
                var serialized = Json.Serialize(expected);
                var actual = Json.Deserialize(serialized);

                ExpressionAssert.AreEqual(expected, actual);
            }
        }

        public class Item
        {
            public string Name { get; set; }

            public string ProductId { get; set; }
        }
    }
}
