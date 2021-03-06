﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Commands.OrderCompleted
{
    public class OrderCompletedCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class OrderCompletedCommandHandler : IRequestHandler<OrderCompletedCommand>
    {
        private readonly ApplicationDbContext _context;

        public OrderCompletedCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(OrderCompletedCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken);

            if (entity == null) throw new Exception("Заказ не найден");

            entity.Status = StatusEnum.Завершен;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}