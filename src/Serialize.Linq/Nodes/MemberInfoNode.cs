#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Internals;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "MI")]
    public class MemberInfoNode : MemberNode<MemberInfo>
    {
        public MemberInfoNode() { }

        public MemberInfoNode(NodeContext factory, MemberInfo memberInfo) : base(factory, memberInfo) { }

        protected override IEnumerable<MemberInfo> GetMemberInfosForType(ExpressionContext context, Type type)
        {
            BindingFlags? flags = GetBindingFlags();
            return flags == null ? type.GetMembers() : type.GetMembers(flags.Value);
        }

        // returning null would disallow access to privates
        private BindingFlags? GetBindingFlags()
        {
            return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        }
    }
}