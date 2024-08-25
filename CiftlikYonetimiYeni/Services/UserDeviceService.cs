using CiftlikYonetimiYeni.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CiftlikYonetimiYeni.Services
{
    public interface IUserDeviceService
    {
        Task<UserDevice> GetByIdAsync(int id);
        Task<IEnumerable<UserDevice>> GetAllAsync();
        Task<UserDevice> CreateAsync(UserDevice userDevice);
        Task UpdateAsync(UserDevice userDevice);
        Task DeleteAsync(int id);
        Task<UserDevice> GetOrCreateDeviceAsync(string deviceId, string brandName, string model, int? userDeviceTypeId, string userAgent);
    }

    public class UserDeviceService : IUserDeviceService
    {
        private readonly IGenericRepository<UserDevice> _repository;

        public UserDeviceService(IGenericRepository<UserDevice> repository)
        {
            _repository = repository;
        }

        public async Task<UserDevice> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<UserDevice>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<UserDevice> CreateAsync(UserDevice userDevice)
        {
            // Benzersiz GUID oluştur ve GeneratedKey kolonuna ata
            userDevice.GeneratedKey = Guid.NewGuid().ToString();

            await _repository.AddAsync(userDevice);
            await _repository.SaveChangesAsync();
            return userDevice;
        }

        public async Task UpdateAsync(UserDevice userDevice)
        {
            _repository.Update(userDevice);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        public async Task<UserDevice> GetOrCreateDeviceAsync(string deviceId, string brandName, string model, int? userDeviceTypeId, string userAgent)
        {
            var device = (await _repository.FindAsync(d => d.DeviceId == deviceId)).FirstOrDefault();

            if (device == null)
            {
                device = new UserDevice
                {
                    DeviceId = deviceId,
                    BrandName = brandName,
                    Model = model,
                    UserDeviceTypeId = userDeviceTypeId,
                    UserAgent = userAgent,
                    Authorized = 0,
                    IsMobile = userDeviceTypeId == 2 ? 1 : 0,
                    RegistrationDate = DateTime.UtcNow,
                    Active = 1,
                    GeneratedKey = Guid.NewGuid().ToString() // GUID oluştur ve GeneratedKey kolonuna ata
                };
                await CreateAsync(device);
            }
            else
            {
                device.BrandName = brandName;
                device.Model = model;
                device.UserAgent = userAgent;
                device.UpdateTime = DateTime.UtcNow;
                await UpdateAsync(device);
            }

            return device;
        }
    }
}
