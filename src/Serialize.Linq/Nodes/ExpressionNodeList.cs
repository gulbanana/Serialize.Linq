#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [CollectionDataContract(Name = "EL")]
    public class ExpressionNodeList : List<ExpressionNode>
    {
        public ExpressionNodeList() { }

        public ExpressionNodeList(NodeContext factory, IEnumerable<Expression> items)
        {
            if (factory == null) new ArgumentNullException("factory");
            if (items == null) throw new ArgumentNullException("items");

            AddRange(items.Select(factory.Create));
        }

        internal IEnumerable<Expression> GetExpressions(ExpressionContext context)
        {
            return this.Select(e => e.ToExpression(context));
        }

        internal IEnumerable<ParameterExpression> GetParameterExpressions(ExpressionContext context)
        {
            return this.OfType<ParameterExpressionNode>().Select(e => (ParameterExpression)e.ToExpression(context));
        }
    }
}