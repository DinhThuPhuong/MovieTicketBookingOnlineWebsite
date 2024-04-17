using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public class ActorRepository : IActorRepository
    {
       
        
        MovieContext db;
        public ActorRepository(MovieContext _db)
        {
            db = _db;
        }
        public List<Actor> GetAll()
        {
            var Actors = db.Actors.ToList();
            return Actors;
        }
        public Actor GetById(int id)
        {
            return db.Actors.SingleOrDefault(n => n.Id == id);
        }
        public Actor GetByName(string name)
        {
            return db.Actors.SingleOrDefault(n => n.Name == name);
        }

        public Actor GetByImage(string image)
        {
            return db.Actors.SingleOrDefault(n => n.Image == image);
        }


        public async Task<int> insert(Actor newActor, IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await Image.CopyToAsync(stream);
                    newActor.Image = Convert.ToBase64String(stream.ToArray());
                }
            }
            db.Actors.Add(newActor);
            int raws = db.SaveChanges();
            return raws;
        }



        public async Task<int> update(Actor EditActor, int id, IFormFile Image)
        {
            var Actor = db.Actors.SingleOrDefault(n => n.Id == id);

            if (Image != null && Image.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await Image.CopyToAsync(stream);
                    EditActor.Image = Convert.ToBase64String(stream.ToArray());
                }
            }

            Actor.Id = EditActor.Id;
            Actor.Name = EditActor.Name;
            if (Image != null && Image.Length > 0)
            {
                Actor.Image = EditActor.Image;
            }
            Actor.Bio = EditActor.Bio;

            int raws = db.SaveChanges();
            return raws;
        }

        public int delete(int id)
        {
            Actor DelAct = db.Actors.SingleOrDefault(n => n.Id == id);
            db.Actors.Remove(DelAct);
            int raws = db.SaveChanges();
            return raws;
        }

    }
}
