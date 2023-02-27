using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly MatjarDBContext _context;
        public UserRepository(MatjarDBContext context)
        {
            _context = context;
        }
        public async Task<User> GetByMobileNumber(string mobileNumber, int? roleId)
        {
            try
            {

                if (roleId != null)
                {
                    if (roleId > 0)
                    {
                        if (_context.Users.Any(u => u.MobileNumber == mobileNumber && u.RoleId == roleId))
                            return await _context.Users.Where(u => u.MobileNumber == mobileNumber && u.RoleId == roleId).FirstOrDefaultAsync();
                        else
                            return null;
                    }
                    else
                    {
                        if (_context.Users.Any(u => u.MobileNumber == mobileNumber))
                            return await _context.Users.Where(u => u.MobileNumber == mobileNumber).FirstOrDefaultAsync();
                        else
                            return null;
                    }
                }
                else
                {
                    if (_context.Users.Any(u => u.MobileNumber == mobileNumber))
                        return await _context.Users.Where(u => u.MobileNumber == mobileNumber).FirstOrDefaultAsync();
                    else
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> UpdateUserLanguage(int userId, int language)
        {
            try
            {
                if (_context.Users.Any((u) => u.Id == userId))
                {
                    User u = await _context.Users.Where((u) => u.Id == userId).FirstOrDefaultAsync();

                    if (u != null)
                    {
                        u.Language = language;
                        await Update(u);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Update(User user)
        {
            try
            {
                if (user != null)
                {
                    _context.Attach(user);
                    _context.Users.Update(user);
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
        public async Task<bool> Delete(User user)
        {
            try
            {
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            } catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Insert(User user)
        {
            try
            {
                if (user != null)
                {
                    _context.Users.Add(user);
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
        public bool ExistsMobileNumber(string mobilNumber)
        {
            try
            {
                return (_context.Users.Any(u => u.MobileNumber == mobilNumber));
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
                return _context.Users.Any(u => u.Id == id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<User> GetById(int id, int? roleId)
        {
            try
            {
                if (roleId != null)
                {
                    if (roleId > 0)
                    {
                        if (_context.Users.Any(x => x.Id == id && x.RoleId == roleId))
                            return await _context.Users.Where(x => x.Id == id && x.RoleId == roleId).FirstOrDefaultAsync();
                        else
                            return null;
                    }
                    else
                    {
                        if (_context.Users.Any(x => x.Id == id))
                            return await _context.Users.FindAsync(id);
                        else
                            return null;
                    }
                }
                else
                    return await _context.Users.FindAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateFirebaseToken(int userId, string firebaseToken)
        {
            try
            {
                if (_context.Users.Any((u) => u.Id == userId))
                {
                    User u = await _context.Users.Where((u) => u.Id == userId).FirstOrDefaultAsync();

                    if (u != null)
                    {
                        u.FirebaseToken = firebaseToken;
                        await Update(u);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<User>> GetUsers(int roleId)
        {
            try
            {
                return await _context.Users.Where(u => u.RoleId == roleId).ToListAsync();
            }
            catch(Exception)
            {
                List<User> x = new List<User>();
                return x;
            }
        }
    }
}
