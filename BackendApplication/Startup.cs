using BackendApplication.Hubs;
using BackendApplication.Options;
using BackendApplication.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

        // Add EF Core
        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var authOptions = new AuthOptions();
                Configuration.GetSection(AuthOptions.SectionName).Bind(authOptions);

                options.Authority = authOptions.Authority;
                options.Audience = authOptions.Audience;

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

        // Ensure database is created and migrated
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            context.Database.EnsureCreated();
            SeedDatabase(context);
        }

        app.UseEndpoints(endpoints => {
            endpoints.MapHub<ChatHub>("/chat");
            endpoints.MapControllers();
        });
    }

    private void SeedDatabase(TodoDbContext context)
    {
        if (!context.Todos.Any())
        {
            var todos = new List<Todo>
            {
                new Todo { Title = "Learn Blazor", IsCompleted = true },
                new Todo { Title = "Build Todo App", IsCompleted = true },
                new Todo { Title = "Implement EF Core", IsCompleted = false },
                new Todo { Title = "Test Database Migration", IsCompleted = false }
            };

            context.Todos.AddRange(todos);
            context.SaveChanges();
        }
    }
}
