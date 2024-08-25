using System.Collections.Generic;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

namespace CiftlikYonetimiYeni.Services
{
    public interface IDeviceProfileService
    {
        Task<DeviceProfile> GetByIdAsync(int id);
        Task<IEnumerable<DeviceProfile>> GetAllAsync();
        Task<DeviceProfile> CreateAsync(DeviceProfile deviceProfile);
        Task UpdateAsync(DeviceProfile deviceProfile);
        Task DeleteAsync(int id);
        Task<IEnumerable<DeviceProfile>> GetDeviceProfilesByPageAsync(int pageNumber, int pageSize);
    }

    public class DeviceProfileService : IDeviceProfileService
    {
        private readonly IGenericRepository<DeviceProfile> _deviceProfileRepository;

        public DeviceProfileService(IGenericRepository<DeviceProfile> deviceProfileRepository)
        {
            _deviceProfileRepository = deviceProfileRepository;
        }

        public async Task<DeviceProfile> GetByIdAsync(int id)
        {
            return await _deviceProfileRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DeviceProfile>> GetAllAsync()
        {
            return await _deviceProfileRepository.GetAllAsync();
        }

        public async Task<DeviceProfile> CreateAsync(DeviceProfile deviceProfile)
        {
            await _deviceProfileRepository.AddAsync(deviceProfile);
            await _deviceProfileRepository.SaveChangesAsync();
            return deviceProfile;
        }

        public async Task UpdateAsync(DeviceProfile deviceProfile)
        {
            _deviceProfileRepository.Update(deviceProfile);
            await _deviceProfileRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _deviceProfileRepository.SoftDeleteAsync(id);
            await _deviceProfileRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceProfile>> GetDeviceProfilesByPageAsync(int pageNumber, int pageSize)
        {
            var deviceProfiles = _deviceProfileRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await Task.FromResult(deviceProfiles.ToList());
        }
    }
}
