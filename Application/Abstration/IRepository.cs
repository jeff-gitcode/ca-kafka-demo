using Domain;

namespace Application.Abstration;

public interface IRepository<T> where T : User
{
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<T> Get(int id);
    Task<T> Delete(int id);
}
