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
using Serialize.Linq.Factories;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "T")]
    public class TypeNode : Node
    {
        public TypeNode() { }

        public TypeNode(INodeFactory factory, Type type)
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
                this.GenericArguments = type.GetGenericArguments().Select(t => new TypeNode(this.Factory, t)).ToArray();

                var typeDefinition = type.GetGenericTypeDefinition();
                if (isAnonymousType || !this.Factory.Settings.UseRelaxedTypeNames)
                    this.Name = typeDefinition.AssemblyQualifiedName;
                else
                    this.Name = typeDefinition.FullName;
                this.AssemblyQualifiedName = typeDefinition.AssemblyQualifiedName;
            }
            else
            {
                if (isAnonymousType || !this.Factory.Settings.UseRelaxedTypeNames)
                    this.Name = type.AssemblyQualifiedName;
                else
                    this.Name = type.FullName;
                this.AssemblyQualifiedName = type.AssemblyQualifiedName;
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
                if (string.IsNullOrWhiteSpace(this.Name))
                    return null;
                throw new SerializationException(string.Format("Failed to serialize '{0}' to a type object.", this.Name));
            }

            if (this.GenericArguments != null)
                type = type.MakeGenericType(this.GenericArguments.Select(t => t.ToType(context)).ToArray());

            return type;
        }
    }
}