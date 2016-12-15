#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Serialize.Linq.Internals;

namespace Serialize.Linq.Nodes
{
    [CollectionDataContract(Name = "MIL")]
    public class MemberInfoNodeList : List<MemberInfoNode>
    {
        public MemberInfoNodeList() { }

        public MemberInfoNodeList(NodeContext factory, IEnumerable<MemberInfo> items = null)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");
            if(items != null)
                AddRange(items.Select(m => new MemberInfoNode(factory, m)));
        }

        public IEnumerable<MemberInfo> GetMembers(ExpressionContext context)
        {
            return this.Select(m => m.ToMemberInfo(context));
        }
    }
}
