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
    [DataContract(Name = "FI")]
    public class FieldInfoNode : MemberNode<FieldInfo>
    {
        public FieldInfoNode() { }

        public FieldInfoNode(NodeContext factory, FieldInfo memberInfo) : base(factory, memberInfo) { }

        protected override IEnumerable<FieldInfo> GetMemberInfosForType(ExpressionContext context, Type type)
        {
            return type.GetFields();
        }
    }
}