using API.Interfaces;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly MatjarDBContext _context;
        public ItemRepository(MatjarDBContext context)
        {
            _context = context;
        }
        public bool ExistsCode(string code)
        {
            try
            {
                return (_context.Items.Any(u => u.Code == code));
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Insert(Item item)
        {
            try
            {
                if (item != null)
                {
                    _context.Items.Add(item);
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
        public async Task<bool> Update(Item item)
        {
            try
            {
                if (item != null)
                {
                    _context.Attach(item);
                    _context.Items.Update(item);
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
        public async Task<bool> Delete(Item item)
        {
            try
            {
                if (item != null)
                {
                    _context.Items.Remove(item);
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
                return _context.Items.Any(i => i.Id == id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Item> GetById(int id)
        {
            try
            {
                return await _context.Items.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Item>> GetItems()
        {
            try
            {
                return await _context.Items.Include(x => x.ItemVariations).Include(x => x.Type).Include(x => x.ItemTags).ToListAsync();
            }
            catch (Exception)
            {
                List<Item> x = new List<Item>();
                return x;
            }
        }

    }
}
