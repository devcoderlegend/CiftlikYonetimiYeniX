using CiftlikYonetimiYeni.Helper;
using CiftlikYonetimiYeni.Models;
using System.Threading.Tasks;
namespace CiftlikYonetimiYeni.Services
{

    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<IEnumerable<Device>> GetUserDevicesAsync(int userId);  // Yeni metod

    }

    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<DeviceUserMapping> _deviceUserMappingRepository;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<DeviceUserMapping> deviceUserMappingRepository)
        {
            _userRepository = userRepository;
            _deviceUserMappingRepository = deviceUserMappingRepository;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email);
            return users.FirstOrDefault();
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            user.Password = PasswordHelper.HashPassword(password);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.SoftDeleteAsync(id);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
                return null;

            if (!PasswordHelper.VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
                return false;

            if (!PasswordHelper.VerifyPassword(oldPassword, user.Password))
                return false;

            user.Password = PasswordHelper.HashPassword(newPassword);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Device>> GetUserDevicesAsync(int userId)
        {
            var deviceMappings = await _deviceUserMappingRepository.FindAsync(d => d.UserId == userId);
            return deviceMappings.Select(dm => dm.Device).ToList();
        }
    }
}