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
    [DataContract(Name = "M")]
    public class MemberExpressionNode : ExpressionNode<MemberExpression>
    {
        public MemberExpressionNode() { }

        public MemberExpressionNode(NodeContext factory, MemberExpression expression) : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "E")]
        public ExpressionNode Expression { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "M")]
        public MemberInfoNode Member { get; set; }

        protected override void Initialize(MemberExpression expression)
        {
            Expression = Context.Create(expression.Expression);
            Member = new MemberInfoNode(Context, expression.Member);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            var member = Member.ToMemberInfo(context);
            return System.Linq.Expressions.Expression.MakeMemberAccess(Expression != null ? Expression.ToExpression(context) : null, member);
        }
    }
}
