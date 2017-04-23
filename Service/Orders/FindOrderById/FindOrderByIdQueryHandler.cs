using System.Linq;
using Domain;
using Persistence;

namespace Service.Orders.FindOrderById
{
    public class FindOrderByIdQueryHandler : IQueryHandler<FindOrderByIdQuery, Order>
    {
        private readonly ICqsDbContext _dbContext;

        public FindOrderByIdQueryHandler(ICqsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order Handle(FindOrderByIdQuery query)
        {
            return _dbContext.Orders.Single(o => o.Id == query.Id);
        }
    }
}
