using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetByMobileNumber(string mobileNumber, int? roleId);

        public Task<bool> UpdateUserLanguage(int userId, int language);

        public bool ExistsMobileNumber(string mobileNumber);

        public Task<bool> Insert(User user);
        public Task<bool> Delete(User user);

        public bool Exists(int id);

        public Task<bool> Update(User user);

        public Task<User> GetById(int id, int? roleId);

        public Task<bool> UpdateFirebaseToken(int userId, string firebaseToken);

        public Task<List<User>> GetUsers(int roleId);
    }
}
