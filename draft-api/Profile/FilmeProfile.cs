using DraftApi.ViewModel;
using DraftDomain.Model;

namespace DraftApi.Profile;
/// <summary>
/// AutoMapper profile for mapping between Filme and CreateFilmeVM objects.
/// </summary>
public class FilmeProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FilmeProfile"/> class.
    /// </summary>
    public FilmeProfile()
    {
        CreateMap<CreateFilmeVM, Filme>();    
        CreateMap<Filme, CreateFilmeVM>();    
        // CreateMap<Filme, SearchFilmeVm
    }    
}
