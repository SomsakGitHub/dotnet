using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public SuperHeroController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<SuperHero>>> Index()
        //{
        //    return Ok(await _context.SuperHeroes.ToListAsync());
        //}

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Index()
        {
            var res = new List<SuperHero>();

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var commandText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes]";
            await connection.OpenAsync();
            using var cmd = new SqlCommand(commandText, connection);
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            
            while (await rdr.ReadAsync())
            {
                var obj = new SuperHero();

                obj.Id = (int)rdr["Id"];
                obj.Name = rdr["Name"] as string;

                res.Add(obj);
            }

            return Ok(res);
        }

        //[HttpGet("getById")]
        //public async Task<ActionResult<SuperHero>> Get(int id)
        //{
        //    var hero = await _context.SuperHeroes.FindAsync(id);
        //    if (hero == null)
        //        return BadRequest("Hero not found.");
        //    return Ok(hero);
        //}

        [HttpGet("getById")]
        public async Task<ActionResult<SuperHero>> Index(int id)
        {
            var res = new List<SuperHero>();

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var cmdText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
            await conn.OpenAsync();
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.Add(new SqlParameter("@id", $"{id}"));
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await rdr.ReadAsync())
            {
                var obj = new SuperHero();

                obj.Id = (int)rdr["Id"];
                obj.Name = rdr["Name"] as string;

                res.Add(obj);
            }

            return Ok(res);
        }

        //[HttpPost]
        //public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        //{
        //    _context.SuperHeroes.Add(hero);
        //    await _context.SaveChangesAsync();

        //    return Ok(await _context.SuperHeroes.ToListAsync());
        //}

        //[HttpPost]
        //public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        //{
        //    using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //    conn.Open();
        //    using var cmd = conn.CreateCommand();
        //    cmd.CommandText = "INSERT INTO [dbo].[SuperHeroes] (name) VALUES (@name)";
        //    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = hero.Name;
        //    using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        //    return Ok(hero);
        //}

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> Add(JsonElement json)
        {
            var superHeroObject = JsonConvert.DeserializeObject<SuperHero>(json.ToString());

            //var name = json.GetProperty("name").GetString();
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var cmdText = "INSERT INTO [dbo].[SuperHeroes] (name) VALUES (@name)";
            await conn.OpenAsync();
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.Add(new SqlParameter("@name", $"{superHeroObject.Name}"));
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            return Ok("success");
        }

        //[HttpPut]
        //public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        //{
        //    var dbHero = await _context.SuperHeroes.FindAsync(request.Id);

        //    if (dbHero is not null)
        //    {
        //        dbHero.Name = request.Name;
        //        await _context.SaveChangesAsync();
        //        return Ok(await _context.SuperHeroes.ToListAsync());
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpPut]
        //public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        //{
        //    using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //    conn.Open();
        //    using var cmd = conn.CreateCommand();
        //    cmd.CommandText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
        //    cmd.Parameters.Add("@id", SqlDbType.Int);
        //    cmd.Parameters["@id"].Value = request.Id;
        //    using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        //    List<SuperHero> result = new List<SuperHero>();

        //    while (await reader.ReadAsync())
        //    {
        //        result.Add(new SuperHero
        //        {
        //            Id = reader.GetInt32(0),
        //            Name = reader.GetString(1)
        //        });
        //    }

        //    if (result.Count > 0)
        //    {
        //        using var connn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //        connn.Open();
        //        using var cmdd = connn.CreateCommand();
        //        cmdd.CommandText = "UPDATE [dbo].[SuperHeroes] SET [Name] = @name WHERE [Id] = @id";
        //        cmdd.Parameters.Add("@name", SqlDbType.NVarChar).Value = request.Name;
        //        cmdd.Parameters.Add("@id", SqlDbType.Int).Value = request.Id;
        //        using var readerr = await cmdd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }

        //    return Ok(request);
        //}

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> Update(JsonElement json)
        {
            var res = new List<SuperHero>();

            var superHeroObj = JsonConvert.DeserializeObject<SuperHero>(json.ToString());

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var cmdText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
            await conn.OpenAsync();
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.Add(new SqlParameter("@id", $"{superHeroObj.Id}"));
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await rdr.ReadAsync())
            {
                var obj = new SuperHero();

                obj.Id = (int)rdr["Id"];
                obj.Name = rdr["Name"] as string;

                res.Add(obj);
            }

            if (res.Count > 0)
            {
                using var conn2 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                var cmdText2 = "UPDATE [dbo].[SuperHeroes] SET [Name] = @name WHERE [Id] = @id";
                await conn2.OpenAsync();
                using var cmd2 = new SqlCommand(cmdText2, conn2);
                cmd2.Parameters.Add(new SqlParameter("@id", $"{superHeroObj.Id}"));
                cmd2.Parameters.Add(new SqlParameter("@name", $"{superHeroObj.Name}"));
                using var rdr2 = await cmd2.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                return Ok("success");
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        //{
        //    var dbHero = await _context.SuperHeroes.FindAsync(id);
        //    //dbHero.Equals(null);
        //    if (dbHero is not null)
        //    {
        //        _context.SuperHeroes.Remove(dbHero);
        //        await _context.SaveChangesAsync();

        //        return Ok(await _context.SuperHeroes.ToListAsync());
        //    }
        //    else
        //    {
        //        return BadRequest("Hero not found.");
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        //{
        //    using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //    conn.Open();
        //    using var cmd = conn.CreateCommand();
        //    cmd.CommandText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
        //    cmd.Parameters.Add("@id", SqlDbType.Int);
        //    cmd.Parameters["@id"].Value = id;
        //    using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        //    List<SuperHero> result = new List<SuperHero>();

        //    while (await reader.ReadAsync())
        //    {
        //        result.Add(new SuperHero
        //        {
        //            Id = reader.GetInt32(0),
        //            Name = reader.GetString(1)
        //        });
        //    }

        //    if (result.Count > 0)
        //    {
        //        using var connn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //        connn.Open();
        //        using var cmdd = connn.CreateCommand();
        //        cmdd.CommandText = "DELETE FROM [dbo].[SuperHeroes] WHERE Id = @id";
        //        cmdd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        //        using var readerr = await cmdd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}

        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            var res = new List<SuperHero>();

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var cmdText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
            await conn.OpenAsync();
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.Add(new SqlParameter("@id", $"{id}"));
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await rdr.ReadAsync())
            {
                var obj = new SuperHero();

                obj.Id = (int)rdr["Id"];
                obj.Name = rdr["Name"] as string;

                res.Add(obj);
            }

            if (res.Count > 0)
            {
                using var conn2 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                var cmdText2 = "DELETE FROM [dbo].[SuperHeroes] WHERE Id = @id";
                await conn2.OpenAsync();
                using var cmd2 = new SqlCommand(cmdText2, conn2);
                cmd2.Parameters.Add(new SqlParameter("@id", $"{id}"));
                using var rdr2 = await cmd2.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                return Ok("success");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
