using DraftDomain.Model;
using DraftDomain.IRepository;
using Microsoft.EntityFrameworkCore;
using DraftInfra.Data;

namespace DraftInfra.Repository;
public class FilmeRepository : IFilmeRepository
{
    FilmeContext _context;
    public FilmeRepository(FilmeContext context)
    {
        _context = context;
    }

    public Task<Guid> AddAsync(Filme filme)
    {
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return Task.FromResult(filme.Id);
    }

    public Task DeleteAsync(Guid id)
    {
        var filme = _context.Filmes.Find(id);
        if (filme != null)
        {
            _context.Filmes.Remove(filme);
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Filme>> GetAllAsync()
    {
        var filmes = await _context.Filmes.ToListAsync();
        return filmes;
    }
    public Task<int> GetCountAsync()
    {
        var count = _context.Filmes.CountAsync();
        return Task.FromResult(count.Result);
    }

    public Task<Filme> GetByIdAsync(Guid id)
    {
        var filme = _context.Filmes.Find(id);
        if (filme == null)
        {
            throw new KeyNotFoundException($"Filme com ID {id} não encontrado.");
        }
        return Task.FromResult(filme);
    }

    public Task UpdateAsync(Filme filme)
    {
        var filmeExistente = _context.Filmes.Find(filme.Id);
        if (filmeExistente != null)
        {
            _context.Entry(filmeExistente).CurrentValues.SetValues(filme);
            _context.SaveChanges();
        }
        return Task.CompletedTask;
    }
}
