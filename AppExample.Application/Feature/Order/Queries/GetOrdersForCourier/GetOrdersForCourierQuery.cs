using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Application.Common.Interfaces;
using AppExample.Core.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Queries.GetOrdersForCourier
{
    public class GetOrdersForCourierQuery : IRequest<IEnumerable<OrderDto>>
    {
    }

    public class GetOrdersForCourierQueryHandler : IRequestHandler<GetOrdersForCourierQuery, IEnumerable<OrderDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetOrdersForCourierQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersForCourierQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(order => order.Status == StatusEnum.ГотовКДоставке
                                ||
                                order.CourierId == _currentUserService.UserId
                                ||
                                order.UserId == _currentUserService.UserId)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}