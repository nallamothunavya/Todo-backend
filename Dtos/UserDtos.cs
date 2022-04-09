using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todo.DTO;

public record UserDto
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }


    [JsonPropertyName("user_name")]
    public string UserName { get; set; }


    [JsonPropertyName("password")]

    public string Password { get; set; }

    [JsonPropertyName("email")]

    public string Email { get; set; }

}

public record UserCreateDto
{


    [JsonPropertyName("user_name")]
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }


    [JsonPropertyName("password")]
    [Required]
    public string Password { get; set; }

    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; }

}

public record UserUpdateDto
{


    [JsonPropertyName("password")]

    public string Password { get; set; }

    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; }

}