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
    public class DepartmentRepository :GenaricRepository<Department>, IDepartmentRepository
    {
        private readonly CompanyDbContext _dbContext;

        public DepartmentRepository(CompanyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Department>> SearchByName(string name)
        {
            return await _dbContext.departments.Where(d => d.Name.Contains(name)).ToListAsync();
        }
    }
}
