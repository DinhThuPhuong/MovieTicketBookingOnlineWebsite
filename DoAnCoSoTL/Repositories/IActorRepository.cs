using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface IActorRepository
    {
       
        int delete(int id);
        List<Actor> GetAll();
        Actor GetById(int id);
        Actor GetByName(string name);
     Task< int> insert(Actor newActor,List<IFormFile> Image);
        Task<int> update(Actor EditActor, int id, List<IFormFile> Image);
    }
}