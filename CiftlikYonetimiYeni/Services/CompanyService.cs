using CiftlikYonetimiYeni.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CiftlikYonetimiYeni.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company> GetByIdAsync(int id);
        Task CreateAsync(Company company);
        Task UpdateAsync(Company company);
        Task SoftDeleteAsync(int id);
        Task<IEnumerable<Company>> GetCompaniesByPageAsync(int pageNumber, int pageSize);
        Task<int> SaveChangesAsync();
    }

    public class CompanyService : ICompanyService
    {
        private readonly IGenericRepository<Company> _companyRepository;

        public CompanyService(IGenericRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyRepository.GetAllAsync();
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await _companyRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Company company)
        {
            await _companyRepository.AddAsync(company);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            await _companyRepository.SoftDeleteAsync(id);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetCompaniesByPageAsync(int pageNumber, int pageSize)
        {
            return await _companyRepository.GetAll()
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _companyRepository.SaveChangesAsync();
        }
    }
}
