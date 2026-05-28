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

        // Проверка логина и пароля
        public async Task<UserDTO?> AuthenticateAsync(string login, string password)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }

        // Получение пользователя по ID
        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        // Получение всех пользователей
        public async Task<List<UserDTO>> GetAllAsync()
        {
            return await _dataContext.Users.ToListAsync();
        }

        // Получение пользователя по логину
        public async Task<UserDTO?> GetUserByLoginAsync(string login)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
