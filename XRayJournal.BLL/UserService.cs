using XRayJournal.Core.IRepositories;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.InputModels;
using Mapster;
using XRayJournal.Core.DTOs;
using XRayJournal.Core.Results;
using XRayJournal.Core;
using XRayJournal.Core.Models;

namespace XRayJournal.BLL
{
    public class UserService
    {
        public IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<UserModel>> AuthenticateAsync(string login, string password)
        {
            try
            {
                var userDto = await _userRepository.AuthenticateAsync(login, password);

                if (userDto == null)
                {
                    return OperationResult<UserModel>.Fail("Неверный логин или пароль");
                }

                var userModel = new UserModel
                {
                    Login = userDto.Login,
                    Role = userDto.Role,
                    FIO = userDto.FIO,
                    FIOshort = userDto.FIOshort,
                    IsAuthenticated = true
                };

                return OperationResult<UserModel>.Ok(userModel);
            }
            catch(Exception ex)
            {
                return OperationResult<UserModel>.Fail($"Ошибка аутентификации: {ex.Message}");
            }
        }

        public async Task<UserRole> GetUserRoleAsync(string login)
        {
            var userDto = await _userRepository.GetUserByLoginAsync(login);
            return userDto?.Role ?? UserRole.Laborant; // Проверка на null
        }

        public async Task<OperationResult<UserOutputModel>> GetUserByLoginAsync(string login) {
            try 
            {
                var user = await _userRepository.GetUserByLoginAsync(login);
                if (user == null)
                {
                    return OperationResult<UserOutputModel>.Fail("Пользователь не найден");
                }

                var outputModel = user.Adapt<UserOutputModel>();
                return OperationResult<UserOutputModel>.Ok(outputModel);
            }
            catch (Exception ex)
            {
                return OperationResult<UserOutputModel>.Fail($"Ошибка при получении пользователя: {ex.Message}");
            }
        }

    }
}
