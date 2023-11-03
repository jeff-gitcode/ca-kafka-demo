using Domain;

namespace Application.Abstration;

public interface IRepository<T> where T : User
{
    IEnumerable<T> GetAll();
    Task<T> Add(T entity);
    T Update(T entity);
    T Get(int id);
    void Delete(int id);
}
