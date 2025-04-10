using dotnet_api_draft.ViewModel;
using Draft.Model;
using AutoMapper;


namespace dotnet_api_draft.Profile;

public class FilmeProfile : AutoMapper.Profile
{
    public FilmeProfile()
    {
        CreateMap<CreateFilmeVM, Filme>();    
    }    
}
