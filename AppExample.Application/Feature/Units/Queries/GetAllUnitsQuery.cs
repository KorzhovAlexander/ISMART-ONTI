using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit = AppExample.Core.Entities.Unit;

namespace AppExample.Application.Feature.Units.Queries
{
    public class GetAllUnitsQuery : IRequest<IEnumerable<Core.Entities.Unit>>
    {
    }

    public class GetAllUnitsQueryHandler : IRequestHandler<GetAllUnitsQuery, IEnumerable<Core.Entities.Unit>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllUnitsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Unit>> Handle(GetAllUnitsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Units.AsQueryable().AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}