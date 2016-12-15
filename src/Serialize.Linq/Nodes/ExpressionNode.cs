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
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode{TExpression}"/> class.
        /// </summary>
        protected ExpressionNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode{TExpression}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="expression">The expression.</param>
        protected ExpressionNode(NodeContext factory, TExpression expression)
            : base(factory, expression.NodeType, expression.Type)
        {
            Initialize(expression);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode{TExpression}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="type">The type.</param>
        protected ExpressionNode(NodeContext factory, ExpressionType nodeType, Type type = null)
            : base(factory, nodeType, type) { }

        /// <summary>
        /// Initializes this instance using the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected abstract void Initialize(TExpression expression);
    }

    [DataContract(Name = "E")]
    public abstract class ExpressionNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode"/> class.
        /// </summary>
        protected ExpressionNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="type">The type.</param>
        protected ExpressionNode(NodeContext factory, ExpressionType nodeType, Type type = null)
            : base(factory)
        {
            NodeType = nodeType;
            Type = new TypeNode(factory, type);
        }

        /// <summary>
        /// Gets or sets the type of the node.
        /// </summary>
        /// <value>
        /// The type of the node.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "NT")]
        public ExpressionType NodeType { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
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
