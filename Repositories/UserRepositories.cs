using Todo.Models;
using Dapper;
using System.Data;
using Todo.Utilities;


namespace Todo.Repostories;


public interface IUserRepository
{
    Task<user> Create(user Item);

    Task<bool> Update(user Item);

    Task<bool> Delete(long UserId);

    Task<user> GetById(long UserId);

    Task<user> GetByUsername(string username);

    Task<List<user>> GetList();

}


public class UserRepository : BaseRepository, IUserRepository
{

    public UserRepository(IConfiguration config) : base(config)

    {

    }

    public async Task<user> Create(user Item)
    {
        var query = $@"INSERT INTO ""{TableNames.user}"" (user_name, password, email) 
       VALUES (@UserName, @Password, @Email)
       RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<user>(query, Item);
            return res;
        }
    }

    public async Task<bool> Delete(long UserId)
    {
        var query = $@"DELETE FROM ""{TableNames.user}"" WHERE user_id = @UserId";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { UserId });
            return res > 0;
        }
    }

    public async Task<user> GetById(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}""
        WHERE user_id = @UserId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<user>(query,
            new
            {
                UserId = UserId
            });
    }

    public async Task<user> GetByUsername(String Username)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}""
        WHERE user_name = @Username";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<user>(query,
            new
            {
                Username = Username
            });
    }

    public async Task<List<user>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.user}""";
        List<user> res;

        using (var con = NewConnection)
        {
            res = (await con.QueryAsync<user>(query)).AsList();
        }

        return res;

    }

    public async Task<bool> Update(user Item)
    {
        var query = $@"UPDATE ""{TableNames.user}"" SET password = @Password,email = @Email WHERE user_id = @UserId";


        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);

            return rowCount == 1;
        }
    }



}