using Application;
using Application.Interface;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistance;
using System.Text;
using WebApi.Middlewares;
using WebApi.Services;
using WebApi.SharedServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtention();
builder.Services.AddApplicationServiceExtentions();
builder.Services.AddInfrastructureServiceExtentions();
builder.Services.AddPersistanceServiceExtentions(builder.Configuration);
builder.Services.AddScoped<IAuthenticatedUser,AuthenticatedUser>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	o.RequireHttpsMetadata = false;
	o.SaveToken = false;
	o.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero,
		ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
		ValidAudience = builder.Configuration["JWTSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))
	};
	o.Events = new JwtBearerEvents()
	{
		OnAuthenticationFailed = c =>
		{
			c.NoResult();
			c.Response.StatusCode = 500;
			c.Response.ContentType = "text/plain";
			return c.Response.WriteAsync(c.Exception.ToString());
		},
		OnChallenge = context =>
		{
			context.HandleResponse();
			context.Response.StatusCode = 401;
			context.Response.ContentType = "text/plain";
			return context.Response.WriteAsync("User unauthorized");
		},
		OnForbidden = context =>
		{
			context.Response.StatusCode = 403;
			context.Response.ContentType = "text/plain";
			return context.Response.WriteAsync("Access is denied due to insufficient permissions. ");
		},
	};
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
