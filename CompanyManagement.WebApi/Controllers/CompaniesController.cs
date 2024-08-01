using CompanyManagement.Core.Entities;
using CompanyManagement.Infrastucture;
using CompanyManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompanyManagement.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetCompanies()
        {
            return await _companyService.GetAll();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _companyService.GetById(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company c)
        {
            await _companyService.SaveCompany(c);

            return CreatedAtAction("GetCompany", new { id = c.CompanyId }, c);
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company c)
        {
            if (id != c.CompanyId)
            {
                return BadRequest();
            }

            await _companyService.Update(c);
            return Ok(c);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var c = await _companyService.Delete(id);
            return Ok(c);
        }
    }
}
