using Todo.DTO;

namespace Todo.Models
{
    public record user
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserDto asDto
        {
            get
            {
                return new UserDto
                {
                    UserId = UserId,
                    UserName = UserName,
                    Password = Password,
                    Email = Email,

                };
            }
        }

    }
}