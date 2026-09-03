using Microsoft.EntityFrameworkCore;
using XRayJournal.Core;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.IRepositories;


namespace XRayJournal.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<UserDTO?> GetByLoginAsync(string login)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<UserDTO?> GetByIdWithCabinetsAsync(int id)
        {
            return await _dataContext.Users
                .Include(u => u.Cabinets)
                .FirstOrDefaultAsync(u => u.ID == id);
        }

        public async Task<List<UserDTO>> GetUsersByIdsAsync(List<int> ids)
        {
            return await _dataContext.Users
                .Where(u => ids.Contains(u.ID))
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(UserDTO user)
        {
            var exist = await _dataContext.Users.FindAsync(user.ID);
            if (exist == null)
            {
                return false;
            }
            _dataContext.Entry(exist).CurrentValues.SetValues(user);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
