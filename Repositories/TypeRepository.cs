using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace API.Repositories
{
    public class TypeRepository: ITypeRepository
    {
        private readonly MatjarDBContext _context;
        public TypeRepository(MatjarDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Insert(Type type)
        {
            try
            {
                if (type != null)
                {
                    _context.Types.Add(type);
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
        public async Task<bool> Update(Type type)
        {
            try
            {
                if (type != null)
                {
                    _context.Attach(type);
                    _context.Types.Update(type);
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
        public async Task<bool> Delete(Type type)
        {
            try
            {
                if (type != null)
                {
                    _context.Types.Remove(type);
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
                return _context.Types.Any(i => i.Id == id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Type> GetById(int id)
        {
            try
            {
                return await _context.Types.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Type>> GetTypes()
        {
            try
            {
                return await _context.Types.ToListAsync();
            }
            catch (Exception)
            {
                List<Type> x = new List<Type>();
                return x;
            }
        }
    }
}
