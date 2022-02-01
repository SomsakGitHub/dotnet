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

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            List<SuperHero> result = new List<SuperHero>();
            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT  [Id],[Name] FROM [superherodb].[dbo].[SuperHeroes]";
            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
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
    }

   
}
