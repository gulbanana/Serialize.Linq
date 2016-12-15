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

namespace Serialize.Linq.Internals
{
    public class MemberTypeEnumerator : IEnumerator<Type>
    {
        private static readonly Type[] _builtinTypes = new[]
        {
            typeof(bool), typeof(byte), typeof(sbyte), typeof(char), typeof(decimal), typeof(double), typeof(float), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(short), typeof(ushort),
            typeof(object), typeof(string),
            typeof(Guid), typeof(Int16),typeof(Int32),typeof(Int64), typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(TimeSpan), typeof(DateTime)
        };

        private int _currentIndex;
        private readonly Type _type;
        private readonly BindingFlags _bindingFlags;
        private readonly HashSet<Type> _seenTypes;
        private Type[] _allTypes;

        public MemberTypeEnumerator(Type type, BindingFlags bindingFlags)
        {
            if (type == null) throw new ArgumentNullException("type");

            _seenTypes = new HashSet<Type>();
            _type = type;
            _bindingFlags = bindingFlags;
            _currentIndex = -1;
        }

        public bool MoveNext()
        {
            if (!IsConsidered) return false;

            if (_allTypes == null) _allTypes = BuildTypes();

            while (++_currentIndex < _allTypes.Length)
            {
                if (IsSeenType(Current)) continue;
                AddSeenType(Current);
                if (IsConsideredType(Current)) break;
            }

            return _currentIndex < _allTypes.Length;
        }

        public void Reset() => _currentIndex = -1;

        public bool IsConsidered => IsConsideredType(_type);

        private bool IsConsideredType(Type type) => !_builtinTypes.Contains(type);

        private bool IsConsideredMember(MemberInfo member) => member is PropertyInfo;

        private bool IsSeenType(Type type) => _seenTypes.Contains(type);

        private void AddSeenType(Type type) => _seenTypes.Add(type);

        public Type Current => _allTypes[_currentIndex]; 

        object System.Collections.IEnumerator.Current => Current;

        private Type[] BuildTypes()
        {
            var types = new List<Type>();
            var members = _type.GetMembers(_bindingFlags);

            foreach (var memberInfo in members.Where(IsConsideredMember))
            {
                types.AddRange(GetRelatedTypes(GetMemberType(memberInfo)));
            }

            return types.ToArray();
        }

        private Type[] GetRelatedTypes(Type type)
        {
            var types = new List<Type> { type };
            if (type.HasElementType) types.AddRange(GetRelatedTypes(type.GetElementType()));

            if (type.GetTypeInfo().IsGenericType)
            {
                foreach (var genericType in type.GetGenericArguments())
                {
                    types.AddRange(GetRelatedTypes(genericType));
                }
            }

            return types.ToArray();
        }

        // https://github.com/dotnet/corefx/issues/4670
        public static Type GetMemberType(MemberInfo member)
        {
            PropertyInfo property = member as PropertyInfo;
            if (property != null) return property.PropertyType;

            MethodInfo method = member as MethodInfo;
            if (method != null) return method.ReturnType;

            FieldInfo field = member as FieldInfo;
            if (field != null) return field.FieldType;

            throw new NotSupportedException("Unable to get return type of MemberInfo of type " + member);
        }

        public void Dispose() { }
    }
}