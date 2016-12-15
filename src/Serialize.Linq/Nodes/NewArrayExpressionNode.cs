#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Internals;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "NA")]   
    public class NewArrayExpressionNode : ExpressionNode<NewArrayExpression>
    {
        public NewArrayExpressionNode() { }

        public NewArrayExpressionNode(NodeContext factory, NewArrayExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "E")]
        public ExpressionNodeList Expressions { get; set; }

        protected override void Initialize(NewArrayExpression expression)
        {
            Expressions = new ExpressionNodeList(Context, expression.Expressions);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            switch (NodeType)
            {
                case ExpressionType.NewArrayBounds:
                    return Expression.NewArrayBounds(Type.ToType(context).GetElementType(), Expressions.GetExpressions(context));

                case ExpressionType.NewArrayInit:
                    return Expression.NewArrayInit(Type.ToType(context).GetElementType(), Expressions.GetExpressions(context));

                default:
                    throw new InvalidOperationException("Unhandeled nody type: " + NodeType);
            }
        }
    }
}
