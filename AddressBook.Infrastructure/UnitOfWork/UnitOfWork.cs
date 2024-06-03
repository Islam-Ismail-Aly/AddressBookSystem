using AddressBook.Core.Interfaces;
using AddressBook.Infrastructure.Data;
using AddressBook.Infrastructure.Repository;

namespace AddressBook.Infrastructure.UnitOfWork
{
    public class UnitOfWork<T> : IDisposable, IUnitOfWork<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<T> _entity;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<T> Entity
        {
            get
            {
                return _entity ?? (_entity = new GenericRepository<T>(_context));
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
