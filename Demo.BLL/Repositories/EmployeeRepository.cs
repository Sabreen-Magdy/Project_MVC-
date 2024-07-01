using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository:GenaricRepository<Employee> ,IEmployeeRepository
    {
        private readonly CompanyDbContext _dbContext;

        public EmployeeRepository(CompanyDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> SearchByName(string name)
        {
            return await _dbContext.employees.Where(e => e.Name.Contains(name)).ToListAsync();
        }
    }
}
