# Serialize.Linq.Core

This is a port of Serialize.Linq to .NET Standard. Unlike the original library, S.L.C. does not support serialization directly.
It translates System.Linq.Expressions classes to an ExpressionNode AST which is decorated with `[DataContract]` attributes for
serialization by any mechanism of your choice.

## Installation

No NuGet package is available, but the library source may be used directly.