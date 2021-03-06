﻿using System;
using System.Linq.Expressions;
using Xunit;

namespace Serialize.Linq.Tests.Issues
{
    /// <summary>
    /// https://github.com/esskar/Serialize.Linq/issues/39
    /// </summary>

    public class Issue39
    {
        private class DataPoint
        {
            public DateTime Timestamp = default(DateTime);
            public int AcctId = default(int);
        }

        [Fact]
        public void ToExpressionNodeWithSimilarConstantNames()
        {
            var feb1 = new DateTime(2015, 2, 1);
            var feb15 = new DateTime(2015, 2, 15);

            Expression<Func<DataPoint, bool>> expression =
                dp => dp.Timestamp >= feb1 && dp.Timestamp < feb15 && dp.AcctId == 1;

            var result = expression.ToExpressionNode();

            Assert.NotNull(result);
        }
    }
}
