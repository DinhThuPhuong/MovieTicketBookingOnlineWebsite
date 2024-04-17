using DoAnCoSoTL.Models;
using Microsoft.CodeAnalysis.Differencing;

namespace DoAnCoSoTL.Repositories
{
    public class CinemaRepository : ICinemaRepository
    {
        MovieContext db;
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay  đổi đường dẫn theo cấu hình của bạn 
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối }

        }
        public CinemaRepository(MovieContext _db)
        {
            db = _db;
        }
        public List<Cinema> GetAll()
        {
            var cinemas = db.Cinemas.ToList();
            return cinemas;
        }
        public Cinema GetById(int id)
        {
            return db.Cinemas.SingleOrDefault(c => c.Id == id);
        }
        public Cinema GetByName(string name)
        {
            return db.Cinemas.SingleOrDefault(c => c.Name == name);
        }
        public Cinema GetByLocation(string location)
        {
            return db.Cinemas.SingleOrDefault(c => c.Location == location);
        }
       
        
        public int delete(int id)
        {
            Cinema delcin = db.Cinemas.SingleOrDefault(c => c.Id == id);
            db.Cinemas.Remove(delcin);
            int raws = db.SaveChanges();
            return raws;
        }

        public async Task<int> insert(Cinema newCinema, IFormFile Image)
        {  
            if (Image != null)
            {
                // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                newCinema.Image = await SaveImage(Image);
            }

            db.Cinemas.Add(newCinema);
            int raws = db.SaveChanges();
            return raws;
        }

        public async Task<int> update(Cinema EditCin, int id, IFormFile Image)
        {
            var cinema = db.Cinemas.SingleOrDefault(c => c.Id == id);
            if (Image != null)
            {
                // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                EditCin.Image = await SaveImage(Image);
            }


            cinema.Name = EditCin.Name;
            cinema.Location = EditCin.Location;
            if (Image!= null)
                cinema.Image = EditCin.Image;
            cinema.Location = EditCin.Location;
            int raws = db.SaveChanges();
            return raws;
        }
    }
}
