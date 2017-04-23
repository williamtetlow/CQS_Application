using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OrderLine
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
    }
}
