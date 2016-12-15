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
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberNode{TMemberInfo}"/> class.
        /// </summary>
        protected MemberNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberNode{TMemberInfo}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="memberInfo">The member info.</param>
        protected MemberNode(NodeContext factory, TMemberInfo memberInfo)
            : base(factory)
        {
            if (memberInfo != null)
                Initialize(memberInfo);
        }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>
        /// The type of the declaring.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "D")]
        public TypeNode DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        /// <value>
        /// The signature.
        /// </value>
        [DataMember(EmitDefaultValue = false, Name = "S")]
        public string Signature { get; set; }

        /// <summary>
        /// Initializes the instance using specified member info.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        protected virtual void Initialize(TMemberInfo memberInfo)
        {
            DeclaringType = new TypeNode(Context, memberInfo.DeclaringType);
            Signature = memberInfo.ToString();
        }

        /// <summary>
        /// Gets the the declaring type.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">DeclaringType is not set.</exception>
        /// <exception cref="System.TypeLoadException">Failed to load DeclaringType:  + DeclaringType</exception>
        protected Type GetDeclaringType(ExpressionContext context)
        {
            if (DeclaringType == null)
                throw new InvalidOperationException("DeclaringType is not set.");

            var declaringType = DeclaringType.ToType(context);
            if (declaringType == null)
                throw new TypeLoadException("Failed to load DeclaringType: " + DeclaringType);

            return declaringType;
        }

        /// <summary>
        /// Converts this instance to an expression.
        /// </summary>
        /// <param name="context">The expression context.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected abstract IEnumerable<TMemberInfo> GetMemberInfosForType(ExpressionContext context, Type type);

        /// <summary>
        /// Converts this instance to a member info object of type TMemberInfo.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual TMemberInfo ToMemberInfo(ExpressionContext context)
        {
            if (string.IsNullOrWhiteSpace(Signature))
                return null;

            var declaringType = GetDeclaringType(context);
            var members = GetMemberInfosForType(context, declaringType);

            var member = members.FirstOrDefault(m => m.ToString() == Signature);
            if (member == null)
                throw new Exception($"MemberInfo not found. DeclaringType: {declaringType} MemberSignature: {Signature}.");
            return member;
        }
    }
}