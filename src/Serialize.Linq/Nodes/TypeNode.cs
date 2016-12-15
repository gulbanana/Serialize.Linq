#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Serialize.Linq.Internals;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "T")]
    public class TypeNode : Node
    {
        public TypeNode() { }

        public TypeNode(NodeContext factory, Type type)
            : base(factory)
        {
            Initialize(type);
        }

        private void Initialize(Type type)
        {
            if (type == null)
                return;

            bool isAttributeDefined = type.GetTypeInfo().GetCustomAttribute(typeof(CompilerGeneratedAttribute)) != null;

            var isAnonymousType = isAttributeDefined
                && type.GetTypeInfo().IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.GetTypeInfo().Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;

            if (type.GetTypeInfo().IsGenericType)
            {
                GenericArguments = type.GetGenericArguments().Select(t => new TypeNode(Context, t)).ToArray();

                var typeDefinition = type.GetGenericTypeDefinition();
                if (isAnonymousType)
                    Name = typeDefinition.AssemblyQualifiedName;
                else
                    Name = typeDefinition.FullName;
                AssemblyQualifiedName = typeDefinition.AssemblyQualifiedName;
            }
            else
            {
                if (isAnonymousType)
                    Name = type.AssemblyQualifiedName;
                else
                    Name = type.FullName;
                AssemblyQualifiedName = type.AssemblyQualifiedName;
            }
        }

        [DataMember(EmitDefaultValue = false, Name = "N")]        
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "Q")]
        public string AssemblyQualifiedName { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "G")]        
        public TypeNode[] GenericArguments { get; set; }

        public Type ToType(ExpressionContext context)
        {
            var type = context.ResolveType(this);
            if (type == null)
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return null;
                throw new SerializationException(string.Format("Failed to serialize '{0}' to a type object.", Name));
            }

            if (GenericArguments != null)
                type = type.MakeGenericType(GenericArguments.Select(t => t.ToType(context)).ToArray());

            return type;
        }
    }
}