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
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "EI")]
    public class ElementInitNode : Node
    {
        public ElementInitNode() { }

        public ElementInitNode(NodeContext factory, ElementInit elementInit) : base(factory)
        {
            Initialize(elementInit);
        }

        private void Initialize(ElementInit elementInit)
        {
            if (elementInit == null) throw new ArgumentNullException("elementInit");

            AddMethod = new MethodInfoNode(Context, elementInit.AddMethod);
            Arguments = new ExpressionNodeList(Context, elementInit.Arguments);
        }

        [DataMember(EmitDefaultValue = false, Name = "A")]
        public ExpressionNodeList Arguments { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "M")]
        public MethodInfoNode AddMethod { get; set; }

        internal ElementInit ToElementInit(ExpressionContext context)
        {
            return Expression.ElementInit(AddMethod.ToMemberInfo(context), Arguments.GetExpressions(context));
        }
    }
}
