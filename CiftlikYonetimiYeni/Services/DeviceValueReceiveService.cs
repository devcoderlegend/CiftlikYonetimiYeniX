using System.Collections.Generic;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

namespace CiftlikYonetimiYeni.Services
{
    public interface IDeviceValueReceiveService
    {
        Task<DeviceValueReceive> GetByIdAsync(int id);
        Task<IEnumerable<DeviceValueReceive>> GetAllAsync();
        Task<DeviceValueReceive> CreateAsync(DeviceValueReceive deviceValueReceive);
        Task UpdateAsync(DeviceValueReceive deviceValueReceive);
        Task DeleteAsync(int id);
        Task<IEnumerable<DeviceValueReceive>> GetDeviceValueReceivesByPageAsync(int pageNumber, int pageSize);
    }

    public class DeviceValueReceiveService : IDeviceValueReceiveService
    {
        private readonly IGenericRepository<DeviceValueReceive> _deviceValueReceiveRepository;

        public DeviceValueReceiveService(IGenericRepository<DeviceValueReceive> deviceValueReceiveRepository)
        {
            _deviceValueReceiveRepository = deviceValueReceiveRepository;
        }

        public async Task<DeviceValueReceive> GetByIdAsync(int id)
        {
            return await _deviceValueReceiveRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DeviceValueReceive>> GetAllAsync()
        {
            return await _deviceValueReceiveRepository.GetAllAsync();
        }

        public async Task<DeviceValueReceive> CreateAsync(DeviceValueReceive deviceValueReceive)
        {
            // Byte array verisinin null veya boş olup olmadığını kontrol et
            if (deviceValueReceive.ReceivedInformation == null || deviceValueReceive.ReceivedInformation.Length == 0)
            {
                throw new ArgumentException("ReceivedInformation cannot be null or empty");
            }

            await _deviceValueReceiveRepository.AddAsync(deviceValueReceive);
            await _deviceValueReceiveRepository.SaveChangesAsync();
            return deviceValueReceive;
        }

        public async Task UpdateAsync(DeviceValueReceive deviceValueReceive)
        {
            _deviceValueReceiveRepository.Update(deviceValueReceive);
            await _deviceValueReceiveRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _deviceValueReceiveRepository.SoftDeleteAsync(id);
            await _deviceValueReceiveRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeviceValueReceive>> GetDeviceValueReceivesByPageAsync(int pageNumber, int pageSize)
        {
            var deviceValueReceives = _deviceValueReceiveRepository.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await Task.FromResult(deviceValueReceives.ToList());
        }
    }
}
