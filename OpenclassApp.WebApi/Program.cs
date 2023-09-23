using Microsoft.AspNetCore.Authentication;
using OpenclassApp.GoogleAuth;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers();
builder.Services.AddGoogleAuth(options =>
{
    options.ClientId = builder.Configuration["GoogleAuth:ClientId"]!;
    options.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"]!;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("claims",
           (HttpContext context) =>
           {
               return context.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
           });

app.MapGet("login",
           () => Results.Challenge(new AuthenticationProperties
                                   {
                                       RedirectUri = "http://localhost:5123/swagger/index.html"
                                   },
                                   authenticationSchemes: new List<string>
                                   {
                                       GoogleAuthConstants.AuthenticationScheme
                                   }));
#endregion

app.Run();
