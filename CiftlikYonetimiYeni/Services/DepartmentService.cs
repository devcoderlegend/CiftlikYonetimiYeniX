using CiftlikYonetimiYeni.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiftlikYonetimiYeni.Services
{
    public interface IDepartmentService
    {
        Task<Department> GetByIdAsync(int id);
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> CreateAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);

        // Sayfalama ile belirli bir aralıktaki Department kayıtlarını getirir.
        Task<IEnumerable<Department>> GetDepartmentsByPageAsync(int pageNumber, int pageSize);
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly IGenericRepository<Department> _repository;

        public DepartmentService(IGenericRepository<Department> repository)
        {
            _repository = repository;
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Department> CreateAsync(Department department)
        {
            await _repository.AddAsync(department);
            await _repository.SaveChangesAsync();
            return department;
        }

        public async Task UpdateAsync(Department department)
        {
            _repository.Update(department);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByPageAsync(int pageNumber, int pageSize)
        {
            // DbSet üzerinden sayfalama işlemi yapıyoruz
            return await _repository.GetAll()
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

    }
}
