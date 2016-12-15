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
    internal static class MemberTypeFinder
    {
        public static IEnumerable<Type> FindTypes(Type baseType)
        {
            var retval = new HashSet<Type>();
            BuildTypes(baseType, new HashSet<Type>(), retval);
            return retval;
        }

        private static bool AnalyseTypes(IEnumerable<Type> types, ISet<Type> seen, ISet<Type> result)
        {
            return types != null && types.Aggregate(false, (current, type) => BuildTypes(type, seen, result) || current);
        }

        private static bool AnalyseType(Type baseType, ISet<Type> seen, ISet<Type> result)
        {
            bool found;
            if (baseType.HasElementType)
            {
                if (!(found = BuildTypes(baseType.GetElementType(), seen, result)))
                    found = seen.Contains(baseType.GetElementType());
            }
            else
            {
                found = true;
            }

            if (baseType.GetTypeInfo().IsGenericType)
                found = AnalyseTypes(baseType.GetGenericArguments(), seen, result) || found;

            found = AnalyseTypes(baseType.GetInterfaces(), seen, result) || found;

            if (baseType.GetTypeInfo().BaseType != null && baseType.GetTypeInfo().BaseType != typeof(object))
                found = BuildTypes(baseType.GetTypeInfo().BaseType, seen, result) || found;
            return found;
        }

        private static bool BuildTypes(Type baseType, ISet<Type> seen, ISet<Type> result)
        {
            if (seen.Contains(baseType)) return false;
            seen.Add(baseType);
            if (!AnalyseType(baseType, seen, result)) return false;

            var enumerator = new MemberTypeEnumerator(baseType, BindingFlags.Instance | BindingFlags.Public);
            if (!enumerator.IsConsidered) return false;
            result.Add(baseType);

            var retval = false;
            while (enumerator.MoveNext())
            {
                var type = enumerator.Current;
                retval = BuildTypes(type, seen, result) || retval;
            }

            return retval;
        }
    }
}
