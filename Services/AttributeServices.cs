using API.Interfaces;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class AttributeServices
    {
        private readonly IPublicRepository _repository;

        public AttributeServices(IPublicRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAttribute(Attribute attribute)
        {
            try
            {
                return await _repository.GetAttributeRepository.Insert(attribute);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _repository.GetAttributeRepository.Exists(id);
        }

        public bool DeleteAttribute(Attribute attribute)
        {
            try
            {
                _repository.GetAttributeRepository.Delete(attribute);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> Update(Attribute attribute)
        {
            try
            {
                return await _repository.GetAttributeRepository.Update(attribute);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }



        public async Task<IEnumerable<Attribute>> GetAttributes(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<Attribute> attributes = await _repository.GetAttributeRepository.GetAttributes();
            paginationInfo.SetValues(pageSize, page, attributes.Count());
            return attributes.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Attribute> GetAttributeById(int id)
        {
            return await _repository.GetAttributeRepository.GetById(id);

        }
    }
}
