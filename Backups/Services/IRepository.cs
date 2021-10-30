using System.Collections.Generic;

namespace Backups.Services
{
    public interface IRepository<T>
    where T : class
    {
        List<T> GetContent();
        void AddObject(T obj);
        void DeleteObject(T obj);
    }
}