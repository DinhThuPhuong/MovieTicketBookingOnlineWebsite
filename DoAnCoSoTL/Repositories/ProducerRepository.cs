using DoAnCoSoTL.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSoTL.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay  đổi đường dẫn theo cấu hình của bạn 
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối }

        }
        public Guid id { set; get; }
        public ProducerRepository()
        {
        id=Guid.NewGuid();
        }

        MovieContext db;
        public ProducerRepository(MovieContext _db)
        {
            db = _db;
        }
        public List<Producer> GetAll()
        {
            var producers = db.Producers.ToList();
            return producers;
        }
        public Producer GetById(int id)
        {
            return db.Producers.SingleOrDefault(n => n.Id == id);
        }
        public Producer GetByName(string name)
        {
            return db.Producers.SingleOrDefault(n => n.Name == name);
        }

        //public Producer GetByImage(byte[] image)
        //{
        //    return db.Producers.SingleOrDefault(n => n.Image == image);
        //}
        public async Task<int> insert(Producer newProducer, IFormFile Image)
        {
            if (Image != null)
            {
                // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                newProducer.Image = await SaveImage(Image);
            }
            db.Producers.Add(newProducer);
            int raws = db.SaveChanges();
            return raws;
        }
        public async Task<int> update(Producer EditProducer, int id,IFormFile Image)
        {
            if (Image != null)
            {
                // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                EditProducer.Image = await SaveImage(Image);
            }
             var Producer = await db.Producers.SingleOrDefaultAsync(n => n.Id == id);
           
                Producer.Id = EditProducer.Id;
                Producer.Name = EditProducer.Name;
                if (Image != null)
                    Producer.Image = EditProducer.Image;
                Producer.Bio = EditProducer.Bio;
                int raws = db.SaveChanges();
                return raws;
           
        }
        public int delete(int id)
        {
            Producer DelPro = db.Producers.SingleOrDefault(n => n.Id == id);
            db.Producers.Remove(DelPro);
            int raws = db.SaveChanges();
            return raws;
        }
    }
}
