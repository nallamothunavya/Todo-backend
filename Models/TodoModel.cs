using Todo.DTO;


namespace Todo.Models;


public record todo
{

    public long TodoId { get; set; }

    public string Tittle { get; set; }

    public long UserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Description { get; set; }

    public bool Completed { get; set; }

    public bool Deleted { get; set; }





    // public TodoDto asDto
    // {
    //     get
    //     {
    //         return new TodoDto
    //         {
    //             TodoId = TodoId,
    //             Tittle = Tittle,
    //             UserId = UserId,
    //             CreatedAt = CreatedAt,
    //             UpdatedAt = UpdatedAt,
    //             Completed = Completed,
    //             Deleted = Deleted,
    //         };
    //     }
    // }
}

