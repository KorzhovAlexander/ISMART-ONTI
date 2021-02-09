using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public string Reason { get; set; }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly ApplicationDbContext _context;

        public CancelOrderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .FirstOrDefaultAsync(order => order.Id == request.OrderId, cancellationToken);

            if (entity == null) throw new Exception("Заказ не найден");

            entity.Status = StatusEnum.Отменен;
            entity.Reason = request.Reason;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}