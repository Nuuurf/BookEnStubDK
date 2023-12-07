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
        public async Task<IActionResult> FindCustomerFromPhone(string? phone)
        {
            try
            {
                Customer? sCustomer;
                if(phone == null)
                {
                    return BadRequest("Phone is required");
                }
                sCustomer = await _customerdata.GetCustomer(phone);
                if (sCustomer == null)
                {
                    return NotFound("No customer with that phonenumber exists");
                }

                DTOCustomer customer = DTO.ConvertToDTOCustomer(sCustomer);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
