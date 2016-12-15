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
            this.Arguments = new ExpressionNodeList(this.Factory, expression.Arguments);
            this.Constructor = new ConstructorInfoNode(this.Factory, expression.Constructor);
            this.Members = new MemberInfoNodeList(this.Factory, expression.Members);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            if (this.Constructor == null)
                return Expression.New(this.Type.ToType(context));

            var constructor = this.Constructor.ToMemberInfo(context);
            if (constructor == null)
                return Expression.New(this.Type.ToType(context));

            var arguments = this.Arguments.GetExpressions(context).ToArray();
            var members = this.Members != null ? this.Members.GetMembers(context).ToArray() : null;
            return members != null && members.Length > 0 ? Expression.New(constructor, arguments, members) : Expression.New(constructor, arguments);
        }
    }
}
