using Library.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Infrastructure.Services
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public CrudServiceAsync(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<T?> ReadAsync(int id)
        {
            var all = await _repository.GetAllAsync();
            return all.FirstOrDefault(e => (e as dynamic).Id == id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await _repository.GetAllAsync();
            return all.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.UpdateAsync(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.DeleteAsync(element);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
