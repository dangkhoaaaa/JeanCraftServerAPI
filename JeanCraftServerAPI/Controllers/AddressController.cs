using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AddressController(IAddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressesByUserId(Guid userId)
        {
            var addresses = await _addressService.GetAddressesByUserId(userId);
            return Ok(addresses);
        }

        //[HttpGet("{id}/user/{userId}")]
        //public async Task<ActionResult<Address>> GetAddressByIdAndUserId(Guid id, Guid userId)
        //{
        //    var address = await _addressService.GetAddressByIdAndUserId(id, userId);
        //    if (address == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(address);
        //}

        [HttpPost("create")]
        public async Task<IActionResult> CreateAddress([FromBody] Address addressdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (addressdto.UserId == null || addressdto.UserId == Guid.Empty)
            {
                return BadRequest("UserId is required and cannot be empty.");
            }

            var newAddress = new Address
            {
                Id = Guid.NewGuid(),
                UserId = addressdto.UserId,
                Type = addressdto.Type,
                Detail = addressdto.Detail
            };

            var createdAddress = await _addressService.CreateAddress(newAddress);

            if (createdAddress == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating address.");
            }

            return Ok(createdAddress);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateAddress([FromQuery]Guid id, [FromBody]AddressDTO address)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var updatedAddress = await _addressService.UpdateAddress(id, address);
            if (updatedAddress == null)
            {
                return NotFound();
            }
            return Ok(updatedAddress);
        }

        [HttpDelete("{id}/user/{userId}")]
        public async Task<IActionResult> DeleteAddress(Guid id, Guid userId)
        {
            var result = await _addressService.DeleteAddress(id, userId);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
