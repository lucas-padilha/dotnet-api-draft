using DraftDomain.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DraftInfra.Data;
using DraftInfra.Repository;
using Microsoft.EntityFrameworkCore;

namespace Draft.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructureService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFilmeRepository, FilmeRepository>();

        services.AddDbContext<FilmeContext>(options =>
            options.UseMySql(configuration.GetConnectionString("FilmeConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("FilmeConnection"))));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        return services;
    }
}
