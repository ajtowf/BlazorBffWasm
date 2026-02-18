using BackendApplication.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace BackendApplication;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        Configuration = configuration;
        Environment = env;
    }

    public IWebHostEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR();
        services.AddControllers();
        services.AddHttpContextAccessor();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://demo.duendesoftware.com";
                options.Audience = "api";

                options.RequireHttpsMetadata = true;
            });

        services.AddAuthorization();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyOrigin() // Blazor WASM origin
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapHub<ChatHub>("/chat");
            endpoints.MapControllers();
        });
    }
}
