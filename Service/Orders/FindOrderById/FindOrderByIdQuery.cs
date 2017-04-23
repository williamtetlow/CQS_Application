using System;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace Service.Orders.FindOrderById
{
    public class FindOrderByIdQuery : IQuery<Order>
    {
        public FindOrderByIdQuery(Guid id)
        {
            Id = id;
        }

        [Required]
        public Guid Id { get; }
    }
}
