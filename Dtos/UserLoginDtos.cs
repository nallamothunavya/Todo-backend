using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todo.DTO;

public record UserLoginDto
{
    public string Username { get; set; } = string.Empty;
    public byte[] Password { get; set; }

}