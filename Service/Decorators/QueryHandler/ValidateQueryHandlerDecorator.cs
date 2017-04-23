using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Decorators.QueryHandler
{
    public class ValidateQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;

        public ValidateQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated)
        {
            _decorated = decorated;
        }

        public TResult Handle(TQuery query)
        {
            var validationContext = new ValidationContext(query);

            Validator.ValidateObject(query, validationContext, validateAllProperties: true);

            return _decorated.Handle(query);
        }
    }
}
