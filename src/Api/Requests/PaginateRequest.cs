namespace Api.Requests;

public class PaginateRequest
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 10;
}
