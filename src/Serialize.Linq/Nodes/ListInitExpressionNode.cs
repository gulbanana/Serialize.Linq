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
    [DataContract(Name = "LI")]
    public class ListInitExpressionNode : ExpressionNode<ListInitExpression>
    {
        public ListInitExpressionNode() { }

        public ListInitExpressionNode(INodeFactory factory, ListInitExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "I")]
        public ElementInitNodeList Initializers { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "N")]
        public ExpressionNode NewExpression { get; set; }

        protected override void Initialize(ListInitExpression expression)
        {
            this.Initializers = new ElementInitNodeList(this.Factory, expression.Initializers);
            this.NewExpression = this.Factory.Create(expression.NewExpression);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            return Expression.ListInit((NewExpression)this.NewExpression.ToExpression(context), this.Initializers.GetElementInits(context));
        }
    }
}
