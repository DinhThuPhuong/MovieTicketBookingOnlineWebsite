using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public class UpdateProfileRepository : IUpdateProfileRepository
    {
        MovieContext db;
        public UpdateProfileRepository(MovieContext _db)
        {
            db = _db;
        }
        public ApplicationUser GetById(string id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }
        //Update user profile -----------------------
        public async Task<int> updateAsync(string id, ApplicationUser UpdateUser,List<IFormFile> Image)
        {
            foreach (var item in Image)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        // UpdateUser.Image = stream.ToString();
                        //UpdateUser.Image = stream.ToArray();
                    }
                }
            }
            var user = db.Users.SingleOrDefault(u => u.Id == id);
           
            user.FullName = UpdateUser.FullName;
            //if (Image.Count != 0)
            //{
            //    user.Image = UpdateUser.Image;
            //}
            user.Address = UpdateUser.Address;
            int raws = db.SaveChanges();
            return raws;

        }
        //Add new user ---------------------------------------------------
        public async Task<int> insert(ApplicationUser NewUser, List<IFormFile> Image)
        {
            foreach (var item in Image)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                     //   NewUser.Image = stream.ToArray();
                    }
                }
            }
            db.Add(NewUser);
            return db.SaveChanges();
        }

    }
}
