using DoAnCoSoTL.Models;

namespace DoAnCoSoTL.Repositories
{
    public interface ICartRepository
    {
        public List<Cart> GetData(Cart cart);
  
        public void Insert(Cart mic);
        public void Delete(Cart cart);
    }
}
