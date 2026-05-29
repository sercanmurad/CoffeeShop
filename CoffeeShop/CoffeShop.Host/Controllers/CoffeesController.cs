using System;
using System.Threading.Tasks;
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
        private readonly ISellCoffee _sellCoffee;

        public CoffeesController(
            ICoffeeCrudService coffeeCrudService,
            IMapper mapper,
            IValidator<AddCoffeeRequest> validator,
            ISellCoffee sellCoffee)
        {
            _coffeeCrudService = coffeeCrudService;
            _mapper = mapper;
            _validator = validator;
            _sellCoffee = sellCoffee;
        }

        [HttpPost(nameof(SellCoffee))]
        public async Task<IActionResult> SellCoffee(Guid coffeeId, Guid customerId)
        {
            if (coffeeId == Guid.Empty || customerId == Guid.Empty)
                return BadRequest("IDs must be valid Guids.");

            var result = await _sellCoffee.Sell(coffeeId, customerId);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCoffee(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("ID must be a valid Guid.");

            var coffee = await _coffeeCrudService.GetByIdAsync(id);
            if (coffee == null)
                return NotFound($"Coffee with ID {id} not found.");

            await _coffeeCrudService.DeleteCoffeeAsync(id);
            return Ok();
        }

        [HttpGet(nameof(GetById))]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("ID must be a valid Guid.");

            var coffee = await _coffeeCrudService.GetByIdAsync(id);
            if (coffee == null)
                return NotFound($"Coffee with ID {id} not found.");

            return Ok(coffee);
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var coffees = await _coffeeCrudService.GetAllCoffeesAsync();
            return Ok(coffees);
        }

        [HttpPost]
        public async Task<IActionResult> AddCoffee([FromBody] AddCoffeeRequest? coffeeRequest)
        {
            if (coffeeRequest == null)
                return BadRequest("Coffee data is null.");

            var result = _validator.Validate(coffeeRequest);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            var coffee = _mapper.Map<Coffee>(coffeeRequest);
            await _coffeeCrudService.AddCoffeeAsync(coffee);
            return Ok();
        }
    }
}
