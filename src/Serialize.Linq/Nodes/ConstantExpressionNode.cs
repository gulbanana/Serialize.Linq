#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using Serialize.Linq.Internals;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Serialize.Linq.Nodes
{
    [DataContract(Name = "C")]
    public class ConstantExpressionNode : ExpressionNode<ConstantExpression>
    {
        private object _value;

        public ConstantExpressionNode() { }

        public ConstantExpressionNode(NodeContext factory, object value) : this(factory, value, null) { }

        public ConstantExpressionNode(NodeContext factory, object value, Type type) : base(factory, ExpressionType.Constant)
        {
            Value = value;
            if (type != null) base.Type = new TypeNode(factory, type);
        }

        public ConstantExpressionNode(NodeContext factory, ConstantExpression expression) : base(factory, expression) { }

        public override TypeNode Type
        {
            get
            {
                return base.Type;
            }
            set
            {
                if (Value != null)
                {
                    if (value == null)
                    {
                        value = new TypeNode(Context, Value.GetType());
                    }
                    else
                    {
                        var context = new ExpressionContext();
                        if (!value.ToType(context).IsInstanceOfType(Value))
                            throw new Exception($"Type '{value.ToType(context)}' is not an instance of the current value type '{Value.GetType()}'.");
                    }
                }
                base.Type = value;
            }
        }

        [DataMember(EmitDefaultValue = false, Name = "V")]
        public object Value
        {
            get { return _value; }
            set
            {
                if (value is Expression)
                    throw new ArgumentException("Expression not allowed.", "value");

                if (value is Type)
                    _value = new TypeNode(Context, value as Type);
                else
                    _value = value;

                if (_value == null || _value is TypeNode)
                    return;

                var type = base.Type != null ? base.Type.ToType(new ExpressionContext()) : null;
                if (type == null)
                {
                    if (Context != null) base.Type = new TypeNode(Context, _value.GetType());
                    return;
                }
                _value = ValueConverter.Convert(_value, type);
            }
        }

        protected override void Initialize(ConstantExpression expression)
        {
            Value = expression.Value;
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            var typeNode = Value as TypeNode;
            if (typeNode != null) return Expression.Constant(typeNode.ToType(context), Type.ToType(context));

            return Type != null
                ? Expression.Constant(Value, Type.ToType(context))
                : Expression.Constant(Value);
        }
    }
}