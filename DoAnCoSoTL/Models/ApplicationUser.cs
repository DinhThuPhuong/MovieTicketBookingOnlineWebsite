using DoAnCoSoTL.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Thêm các thuộc tính tùy chỉnh của người dùng
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public byte[] Image { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public virtual List<MovieOrder> MovieOrders { get; set; }
    public virtual List<Cart> Carts { get; set; }

    // Các thuộc tính khác tùy thuộc vào yêu cầu của ứng dụng

    // Thêm các liên kết đến các quan hệ khác
    //public virtual ICollection<MovieOrder> Orders { get; set; }
}
