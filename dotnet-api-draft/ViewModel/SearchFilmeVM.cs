using dotnet_api_draft.ViewModel;

namespace dotnet_api_draft.ViewModel;

public class SearchFilmeVM
{
    public List<CreateFilmeVM> listCreatedFilmesVM
    {
        get
        {
            return listCreatedFilmesVM.Skip(PageNumber * SizePage).Take(SizePage).ToList();
        }
        set
        {
            listCreatedFilmesVM = value;
        }
    }

    public int SizePage { get; set; }
    public int PageNumber { get; set; }
    public int TotalItens
    {
        get
        {
            return listCreatedFilmesVM.Count;
        }
    }
    public int TotalPages
    {
        get
        {
            return listCreatedFilmesVM.Count / SizePage + (listCreatedFilmesVM.Count % SizePage > 0 ? 1 : 0);
        }
    }
}
