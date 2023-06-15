using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configurar as dependências e serviços da aplicação

        // Exemplo: Configurar uma conexão de banco de dados usando a configuração do appsettings.json
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<SeuDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Exemplo: Registrar um serviço personalizado
        services.AddScoped<IMeuServico, MeuServico>();

        // Exemplo: Configurar autenticação
        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]))
                };
            });

        // Mais configurações e serviços...

        // Configurar o MVC (se estiver usando)
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configurar o pipeline de requisição HTTP
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Configurações de produção
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        // Configurações adicionais do pipeline...

        // Usar o MVC (se estiver usando)
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
