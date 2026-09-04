using Microsoft.EntityFrameworkCore;
using Npgsql;
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

        public async Task<bool> UpdatePasswordAsync(int userId, string newHash, DateTime pwDate)
        {
            try{
                var user = await _dataContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                user.PwHash = newHash;
                user.PwDate = pwDate;

                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"=== DbUpdateException ===");
                Console.WriteLine($"Message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    if (ex.InnerException is PostgresException pgEx)
                    {
                        Console.WriteLine($"Postgres ErrorCode: {pgEx.SqlState}");
                        Console.WriteLine($"Detail: {pgEx.Detail}");
                        Console.WriteLine($"Hint: {pgEx.Hint}");
                        Console.WriteLine($"Where: {pgEx.Where}");
                    }
                }
                throw;
            }
        }
    }
}
