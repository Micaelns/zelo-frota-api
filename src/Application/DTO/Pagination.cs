namespace Application.DTO;

public class Pagination
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public Pagination(int totalItems, int page, int perPage)
    {
        TotalItems = totalItems;
        CurrentPage = page;
        TotalPages = (int)Math.Ceiling((double)totalItems / perPage);
        PerPage = perPage;
    }
    public Pagination() { }
}
