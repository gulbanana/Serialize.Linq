using Serialize.Linq.Tests.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Serialize.Linq.Tests.Issues
{
    // https://github.com/esskar/Serialize.Linq/issues/35

    public class Issue35
    {
        [Fact]
        public void LetExpressionTests()
        {
            var expressions = new List<Expression>();

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> intExpr = c =>
                from x in c
                let test = 8
                where x == test
                select x;
            expressions.Add(intExpr);

            Expression<Func<IEnumerable<string>, IEnumerable<string>>> strExpr = c =>
                from x in c
                let test = "bar"
                where x == test
                select x;
            expressions.Add(strExpr);            

            foreach (var expected in expressions)
            {
                var serialized = Json.Serialize(expected);
                var actual = Json.Deserialize(serialized);

                ExpressionAssert.AreEqual(expected, actual);
            }
        }
    }
}
