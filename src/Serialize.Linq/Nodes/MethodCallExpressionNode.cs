#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Internals;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "MC")]   
    public class MethodCallExpressionNode : ExpressionNode<MethodCallExpression>
    {
        public MethodCallExpressionNode() { }

        public MethodCallExpressionNode(NodeContext factory, MethodCallExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "A")]
        public ExpressionNodeList Arguments { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "M")]
        public MethodInfoNode Method { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "O")]
        public ExpressionNode Object { get; set; }

        protected override void Initialize(MethodCallExpression expression)
        {
            Arguments = new ExpressionNodeList(Context, expression.Arguments);
            Method = new MethodInfoNode(Context, expression.Method);
            Object = Context.Create(expression.Object);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            Expression objectExpression = null;
            if (Object != null)
                objectExpression = Object.ToExpression(context);

            return Expression.Call(objectExpression, Method.ToMemberInfo(context), Arguments.GetExpressions(context).ToArray());
        }
    }
}
