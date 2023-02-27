using API.Services;
using API.Utils.Messages;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TypeServices _typeServices;

        public TypeController(IMapper mapper, TypeServices typeServices)
        {
            _mapper = mapper;
            _typeServices = typeServices;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Insert([FromBody] Type type)
        {

            if (type == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }

            bool res = await _typeServices.AddType(type);
            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }


            return Ok(type);

        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Type type)
        {

            if (type == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }


            bool res = await _typeServices.Update(type);

            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.UPDATED_SUCCESSFULLY);


        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteType(int id)
        {

            Type type = await _typeServices.GetTypeById(id);

            if (type == null)
            {
                return NotFound(CommonMessages.NOT_FOUND);
            }

            if (!_typeServices.DeleteType(type))
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.DELETED_SUCCESSFULLY);
        }


        // GET: api/item/items
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTypes(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Type> types = await _typeServices.GetTypes(page, pageSize, paginationInfo);
            IEnumerable<Type> type = _mapper.Map<IEnumerable<Type>>(types);

            var response = new
            {
                Types = type,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }
    }
}
