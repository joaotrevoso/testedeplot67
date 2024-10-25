using Microsoft.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapGet("/webhook", async context =>
{
    var req = context.Request.Query;

    const string myToken = "teste";

    req.TryGetValue("hub.mode",out var hub);
    req.TryGetValue("hub.challenge", out var challenge);
    req.TryGetValue("hub.verify_token", out var token);

    if(!String.IsNullOrWhiteSpace(hub) && !String.IsNullOrWhiteSpace(token))
    { 
        if(hub.ToString().Equals("subscribe") && token.ToString().Equals(myToken))
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(challenge);
        }
        else
        {
            context.Response.StatusCode = 403;
        }
    }
    else
    {
        context.Response.StatusCode = 403;

    }

});

app.MapPost("/webhook", async context =>
{

});

app.Run();
