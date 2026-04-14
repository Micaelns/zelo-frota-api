namespace Api.Requests;

public class PaginateRequest
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
