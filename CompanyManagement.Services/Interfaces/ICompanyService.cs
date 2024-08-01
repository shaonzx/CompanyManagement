using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyManagement.Core.Entities;

namespace CompanyManagement.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAll();
        Task<Company> GetById(int id);
        Task SaveCompany(Company c);
        Task Update(Company c);
        Task<Company> Delete(int id);
    }
}
