

namespace DraftApi.ViewModel;

/// <summary>
/// Represents the view model for searching filmes.
/// </summary>
public class SearchFilmeVM
{
    private  Dictionary<Guid,CreateFilmeVM> _dicCreatedFilmesVM;
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchFilmeVM"/> class.
    /// </summary>
    public SearchFilmeVM()
    {
        _dicCreatedFilmesVM = new Dictionary<Guid, CreateFilmeVM>();
    }
    /// <summary>
    /// Gets or sets the dictionary of created filmes view models.
    /// </summary>
    public Dictionary<Guid, CreateFilmeVM> dicCreatedFilmesVM
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

    /// <summary>
    /// Gets or sets the size of the page.
    /// </summary>
    public int SizePage { get; set; }
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; }
    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int TotalItens { get; set; }

    /// <summary>
    /// Gets the total number of pages based on the total items and page size.
    /// </summary>
    public int TotalPages => TotalItens / SizePage + (TotalItens % SizePage > 0 ? 1 : 0);
}
