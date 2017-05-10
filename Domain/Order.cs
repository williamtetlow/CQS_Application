using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        public Guid Id { get; set; }

        public string User { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
