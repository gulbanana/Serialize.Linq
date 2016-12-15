#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Factories;
using System.Collections.Generic;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "L")]
    public class LambdaExpressionNode : ExpressionNode<LambdaExpression>
    {
        public LambdaExpressionNode() { }

        public LambdaExpressionNode(INodeFactory factory, LambdaExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "B")]
        public ExpressionNode Body { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "P")]
        public ExpressionNodeList Parameters { get; set; }

        protected override void Initialize(LambdaExpression expression)
        {
            Parameters = new ExpressionNodeList(Factory, expression.Parameters);
            Body = Factory.Create(expression.Body);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            var body = Body.ToExpression(context);
            var parameters = Parameters.GetParameterExpressions(context).ToArray();

            var bodyParameters = GetNodes(body).OfType<ParameterExpression>().ToArray();
            for (var i = 0; i < parameters.Length; ++i)
            {
                var matchingParameter = bodyParameters.Where(p => p.Name == parameters[i].Name && p.Type == parameters[i].Type).ToArray();
                if (matchingParameter.Length == 1)
                    parameters[i] = matchingParameter.First();
            }

            return Expression.Lambda(Type.ToType(context), body, parameters);
        }

        private static IEnumerable<Expression> GetNodes(Expression expression)
        {
            foreach (var node in GetLinkNodes(expression))
            {
                foreach (var subNode in GetNodes(node))
                    yield return subNode;
            }
            yield return expression;
        }

        private static IEnumerable<Expression> GetLinkNodes(Expression expression)
        {
            if (expression is LambdaExpression)
            {
                var lambdaExpression = (LambdaExpression)expression;

                yield return lambdaExpression.Body;
                foreach (var parameter in lambdaExpression.Parameters)
                    yield return parameter;
            }
            else if (expression is BinaryExpression)
            {
                var binaryExpression = (BinaryExpression)expression;

                yield return binaryExpression.Left;
                yield return binaryExpression.Right;
            }
            else if (expression is ConditionalExpression)
            {
                var conditionalExpression = (ConditionalExpression)expression;

                yield return conditionalExpression.IfTrue;
                yield return conditionalExpression.IfFalse;
                yield return conditionalExpression.Test;
            }
            else if (expression is InvocationExpression)
            {
                var invocationExpression = (InvocationExpression)expression;
                yield return invocationExpression.Expression;
                foreach (var argument in invocationExpression.Arguments)
                    yield return argument;
            }
            else if (expression is ListInitExpression)
            {
                yield return (expression as ListInitExpression).NewExpression;
            }
            else if (expression is MemberExpression)
            {
                yield return (expression as MemberExpression).Expression;
            }
            else if (expression is MemberInitExpression)
            {
                yield return (expression as MemberInitExpression).NewExpression;
            }
            else if (expression is MethodCallExpression)
            {
                var methodCallExpression = (MethodCallExpression)expression;
                foreach (var argument in methodCallExpression.Arguments)
                    yield return argument;
                if (methodCallExpression.Object != null)
                    yield return methodCallExpression.Object;
            }
            else if (expression is NewArrayExpression)
            {
                foreach (var item in (expression as NewArrayExpression).Expressions)
                    yield return item;
            }
            else if (expression is NewExpression)
            {
                foreach (var item in (expression as NewExpression).Arguments)
                    yield return item;
            }
            else if (expression is TypeBinaryExpression)
            {
                yield return (expression as TypeBinaryExpression).Expression;
            }
            else if (expression is UnaryExpression)
            {
                yield return (expression as UnaryExpression).Operand;
            }
        }
    }
}
