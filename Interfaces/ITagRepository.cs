using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ITagRepository
    {
        public Task<bool> Insert(Tag item);
        public Task<bool> Delete(Tag item);
        public bool Exists(int id);
        public Task<bool> Update(Tag item);
        public Task<Tag> GetById(int id);
        public Task<List<Tag>> GetTags();
    }
}
