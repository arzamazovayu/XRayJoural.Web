using Mapster;
using XRayJournal.Core;
using XRayJournal.Core.IRepositories;
using XRayJournal.Core.Models;
using XRayJournal.Core.OutputModels;
using XRayJournal.Core.Results;

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
                var userDto = await _userRepository.GetByLoginAsync(login);

                if (userDto == null)
                {
                    return OperationResult<UserModel>.Fail("Неверный логин или пароль");
                }

                bool IsValidPassword = BCrypt.Net.BCrypt.Verify(password, userDto.PwHash);

                if (!IsValidPassword)
                {
                    return OperationResult<UserModel>.Fail("Неверный логин или пароль");
                }

                string defaultHash = "$2a$12$VqlwKy2yA/1hwMK9YWHoxuCbS.wZdCS/iZ5.V2FyKcUwsfAqEscOa";
                bool isDefault = (userDto.PwHash == defaultHash);

                var userModel = new UserModel
                {
                    Login = userDto.Login,
                    Role = userDto.Role,
                    ID = userDto.ID,
                    CabinetId = userDto.CabinetId,
                    IsAuthenticated = true,
                    RequiresPasswordChange = true,
                };

                return OperationResult<UserModel>.Ok(userModel);
            }
            catch (Exception ex)
            {
                return OperationResult<UserModel>.Fail($"Ошибка аутентификации: {ex.Message}");
            }
        }

        public async Task<int> GetUserCabinetIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdWithCabinetsAsync(userId);
            // Если у пользователя несколько кабинетов, можно вернуть первый или текущий
            return user?.Cabinets?.FirstOrDefault()?.Id ?? 0;
        }

        public async Task<OperationResult<UserOutputModel>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user != null)
                {
                    var result = user.Adapt<UserOutputModel>();
                    return OperationResult<UserOutputModel>.Ok(result);
                }
                else
                {
                    return OperationResult<UserOutputModel>.Fail($"Пользователь с id = {id} не найден.");
                }
            }
            catch (Exception ex)
            {
                return OperationResult<UserOutputModel>.Fail($"Ошибка поиска пользователя: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<UserOutputModel>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var result = users.Adapt<List<UserOutputModel>>();
                return OperationResult<List<UserOutputModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<List<UserOutputModel>>.Fail($"Ошибка получения пользователей: {ex.Message}");
            }
        }
    }
}
