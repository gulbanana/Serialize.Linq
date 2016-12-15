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
    [DataContract(Name = "MIN")]
    public class MethodInfoNode : MemberNode<MethodInfo>
    {
        public MethodInfoNode() { }

        public MethodInfoNode(NodeContext factory, MethodInfo memberInfo) : base(factory, memberInfo) { }

        protected override IEnumerable<MethodInfo> GetMemberInfosForType(ExpressionContext context, Type type)
        {
            return type.GetMethods();
        }

        [DataMember(EmitDefaultValue = false, Name = "I")]
        public bool IsGenericMethod { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "G")]
        public TypeNode[] GenericArguments { get; set; }

        protected override void Initialize(MethodInfo memberInfo)
        {
            base.Initialize(memberInfo);
            if (!memberInfo.IsGenericMethod) return;

            IsGenericMethod = true;
            Signature = memberInfo.GetGenericMethodDefinition().ToString();
            GenericArguments = memberInfo.GetGenericArguments().Select(a => new TypeNode(Context, a)).ToArray();
        }

        public override MethodInfo ToMemberInfo(ExpressionContext context)
        {
            var method = base.ToMemberInfo(context);
            if (method == null) return null;

            if (IsGenericMethod && GenericArguments != null && GenericArguments.Length > 0)
            {
                var arguments = GenericArguments
                    .Select(a => a.ToType(context))
                    .Where(t => t != null).ToArray();
                if (arguments.Length > 0)
                    method = method.MakeGenericMethod(arguments);
            }
            return method;
        }
    }
}