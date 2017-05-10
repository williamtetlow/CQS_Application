using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.v3;
using Service;
using Service.Orders.CreateOrder;

namespace CqsApplication.Controllers
{
    [Route("api/orders")]
    public class OrdersController : BaseController
    {
        private readonly ICommandHandler<CreateOrderCommand> _createOrderCommandHandler;
        private readonly ILogger _logger;

        public OrdersController(ICommandHandler<CreateOrderCommand> createOrderCommandHandler,
            ILogger logger)
        {
            _createOrderCommandHandler = createOrderCommandHandler;
            _logger = logger;
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] IEnumerable<Guid> orderLineIds)
        {
            _logger.LogDebug($"CreateOrder called - OrderLineIds: {orderLineIds.ToJson()}.");

            try
            {
                var user = GetUsername();

                var createOrderCommand = new CreateOrderCommand(user: user, orderLineIds: orderLineIds);

                _createOrderCommandHandler.Handle(createOrderCommand);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"CreateOrder failed with exception {e.Message}", e);

                return StatusCode(500);
            }
        }
    }
}
