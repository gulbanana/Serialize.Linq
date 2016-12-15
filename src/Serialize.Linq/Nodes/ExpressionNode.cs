#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Internals;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "tE")]    
    public abstract class ExpressionNode<TExpression> : ExpressionNode where TExpression : Expression
    {
        protected ExpressionNode() { }

        protected ExpressionNode(NodeContext factory, TExpression expression) : base(factory, expression.NodeType, expression.Type)
        {
            Initialize(expression);
        }
        protected ExpressionNode(NodeContext factory, ExpressionType nodeType, Type type = null) : base(factory, nodeType, type) { }

        protected abstract void Initialize(TExpression expression);
    }

    [DataContract(Name = "E")]
    public abstract class ExpressionNode : Node
    {
        protected ExpressionNode() { }

        protected ExpressionNode(NodeContext factory, ExpressionType nodeType, Type type = null) : base(factory)
        {
            NodeType = nodeType;
            Type = new TypeNode(factory, type);
        }

        [DataMember(EmitDefaultValue = false, Name = "NT")]
        public ExpressionType NodeType { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "T")]
        public virtual TypeNode Type { get; set; }

        public virtual Expression ToExpression(ExpressionContext context)
        {
            return null;
        }

        public Expression ToExpression()
        {
            return ToExpression(new ExpressionContext());
        }

        public static ExpressionNode FromExpression(Expression expression)
        {
            var lambda = expression as LambdaExpression;
            var factory = new NodeContext(lambda?.Parameters?.Select(p => p.Type));
            return factory.Create(expression);
        }

        public override string ToString()
        {
            return ToExpression().ToString();
        }
    }
}
