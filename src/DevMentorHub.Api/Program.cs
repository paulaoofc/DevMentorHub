using DevMentorHub.Infrastructure.Data;
using DevMentorHub.Infrastructure.Repositories;
using DevMentorHub.Infrastructure.Services;
using DevMentorHub.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using DevMentorHub.Application.Commands;
using DevMentorHub.Application.Mappings;
using Polly;
using System.Net.Http;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DevMentorHub.Application.Validators.RegisterUserCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DevMentorHubDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection") ?? builder.Configuration["ConnectionStrings:DefaultConnection"];
    opt.UseNpgsql(cs);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISnippetRepository, SnippetRepository>();
builder.Services.AddScoped<ICodeReviewRepository, CodeReviewRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

IAsyncPolicy<HttpResponseMessage> retryPolicy = Polly.Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .OrResult(resp => (int)resp.StatusCode >= 500)
    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));

builder.Services.AddHttpClient<IChatGptService, ChatGptService>()
    .AddPolicyHandler(retryPolicy);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddSingleton<IMapper>(sp => mapperConfig.CreateMapper());

var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-secret-key-change";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
