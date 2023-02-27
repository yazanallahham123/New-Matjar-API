using API.Interfaces;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class TypeServices
    {
        private readonly IPublicRepository _repository;

        public TypeServices(IPublicRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddType(Type type)
        {
            try
            {
                return await _repository.GetTypeRepository.Insert(type);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _repository.GetTypeRepository.Exists(id);
        }

        public bool DeleteType(Type type)
        {
            try
            {
                _repository.GetTypeRepository.Delete(type);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> Update(Type type)
        {
            try
            {
                return await _repository.GetTypeRepository.Update(type);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }



        public async Task<IEnumerable<Type>> GetTypes(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<Type> types = await _repository.GetTypeRepository.GetTypes();
            paginationInfo.SetValues(pageSize, page, types.Count());
            return types.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Type> GetTypeById(int id)
        {
            return await _repository.GetTypeRepository.GetById(id);

        }
    }
}
