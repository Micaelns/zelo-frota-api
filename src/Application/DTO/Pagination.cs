namespace Application.DTO;

public class Pagination
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    private int _totalItems;
    public int TotalItems
    {
        get => _totalItems;
        set
        {
            _totalItems = value;
            TotalPages = (int)Math.Ceiling((double)_totalItems / PerPage);
        }
    }

    public Pagination() { }

    public Pagination(int page, int perPage) 
    {
        CurrentPage = Math.Max(0, page);
        PerPage = Math.Clamp(perPage, 1, 100);
    }
}
