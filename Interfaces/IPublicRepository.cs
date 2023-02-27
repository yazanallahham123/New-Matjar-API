using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IPublicRepository
    {
        IUserRepository GetUserRepository { get; }
        IItemRepository GetItemRepository { get; }
        ITagRepository GetTagRepository { get; }
        ITypeRepository GetTypeRepository { get; }
        IAttributeRepository GetAttributeRepository { get; }
        IErrorRepository GetErrorRepository { get; }
    }
}
