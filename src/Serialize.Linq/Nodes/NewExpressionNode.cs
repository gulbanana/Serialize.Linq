#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Factories;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "N")]
    public class NewExpressionNode : ExpressionNode<NewExpression>
    {
        public NewExpressionNode() { }

        public NewExpressionNode(INodeFactory factory, NewExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false, Name = "A")]
        public ExpressionNodeList Arguments { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "C")]
        public ConstructorInfoNode Constructor { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "M")]
        public MemberInfoNodeList Members { get; set; }

        protected override void Initialize(NewExpression expression)
        {
            Arguments = new ExpressionNodeList(Factory, expression.Arguments);
            Constructor = new ConstructorInfoNode(Factory, expression.Constructor);
            Members = new MemberInfoNodeList(Factory, expression.Members);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            if (Constructor == null)
                return Expression.New(Type.ToType(context));

            var constructor = Constructor.ToMemberInfo(context);
            if (constructor == null)
                return Expression.New(Type.ToType(context));

            var arguments = Arguments.GetExpressions(context).ToArray();
            var members = Members != null ? Members.GetMembers(context).ToArray() : null;
            return members != null && members.Length > 0 ? Expression.New(constructor, arguments, members) : Expression.New(constructor, arguments);
        }
    }
}
