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
            Test = Factory.Create(expression.Test);
            IfTrue = Factory.Create(expression.IfTrue);
            IfFalse = Factory.Create(expression.IfFalse);
        }

        /// <summary>
        /// Converts this instance to an expression.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override Expression ToExpression(ExpressionContext context)
        {
            return Expression.Condition(Test.ToExpression(context), IfTrue.ToExpression(context), IfFalse.ToExpression(context));
        }
    }
}
