--------------------------------------------------------------------------------------------------
****This Demo Solution implements Token Based Authentication using ASPNET WebApi and Owin.*****
---------------------------------------------------------------------------------------------------

The OAuth framework enables a third-party application to obtain limited access to a HTTP service.
Token based authentication is preferred approach for WebApi, where the token is sent to the server with each request.
It helps in implemeting loose coupling and is mobile friendly (as no cookie is used)

****Nuget Packages to be Installed****
>Install-Package Microsoft.AspNet.Identity.Owin
>Install-Package Microsoft.AspNet.WebApi.Client
>Install-Package Microsoft.Owin.Host.SystemWeb

This solution uses EntityFramework 
Following Migration commands can be used to create the auth database:
>enable-migrations -ContextTypeName OwinAuthenticationDemo.Models.OwinAuthDbContext -MigrationsDirectory:EntitiesMigrations
>Add-Migration - configuration OwinAuthenticationDemo.EntitiesMigrations.Configuration InitialEntities Scaffolding migration "InitialEntities"
>Update-Database -configuration:OwinAuthenticationDemo.EntitiesMigrations.Configuration -verbose


****Testing****
The solution can be tested through Postman by first requesting the token:
http://localhost:56391/token
send credentials in Body x-www-form-urlencoded
username: {from_your_database}
password: {from_your_database}
grant_type: password

Users can be added by calling api method:
http://localhost:56391/api/Test/AddApiUserAsync
pass the Authorization (Bearer) token in the header
user to be created is harcoded within the code (modify the code to create multiple users)

Call an authorized method:
http://localhost:56391/api/TestMethod
pass the Authorization (Bearer) token in the header


Call an authorized method (using custom authorization attribute):
http://localhost:56391/api/Test/GetSuccess
pass the Authorization (Bearer) token in the header

If Token is not passed then the api should result 'unauthorized' response