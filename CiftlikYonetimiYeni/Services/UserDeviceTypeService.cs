using CiftlikYonetimiYeni.Models;
namespace CiftlikYonetimiYeni.Services
{
    public interface IUserDeviceTypeService
    {
        Task<UserDeviceType> GetByIdAsync(int id);
        Task<IEnumerable<UserDeviceType>> GetAllAsync();
        Task<UserDeviceType> CreateAsync(UserDeviceType userDeviceType);
        Task UpdateAsync(UserDeviceType userDeviceType);
        Task DeleteAsync(int id);
    }

    public class UserDeviceTypeService : IUserDeviceTypeService
    {
        private readonly IGenericRepository<UserDeviceType> _repository;

        public UserDeviceTypeService(IGenericRepository<UserDeviceType> repository)
        {
            _repository = repository;
        }

        public async Task<UserDeviceType> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<UserDeviceType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<UserDeviceType> CreateAsync(UserDeviceType userDeviceType)
        {
            await _repository.AddAsync(userDeviceType);
            await _repository.SaveChangesAsync();
            return userDeviceType;
        }

        public async Task UpdateAsync(UserDeviceType userDeviceType)
        {
            _repository.Update(userDeviceType);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
            await _repository.SaveChangesAsync();
        }
    }


}
