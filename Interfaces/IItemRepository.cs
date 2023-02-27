using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IItemRepository
    {
        public bool ExistsCode(string code);
        public Task<bool> Insert(Item item);
        public Task<bool> Delete(Item item);
        public bool Exists(int id);
        public Task<bool> Update(Item item);
        public Task<Item> GetById(int id);
        public Task<List<Item>> GetItems();
    }
}
