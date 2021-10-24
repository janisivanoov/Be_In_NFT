namespace TwitterSystem.Data.Repository
{
    using System.Linq;

    public interface IRepository<T>
    {
        IQueryable<T> All();

        T Find(object id);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(object id);

        void SaveChanges();
    }
}
