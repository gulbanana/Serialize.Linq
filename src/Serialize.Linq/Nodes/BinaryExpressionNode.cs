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
    [DataContract(Name = "B")]
    public class BinaryExpressionNode : ExpressionNode<BinaryExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpressionNode"/> class.
        /// </summary>
        public BinaryExpressionNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpressionNode"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="expression">The expression.</param>
        public BinaryExpressionNode(INodeFactory factory, BinaryExpression expression)
            : base(factory, expression) { }

        /// <summary>
        /// Gets or sets the conversion.
        /// </summary>
        /// <value>
        /// The conversion.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "C")]
        public ExpressionNode Conversion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is lifted to null.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is lifted to null; otherwise, <c>false</c>.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "I")]
        public bool IsLiftedToNull { get; set; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "L")]
        public ExpressionNode Left { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "M")]
        public MethodInfoNode Method { get; set; }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "R")]
        public ExpressionNode Right { get; set; }

        /// <summary>
        /// Initializes the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected override void Initialize(BinaryExpression expression)
        {
            Left = Factory.Create(expression.Left);
            Right = Factory.Create(expression.Right);
            Conversion = Factory.Create(expression.Conversion);
            Method = new MethodInfoNode(Factory, expression.Method);
            IsLiftedToNull = expression.IsLiftedToNull;
        }

        /// <summary>
        /// Converts this instance to an expression.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
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
