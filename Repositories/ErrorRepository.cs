using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using API.Interfaces;

namespace API.Repositories
{
    public class ErrorRepository: IErrorRepository
    {

        private readonly MatjarDBContext _context;
        public ErrorRepository(MatjarDBContext context)
        {
            _context = context;
        }
        public async Task<bool> LogError(int userId, string type, string message, string innerMessage, string code, string location)
        {
            try
            {
                ErrorLog errorLog = new ErrorLog();
                errorLog.Date = DateTime.Now;
                errorLog.UserId = userId;
                errorLog.Type = type;
                errorLog.Message = message;
                errorLog.InnerMessage = innerMessage;
                errorLog.Location = location;

                _context.ErrorLogs.Add(errorLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
