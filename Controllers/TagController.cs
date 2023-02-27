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
    public class TagController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TagServices _tagServices;

        public TagController(IMapper mapper, TagServices tagServices)
        {
            _mapper = mapper;
            _tagServices = tagServices;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Insert([FromBody] Tag tag)
        {

            if (tag == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }


            bool res = await _tagServices.AddTag(tag);
            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }


            return Ok(tag);

        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Tag tag)
        {

            if (tag == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }


            bool res = await _tagServices.Update(tag);

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
        public async Task<IActionResult> DeleteTag(int id)
        {

            Tag tag = await _tagServices.GetTagById(id);

            if (tag == null)
            {
                return NotFound(CommonMessages.NOT_FOUND);
            }

            if (!_tagServices.DeleteTag(tag))
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.DELETED_SUCCESSFULLY);
        }


        // GET: api/item/items
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTags(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Tag> tags = await _tagServices.GetTags(page, pageSize, paginationInfo);
            IEnumerable<Tag> tag = _mapper.Map<IEnumerable<Tag>>(tags);

            var response = new
            {
                Tags = tag,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }
    }
}
