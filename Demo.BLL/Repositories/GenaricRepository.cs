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
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly CompanyDbContext dbContext;

        public GenaricRepository(CompanyDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<int> Add(T item)
        {
            await dbContext.Set<T>().AddAsync(item);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(T item)
        {
            dbContext.Set<T>().Remove(item);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<int> Update(T item)
        {
            dbContext.Set<T>().Update(item);
            return await dbContext.SaveChangesAsync();
        }
        public async Task<T> GetById(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);

        }
    }
}
