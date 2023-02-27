using API.Interfaces;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class TagServices
    {
        private readonly IPublicRepository _repository;

        public TagServices(IPublicRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddTag(Tag tag)
        {
            try
            {
                return await _repository.GetTagRepository.Insert(tag);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _repository.GetTagRepository.Exists(id);
        }

        public bool DeleteTag(Tag tag)
        {
            try
            {
                _repository.GetTagRepository.Delete(tag);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> Update(Tag tag)
        {
            try
            {
                return await _repository.GetTagRepository.Update(tag);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }



        public async Task<IEnumerable<Tag>> GetTags(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<Tag> tags = await _repository.GetTagRepository.GetTags();
            paginationInfo.SetValues(pageSize, page, tags.Count());
            return tags.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Tag> GetTagById(int id)
        {
            return await _repository.GetTagRepository.GetById(id);

        }
    }
}
