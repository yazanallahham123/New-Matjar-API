using API.Interfaces;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ItemServices
    {
        private readonly IPublicRepository _repository;

        public ItemServices(IPublicRepository repository)
        {
            _repository = repository;
        }
        public bool ExistsCode(string code)
        {
            return _repository.GetItemRepository.ExistsCode(code);
        }

        public async Task<bool> AddItem(Item item)
        {
            try
            {
                return await _repository.GetItemRepository.Insert(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _repository.GetItemRepository.Exists(id);
        }

        public bool DeleteItem(Item item)
        {
            try
            {
                _repository.GetItemRepository.Delete(item);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> Update(Item item)
        {
            try
            {
                return await _repository.GetItemRepository.Update(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

 

        public async Task<IEnumerable<Item>> GetItems(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<Item> items = await _repository.GetItemRepository.GetItems();
            paginationInfo.SetValues(pageSize, page, items.Count());
            return items.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _repository.GetItemRepository.GetById(id);

        }
    }
}
