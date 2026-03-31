namespace Domain.Entities;

public class Base()
{
    public int Id { get; set; } = 0;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Deleted { get; set; } = null;
}
