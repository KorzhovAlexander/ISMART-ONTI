using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Queries.TrackOrder
{
    public class TrackOrderQuery : IRequest<OrderDto>
    {
        public int Number { get; set; }
    }

    public class TrackOrderQueryHandler : IRequestHandler<TrackOrderQuery, OrderDto>
    {
        private readonly ApplicationDbContext _context;

        public TrackOrderQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> Handle(TrackOrderQuery request, CancellationToken cancellationToken)
        {
            var maxProgress = Enum.GetValues(typeof(StatusEnum)).Length - 1;

            return await _context.Orders
                .Where(order => order.Number == request.Number)
                .Select(order => new OrderDto
                {
                    Id = order.Id,
                    Number = order.Number,
                    UserId = order.UserId,
                    Username = order.User.UserName,
                    CourierId = order.CourierId,
                    CourierName = order.Courier.UserName,
                    DateTo = order.DateTo,
                    DateFrom = order.DateFrom,
                    Status = order.Status,
                    StartPoint = order.StartPoint,
                    EndPoint = order.EndPoint,
                    TotalCost = order.TotalCost,
                    ServiceTypeId = order.ServiceTypeId,
                    NameServiceType = order.ServiceType.Name,
                    StatusName = order.Status.ToString(),
                    Progress = order.Status == StatusEnum.Отменен
                        ? 0
                        : (int) order.Status,
                    MaxProgress = maxProgress
                })
                .FirstOrDefaultAsync(cancellationToken);
            ;
        }
    }
}