using DAM.DAM.BLL.Interfaces;
using DAM.DAM.DAL.Interfaces;

namespace DAM.DAM.BLL.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
        }
    }

}
