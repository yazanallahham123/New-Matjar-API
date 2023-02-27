using API.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly MatjarDBContext _context;
        public AttributeRepository(MatjarDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Insert(Attribute attribute)
        {
            try
            {
                if (attribute != null)
                {
                    _context.Attributes.Add(attribute);
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
        public async Task<bool> Update(Attribute attribute)
        {
            try
            {
                if (attribute != null)
                {
                    _context.Attach(attribute);
                    _context.Attributes.Update(attribute);
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
        public async Task<bool> Delete(Attribute attribute)
        {
            try
            {
                if (attribute != null)
                {
                    _context.Attributes.Remove(attribute);
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
                return _context.Attributes.Any(i => i.Id == id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Attribute> GetById(int id)
        {
            try
            {
                return await _context.Attributes.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Attribute>> GetAttributes()
        {
            try
            {
                return await _context.Attributes.ToListAsync();
            }
            catch (Exception)
            {
                List<Attribute> x = new List<Attribute>();
                return x;
            }
        }
    }
}
