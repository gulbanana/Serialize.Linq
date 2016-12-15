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
            Initializers = new ElementInitNodeList(Factory, expression.Initializers);
            NewExpression = Factory.Create(expression.NewExpression);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            return Expression.ListInit((NewExpression)NewExpression.ToExpression(context), Initializers.GetElementInits(context));
        }
    }
}
