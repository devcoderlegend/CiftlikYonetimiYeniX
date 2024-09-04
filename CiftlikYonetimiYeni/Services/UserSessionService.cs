using CiftlikYonetimiYeni.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiftlikYonetimiYeni.Services
{
    public interface IUserSessionService
    {
        Task<UserSession> CreateSessionAsync(UserSession session);

        Task<UserSession> GetSessionByIdAsync(int sessionId);
        Task<IEnumerable<UserSession>> GetSessionsByUserIdAsync(int userId);
        Task<bool> ValidateSessionAsync(int userId, string sessionKey);
        Task EndSessionAsync(int sessionId);
        Task EndAllUserSessionsAsync(int userId);
        Task CleanExpiredSessionsAsync();
        

    }
    public class UserSessionService : IUserSessionService
    {
        private readonly IGenericRepository<UserSession> _sessionRepository;

        public UserSessionService(IGenericRepository<UserSession> sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<UserSession> CreateSessionAsync(UserSession session)
        {
            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();
            return session;
        }

        public async Task<UserSession> GetSessionByIdAsync(int sessionId)
        {
            return await _sessionRepository.GetByIdAsync(sessionId);
        }

        public async Task<IEnumerable<UserSession>> GetSessionsByUserIdAsync(int userId)
        {
            return await _sessionRepository.FindAsync(s => s.UserId == userId && s.Active == 1);
        }

        public async Task<bool> ValidateSessionAsync(int userId, string sessionKey)
        {
            var session = (await _sessionRepository.FindAsync(s =>
                s.UserId == userId &&
                s.GeneratedKey == sessionKey &&
                s.Active == 1 &&
                s.ExpireTime > DateTime.UtcNow
            )).FirstOrDefault();

            return session != null;
        }

        public async Task EndSessionAsync(int sessionId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session != null)
            {
                session.Active = 0;
                await _sessionRepository.SaveChangesAsync();
            }
        }

        public async Task EndAllUserSessionsAsync(int userId)
        {
            var sessions = await _sessionRepository.FindAsync(s => s.UserId == userId && s.Active == 1);
            foreach (var session in sessions)
            {
                session.Active = 0;
            }
            await _sessionRepository.SaveChangesAsync();
        }

        public async Task CleanExpiredSessionsAsync()
        {
            var expiredSessions = await _sessionRepository.FindAsync(s => s.ExpireTime < DateTime.UtcNow && s.Active == 1);
            foreach (var session in expiredSessions)
            {
                session.Active = 0;
            }
            await _sessionRepository.SaveChangesAsync();
        }

        public async Task<UserSession> GetActiveSessionAsync(int userId)
        {
            return await _sessionRepository
                .GetAll()
                .Where(s => s.UserId == userId && s.ExpireTime > DateTime.UtcNow && s.Active == 1)
                .FirstOrDefaultAsync();
        }

    }
}