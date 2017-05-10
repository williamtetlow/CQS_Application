using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Service.OrderLines.FindOrderLinesByIds
{
    public class FindOrderLinesByIdsQuery : IQuery<IEnumerable<OrderLine>>
    {
        public FindOrderLinesByIdsQuery(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }

        [Required]
        public IEnumerable<Guid> Ids { get; }
    }
}
