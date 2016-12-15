#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System.Reflection;
using Xunit;
using Serialize.Linq.Tests.Internals;
using Serialize.Linq.Internals;

namespace Serialize.Linq.Tests
{
    
    public class MemberInfoExtensionsTests
    {
        

        [Fact]
        public void GetReturnTypeOfPropertyTest()
        {
            var actual = MemberTypeEnumerator.GetMemberType(typeof(Bar).GetProperty("FirstName"));
            Assert.Equal(typeof(string), actual);
        }

        [Fact]
        public void GetReturnTypeOfFieldTest()
        {
            var actual = MemberTypeEnumerator.GetMemberType(typeof(Bar).GetField("IsFoo"));
            Assert.Equal(typeof(bool), actual);
        }

        [Fact]
        public void GetReturnTypeOfMethodTest()
        {
            var actual = MemberTypeEnumerator.GetMemberType(typeof(Bar).GetMethod("GetName"));
            Assert.Equal(typeof(string), actual);
        }
    }
}