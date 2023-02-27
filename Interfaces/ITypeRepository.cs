using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ITypeRepository
    {
        public Task<bool> Insert(Type type);
        public Task<bool> Delete(Type type);
        public bool Exists(int id);
        public Task<bool> Update(Type type);
        public Task<Type> GetById(int id);
        public Task<List<Type>> GetTypes();
    }
}
