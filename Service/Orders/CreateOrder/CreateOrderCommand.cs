using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Orders.CreateOrder
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(string user, IEnumerable<Guid> orderLineIds)
        {
            User = user;
            OrderLineIds = orderLineIds;
        }

        [Required(AllowEmptyStrings = false)]
        public string User { get; }

        [Required]
        public IEnumerable<Guid> OrderLineIds { get; }
    }
}
