﻿#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Internals;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "IF")]   
    public class ConditionalExpressionNode : ExpressionNode<ConditionalExpression>
    {
        public ConditionalExpressionNode() { }

        public ConditionalExpressionNode(NodeContext factory, ConditionalExpression expression) : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "IFF")]
        public ExpressionNode IfFalse { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "IFT")]
        public ExpressionNode IfTrue { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "C")]
        public ExpressionNode Test { get; set; }

        protected override void Initialize(ConditionalExpression expression)
        {
            Test = Context.Create(expression.Test);
            IfTrue = Context.Create(expression.IfTrue);
            IfFalse = Context.Create(expression.IfFalse);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            return Expression.Condition(Test.ToExpression(context), IfTrue.ToExpression(context), IfFalse.ToExpression(context));
        }
    }
}
