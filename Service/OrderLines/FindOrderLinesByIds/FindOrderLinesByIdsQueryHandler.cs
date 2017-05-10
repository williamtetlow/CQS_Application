using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Persistence;

namespace Service.OrderLines.FindOrderLinesByIds
{
    public class FindOrderLinesByIdsQueryHandler : IQueryHandler<FindOrderLinesByIdsQuery, IEnumerable<OrderLine>>
    {
        private readonly ICqsDbContext _dbContext;

        public FindOrderLinesByIdsQueryHandler(ICqsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<OrderLine> Handle(FindOrderLinesByIdsQuery query)
        {
            return _dbContext.OrderLines.Where(x => query.Ids.Contains(x.OrderId));
        }
    }
}
