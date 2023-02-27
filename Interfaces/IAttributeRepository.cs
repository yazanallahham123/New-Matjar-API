using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAttributeRepository
    {
        public Task<bool> Insert(Attribute attribute);
        public Task<bool> Delete(Attribute attribute);
        public bool Exists(int id);
        public Task<bool> Update(Attribute attribute);
        public Task<Attribute> GetById(int id);
        public Task<List<Attribute>> GetAttributes();
    }
}
