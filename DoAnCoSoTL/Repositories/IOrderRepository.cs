using Microsoft.AspNetCore.Mvc;
using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface IOrderRepository
    {
        //Task<IEnumerable<Order>> GetAllAsync();
        //Task<Order> GetByIdAsync(int id);
        ////Task AddAsync(Product product);
        ////Task UpdateAsync(Product product);
        ////Task DeleteAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        IEnumerable<Order> GetAll();
        Task<int> GetTotalQuantitySoldAsync(int movieId);

    }
}
