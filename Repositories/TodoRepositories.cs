
using Todo.Models;
using Dapper;
using System.Data;
using Todo.Utilities;


namespace Todo.Repostories;


public interface ITodoRepository
{
    Task<todo> Create(todo Item);

    Task<bool> Update(todo Item);

    Task<bool> Delete(long TodoId);

    Task<todo> GetById(long TodoId);

    Task<List<todo>> GetList();

    Task<List<todo>> GetSingleTodos(long UserId);

}


public class TodoRepository : BaseRepository, ITodoRepository
{

    public TodoRepository(IConfiguration config) : base(config)

    {

    }

    public async Task<todo> Create(todo Item)
    {
        var query = $@"INSERT INTO ""{TableNames.todo}"" (tittle, user_id, created_at, updated_at,description,completed,deleted) 
       VALUES (@Tittle, @UserId, @CreatedAt, @UpdatedAt, @Description, @Completed, @Deleted)
       RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<todo>(query, Item);
            return res;
        }
    }

    public async Task<bool> Delete(long TodoId)
    {
        var query = $@"DELETE FROM ""{TableNames.todo}"" WHERE todo_id = @TodoId";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { TodoId });
            return res > 0;
        }
    }

    public async Task<todo> GetById(long TodoId)
    {
        var query = $@"SELECT * FROM ""{TableNames.todo}""
        WHERE todo_id = @TodoId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<todo>(query,
            new
            {
                TodoId = TodoId
            });
    }

    public async Task<List<todo>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.todo}""";
        List<todo> res;

        using (var con = NewConnection)
        {
            res = (await con.QueryAsync<todo>(query)).AsList();
        }

        return res;

    }

    public async Task<List<todo>> GetSingleTodos(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.todo}"" WHERE user_id = @UserId;";
        using (var con = NewConnection)
            return (await con.QueryAsync<todo>(query, new { UserId })).ToList();
    }

    public async Task<bool> Update(todo Item)
    {
        var query = $@"UPDATE ""{TableNames.todo}"" SET created_at= @CreatedAt,updated_at=@UpdatedAt,description=@Description,completed= @completed,deleted=@deleted WHERE todo_id = @TodoId";


        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);

            return rowCount == 1;
        }
    }



}
