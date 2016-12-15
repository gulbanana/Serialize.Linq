#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Factories;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "I")]
    public class InvocationExpressionNode : ExpressionNode<InvocationExpression>
    {
        public InvocationExpressionNode() { }

        public InvocationExpressionNode(INodeFactory factory, InvocationExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "A")]
        public ExpressionNodeList Arguments { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "E")]
        public ExpressionNode Expression { get; set; }

        protected override void Initialize(InvocationExpression expression)
        {
            Arguments = new ExpressionNodeList(Factory, expression.Arguments);
            Expression = Factory.Create(expression.Expression);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            return System.Linq.Expressions.Expression.Invoke(Expression.ToExpression(context), Arguments.GetExpressions(context));
        }
    }
}
