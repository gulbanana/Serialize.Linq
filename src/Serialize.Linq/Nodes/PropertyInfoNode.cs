#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Serialize.Linq.Internals;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "PI")]
    public class PropertyInfoNode : MemberNode<PropertyInfo>
    {
        public PropertyInfoNode() { }

        public PropertyInfoNode(NodeContext factory, PropertyInfo memberInfo) 
            : base(factory, memberInfo) { }

        protected override IEnumerable<PropertyInfo> GetMemberInfosForType(ExpressionContext context, Type type)
        {
            return type.GetProperties();
        }
    }
}