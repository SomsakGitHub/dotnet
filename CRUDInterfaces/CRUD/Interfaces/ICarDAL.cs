using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CRUD
{
    public interface ICarDAL
    {
        Task<List<Car>> GetAllCarAsync();
        Task<List<Car>> GetCarByIdAsync(int id);
        Task<string> AddCar(JsonElement json);
        Task<string> UpdateCar(JsonElement json);
        Task<string> DeleteCar(int id);
    }
}
