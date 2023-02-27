using API.Services;
using API.Utils.Messages;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ItemServices _itemServices;

        public ItemController(IMapper mapper, ItemServices itemServices)
        {
            _mapper = mapper;
            _itemServices = itemServices;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Insert([FromBody] Item item)
        {

            if (item == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }

            if (_itemServices.ExistsCode(item.Code))
            {
                ModelState.AddModelError("item", ItemErrorMessages.CODE_DUPLICATED);
                return BadRequest(AuthErrorMessages.ModelStateParser(ModelState));
            }

            bool res = await _itemServices.AddItem(item);
            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }


            return Ok(item);

        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Item item)
        {

            if (item == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }


            bool res = await _itemServices.Update(item);

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
        public async Task<IActionResult> DeleteItem(int id)
        {

            Item item = await _itemServices.GetItemById(id);

            if (item == null)
            {
                return NotFound(CommonMessages.NOT_FOUND);
            }

            if (!_itemServices.DeleteItem(item))
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.DELETED_SUCCESSFULLY);
        }
   

        // GET: api/item/items
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Item> items = await _itemServices.GetItems(page, pageSize, paginationInfo);
            IEnumerable<Item> item = _mapper.Map<IEnumerable<Item>>(items);

            var response = new
            {
                Items = item,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }
    }
}
