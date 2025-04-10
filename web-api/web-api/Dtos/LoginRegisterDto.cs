using System.ComponentModel.DataAnnotations;

namespace web_api.Dtos;

public class LoginRegisterDto
{
    [EmailAddress]
    public required string Username { get; set; }
    public required string Password { get; set; }
}
