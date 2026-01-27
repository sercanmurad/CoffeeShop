using CoffeShop.BL.Interfaces;
using CoffeShop.Models.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CoffeShop.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerCrudService _customerCrudService;
        private readonly IValidator<Customer> _validator;

        public CustomersController(ICustomerCrudService customerCrudService, IValidator<Customer> validator)
        {
            _customerCrudService = customerCrudService;
            _validator = validator;
        }

        [HttpDelete]
        public IActionResult DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID must be a valid GUID.");
            }

            var customer = _customerCrudService.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            _customerCrudService.DeleteCustomer(id);
            return Ok();
        }

        [HttpGet(nameof(GetById))]
        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID must be a valid GUID.");
            }

            var customer = _customerCrudService.GetById(id);

            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return Ok(customer);
        }

        [HttpGet(nameof(GetAll))]
        public IActionResult GetAll()
        {
            var customers = _customerCrudService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer? customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is null.");
            }

            var result = _validator.Validate(customer);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            _customerCrudService.AddCustomer(customer);
            return Ok();
        }
    }
}
