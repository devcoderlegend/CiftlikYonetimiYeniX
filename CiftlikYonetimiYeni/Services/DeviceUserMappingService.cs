using System.Collections.Generic;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

namespace CiftlikYonetimiYeni.Services
{
    public interface IDeviceUserMappingService
    {
        Task<DeviceUserMapping> GetByIdAsync(int id);
        Task<IEnumerable<DeviceUserMapping>> GetAllAsync();
        Task<DeviceUserMapping> CreateAsync(DeviceUserMapping deviceUserMapping);
        Task UpdateAsync(DeviceUserMapping deviceUserMapping);
        Task DeleteAsync(int id);
        Task<IEnumerable<DeviceUserMapping>> GetDeviceUserMappingsByPageAsync(int pageNumber, int pageSize);
    }

    public class DeviceUserMappingService : IDeviceUserMappingService
    {
        private readonly IGenericRepository<DeviceUserMapping> _deviceUserMappingRepository;

        public DeviceUserMappingService(IGenericRepository<DeviceUserMapping> deviceUserMappingRepository)
        {
            _deviceUserMappingRepository = deviceUserMappingRepository;
        }

        public async Task<DeviceUserMapping> GetByIdAsync(int id)
        {
            return await _deviceUserMappingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DeviceUserMapping>> GetAllAsync()
        {
            return await _deviceUserMappingRepository.GetAllAsync();
        }

        public async Task<DeviceUserMapping> CreateAsync(DeviceUserMapping deviceUserMapping)
        {
            await _deviceUserMappingRepository.AddAsync(deviceUserMapping);
            await _deviceUserMappingRepository.SaveChangesAsync();
            return deviceUserMapping;
        }

        public async Task UpdateAsync(DeviceUserMapping deviceUserMapping)
        {
            _deviceUserMappingRepository.Update(deviceUserMapping);
            await _deviceUserMappingRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _deviceUserMappingRepository.SoftDeleteAsync(id);
            await _deviceUserMappingRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceUserMapping>> GetDeviceUserMappingsByPageAsync(int pageNumber, int pageSize)
        {
            var deviceUserMappings = _deviceUserMappingRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await Task.FromResult(deviceUserMappings.ToList());
        }
    }
}
