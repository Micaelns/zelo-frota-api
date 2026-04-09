namespace Domain.Entities;

public class Base()
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Deleted { get; set; } = null;
}
