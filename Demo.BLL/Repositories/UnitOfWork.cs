using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmployeeRepository _EmployeeRepository { get; set; }
        public IDepartmentRepository _DepartmentRepository { get; set; }
        public CompanyDbContext _DbContext { get; }

        public UnitOfWork(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository, CompanyDbContext dbContext)
        {
            _EmployeeRepository=employeeRepository;
            _DepartmentRepository=departmentRepository;
            _DbContext = dbContext;
        }
    }
}
