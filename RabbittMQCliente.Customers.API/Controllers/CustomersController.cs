using MessagingEvents.Shared;
using MessagingEvents.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using RabbittMQCliente.Customers.API.Bus;

namespace RabbittMQCliente.Customers.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        const string ROUTING_KEY = "building";
        private readonly IBusService _busService;
        public CustomersController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerInputModel inputModel)
        {
            var @event = new CustomerCreated(inputModel.Id, inputModel.FullName, inputModel.Email, inputModel.PhoneNumber, inputModel.BirthDate);

            await _busService.Publish(ROUTING_KEY, @event);

            return NoContent();
        }
    }
}
