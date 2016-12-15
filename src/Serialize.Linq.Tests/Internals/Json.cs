using Newtonsoft.Json;
using Serialize.Linq.Nodes;
using System.Linq.Expressions;

namespace Serialize.Linq.Tests
{
    internal static class Json
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,       
        };

        private static readonly ExpressionConverter _converter = new ExpressionConverter();

        public static string Serialize(Expression expression)
        {
            var node = _converter.Convert(expression);
            return JsonConvert.SerializeObject(node, _settings);
        }

        public static Expression Deserialize(string text)
        {
            var node = JsonConvert.DeserializeObject(text, _settings) as ExpressionNode;
            return node?.ToExpression();
        }

        public static ExpressionNode ToExpressionNode(this Expression expression)
        {
            return _converter.Convert(expression);
        }
    }
}
