#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Internals;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "P")]
    public class ParameterExpressionNode : ExpressionNode<ParameterExpression>
    {
        public ParameterExpressionNode() { }

        public ParameterExpressionNode(NodeContext factory, ParameterExpression expression) : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "I")]
        public bool IsByRef { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "N")]
        public string Name { get; set; }

        protected override void Initialize(ParameterExpression expression)
        {
            IsByRef = expression.IsByRef;
            Name = expression.Name;
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            return context.GetParameterExpression(this);
        }
    }
}
