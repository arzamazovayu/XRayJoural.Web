using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRayJournal.Core.DTOs;

namespace XRayJournal.Core.IRepositories
{
    public interface IUserRepository
    {
        public Task<UserDTO?> AuthenticateAsync(string login, string password);

        public Task<UserDTO?> GetByIdAsync(int id);

        public Task<List<UserDTO>> GetAllAsync();

        public Task<UserDTO?> GetUserByLoginAsync(string login);

        public Task<UserDTO?> GetByIdWithCabinetsAsync(int id);
    }
}
