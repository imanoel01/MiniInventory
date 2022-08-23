using System.Reflection;
using System.Text;
using System.Text.Json;
using Application;
using Application.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniInventory.Services;
using MiniInventoryApi;
using Persistence;
 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

// Adding Authentication  

//var audiences = (configuration["JWT:Audiences"] ?? "").Split(new string[] { "," }, StringSplitOptions.None).ToList();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        ClockSkew = TimeSpan.Zero,
    };
    options.Events = new JwtBearerEvents()
    {
        OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                        // Add some extra context for expired tokens.
                        var isExpired = false;
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                            context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                            context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                            isExpired = true;
                        }

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new UnauthorizedResponse
                        {
                            ErrorMessage = context.ErrorDescription,
                            IsExpired = isExpired
                        }));
                    },
        OnForbidden = context =>
                   {
                       context.Response.StatusCode = 403;
                       context.Response.ContentType = "application/json";
                       return context.Response.WriteAsync(JsonSerializer.Serialize(new BaseResponse
                       {
                           ErrorMessage = "You do not have the permission to access this resource"
                       }));
                   },
    };
});

builder.Services.RegisterOtherServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c=>
//    //use fully qualified object names
//                 c.CustomSchemaIds(x => x.FullName)
// );



builder.Services.AddSwaggerGen(c =>
          {
              // doing this to ignore JsonIgnored properties from FromQuery. (serialization doesn't take place using FromQuery, so it's not ignored by default
               c.OperationFilter<SwaggerSetup.RemoveJsonIgnoreFromQueryOperationFilter>();

              //use fully qualified object names
              c.CustomSchemaIds(x => x.FullName);

              c.SwaggerDoc("v1", new OpenApiInfo
              {
                  Version = "v1",
                  Title = "Accounts Microservice",
                  Description = "",
                  TermsOfService = new Uri("http://tempuri.org/terms"),
                  Contact = new OpenApiContact()
                  {
                      Name = "",
                      Email = "",
                      //  Url = new Uri("")
                  }
              });

              c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                  In = ParameterLocation.Header,
                  Description = "Bearer {your token}",
                  Name = "Authorization",
                  Type = SecuritySchemeType.ApiKey,
                  BearerFormat = "JWT"
              });

              c.AddSecurityRequirement(new OpenApiSecurityRequirement()
              {
                    {   new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] {}}
              });

              var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
              var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
              c.IncludeXmlComments(xmlPath);
          });


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
