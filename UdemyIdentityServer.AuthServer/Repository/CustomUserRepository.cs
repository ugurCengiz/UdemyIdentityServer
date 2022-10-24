using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Models;

namespace UdemyIdentityServer.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _context;

        public CustomUserRepository(CustomDbContext context)
        {
            _context = context;
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await  _context.CustomUser.FirstOrDefaultAsync(x => x.Email == email);

        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _context.CustomUser.FindAsync(id);
        }

        public async Task<bool> Validate(string email, string password)
        {
           return await _context.CustomUser.AnyAsync(x=>x.Email==email && x.Password==password);
        }
    }
}
