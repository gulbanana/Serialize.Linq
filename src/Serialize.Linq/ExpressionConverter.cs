using Serialize.Linq.Factories;
using Serialize.Linq.Nodes;
using System.Linq;
using System.Linq.Expressions;

namespace Serialize.Linq
{
    public class ExpressionConverter
    {
        public ExpressionNode Convert(Expression expression, FactorySettings factorySettings = null)
        {
            var factory = CreateFactory(expression, factorySettings);
            return factory.Create(expression);
        }

        protected virtual INodeFactory CreateFactory(Expression expression, FactorySettings factorySettings)
        {
            var lambda = expression as LambdaExpression;
            if(lambda != null)
                return new DefaultNodeFactory(lambda.Parameters.Select(p => p.Type), factorySettings);
            return new NodeFactory(factorySettings);
        }        
    }
}
