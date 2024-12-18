using MauiLibrariesGraphQL;

var builder = WebApplication.CreateBuilder(args);

//Logger
builder.Logging
        .ClearProviders()
        .AddJsonConsole();
 
// Add services to the container.
builder.Services
        .AddGraphQLServer()
        .AddQueryType<Query>();

// Add AWS Lambda support. When running the application as an AWS Serverless application, Kestrel is replaced
// with a Lambda function contained in the Amazon.Lambda.AspNetCoreServer package, which marshals the request into the ASP.NET Core hosting framework.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);


var app = builder.Build();

app.UseHttpsRedirection();
app.MapGraphQL();

app.Run();
