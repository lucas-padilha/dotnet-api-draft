using System.ComponentModel.DataAnnotations;
using DraftDomain.Model;

namespace DraftDomain.IRepository;

public interface IFilmeRepository
{
    Task<IEnumerable<Filme>> GetAllAsync();
    Task<int> GetCountAsync();
    Task<Filme> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(Filme filme);
    Task UpdateAsync(Filme filme);
    Task DeleteAsync(Guid id);
}
