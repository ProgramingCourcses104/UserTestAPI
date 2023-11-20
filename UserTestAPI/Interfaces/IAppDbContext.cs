using Microsoft.EntityFrameworkCore;
using UserTestAPI.Models;

namespace UserTestAPI.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; set; }
        void Save();
    }
}
