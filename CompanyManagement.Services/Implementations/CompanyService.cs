using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyManagement.Core.Entities;
using CompanyManagement.Infrastucture;
using CompanyManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetAll()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetById(int id)
        {
            var c = await _context.Companies.FindAsync(id);

            if (c == null)
            {
                throw new Exception("No data found");
            }

            return c;
        }

        public async Task SaveCompany(Company c)
        {
            _context.Companies.Add(c);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Company c)
        {
            var existing = await _context.Companies.SingleAsync(x => x.CompanyId == c.CompanyId);
            
            //need to write some auto mapper for fieldChange
            existing.CompanyAddress = c.CompanyAddress;
            existing.CompanyName = c.CompanyName;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<Company> Delete(int id)
        {
            var c = await _context.Companies.FindAsync(id);
            if (c == null)
            {
                throw new Exception("Not found");
            }

            _context.Companies.Remove(c);
            await _context.SaveChangesAsync();

            return c;
        }
    }
}
