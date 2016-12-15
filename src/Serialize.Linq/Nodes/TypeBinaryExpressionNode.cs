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
    [DataContract(Name = "TB")]   
    public class TypeBinaryExpressionNode : ExpressionNode<TypeBinaryExpression>
    {
        public TypeBinaryExpressionNode() { }

        public TypeBinaryExpressionNode(NodeContext factory, TypeBinaryExpression expression) : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "E")]
        public ExpressionNode Expression { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "O")]
        public TypeNode TypeOperand { get; set; }

        protected override void Initialize(TypeBinaryExpression expression)
        {
            Expression = Context.Create(expression.Expression);
            TypeOperand = new TypeNode(Context, expression.TypeOperand);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            switch (NodeType)
            {
                case ExpressionType.TypeIs:
                    return System.Linq.Expressions.Expression.TypeIs(Expression.ToExpression(context), TypeOperand.ToType(context));
                case ExpressionType.TypeEqual:
                    return System.Linq.Expressions.Expression.TypeEqual(Expression.ToExpression(context), TypeOperand.ToType(context));
                default:
                    throw new NotSupportedException("unrecognised TypeBinaryExpression.NodeType " + Enum.GetName(typeof(ExpressionType), NodeType));
            }
        }
    }
}
