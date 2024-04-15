using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface IProducerRepository
    {
        Guid id { get; set; }
        int delete(int id);
        List<Producer> GetAll();
        Producer GetById(int id);
        Producer GetByName(string name);
        Task<int> insert(Producer newProducer, List<IFormFile> Image);
        Task<int> update(Producer EditProducer, int id, List<IFormFile> Image);
    }
}