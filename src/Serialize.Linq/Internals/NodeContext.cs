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
using System.Linq.Expressions;
using System.Reflection;
using Serialize.Linq.Nodes;

namespace Serialize.Linq.Internals
{
    public class NodeContext
    {
        private readonly Type[] _expectedTypes;

        public NodeContext(IEnumerable<Type> parameterTypes = null)
        {
            if (parameterTypes != null)
            {
                var expectedTypes = new HashSet<Type>();

                foreach (var type in parameterTypes)
                {
                    if (type == null) throw new ArgumentOutOfRangeException("types");
                    expectedTypes.UnionWith(MemberTypeFinder.FindTypes(type));
                }

                _expectedTypes = expectedTypes.ToArray();
            }
            else
            {
                _expectedTypes = new Type[0];
            }
        }

        public ExpressionNode Create(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            if (_expectedTypes.Any())
            {
                var member = expression as MemberExpression;
                if (member != null) return ResolveMemberExpression(member);

                var method = expression as MethodCallExpression;
                if (method != null) return ResolveMethodCallExpression(method);
            }

            return CreateWithoutResolve(expression);
        }

        private ExpressionNode CreateWithoutResolve(Expression expression)
        {
            if (expression is BinaryExpression) return new BinaryExpressionNode(this, expression as BinaryExpression);
            if (expression is ConditionalExpression) return new ConditionalExpressionNode(this, expression as ConditionalExpression);
            if (expression is ConstantExpression) return new ConstantExpressionNode(this, expression as ConstantExpression);
            if (expression is InvocationExpression) return new InvocationExpressionNode(this, expression as InvocationExpression);
            if (expression is LambdaExpression) return new LambdaExpressionNode(this, expression as LambdaExpression);
            if (expression is ListInitExpression) return new ListInitExpressionNode(this, expression as ListInitExpression);
            if (expression is MemberExpression) return new MemberExpressionNode(this, expression as MemberExpression);
            if (expression is MemberInitExpression) return new MemberInitExpressionNode(this, expression as MemberInitExpression);
            if (expression is MethodCallExpression) return new MethodCallExpressionNode(this, expression as MethodCallExpression);
            if (expression is NewArrayExpression) return new NewArrayExpressionNode(this, expression as NewArrayExpression);
            if (expression is NewExpression) return new NewExpressionNode(this, expression as NewExpression);
            if (expression is ParameterExpression) return new ParameterExpressionNode(this, expression as ParameterExpression);
            if (expression is TypeBinaryExpression) return new TypeBinaryExpressionNode(this, expression as TypeBinaryExpression);
            if (expression is UnaryExpression) return new UnaryExpressionNode(this, expression as UnaryExpression);

            throw new ArgumentException("Unknown expression of type " + expression.GetType());
        }

        /// <exception cref="System.NotSupportedException">MemberType ' + memberExpression.Member.MemberType + ' not yet supported.</exception>
        private bool TryGetConstantValueFromMemberExpression(MemberExpression memberExpression, out object constantValue, out Type constantValueType)
        {
            constantValue = null;
            constantValueType = null;

            var run = memberExpression;
            while (true)
            {
                var next = run.Expression as MemberExpression;
                if (next == null)
                    break;

                run = next;
            }

            if (IsExpectedType(run.Member.DeclaringType))
                return false;

            var field = memberExpression.Member as FieldInfo;
            if (field != null)
            {
                if (memberExpression.Expression != null)
                {
                    if (memberExpression.Expression.NodeType == ExpressionType.Constant)
                    {
                        var constantExpression = (ConstantExpression)memberExpression.Expression;
                        var flags = GetBindingFlags();
                        var fields = flags == null ? constantExpression.Type.GetFields() : constantExpression.Type.GetFields(flags.Value);
                        var memberField = fields.Single(n => memberExpression.Member.Name.Equals(n.Name));
                        constantValueType = memberField.FieldType;
                        constantValue = memberField.GetValue(constantExpression.Value);
                        return true;
                    }
                    var subExpression = memberExpression.Expression as MemberExpression;
                    if (subExpression != null)
                        return TryGetConstantValueFromMemberExpression(subExpression, out constantValue, out constantValueType);
                }
                if (field.IsPrivate || field.IsFamilyAndAssembly)
                {
                    constantValue = field.GetValue(null);
                    return true;
                }
            }
            else if (memberExpression.Member is PropertyInfo)
            {
                try
                {
                    constantValue = Expression.Lambda(memberExpression).Compile().DynamicInvoke();
                    return true;
                }
                catch (InvalidOperationException)
                {
                    constantValue = null;
                    return false;
                }
            }
            else
            {
                throw new NotSupportedException("MemberType '" + memberExpression.Member.GetType().Name + "' not yet supported.");
            }

            return false;
        }

        private ExpressionNode ResolveMemberExpression(MemberExpression memberExpression)
        {
            Expression inlineExpression;
            if (TryInlineExpression(memberExpression, out inlineExpression))
                return Create(inlineExpression);

            object constantValue;
            Type constantValueType;

            return TryGetConstantValueFromMemberExpression(memberExpression, out constantValue, out constantValueType)
                ? new ConstantExpressionNode(this, constantValue, constantValueType)
                : CreateWithoutResolve(memberExpression);
        }

        private ExpressionNode ResolveMethodCallExpression(MethodCallExpression methodCallExpression)
        {
            var memberExpression = methodCallExpression.Object as MemberExpression;
            if (memberExpression != null)
            {
                object constantValue;
                Type constantValueType;
                if (TryGetConstantValueFromMemberExpression(memberExpression, out constantValue, out constantValueType))
                {
                    if (methodCallExpression.Arguments.Count == 0)
                        return new ConstantExpressionNode(this, Expression.Lambda(methodCallExpression).Compile().DynamicInvoke());
                }
            }
            else if (methodCallExpression.Method.Name == "ToString" && methodCallExpression.Method.ReturnType == typeof(string))
            {
                var constantValue = Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
                return new ConstantExpressionNode(this, constantValue);
            }
            return CreateWithoutResolve(methodCallExpression);
        }

        private bool TryInlineExpression(MemberExpression memberExpression, out Expression inlineExpression)
        {
            inlineExpression = null;

            if (!(memberExpression.Member is FieldInfo)) return false;

            if (memberExpression.Expression == null || memberExpression.Expression.NodeType != ExpressionType.Constant) return false;

            var constantExpression = (ConstantExpression)memberExpression.Expression;
            var flags = GetBindingFlags();
            var fields = flags == null
                ? constantExpression.Type.GetFields()
                : constantExpression.Type.GetFields(flags.Value);
            var memberField = fields.Single(n => memberExpression.Member.Name.Equals(n.Name));
            var constantValue = memberField.GetValue(constantExpression.Value);

            inlineExpression = constantValue as Expression;
            return inlineExpression != null;
        }

        private bool IsExpectedType(Type declaredType)
        {
            foreach (var expectedType in _expectedTypes)
            {
                if (declaredType == expectedType || declaredType.GetTypeInfo().IsSubclassOf(expectedType))
                    return true;
                if (expectedType.GetTypeInfo().IsInterface)
                {
                    var resultTypes = declaredType.GetInterfaces();
                    if (resultTypes.Contains(expectedType))
                        return true;
                }
            }

            return false;
        }

        // returning null would disallow access to privates
        private BindingFlags? GetBindingFlags()
        {
            return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        }
    }
}