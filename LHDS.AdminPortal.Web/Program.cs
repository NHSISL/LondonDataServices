// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    // Set X-Frame-Options header
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
        await next();
    });

    // Set X-Content-Type-Options header
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        await next();
    });

    // Set Content-Security-Policy header
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Content-Security-Policy", "your-policy-here");
        await next();
    });
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.Run();
