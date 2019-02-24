"# Product.WebApi" </br>
.net cli commands</br>
</br>
dotnet new sln -n ProductWebApi.sln
</br>
dotnet new webapi -lang C# -n Product.WebApi -o Product.WebApi</br>
dot net restore</br>
dotnet sln  ProductWebApi.sln add ./Product.WebApi/Product.WebApi.csproj</br>
</br>
dotnet add package Microsoft.EntityFrameworkCore.Design</br>
dotnet add package Microsoft.EntityFrameworkCore.Sqlite</br>
dotnet add package Microsoft.AspNet.Mvc.Abstractions</br>
</br>
dotnet new xUnit -n  Product.WebApi.Tests -o Product.WebApi.Tests</br>
</br>
dotnet add package Microsoft.AspNet.Mvc.Core</br>
dotnet add package Microsoft.AspNetCore.App</br>
dotnet add package Microsoft.AspNet.Mvc.Abstractions</br>
</br>
dotnet sln  ProductWebApi.sln add ./Product.WebApi.Tests/Product.WebApi.Tests.csproj</br>
</br>
dotnet ef migrations add InitialCreate</br>
dotnet ef database update</br>
dotnet ef migrations remove</br>
</br>
dotnet add package Microsoft.Extensions.Configuration.Binder</br>
dotnet add package Microsoft.Extensions.Configuration.Json</br>
dotnet add package Microsoft.Extensions.Configuration.FileExtensions</br>
dotnet add package Microsoft.Extensions.Configuration</br>
</br>
dotnet add package System.IdentityModel.Tokens.Jwt</br>
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer</br>
