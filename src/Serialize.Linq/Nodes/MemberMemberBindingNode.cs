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
    [DataContract(Name = "MMB")]
    public class MemberMemberBindingNode : MemberBindingNode
    {
        public MemberMemberBindingNode() { }

        public MemberMemberBindingNode(INodeFactory factory, MemberMemberBinding memberMemberBinding)
            : base(factory, memberMemberBinding.BindingType, memberMemberBinding.Member)
        {
            Bindings = new MemberBindingNodeList(factory, memberMemberBinding.Bindings);
        }

        [DataMember(EmitDefaultValue = false, Name = "B")]
        public MemberBindingNodeList Bindings { get; set; }

        internal override MemberBinding ToMemberBinding(ExpressionContext context)
        {
            return Expression.MemberBind(Member.ToMemberInfo(context), Bindings.GetMemberBindings(context));
        }
    }
}
