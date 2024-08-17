using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiftlikYonetimiYeni.Models;

namespace CiftlikYonetimiYeni.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetByIdAsync(int id);
        Task<IEnumerable<RefreshToken>> GetAllAsync();
        Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
        Task DeleteAsync(int id);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task RevokeAsync(string token);
        Task<bool> IsTokenActiveAsync(string token);
    }

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IGenericRepository<RefreshToken> _repository;

        public RefreshTokenService(IGenericRepository<RefreshToken> repository)
        {
            _repository = repository;
        }

        public async Task<RefreshToken> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RefreshToken>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            await _repository.AddAsync(refreshToken);
            await _repository.SaveChangesAsync();
            return refreshToken;
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _repository.Update(refreshToken);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var tokens = await _repository.FindAsync(rt => rt.Token == token);
            return tokens.FirstOrDefault();
        }

        public async Task RevokeAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);
            if (refreshToken != null)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await UpdateAsync(refreshToken);
            }
        }

        public async Task<bool> IsTokenActiveAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);
            return refreshToken != null && refreshToken.Expires > DateTime.UtcNow && refreshToken.Revoked == null;
        }
    }
}
