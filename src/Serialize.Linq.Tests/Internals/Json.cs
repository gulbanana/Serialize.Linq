using Newtonsoft.Json;
using Serialize.Linq.Nodes;
using System.Linq.Expressions;
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Linq;

namespace Serialize.Linq.Tests
{
    internal static class Json
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Binder = new DataContractBinder()
        };

        public static string Serialize(Expression expression)
        {
            var node = ExpressionNode.FromExpression(expression);
            return JsonConvert.SerializeObject(node, _settings);
        }

        public static Expression Deserialize(string text)
        {
            var node = JsonConvert.DeserializeObject(text, _settings) as ExpressionNode;
            return node?.ToExpression();
        }

        public static ExpressionNode ToExpressionNode(this Expression expression)
        {
            return ExpressionNode.FromExpression(expression);
        }

        private class DataContractBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                if (assemblyName == "SL") assemblyName = "Serialize.Linq, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null";
                var a = Assembly.Load(new AssemblyName(assemblyName));
                var result = a.GetTypes().Where(t => DCName(t) == typeName).SingleOrDefault();
                return result ?? Type.GetType(Assembly.CreateQualifiedName(assemblyName, typeName));
            }

            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = serializedType.GetTypeInfo().Assembly.FullName;
                if (assemblyName == "Serialize.Linq, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null") assemblyName = "SL";
                typeName = DCName(serializedType) ?? serializedType.FullName;
            }

            private string DCName(Type t)
            {
                return t.GetTypeInfo().GetCustomAttribute<DataContractAttribute>()?.Name ??
                       t.GetTypeInfo().GetCustomAttribute<CollectionDataContractAttribute>()?.Name ??
                       t.FullName;
            }
        }
    }
}
