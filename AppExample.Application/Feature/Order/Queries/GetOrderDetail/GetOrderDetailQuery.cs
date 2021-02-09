using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppExample.Application.Feature.Order.Queries.GetOrderDetail
{
    public class GetOrderDetailQuery : IRequest<OrderDetailsDto>
    {
        public int OrderId { get; set; }
    }
    
    public class GetOrderDetailQueryHandler : IRequestHandler<GetOrderDetailQuery,OrderDetailsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderDetailQueryHandler(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDetailsDto> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Include(i => i.OrderStructures)
                .Where(order => order.Id == request.OrderId)
                .ProjectTo<OrderDetailsDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}