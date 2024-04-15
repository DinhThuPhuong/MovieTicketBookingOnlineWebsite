namespace DoAnCoSoTL.Repositories
{
    public interface IUpdateProfileRepository
    {
        ApplicationUser GetById(string id);
        Task<int> insert(ApplicationUser NewUser, List<IFormFile> Image);
        Task<int> updateAsync(string id, ApplicationUser UpdateUser, List<IFormFile> Image);
    }
}