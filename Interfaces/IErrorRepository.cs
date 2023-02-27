using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IErrorRepository
    {
       public Task<bool> LogError(int userId, string type, string message, string innerMessage, string code, string location);
    }
}
