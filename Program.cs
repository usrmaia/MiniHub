using AutoMapper;
using Hub.API.ServiceFilters;
using Hub.Application.Interfaces;
using Hub.Application.Services;
using Hub.Application.Validators;
using Hub.Domain.DTOs;
using Hub.Domain.Exceptions;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;
using Hub.Infra.Data.Migrations;
using Hub.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Net;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Logger

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog();

#endregion

#region AutoMapper

var autoMapperConfig = new MapperConfiguration(configure =>
{
    configure.CreateMap<UserDTO, IdentityUser>().ReverseMap();
    configure.CreateMap<RoleDTO, IdentityRole>().ReverseMap();
});

builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

#endregion

#region DI

IConfiguration configuration = builder.Configuration.GetSection("Jwt");
builder.Services.AddSingleton(configuration);

builder.Services.AddTransient<AuthAndUserExtractionFilter>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<UserValidator>();
builder.Services.AddTransient<PasswordValidator>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IdentityUserRole<string>>();
builder.Services.AddTransient<SignInManager<IdentityUser>>();

builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();

builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddTransient<IUserRoleService, UserRoleService>();

builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddTransient<IFlagRepository, FlagRepository>();
builder.Services.AddTransient<IFlagService, FlagService>();

builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<IStorageService, StorageService>();

builder.Services.AddTransient<IFileFlagRepository, FileFlagRepository>();
builder.Services.AddTransient<IFileFlagService, FileFlagService>();

builder.Services.AddTransient<IFileRoleRepository, FileRoleRepository>();
builder.Services.AddTransient<IFileRoleService, FileRoleService>();

builder.Services.AddTransient<IDirectoryRepository, DirectoryRepository>();
builder.Services.AddTransient<IDirectoryService, DirectoryService>();

builder.Services.AddTransient<IDirectoryFlagRepository, DirectoryFlagRepository>();
builder.Services.AddTransient<IDirectoryFlagService, DirectoryFlagService>();

builder.Services.AddTransient<IDirectoryRoleRepository, DirectoryRoleRepository>();
builder.Services.AddTransient<IDirectoryRoleService, DirectoryRoleService>();

#endregion

#region CORS

var cors = builder.Configuration.GetSection("Cors");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: cors["PolicyName"] ?? throw new AppException("Cors: PolicyName is null!", HttpStatusCode.InternalServerError), builder =>
    {
        builder.AllowAnyOrigin()
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Authorization", "Content-Type");
    });
});

#endregion

#region Context

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // options.UseInMemoryDatabase("InMemoryDatabaseName");
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new AppException("JwtConfig: Issuer is null", HttpStatusCode.InternalServerError),
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new AppException("JwtConfig: Audience is null", HttpStatusCode.InternalServerError),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? throw new AppException("JwtConfig: Secret is null", HttpStatusCode.InternalServerError))),
        };
    }
);

#endregion

#region Swagger

builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new()
    {
        Title = "MiniHub.API - Um Hub de arquivos para o sua empresa ou projeto",
        Version = "v1",
        // Dê uma descrição longa e detalhada
        Description = "O MiniHub.API é um sistema de gerenciamento de arquivos para empresas e projetos. Com ele, é possível criar, editar, compartilhar e excluir arquivos, além de gerenciar usuários e permissões. Tudo isso na rede interna da sua empresa ou projeto.",
        Contact = new OpenApiContact
        {
            Name = "George Maia",
            Email = "georgemaiaf@gmail.com",
            Url = new Uri("https://github.com/usrmaia/")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/license/mit/"),
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setup.IncludeXmlComments(xmlPath);

    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer"
    });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

var helperMigration = new HelperMigration(app.Services);

app.UseExceptionHandler("/api/Error");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(cors["PolicyName"] ?? throw new AppException("Cors: PolicyName is null!", HttpStatusCode.InternalServerError));

app.UseAuthorization();

app.MapControllers();

app.Run();
