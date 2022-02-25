using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

namespace CRUD
{
    public partial class CarDAL: ICarDAL
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public CarDAL(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<List<Car>> GetAllCarAsync()
        {
            var res = new List<Car>();

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var commandText = "SELECT [Id],[Name] FROM [InterfacesDALDB].[dbo].[Car]";
            await connection.OpenAsync();
            using var cmd = new SqlCommand(commandText, connection);
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (await rdr.ReadAsync())
            {
                var obj = new Car();

                obj.Id = (int)rdr["Id"];
                obj.Name = rdr["Name"] as string;

                res.Add(obj);
            }

            return res;
        }

        public async Task<List<Car>> GetCarByIdAsync(int id)
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

            return res;
        }

        public async Task<string> AddCar(JsonElement json)
        {
            //var name = json.GetProperty("name").GetString();
            var superHeroObject = JsonConvert.DeserializeObject<Car>(json.ToString());

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var cmdText = "INSERT INTO [dbo].[Car] (name) VALUES (@name)";
            await conn.OpenAsync();
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.Add(new SqlParameter("@name", $"{superHeroObject.Name}"));
            using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            //return Ok("success");
            return "success";
        }

        public async Task<string> UpdateCar(JsonElement json)
        {
            var res = new List<Car>();

            var superHeroObj = JsonConvert.DeserializeObject<Car>(json.ToString());

            using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var cmdText = "SELECT [Id],[Name] FROM [InterfacesDALDB].[dbo].[Car] WHERE Id = @id";
            await conn.OpenAsync();
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.Add(new SqlParameter("@id", $"{superHeroObj.Id}"));
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
                var cmdText2 = "UPDATE [dbo].[Car] SET [Name] = @name WHERE [Id] = @id";
                await conn2.OpenAsync();
                using var cmd2 = new SqlCommand(cmdText2, conn2);
                cmd2.Parameters.Add(new SqlParameter("@id", $"{superHeroObj.Id}"));
                cmd2.Parameters.Add(new SqlParameter("@name", $"{superHeroObj.Name}"));
                using var rdr2 = await cmd2.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                //return Ok("success");
                return "success";
            }
            else
            {
                //return NotFound();
                return "notFound";
            }
        }

        public async Task<string> DeleteCar(int id)
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

                //return Ok("success");
                return "success";
            }
            else
            {
                //return NotFound();
                return "notFound";
            }
        }
    }
}
