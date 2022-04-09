using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todo.DTO;

public record TodoDto
{
    [JsonPropertyName("todo_id")]
    public long TodoId { get; set; }

    [JsonPropertyName("tittle")]

    public string Tittle { get; set; }

    [JsonPropertyName("user_id")]

    public long UserId { get; set; }

    [JsonPropertyName("created_at")]

    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]

    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("description")]

    public string Description { get; set; }

    [JsonPropertyName("completed")]

    public bool Completed { get; set; }

    [JsonPropertyName("deleted")]

    public bool Deleted { get; set; }

}

public record TodoCreateDto
{


    [JsonPropertyName("tittle")]

    public string Tittle { get; set; }

    [JsonPropertyName("user_id")]

    public long UserId { get; set; }

    [JsonPropertyName("created_at")]

    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]

    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("description")]

    public string Description { get; set; }

    [JsonPropertyName("completed")]

    public bool Completed { get; set; }

    [JsonPropertyName("deleted")]

    public bool Deleted { get; set; }


}

public record TodoUpdateDto
{


    [JsonPropertyName("created_at")]

    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]

    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("description")]

    public string Description { get; set; }

    [JsonPropertyName("completed")]

    public bool Completed { get; set; }

    [JsonPropertyName("deleted")]

    public bool Deleted { get; set; }


}