using dotnet_api_draft.ViewModel;

namespace dotnet_api_draft.ViewModel;

public class SearchFilmeVM
{
    private  Dictionary<Guid,CreateFilmeVM> _dicCreatedFilmesVM;
    public SearchFilmeVM()
    {
        _dicCreatedFilmesVM = new Dictionary<Guid, CreateFilmeVM>();
    }
    public Dictionary<Guid,CreateFilmeVM> dicCreatedFilmesVM
    {
        get
        {            
            return _dicCreatedFilmesVM;
        }
        set
        {
            _dicCreatedFilmesVM = value;
        }
    }

    public int SizePage { get; set; }
    public int PageNumber { get; set; }
    public int TotalItens { get; set; }
    
    public int TotalPages
    {
        get
        {
            return TotalItens / SizePage + (TotalItens % SizePage > 0 ? 1 : 0);
        }
    }
}
