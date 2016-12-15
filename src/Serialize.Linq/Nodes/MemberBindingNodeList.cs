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
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [CollectionDataContract(Name = "MBL")]    
    public class MemberBindingNodeList : List<MemberBindingNode>
    {
        public MemberBindingNodeList() { }

        public MemberBindingNodeList(NodeContext factory, IEnumerable<MemberBinding> items)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");
            if (items == null)
                throw new ArgumentNullException("items");
            AddRange(items.Select(m => MemberBindingNode.Create(factory, m)));
        }

        internal IEnumerable<MemberBinding> GetMemberBindings(ExpressionContext context)
        {
            return this.Select(memberBindingEntity => memberBindingEntity.ToMemberBinding(context));
        }
    }
}
