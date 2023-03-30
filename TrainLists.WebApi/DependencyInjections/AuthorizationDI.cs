using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TrainLists.Application.Exceptions;
using TrainLists.Application.Models;
using TrainLists.WebApi.Extensions;
using TrainLists.WebApi.Modules;

namespace TrainLists.WebApi.DependencyInjections
{
    public static class AuthorizationDI
    {
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthManager.ISSUER,
                ValidateAudience = true,
                ValidAudience = AuthManager.AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = AuthManager.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true

            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = tokenValidationParameters;
                        options.Events = new JwtBearerEvents()
                        {
                            OnChallenge = context =>
                            {
                                context.HandleResponse();

                                context.Response.StatusCode = (int)HttpStatusCode.OK;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteJsonAsync(
                                    new AppResponce(
                                        new AppError
                                        {
                                            Message = "Доступ запрещен",
                                            Code = ErrorCode.AccessDenied.ToString()
                                        }
                                    )
                                );
                                
                            }

                        };

                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("all" ,x =>
                {
                    x.RequireAuthenticatedUser();
                    x.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                });
            });

            //Авторизация Swagger
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type =  SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

            });

            return services;
        }
    }

}