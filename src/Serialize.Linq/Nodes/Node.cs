#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Runtime.Serialization;
using Serialize.Linq.Internals;

namespace Serialize.Linq.Nodes
{
    [DataContract]
    [KnownType(typeof(BinaryExpressionNode))]
    [KnownType(typeof(ConditionalExpressionNode))]
    [KnownType(typeof(ConstantExpressionNode))]
    [KnownType(typeof(ConstructorInfoNode))]
    [KnownType(typeof(ElementInitNode))]
    [KnownType(typeof(ElementInitNodeList))]
    [KnownType(typeof(ExpressionNode))]
    [KnownType(typeof(ExpressionNodeList))]
    [KnownType(typeof(FieldInfoNode))]
    [KnownType(typeof(InvocationExpressionNode))]
    [KnownType(typeof(LambdaExpressionNode))]
    [KnownType(typeof(ListInitExpressionNode))]
    [KnownType(typeof(MemberAssignmentNode))]
    [KnownType(typeof(MemberBindingNode))]
    [KnownType(typeof(MemberBindingNodeList))]
    [KnownType(typeof(MemberExpressionNode))]
    [KnownType(typeof(MemberInfoNode))]
    [KnownType(typeof(MemberInfoNodeList))]    
    [KnownType(typeof(MemberInitExpressionNode))]
    [KnownType(typeof(MemberListBindingNode))]
    [KnownType(typeof(MemberMemberBindingNode))]
    [KnownType(typeof(MethodCallExpressionNode))]
    [KnownType(typeof(NewArrayExpressionNode))]
    [KnownType(typeof(NewExpressionNode))]
    [KnownType(typeof(ParameterExpressionNode))]
    [KnownType(typeof(PropertyInfoNode))]    
    [KnownType(typeof(TypeBinaryExpressionNode))]
    [KnownType(typeof(TypeNode))]
    [KnownType(typeof(UnaryExpressionNode))]
    public abstract class Node
    {
        protected Node() { }

        protected Node(NodeContext context)
        {
            if(context == null) throw new ArgumentNullException("context");
            Context = context;
        }

        [IgnoreDataMember]
        public readonly NodeContext Context;        
    }
}
