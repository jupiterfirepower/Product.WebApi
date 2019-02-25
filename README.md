"# Product.WebApi .net core 2.2 C# EntityFramework Code First SQLite Json Web Token(JWT) UnitOfWork Repository" </br>
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
</br>
install postman for testing or analog.</br>
https://www.getpostman.com/downloads/</br>
</br>
postman</br>
</br> 
POST  https://localhost:44338/api/token </br>
Set Headers </br>
Content-Type application/json </br>
Set Body</br>
{</br>
  "username": "sysusr",</br>
  "password": "syspasswd"</br>
}</br>
Click Send button</br>
</br>
You get like this in Response</br>
{</br>
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzeXN1c3IiLCJlbWFpbCI6InN5c3VzckBuZXQudWEiLCJqdGkiOiIxZTNkNGEwMi02NjJlLTQzMWItOWMxYi1mYTUzNGFhNzBkZTUiLCJleHAiOjE1NTEwMjU0NzAsImlzcyI6InByb2R1Y3R3ZWJhcGkubmV0IiwiYXVkIjoicHJvZHVjdHdlYmFwaS5uZXQifQ.LNlll0ePSbvO-QXUHxmH1U6izBdYQ_B1aiUiBtGDtZg"</br>
}</br>
</br>
![Alt text](Images/PostmanGetHeader.png?raw=true "Postman Header")
</br>
![Alt text](Images/PostmanGetBody.png?raw=true "Postman Body")
</br>
GET https://localhost:44338/api/products</br>
Set Headers (Authorization Bearer {token})</br>
Authorization Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzeXN1c3IiLCJlbWFpbCI6InN5c3VzckBuZXQudWEiLCJqdGkiOiIxZTNkNGEwMi02NjJlLTQzMWItOWMxYi1mYTUzNGFhNzBkZTUiLCJleHAiOjE1NTEwMjU0NzAsImlzcyI6InByb2R1Y3R3ZWJhcGkubmV0IiwiYXVkIjoicHJvZHVjdHdlYmFwaS5uZXQifQ.LNlll0ePSbvO-QXUHxmH1U6izBdYQ_B1aiUiBtGDtZg</br>
</br>
Click Send button and you get in Response lis of products as json array.</br>
![Alt text](Images/PostmanGetProductsHeader.png?raw=true "Postman Get Products Header")
