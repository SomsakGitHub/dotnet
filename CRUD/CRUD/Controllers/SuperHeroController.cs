using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

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
        //public async Task<ActionResult<List<SuperHero>>> Get()
        //{
        //    return Ok(await _context.SuperHeroes.ToListAsync());
        //}

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes]";
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            List<SuperHero> result = new List<SuperHero>();

            while (await reader.ReadAsync())
            {
                result.Add(new SuperHero
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<SuperHero>> Get(int id)
        //{
        //    var hero = await _context.SuperHeroes.FindAsync(id);
        //    if (hero == null)
        //        return BadRequest("Hero not found.");
        //    return Ok(hero);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = id;
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            List<SuperHero> result = new List<SuperHero>();

            while (await reader.ReadAsync())
            {
                result.Add(new SuperHero
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return Ok(result);
        }

        //[HttpPost]
        //public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        //{
        //    _context.SuperHeroes.Add(hero);
        //    await _context.SaveChangesAsync();

        //    return Ok(await _context.SuperHeroes.ToListAsync());
        //}

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO [dbo].[SuperHeroes] (name) VALUES (@name)";
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = hero.Name;
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            return Ok(hero);
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

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = request.Id;
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            List<SuperHero> result = new List<SuperHero>();

            while (await reader.ReadAsync())
            {
                result.Add(new SuperHero
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            if (result.Count > 0)
            {
                using var connn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                connn.Open();
                using var cmdd = connn.CreateCommand();
                cmdd.CommandText = "UPDATE [dbo].[SuperHeroes] SET [Name] = @name WHERE [Id] = @id";
                cmdd.Parameters.Add("@name", SqlDbType.NVarChar).Value = request.Name;
                cmdd.Parameters.Add("@id", SqlDbType.Int).Value = request.Id;
                using var readerr = await cmdd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            else
            {
                return NotFound();
            }

            return Ok(request);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes] WHERE Id = @id";
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = id;
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            List<SuperHero> result = new List<SuperHero>();

            while (await reader.ReadAsync())
            {
                result.Add(new SuperHero
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            if (result.Count > 0)
            {
                using var connn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                connn.Open();
                using var cmdd = connn.CreateCommand();
                cmdd.CommandText = "DELETE FROM [dbo].[SuperHeroes] WHERE Id = @id";
                cmdd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                using var readerr = await cmdd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            else
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
