namespace Application.DTO.Authentic;

public class UserRoleDTO
{
    public int UserId { get; set; }
    public required List<string> Roles {  get; set; }
}