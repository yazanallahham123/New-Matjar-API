using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace API.Repositories
{
    public class TagRepository: ITagRepository
    {
        private readonly MatjarDBContext _context;
        public TagRepository(MatjarDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Insert(Tag tag)
        {
            try
            {
                if (tag != null)
                {
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Update(Tag tag)
        {
            try
            {
                if (tag != null)
                {
                    _context.Attach(tag);
                    _context.Tags.Update(tag);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Delete(Tag tag)
        {
            try
            {
                if (tag != null)
                {
                    _context.Tags.Remove(tag);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Exists(int id)
        {
            try
            {
                return _context.Tags.Any(i => i.Id == id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Tag> GetById(int id)
        {
            try
            {
                return await _context.Tags.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Tag>> GetTags()
        {
            try
            {
                return await _context.Tags.ToListAsync();
            }
            catch (Exception)
            {
                List<Tag> x = new List<Tag>();
                return x;
            }
        }
    }
}
