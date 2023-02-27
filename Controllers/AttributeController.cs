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
    public class AttributeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AttributeServices _attributeServices;

        public AttributeController(IMapper mapper, AttributeServices attributeServices)
        {
            _mapper = mapper;
            _attributeServices = attributeServices;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Insert([FromBody] Attribute attribute)
        {

            if (attribute == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }

            bool res = await _attributeServices.AddAttribute(attribute);
            if (!res)
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }


            return Ok(attribute);

        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Attribute attribute)
        {

            if (attribute == null)
            {
                return BadRequest(CommonMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CommonMessages.ModelStateParser(ModelState));
            }


            bool res = await _attributeServices.Update(attribute);

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
        public async Task<IActionResult> DeleteAttribute(int id)
        {

            Attribute attribute = await _attributeServices.GetAttributeById(id);

            if (attribute == null)
            {
                return NotFound(CommonMessages.NOT_FOUND);
            }

            if (!_attributeServices.DeleteAttribute(attribute))
            {
                return BadRequest(CommonMessages.ERROR_UPDATE);
            }

            return Ok(CommonMessages.DELETED_SUCCESSFULLY);
        }


        // GET: api/item/items
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttributes(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Attribute> attributes = await _attributeServices.GetAttributes(page, pageSize, paginationInfo);
            IEnumerable<Attribute> attribute = _mapper.Map<IEnumerable<Attribute>>(attributes);

            var response = new
            {
                Attributes = attribute,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }
    }
}
