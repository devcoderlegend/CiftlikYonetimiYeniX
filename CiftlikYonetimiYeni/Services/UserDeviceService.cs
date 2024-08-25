using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

public interface IUserDeviceService
{
    Task<UserDevice> RegisterUserDeviceAsync(UserDevice userDevice);
    Task<UserDevice> GetUserDeviceByIdAsync(int id);
    Task<IEnumerable<UserDevice>> GetAllUserDevicesAsync();
    Task<UserDevice> GetOrCreateDeviceAsync(string deviceId, string brandName, string model, int userDeviceTypeId, string userAgent);

}

public class UserDeviceService : IUserDeviceService
{
    private readonly IGenericRepository<UserDevice> _userDeviceRepository;

    public UserDeviceService(IGenericRepository<UserDevice> userDeviceRepository)
    {
        _userDeviceRepository = userDeviceRepository;
    }

    public async Task<UserDevice> RegisterUserDeviceAsync(UserDevice userDevice)
    {
        userDevice.RegistrationDate = DateTime.UtcNow;
        userDevice.GeneratedKey = GenerateGeneratedKey(); // Rastgele anahtar oluşturma
        await _userDeviceRepository.AddAsync(userDevice);
        await _userDeviceRepository.SaveChangesAsync();
        return userDevice;
    }

    public async Task<UserDevice> GetUserDeviceByIdAsync(int id)
    {
        return await _userDeviceRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<UserDevice>> GetAllUserDevicesAsync()
    {
        return await _userDeviceRepository.GetAllAsync();
    }

    private string GenerateGeneratedKey()
    {
        return Guid.NewGuid().ToString(); // Benzersiz bir anahtar üretir
    }
    public async Task<UserDevice> GetOrCreateDeviceAsync(string deviceId, string brandName, string model, int userDeviceTypeId, string userAgent)
    {
        // Önce cihaz mevcut mu kontrol et
        var existingDevice = (await _userDeviceRepository.FindAsync(d => d.DeviceId == deviceId)).FirstOrDefault();
        if (existingDevice != null)
        {
            // Cihaz mevcutsa, onu geri döndür
            return existingDevice;
        }

        // Cihaz mevcut değilse, yeni bir cihaz oluştur
        var newDevice = new UserDevice
        {
            DeviceId = deviceId,
            BrandName = brandName,
            Model = model,
            UserDeviceTypeId = userDeviceTypeId,
            UserAgent = userAgent,
            RegistrationDate = DateTime.UtcNow
        };

        await _userDeviceRepository.AddAsync(newDevice);
        await _userDeviceRepository.SaveChangesAsync();

        return newDevice;
    }
}
