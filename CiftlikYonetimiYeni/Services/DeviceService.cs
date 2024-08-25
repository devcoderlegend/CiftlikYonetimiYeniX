using System.Collections.Generic;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

namespace CiftlikYonetimiYeni.Services
{
    public interface IDeviceService
    {
        Task<Device> GetByIdAsync(int id);
        Task<IEnumerable<Device>> GetAllAsync();
        Task<Device> CreateAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(int id);
        Task<IEnumerable<Device>> GetDevicesByPageAsync(int pageNumber, int pageSize);
    }

    public class DeviceService : IDeviceService
    {
        private readonly IGenericRepository<Device> _deviceRepository;

        public DeviceService(IGenericRepository<Device> deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<Device> GetByIdAsync(int id)
        {
            return await _deviceRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await _deviceRepository.GetAllAsync();
        }

        public async Task<Device> CreateAsync(Device device)
        {
            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();
            return device;
        }

        public async Task UpdateAsync(Device device)
        {
            _deviceRepository.Update(device);
            await _deviceRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _deviceRepository.SoftDeleteAsync(id);
            await _deviceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByPageAsync(int pageNumber, int pageSize)
        {
            var devices = _deviceRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await Task.FromResult(devices.ToList());
        }
    }
}
