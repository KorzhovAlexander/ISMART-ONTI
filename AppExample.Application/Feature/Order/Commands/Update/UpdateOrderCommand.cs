using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit = MediatR.Unit;

namespace AppExample.Application.Feature.Order.Commands.Update
{
    public class UpdateOrderCommand : IRequest
    {
        public int Id { get; set; }

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

        public int? CourierId { get; set; }
        
        public IEnumerable<Create.OrderStructureDto> OrderStructures { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdateOrderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .Include(i => i.OrderStructures)
                .FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken);

            if (entity == null) throw new Exception("Заказ не найден");

            entity.StartPoint = request.StartPoint;
            entity.EndPoint = request.EndPoint;
            entity.DateFrom = request.DateFrom;
            entity.DateTo = request.DateTo;
            entity.ServiceTypeId = request.ServiceTypeId;
            entity.OrderStructures = request.OrderStructures
                .Select(orderStr => new OrderStructure
                {
                    Count = orderStr.Count,
                    Product = orderStr.Product,
                    UnitId = orderStr.UnitId
                }).ToList();
            entity.CourierId = request.CourierId;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}