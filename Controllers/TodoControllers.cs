using Microsoft.AspNetCore.Mvc;
using Todo.Repostories;
using Todo.Models;
using Todo.DTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Todo.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{

    private readonly ITodoRepository _todo;
    private readonly ILogger<TodoController> _logger;

    public TodoController(ILogger<TodoController> logger, ITodoRepository todo)
    {
        _logger = logger;

        _todo = todo;


    }

    [HttpGet]

    public async Task<ActionResult<List<TodoDto>>> GetAllusers()
    {

        var todoList = await _todo.GetList();

        var dtoList = todoList.Select(x => x);


        return Ok(dtoList);
    }

    [HttpGet("{todo_id}")]

    public async Task<ActionResult<TodoDto>> GetUserById([FromRoute] long todo_id)
    {
        var user = await _todo.GetById(todo_id);

        if (user is null)
            return NotFound("No user found with given todo_id");

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TodoCreateDto Data)
    {
        var authFromHeader = Request.Headers.Authorization.ToString();
        var jwt = authFromHeader.Replace("Bearer ", string.Empty);

        var handler = new JwtSecurityTokenHandler();


        var jsonToken = handler.ReadJwtToken(jwt);
        var tokenS = jsonToken as JwtSecurityToken;




        var userId = tokenS.Claims.First(claim => claim.Type == ClaimTypes.SerialNumber).Value;

        if (userId != null)
        {
            var toCreateTodo = new todo
            {
                Tittle = Data.Tittle.Trim(),

                UserId = long.Parse(userId),
                // UserId = Data.UserId,

                CreatedAt = Data.CreatedAt.UtcDateTime,

                UpdatedAt = Data.UpdatedAt.UtcDateTime,

                Description = Data.Description.Trim(),

                Completed = Data.Completed,

                Deleted = Data.Deleted,
            };

            var res = await _todo.Create(toCreateTodo);

            return StatusCode(StatusCodes.Status201Created, res);
        }
        else
        {
            return StatusCode(StatusCodes.Status403Forbidden, "unauthorized");

        }
    }

    [HttpPut("{todo_id}")]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromRoute] long todo_id,
    [FromBody] TodoUpdateDto Data)
    {
        var existing = await _todo.GetById(todo_id);
        var currentUserId = GetCurrentUserId();
        if (existing.UserId != Int32.Parse(currentUserId))
            return Unauthorized("you are not authorized");
        if (existing is null)
            return NotFound("No user found with given todo id");

        var toUpdateTodo = existing with
        {
            CreatedAt = Data.CreatedAt.UtcDateTime,

            UpdatedAt = Data.UpdatedAt.UtcDateTime,

            Description = Data.Description.Trim(),

            Completed = Data.Completed,

            Deleted = Data.Deleted,
        };

        var didUpdate = await _todo.Update(toUpdateTodo);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

        return NoContent();
    }

    [HttpDelete("{todo_id}")]
    [Authorize]
    public async Task<ActionResult> DeleteUser([FromRoute] long todo_id)
    {
        var existing = await _todo.GetById(todo_id);
        var currentUserId = GetCurrentUserId();
        if (existing.UserId != Int32.Parse(currentUserId))
            return Unauthorized("you are not authorized");

        if (existing is null)
            return NotFound("No user found with given todo id");

        var didDelete = await _todo.Delete(todo_id);

        return NoContent();

    }

    [HttpGet("singletodos")]
    [Authorize]

    public async Task<ActionResult<List<TodoDto>>> GetSingleTodos()
    {
        var id = GetCurrentUserId();
        var Todo = await _todo.GetSingleTodos(Int32.Parse(id));
        if (Todo is null)
            return NotFound("No Todo found with given todoid");


        return Ok(Todo);
    }

    private string GetCurrentUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        var userClaims = identity.Claims;

        return (userClaims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value);

    }


}




