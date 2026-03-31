namespace Domain.Entities;

public class Base(int id)
{
    public int Id { get; set; } = id;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Deleted { get; set; } = null;
 }
