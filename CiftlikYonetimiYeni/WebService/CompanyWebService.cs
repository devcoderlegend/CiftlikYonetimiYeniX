using Microsoft.AspNetCore.Mvc;
using CiftlikYonetimiYeni.Models;
using CiftlikYonetimiYeni.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CiftlikYonetimiYeni.WebService
{
    [Authorize]  // Tüm controller seviyesinde JWT doğrulaması
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyWebService : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyWebService(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var response = new ApiResponse<Company>();
            var company = await _companyService.GetByIdAsync(id);

            if (company == null)
            {
                response.Success = false;
                response.Message = "Company not found.";
                return NotFound(response);
            }

            response.Success = true;
            response.Message = "Company retrieved successfully.";
            response.Data = company;
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var response = new ApiResponse<IEnumerable<Company>>();
            var companies = await _companyService.GetAllAsync();

            response.Success = true;
            response.Message = "Companies retrieved successfully.";
            response.Data = companies;
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] Company company)
        {
            var response = new ApiResponse<Company>();

            await _companyService.CreateAsync(company);

            response.Success = true;
            response.Message = "Company created successfully.";
            response.Data = company;

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company company)
        {
            var response = new ApiResponse<object>();

            if (id != company.Id)
            {
                response.Success = false;
                response.Message = "Company ID mismatch.";
                return BadRequest(response);
            }

            await _companyService.UpdateAsync(company);
            response.Success = true;
            response.Message = "Company updated successfully.";
            return NoContent();
        }

        
    }
}
