using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenaricRepository<Employee>
    {
        public Task<IEnumerable<Employee>> SearchByName(string name);

    }
}
