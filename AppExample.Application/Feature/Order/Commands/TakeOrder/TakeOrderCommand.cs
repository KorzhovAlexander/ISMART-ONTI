using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Application.Common.Interfaces;
using AppExample.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Commands.TakeOrder
{
    public class TakeOrderCommand : IRequest
    {
        public int OrderId { get; set; }
    }

    public class TakeOrderCommandHandler : IRequestHandler<TakeOrderCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public TakeOrderCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(TakeOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .FirstOrDefaultAsync(order => order.Id == request.OrderId, cancellationToken);

            if (entity == null) throw new Exception("Заказ не найден");
            if (entity.CourierId != null) throw new Exception("Заказ выполняется другим курьером");

            entity.CourierId = _currentUserService.UserId;
            entity.Status = StatusEnum.Доставка;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}