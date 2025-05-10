
using DraftDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace DraftInfra.Data;

public class FilmeContext : DbContext
{
    public FilmeContext(DbContextOptions<FilmeContext> options) : base(options)
    {

    }
    public DbSet<Filme> Filmes { get; set; } = null!;
    
}