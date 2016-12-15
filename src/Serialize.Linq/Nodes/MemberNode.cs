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
using System.Reflection;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "MN")]
    public abstract class MemberNode<TMemberInfo> : Node where TMemberInfo : MemberInfo
    {
        protected MemberNode() { }

        protected MemberNode(NodeContext factory, TMemberInfo memberInfo) : base(factory)
        {
            if (memberInfo != null) Initialize(memberInfo);
        }

        [DataMember(EmitDefaultValue = false, Name = "D")]
        public TypeNode DeclaringType { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "S")]
        public string Signature { get; set; }

        protected virtual void Initialize(TMemberInfo memberInfo)
        {
            DeclaringType = new TypeNode(Context, memberInfo.DeclaringType);
            Signature = memberInfo.ToString();
        }

        protected Type GetDeclaringType(ExpressionContext context)
        {
            if (DeclaringType == null) throw new InvalidOperationException("DeclaringType is not set.");

            var declaringType = DeclaringType.ToType(context);
            if (declaringType == null) throw new TypeLoadException("Failed to load DeclaringType: " + DeclaringType);

            return declaringType;
        }

        protected abstract IEnumerable<TMemberInfo> GetMemberInfosForType(ExpressionContext context, Type type);

        public virtual TMemberInfo ToMemberInfo(ExpressionContext context)
        {
            if (string.IsNullOrWhiteSpace(Signature)) return null;

            var declaringType = GetDeclaringType(context);
            var members = GetMemberInfosForType(context, declaringType);

            var member = members.FirstOrDefault(m => m.ToString() == Signature);
            if (member == null) throw new Exception($"MemberInfo not found. DeclaringType: {declaringType} MemberSignature: {Signature}.");

            return member;
        }
    }
}