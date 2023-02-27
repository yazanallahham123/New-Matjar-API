using API.Interfaces;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class UserServices
    {
        private readonly IPublicRepository _repository;
        private readonly SecurityServices _securityServices;

        public UserServices(IPublicRepository repository, SecurityServices securityServices)
        {
            _repository = repository;
            _securityServices = securityServices;
        }

        public async Task<User> GetByMobileNumber(string mobileNumber, int? roleId)
        {
            return await _repository.GetUserRepository.GetByMobileNumber(mobileNumber, roleId);
        }

        public async Task<bool> UpdateUserLanguage(int userId, int language)
        {

            try
            {
                return await _repository.GetUserRepository.UpdateUserLanguage(userId, language);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public bool ExistsMobileNumber(string mobileNumber)
        {
            return _repository.GetUserRepository.ExistsMobileNumber(mobileNumber);
        }

        public async Task<bool> AddUser(User user, RoleType role)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                user.RoleId = (int)role;
                return await _repository.GetUserRepository.Insert(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<User> GetUserById(int id, int? roleId)
        {
            return await _repository.GetUserRepository.GetById(id, roleId);

        }

        public bool Exists(int id)
        {
            return _repository.GetUserRepository.Exists(id);
        }

        public bool DeleteUser(User user)
        {
            try
            {
                _repository.GetUserRepository.Delete(user);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool ChangePassword(User oldUser, string newPassword)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(newPassword);
                oldUser.Password = hashSalt.Hash;
                oldUser.StoredSalt = hashSalt.Salt;
                _repository.GetUserRepository.Update(oldUser);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
        public async Task<bool> Update(User user)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                return await _repository.GetUserRepository.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateFirebaseToken(int userId, string firebaseToken)
        {
            try
            {
                return await _repository.GetUserRepository.UpdateFirebaseToken(userId, firebaseToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<IEnumerable<User>> GetUsers(int page, int pageSize, PaginationInfo paginationInfo, int roleId)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetUsers(roleId);
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }
}
