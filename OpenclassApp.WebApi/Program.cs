using OpenclassApp.Authentication;
using OpenclassApp.GoogleSignIn;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication(AuthenticationConstants.AuthenticationScheme).AddOpenclassScheme();
builder.Services.AddGoogleSignIn(options =>
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
app.UseGoogleSignIn();
app.MapControllers();

app.MapGet("claims",
           (HttpContext context) =>
           {
               return context.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
           });

app.MapGet("login", () => Results.SignIn(new ClaimsPrincipal()));

// app.MapGet("login",
//            () => Results.Challenge(new AuthenticationProperties
//                                    {
//                                        RedirectUri = "http://localhost:5123/swagger/index.html"
//                                    },
//                                    authenticationSchemes: new List<string>
//                                    {
//                                        GoogleAuthConstants.AuthenticationScheme
//                                    }));
#endregion

app.Run();
