using System.Collections.Generic;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

namespace CiftlikYonetimiYeni.Services
{
    public interface IDeviceProfileAttributeService
    {
        Task<DeviceProfileAttribute> GetByIdAsync(int id);
        Task<IEnumerable<DeviceProfileAttribute>> GetAllAsync();
        Task<DeviceProfileAttribute> CreateAsync(DeviceProfileAttribute deviceProfileAttribute);
        Task UpdateAsync(DeviceProfileAttribute deviceProfileAttribute);
        Task DeleteAsync(int id);
        Task<IEnumerable<DeviceProfileAttribute>> GetDeviceProfileAttributesByPageAsync(int pageNumber, int pageSize);
    }

    public class DeviceProfileAttributeService : IDeviceProfileAttributeService
    {
        private readonly IGenericRepository<DeviceProfileAttribute> _deviceProfileAttributeRepository;

        public DeviceProfileAttributeService(IGenericRepository<DeviceProfileAttribute> deviceProfileAttributeRepository)
        {
            _deviceProfileAttributeRepository = deviceProfileAttributeRepository;
        }

        public async Task<DeviceProfileAttribute> GetByIdAsync(int id)
        {
            return await _deviceProfileAttributeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DeviceProfileAttribute>> GetAllAsync()
        {
            return await _deviceProfileAttributeRepository.GetAllAsync();
        }

        public async Task<DeviceProfileAttribute> CreateAsync(DeviceProfileAttribute deviceProfileAttribute)
        {
            await _deviceProfileAttributeRepository.AddAsync(deviceProfileAttribute);
            await _deviceProfileAttributeRepository.SaveChangesAsync();
            return deviceProfileAttribute;
        }

        public async Task UpdateAsync(DeviceProfileAttribute deviceProfileAttribute)
        {
            _deviceProfileAttributeRepository.Update(deviceProfileAttribute);
            await _deviceProfileAttributeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _deviceProfileAttributeRepository.SoftDeleteAsync(id);
            await _deviceProfileAttributeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceProfileAttribute>> GetDeviceProfileAttributesByPageAsync(int pageNumber, int pageSize)
        {
            var deviceProfileAttributes = _deviceProfileAttributeRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await Task.FromResult(deviceProfileAttributes.ToList());
        }
    }
}
