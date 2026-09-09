using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence.Repositories
{
    public class DepartamentoRepository : IRepository<Departamento>
    {
        private readonly RapidWikiDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DepartamentoRepository(RapidWikiDbContext context)
        {
            _context = context;
        
        }

        public Task<Departamento> CreateAsync(Departamento entity)
        {
            throw new NotImplementedException();
        }

        
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Departamento>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Departamento> GetByIdAsync(Guid id)
        {
            return await _context.Departamentos.FindAsync(id);
        }

        public Task UpdateAsync(Departamento entity)
        {
            throw new NotImplementedException();
        }
    }
        
}