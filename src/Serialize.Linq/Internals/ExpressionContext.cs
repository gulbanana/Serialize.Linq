﻿#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Nodes;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Serialize.Linq.Internals
{
    public class ExpressionContext
    {
        private readonly ConcurrentDictionary<string, ParameterExpression> _parameterExpressions;
        private readonly ConcurrentDictionary<string, Type> _typeCache;

        public ExpressionContext()
        {
            _parameterExpressions = new ConcurrentDictionary<string, ParameterExpression>();
            _typeCache = new ConcurrentDictionary<string, Type>();
        }

        public virtual ParameterExpression GetParameterExpression(ParameterExpressionNode node)
        {
            if(node == null) throw new ArgumentNullException("node");

            var key = node.Type.Name + Environment.NewLine + node.Name;
            return _parameterExpressions.GetOrAdd(key, k => Expression.Parameter(node.Type.ToType(this), node.Name));
        }

        public virtual Type ResolveType(TypeNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            if (string.IsNullOrWhiteSpace(node.AssemblyQualifiedName)) return null;

            return _typeCache.GetOrAdd(node.AssemblyQualifiedName, n =>
            {
                var type = Type.GetType(n);
                if (type == null)
                {
                    throw new Exception($"Type {node.AssemblyQualifiedName} not available in current app domain");
                }
                return type;
            });
        }
    }
}
