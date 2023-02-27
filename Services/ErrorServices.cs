using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using API.Interfaces;

namespace API.Services
{
    public class ErrorServices
    {
        private readonly IPublicRepository _repository;

        public ErrorServices(IPublicRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> LogError(int userId, string type, string message, string innerMessage, string code, string location)
        {
            try
            {
                return await _repository.GetErrorRepository.LogError(userId, type, message, innerMessage, code, location);
               
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
