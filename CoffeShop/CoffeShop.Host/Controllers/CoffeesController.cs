using CoffeShop.BL.Interfaces;
using CoffeShop.Models.Dto;
using CoffeShop.Models.Requests;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace CoffeShop.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeesController : ControllerBase
    {
        private readonly ICoffeeCrudService _coffeeCrudService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddCoffeeRequest> _validator;

        public CoffeesController(
            ICoffeeCrudService coffeeCrudService,
            IMapper mapper,
            IValidator<AddCoffeeRequest> validator)
        {
            _coffeeCrudService = coffeeCrudService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpDelete]
        public IActionResult DeleteCoffee(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID must be a valid Guid.");
            }

            var coffee = _coffeeCrudService.GetById(id);
            if (coffee == null)
            {
                return NotFound($"Coffee with ID {id} not found.");
            }

            _coffeeCrudService.DeleteCoffee(id);
            return Ok();
        }

        [HttpGet(nameof(GetById))]
        public IActionResult GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID must be a valid Guid.");
            }

            var coffee = _coffeeCrudService.GetById(id);

            if (coffee == null)
            {
                return NotFound($"Coffee with ID {id} not found.");
            }

            return Ok(coffee);
        }

        [HttpGet(nameof(GetAll))]
        public IActionResult GetAll()
        {
            var coffees = _coffeeCrudService.GetAllCoffees();
            return Ok(coffees);
        }

        [HttpPost]
        public IActionResult AddCoffee([FromBody] AddCoffeeRequest? coffeeRequest)
        {
            if (coffeeRequest == null)
            {
                return BadRequest("Coffee data is null.");
            }

            var result = _validator.Validate(coffeeRequest);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var coffee = _mapper.Map<Coffee>(coffeeRequest);

            _coffeeCrudService.AddCoffee(coffee);

            return Ok();
        }
    }
}
