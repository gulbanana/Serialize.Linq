#region Copyright
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
    [DataContract(Name = "U")]
    public class UnaryExpressionNode : ExpressionNode<UnaryExpression>
    {
        public UnaryExpressionNode() { }

        public UnaryExpressionNode(NodeContext factory, UnaryExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "O")]
        public ExpressionNode Operand { get; set; }

        protected override void Initialize(UnaryExpression expression)
        {
            Operand = Context.Create(expression.Operand);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            return NodeType == ExpressionType.UnaryPlus
                ? Expression.UnaryPlus(Operand.ToExpression(context))
                : Expression.MakeUnary(NodeType, Operand.ToExpression(context), Type.ToType(context));
        }
    }
}
