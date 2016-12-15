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
    [DataContract(Name = "IF")]   
    public class ConditionalExpressionNode : ExpressionNode<ConditionalExpression>
    {
        public ConditionalExpressionNode() { }

        public ConditionalExpressionNode(INodeFactory factory, ConditionalExpression expression) : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "IFF")]
        public ExpressionNode IfFalse { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "IFT")]
        public ExpressionNode IfTrue { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "C")]
        public ExpressionNode Test { get; set; }

        /// <summary>
        /// Initializes the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected override void Initialize(ConditionalExpression expression)
        {
            this.Test = this.Factory.Create(expression.Test);
            this.IfTrue = this.Factory.Create(expression.IfTrue);
            this.IfFalse = this.Factory.Create(expression.IfFalse);
        }

        /// <summary>
        /// Converts this instance to an expression.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override Expression ToExpression(ExpressionContext context)
        {
            return Expression.Condition(this.Test.ToExpression(context), this.IfTrue.ToExpression(context), this.IfFalse.ToExpression(context));
        }
    }
}
