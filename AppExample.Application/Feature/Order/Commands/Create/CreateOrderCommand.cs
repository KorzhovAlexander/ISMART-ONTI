using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Application.Common.Interfaces;
using AppExample.Core.Entities;
using AppExample.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Commands.Create
{
    public class CreateOrderCommand : IRequest<int>
    {
        /// <summary>
        /// Адресс, откуда надо забрать
        /// </summary>
        public string StartPoint { get; set; }

        /// <summary>
        /// Адресс, куда надо доставить
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Дата, с
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата, по
        /// </summary>
        public DateTime? DateTo { get; set; }

        public int ServiceTypeId { get; set; }

        public IEnumerable<OrderStructureDto> OrderStructures { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrderCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var number = await _context.Orders.AnyAsync(cancellationToken)
                ? await _context.Orders.MaxAsync(order => order.Number, cancellationToken) + 1
                : 1;

            var entity = new Core.Entities.Order
            {
                Number = number,
                Status = StatusEnum.ВОбработке,
                UserId = _currentUserService.UserId,

                StartPoint = request.StartPoint,
                EndPoint = request.EndPoint,
                ServiceTypeId = request.ServiceTypeId,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                OrderStructures = request.OrderStructures
                    .Select(orderStr => new OrderStructure
                    {
                        Count = orderStr.Count,
                        Product = orderStr.Product,
                        UnitId = orderStr.UnitId
                    }).ToList(),
                IsRemove = false
            };

            await _context.Orders.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}