using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        ICarDAL _iCarDAL;
        public CarController(DataContext context, IConfiguration configuration, ICarDAL iCarDAL)
        {
            _context = context;
            _configuration = configuration;
            _iCarDAL = iCarDAL;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var res = await _iCarDAL.GetAllCarAsync();
                return Ok(res);
            }
            catch (Exception e)
            {
                return new ContentResult() { Content = e.ToString(), StatusCode = 500 };
            }
        }

        [HttpGet("getById")]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var res = await _iCarDAL.GetCarByIdAsync(id); ;
                return Ok(res);
            }
            catch (Exception e)
            {
                return new ContentResult() { Content = e.ToString(), StatusCode = 500 };
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(JsonElement json)
        {
            try
            {
                var res = await _iCarDAL.AddCar(json); ;
                return Ok(res);
            }
            catch (Exception e)
            {
                return new ContentResult() { Content = e.ToString(), StatusCode = 500 };
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(JsonElement json)
        {
            try
            {
                var res = await _iCarDAL.UpdateCar(json); ;
                return Ok(res);
            }
            catch (Exception e)
            {
                return new ContentResult() { Content = e.ToString(), StatusCode = 500 };
            }
        }

        [HttpDelete]
        public async Task<ActionResult<List<Car>>> Delete(int id)
        {
            var res = new List<Car>();

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var cmdText = "SELECT [Id],[Name] FROM [InterfacesDALDB].[dbo].[Car] WHERE Id = @id";
            await conn.OpenAsync();
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.Add(new SqlParameter("@id", $"{id}"));
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await rdr.ReadAsync())
            {
                var obj = new Car();

                obj.Id = (int)rdr["Id"];
                obj.Name = rdr["Name"] as string;

                res.Add(obj);
            }

            if (res.Count > 0)
            {
                using var conn2 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                var cmdText2 = "DELETE FROM [dbo].[Car] WHERE Id = @id";
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
