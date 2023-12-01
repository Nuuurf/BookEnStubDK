using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.BusinessLogic;
using RestfulApi.DTOs;
using RestfulApi.Models;

namespace RestfulApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerData _customerdata;

        public CustomerController(ICustomerData customerdata)
        {
            _customerdata = customerdata;
        }

        [HttpGet("{phone}")]
        public async Task<IActionResult> getCustomerFromPhone(string? phone)
        {
            try
            {
                if(phone == null)
                {
                    return BadRequest("Phone is required");
                }

                DTOCustomer customer = DTO.ConvertToDTOCustomer(await _customerdata.GetCustomer(phone));

                if (customer == null)
                {
                    return NotFound("No customer with that phonenumber exists");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
