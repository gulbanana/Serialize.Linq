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
    [DataContract(Name = "MA")]   
    public class MemberAssignmentNode : MemberBindingNode
    {
        public MemberAssignmentNode() { }

        public MemberAssignmentNode(NodeContext factory, MemberAssignment memberAssignment)
            : base(factory, memberAssignment.BindingType, memberAssignment.Member)
        {
            Expression = Context.Create(memberAssignment.Expression);
        }

        [DataMember(EmitDefaultValue = false, Name = "E")]
        public ExpressionNode Expression { get; set; }

        internal override MemberBinding ToMemberBinding(ExpressionContext context)
        {
            return System.Linq.Expressions.Expression.Bind(Member.ToMemberInfo(context), Expression.ToExpression(context));
        }
    }
}
