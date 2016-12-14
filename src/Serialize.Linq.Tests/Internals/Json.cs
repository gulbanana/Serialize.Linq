using Newtonsoft.Json;
using Serialize.Linq.Extensions;
using Serialize.Linq.Nodes;
using System.Linq.Expressions;

namespace Serialize.Linq.Tests
{
    internal static class Json
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,       
        };

        public static string Serialize(Expression e)
        {
            var node = e.ToExpressionNode();
            return JsonConvert.SerializeObject(node, settings);
        }

        public static Expression Deserialize(string s)
        {
            var node = JsonConvert.DeserializeObject(s, settings) as ExpressionNode;
            return node?.ToExpression();
        }
    }
}
