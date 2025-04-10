using Draft.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnet_api_draft;

public class FilmeContext :DbContext
{
    public FilmeContext(DbContextOptions<FilmeContext> options) : base(options)
    {

    }
    public DbSet<Filme> Filmes { get; set; } = null!;
    
}