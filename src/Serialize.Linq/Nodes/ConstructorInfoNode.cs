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
    [DataContract(Name = "CI")]
    public class ConstructorInfoNode : MemberNode<ConstructorInfo>
    {
        public ConstructorInfoNode() { }

        public ConstructorInfoNode(NodeContext factory, ConstructorInfo memberInfo) : base(factory, memberInfo) { }

        protected override IEnumerable<ConstructorInfo> GetMemberInfosForType(ExpressionContext context, Type type)
        {
            return type.GetConstructors();
        }
    }
}