namespace Application.DTO.Authentic;

public class RoleDTO
{
    public string Name { get; set; } = string.Empty;
    public int SoftwareId { get; set; } = 0;
    public required List<string> Permissions { get; set; }
}
