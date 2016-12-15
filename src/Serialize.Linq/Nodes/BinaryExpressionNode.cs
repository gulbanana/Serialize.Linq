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
    [DataContract(Name = "B")]
    public class BinaryExpressionNode : ExpressionNode<BinaryExpression>
    {
        public BinaryExpressionNode() { }

        public BinaryExpressionNode(NodeContext factory, BinaryExpression expression) : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "C")]
        public ExpressionNode Conversion { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "I")]
        public bool IsLiftedToNull { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "L")]
        public ExpressionNode Left { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "M")]
        public MethodInfoNode Method { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "R")]
        public ExpressionNode Right { get; set; }

        protected override void Initialize(BinaryExpression expression)
        {
            Left = Context.Create(expression.Left);
            Right = Context.Create(expression.Right);
            Conversion = Context.Create(expression.Conversion);
            Method = new MethodInfoNode(Context, expression.Method);
            IsLiftedToNull = expression.IsLiftedToNull;
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            var conversion = Conversion != null ? Conversion.ToExpression() as LambdaExpression : null;
            if (Method != null && conversion != null)
                return Expression.MakeBinary(
                    NodeType,
                    Left.ToExpression(context), Right.ToExpression(context),
                    IsLiftedToNull,
                    Method.ToMemberInfo(context),
                    conversion);
            if (Method != null)
                return Expression.MakeBinary(
                    NodeType,
                    Left.ToExpression(context), Right.ToExpression(context),
                    IsLiftedToNull,
                    Method.ToMemberInfo(context));
            return Expression.MakeBinary(NodeType,
                    Left.ToExpression(context), Right.ToExpression(context));
        }
    }
}
