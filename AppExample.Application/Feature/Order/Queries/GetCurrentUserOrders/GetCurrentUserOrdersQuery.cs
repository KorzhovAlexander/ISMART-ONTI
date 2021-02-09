using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Queries.GetCurrentUserOrders
{
    public class GetCurrentUserOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
    }

    public class GetCurrentUserOrdersQueryHandler : IRequestHandler<GetCurrentUserOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetCurrentUserOrdersQueryHandler(ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetCurrentUserOrdersQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(order => order.UserId == _currentUserService.UserId
                                &&
                                !order.IsRemove)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}