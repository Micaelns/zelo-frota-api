namespace Application.DTO.Authentic;

public class LogonRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int SoftwareId { get; set; } = 0;

}
