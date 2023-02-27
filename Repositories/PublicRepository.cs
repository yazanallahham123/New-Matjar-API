using API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class PublicRepository : IPublicRepository
    {
        private readonly MatjarDBContext _context;
        private IUserRepository _userRepository;
        private IItemRepository _itemRepository;
        private ITagRepository _tagRepository;
        private ITypeRepository _typeRepository;
        private IAttributeRepository _attributeRepository;
        private IErrorRepository _errorRepository;

        public PublicRepository(MatjarDBContext context)
        {
            _context = context;
        }

        public IUserRepository GetUserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public IItemRepository GetItemRepository
        {
            get
            {
                return _itemRepository ??= new ItemRepository(_context);
            }
        }
        public ITagRepository GetTagRepository
        {
            get
            {
                return _tagRepository ??= new TagRepository(_context);
            }
        }

        public ITypeRepository GetTypeRepository
        {
            get
            {
                return _typeRepository ??= new TypeRepository(_context);
            }
        }

        public IAttributeRepository GetAttributeRepository
        {
            get
            {
                return _attributeRepository ??= new AttributeRepository(_context);
            }
        }


        public IErrorRepository GetErrorRepository
        {
            get
            {
                return _errorRepository ??= new ErrorRepository(_context);
            }
        }
    }
}
