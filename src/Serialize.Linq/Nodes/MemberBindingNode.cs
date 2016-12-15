#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Internals;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "MB")]
    public abstract class MemberBindingNode : Node
    {
        protected MemberBindingNode() { }

        protected MemberBindingNode(NodeContext factory)
            : base(factory) { }

        protected MemberBindingNode(NodeContext factory, MemberBindingType bindingType, MemberInfo memberInfo)
            : base(factory)
        {
            BindingType = bindingType;
            Member = new MemberInfoNode(Context, memberInfo);
        }
        
        [DataMember(EmitDefaultValue = false, Name = "BT")]
        public MemberBindingType BindingType { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "M")]
        public MemberInfoNode Member { get; set; }

        internal abstract MemberBinding ToMemberBinding(ExpressionContext context);

        internal static MemberBindingNode Create(NodeContext factory, MemberBinding memberBinding)
        {
            MemberBindingNode memberBindingNode = null;

            if (memberBinding is MemberAssignment)
                memberBindingNode = new MemberAssignmentNode(factory, (MemberAssignment)memberBinding);
            else if (memberBinding is MemberListBinding)
                memberBindingNode = new MemberListBindingNode(factory, (MemberListBinding)memberBinding);
            else if (memberBinding is MemberMemberBinding)
                memberBindingNode = new MemberMemberBindingNode(factory, (MemberMemberBinding)memberBinding);
            else if (memberBinding != null)
                throw new ArgumentException("Unknown member binding of type " + memberBinding.GetType(), "memberBinding");

            return memberBindingNode;
        }
    }
}
