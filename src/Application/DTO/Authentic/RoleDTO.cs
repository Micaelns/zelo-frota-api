namespace Application.DTO.Authentic
{
    public class RoleDTO
    {
        public string Name { get; set; } = string.Empty; 
        public required List<string> Permissions { get; set; }
    }
}
