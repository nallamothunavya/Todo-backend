using Microsoft.AspNetCore.Mvc;
using Todo.Repostories;
using Todo.Models;
using Todo.DTO;





namespace Todo.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly IUserRepository _user;


    private readonly ILogger<UserController> _logger;


    public UserController(ILogger<UserController> logger, IUserRepository user)
    {
        _logger = logger;

        _user = user;





    }

    [HttpGet]

    public async Task<ActionResult<List<UserDto>>> GetAllusers()
    {

        var usersList = await _user.GetList();

        var dtoList = usersList.Select(x => x.asDto);


        return Ok(dtoList);
    }

    [HttpGet("{user_id}")]

    public async Task<ActionResult<UserDto>> GetUserById([FromRoute] long user_id)
    {
        var user = await _user.GetById(user_id);

        if (user is null)
            return NotFound("No user found with given user_id");

        return Ok(user.asDto);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UserCreateDto Data)
    {
        var toCreateuser = new user
        {
            UserName = Data.UserName.Trim(),
            Email = Data.Email.Trim(),
            Password = Data.Password.Trim(),


        };

        var res = await _user.Create(toCreateuser);

        return StatusCode(StatusCodes.Status201Created, res.asDto);
    }

    [HttpPut("{user_id}")]

    public async Task<ActionResult> UpdateUser([FromRoute] long user_id,
    [FromBody] UserUpdateDto Data)
    {
        var existing = await _user.GetById(user_id);
        if (existing is null)
            return NotFound("No user found with given user id");

        var toUpdateUser = existing with
        {
            Email = Data.Email?.Trim()?.ToLower() ?? existing.Email,
            Password = Data.Password?.Trim()?.ToLower() ?? existing.Password,

        };

        var didUpdate = await _user.Update(toUpdateUser);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

        return NoContent();
    }

    [HttpDelete("{user_id}")]

    public async Task<ActionResult> DeleteUser([FromRoute] long user_id)
    {
        var existing = await _user.GetById(user_id);

        if (existing is null)
            return NotFound("No user found with given user id");

        var didDelete = await _user.Delete(user_id);

        return NoContent();

    }

}








